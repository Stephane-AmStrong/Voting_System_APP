using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Account { get; }
        ICandidateRepository Candidate { get; }
        ICategoryRepository Category { get; }
        IVoteRepository Vote { get; }
        IVoterRepository Voter { get; }

        //Services
        IEmailService Email { get; }
        ITokenService Token { get; }

        Task SaveAsync();
    }
}
