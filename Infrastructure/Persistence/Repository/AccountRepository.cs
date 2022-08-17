using Application.Models;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Persistence.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Enums;

namespace Persistence.Repository
{
    public class AccountRepository : TokenRepository, IAccountRepository
    {
        private readonly UserManager<Voter> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;


        public AccountRepository
        (
            AppDbContext identityContext,
            UserManager<Voter> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings
        ) : base(identityContext, userManager, roleManager, jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }



        public async Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress)
        {
            var voter = await _userManager.FindByNameAsync(loginModel.Email);
            if (voter == null) throw new ApiException($"No Accounts Registered with {loginModel.Email}.");

            var authenticationSucceeded = await _userManager.CheckPasswordAsync(voter, loginModel.Password);
            if (!authenticationSucceeded) throw new ApiException($"Invalid Credentials for '{loginModel.Email}'.");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(voter);
            if (!isEmailConfirmed) throw new ApiException($"Account Not Confirmed for '{loginModel.Email}'.");

            var jwtSecurityToken = await GenerateJWToken(voter);
            var userToken = await GenerateRefreshTokenAsync(ipAddress, voter.Id);

            return new AuthenticationModel
            {
                Voter = voter,

                AccessToken = new AccessToken
                {
                    Value = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpiryDate = jwtSecurityToken.ValidTo
                },

                RefreshToken = userToken,
                IsSuccess = true
            };
        }



        public async Task<AuthenticationModel> RegisterAsync(Voter voter, string password, string origin)
        {
            voter.UserName = voter.Email;

            var voterWithSameUserName = await _userManager.FindByNameAsync(voter.UserName);
            if (voterWithSameUserName != null) throw new ApiException($"Username '{voter.UserName}' is already taken.");

            var userWithSameEmail = await _userManager.FindByEmailAsync(voter.Email);
            if (userWithSameEmail != null) throw new ApiException($"Email {voter.Email } is already registered.");

            var result = await _userManager.CreateAsync(voter, password);
            var errorMessage = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errorMessage.Append(error.Description);
            }

            if (!result.Succeeded) throw new ApiException($"{errorMessage}");
            //grant to the registered user all the claim of a voter
            await _userManager.AddToRoleAsync(voter, EnumRole.Voter.ToString());

            var emailConfirmationToken = await GenerateEmailConfirmationTokenAsync(voter, origin);

            return new AuthenticationModel
            {
                Voter = voter,
                AccessToken = new AccessToken
                {
                    Value = emailConfirmationToken,
                },
                IsSuccess = true
            };
        }



        private async Task<string> GenerateEmailConfirmationTokenAsync(Voter voter, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(voter);
            code = await EncodeStringAsync(code);
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailVerificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", voter.Id);
            emailVerificationUri = QueryHelpers.AddQueryString(emailVerificationUri, "code", code);

            return emailVerificationUri;
        }



        public async Task<string> ConfirmEmailAsync(string voterId, string code)
        {
            var voter = await _userManager.FindByIdAsync(voterId);
            code = await DecodeStringAsync(code);
            var result = await _userManager.ConfirmEmailAsync(voter, code);

            if (!result.Succeeded) throw new ApiException($"An error occured while confirming {voter.Email}.");

            return $"congratulations, your email: {voter.UserName} has been validated. You can login to your account";
        }



        public async Task<string> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var account = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {resetPasswordRequest.Email}.");

            //décode token
            var decodedToken = await Task.Run(() => WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));
            resetPasswordRequest.Token = await Task.Run(() => Encoding.UTF8.GetString(decodedToken));

            var result = await _userManager.ResetPasswordAsync(account, resetPasswordRequest.Token, resetPasswordRequest.Password);

            if (!result.Succeeded) throw new ApiException($"Error occured while reseting the password.");
            return $"{resetPasswordRequest.Email}, message: Password Resetted.";
        }



        public async Task AddToWorkstationAsync(Voter voter, string roleName)
        {
            await _userManager.AddToRoleAsync(voter, roleName);
        }


        public async Task RemoveFromWorkstationAsync(Voter voter, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(voter, roleName);
        }

        public async Task<Voter> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<Voter> FindByEmailAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }
    }
}
