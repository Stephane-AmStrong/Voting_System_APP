using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;
using Application.Models;
using Domain.Settings;
using Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Repository
{
    public class TokenRepository : IdentityRepositoryBase<UserToken>, ITokenService
    {
        private readonly UserManager<Voter> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;


        public TokenRepository
        (
            AppDbContext identityContext,
            UserManager<Voter> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings
        ) : base(identityContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> EncodeStringAsync(string stringToEncode)
        {
            var encodedBytes = await Task.Run(() => Encoding.UTF8.GetBytes(stringToEncode));
            return await Task.Run(() => WebEncoders.Base64UrlEncode(encodedBytes));
        }

        public async Task<string> DecodeStringAsync(string stringToDecode)
        {
            var decodedByte = await Task.Run(() => WebEncoders.Base64UrlDecode(stringToDecode));
            return await Task.Run(() => Encoding.UTF8.GetString(decodedByte));
        }

        public async Task<RefreshTokens> RefreshAsync(string accessToken, string refreshToken, string ipAddress)
        {
            var principal = await GetPrincipalFromExpiredTokenAsync(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var appUser = await _userManager.FindByNameAsync(username);
            var userRefreshToken = await GetByUserIdAsync(appUser.Id);

            if (appUser == null || userRefreshToken == null || userRefreshToken.Value != refreshToken || userRefreshToken.IsActive == false) throw new ApiException($"Invalid client request.");

            var newAccessToken = await GenerateJWToken(appUser);
            var newRefreshToken = await GenerateRefreshTokenAsync(ipAddress, appUser.Id);

            newRefreshToken = await CommitAsync(newRefreshToken);

            return new RefreshTokens
            {
                //AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                AccessToken = new AccessToken
                {
                    Value = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    ExpiryDate = newAccessToken.ValidTo
                },
                RefreshToken = newRefreshToken
            };
        }

        public async Task<UserToken> CommitAsync(UserToken refreshToken)
        {
            var newestTokenId = refreshToken.Id;
            var lastRefreshToken = await GetByIdAsync(newestTokenId);

            if (lastRefreshToken == null)
            {
                await CreateAsync(refreshToken);
            }
            else
            {
                newestTokenId = Guid.NewGuid().ToString();
                lastRefreshToken.Id = newestTokenId;

                lastRefreshToken.UserId = refreshToken.UserId;
                lastRefreshToken.Value = refreshToken.Value;
                lastRefreshToken.ExpiryDate = refreshToken.ExpiryDate;

                await UpdateAsync(lastRefreshToken);
            }

            await SaveAsync();

            lastRefreshToken = await GetByIdAsync(newestTokenId);

            if (lastRefreshToken != null) return lastRefreshToken;

            throw new ApiException($"An error occurred during the generation of the Token.");
        }

        public async Task<AuthenticationModel> GeneratePasswordResetTokenAsync(string email)
        {
            var account = await _userManager.FindByEmailAsync(email);

            // always return ok response to prevent email enumeration
            if (account == null)
            {
                return new AuthenticationModel
                {
                    IsSuccess = false,
                    Message = "No user associated with this email",
                };
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(account);
            //resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
            resetToken = await EncodeStringAsync(resetToken);

            return new AuthenticationModel
            {
                AccessToken = new AccessToken
                {
                    Value = resetToken,
                    //ExpiryDate = 
                },
                IsSuccess = true,
                //Message = "Password reset url has been sent to your email successfully",
            };
        }

        public async Task<UserToken> GenerateRefreshTokenAsync(string ipAddress, string userId)
        {
            var random = new Random();
            var randomBytes = new byte[40];

            await Task.Run(() => random.NextBytes(randomBytes));

            //using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            //await Task.Run(() => rngCryptoServiceProvider.GetBytes(randomBytes));

            using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            await Task.Run(() => rngCryptoServiceProvider.GetBytes(randomBytes));

            // convert random bytes to hex string
            var randomTokenString = await Task.Run(() => BitConverter.ToString(randomBytes).Replace("-", ""));
            //var randomTokenString = await Task.Run(() => Convert.ToBase64String(randomBytes));

            var userRefreshToken = await GetByUserIdAsync(userId);

            return new UserToken
            {
                Id = userRefreshToken != null ? userRefreshToken.Id : Guid.NewGuid().ToString(),
                UserId = userId,
                //LoginProvider = randomTokenString,
                Value = randomTokenString,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };
        }

        public async Task<JwtSecurityToken> GenerateJWToken(Voter appUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            
            //if verifier si l'utilisateur a au moins un role 
            var roleName = (await _userManager.GetRolesAsync(appUser))[0];
            var role = await _roleManager.FindByNameAsync(roleName);


            var roleClaims = new List<Claim>();
            roleClaims.Add(new Claim(ClaimTypes.Role, roleName));
            roleClaims.AddRange(await _roleManager.GetClaimsAsync(role));

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim("ip", ipAddress),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.GivenName, appUser.LastName),
                new Claim(ClaimTypes.Surname, appUser.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
            };

            ClaimsPrincipal principal = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;

            try
            {
                principal = await Task.Run(() => tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken));
            }
            catch (Exception ex)
            {
                if (ex is SecurityTokenException) throw new SecurityTokenException($"Token validation failed : {ex.Message}");
                //if (ex is SecurityTokenSignatureKeyNotFoundException) throw new SecurityTokenException($"Token validation failed : {ex.Message}");
                throw new SecurityTokenException($"Token validation failed: {ex.Message}");
            }

            var jwtSecurityToken = validatedToken as JwtSecurityToken;

            if (principal == null || jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<UserToken> GetByIdAsync(string id)
        {
            return await BaseFindByCondition(userToken => userToken.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<UserToken> GetByUserIdAsync(string userId)
        {
            return await Task.Run(() => BaseFindByCondition(x => x.UserId == userId).OrderByDescending(x => x.ExpiryDate).FirstOrDefault());
        }

        public async Task CreateAsync(UserToken refreshToken)
        {
            await BaseCreateAsync(refreshToken);
        }

        public async Task UpdateAsync(UserToken refreshToken)
        {
            await BaseUpdateAsync(refreshToken);
        }

        public async Task DeleteAsync(UserToken refreshToken)
        {
            await BaseDeleteAsync(refreshToken);
        }

        public async Task SaveAsync()
        {
            await BaseSaveAsync();
        }
    }
}