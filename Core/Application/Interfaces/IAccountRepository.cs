using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenticationModel> RegisterAsync(Voter voter, string password, string origin);
        Task<string> ConfirmEmailAsync(string voterId, string code);
        Task<Voter> FindByNameAsync(string userName);
        Task<Voter> FindByEmailAsync(string userEmail);
        Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress);
        Task<string> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task AddToWorkstationAsync(Voter voter, string roleName);
        Task RemoveFromWorkstationAsync(Voter voter, string roleName);
    }
}
