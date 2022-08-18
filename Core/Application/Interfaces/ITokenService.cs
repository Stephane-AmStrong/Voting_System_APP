using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> EncodeStringAsync(string stringToEncode);
        Task<string> DecodeStringAsync(string stringToDecode);

        Task<AuthenticationModel> GeneratePasswordResetTokenAsync(string email);
        Task<JwtSecurityToken> GenerateJWToken(Voter appUser);
        Task<UserToken> GenerateRefreshTokenAsync(string ipAddress, string userId);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<RefreshTokens> RefreshAsync(string accessToken, string refreshToken, string ipAddress);


        Task<UserToken> CommitAsync(UserToken refreshToken);
        Task<UserToken> GetByIdAsync(string Id);
        Task<UserToken> GetByUserIdAsync(string userId);

        Task CreateAsync(UserToken refreshToken);
        Task UpdateAsync(UserToken refreshToken);
        Task DeleteAsync(UserToken refreshToken);

        Task SaveAsync();
    }
}
