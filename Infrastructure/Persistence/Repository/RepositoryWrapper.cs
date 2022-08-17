using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Settings;
using Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Persistence.Service;

namespace Persistence.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private IEmailService _email;

        private readonly ISortHelper<Candidate> _candidateSortHelper;
        private readonly ISortHelper<Category> _categorySortHelper;
        private readonly ISortHelper<Vote> _voteSortHelper;
        private readonly ISortHelper<Voter> _voterSortHelper;

        private readonly IOptions<EmailSettings> _mailSettings;
        private readonly IOptions<JWTSettings> _jwtSettings;

        private readonly UserManager<Voter> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly AppDbContext _appDbContext;

        private IAccountRepository _account;
        private ICandidateRepository _candidate;
        private ICategoryRepository _category;
        private IVoterRepository _voter;
        private IVoteRepository _vote;
        private ITokenService _token;

        private string filePath;

        public string Path
        {
            set { filePath = value; }
        }


        public IEmailService Email
        {
            get
            {
                if (_email == null)
                {
                    _email = new EmailService(_mailSettings);
                }
                return _email;
            }
        }


        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_appDbContext, _userManager, _roleManager, _jwtSettings);
                }
                return _account;
            }
        }


        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_appDbContext, _categorySortHelper);
                }
                return _category;
            }
        }


        public IVoteRepository Vote
        {
            get
            {
                if (_vote == null)
                {
                    _vote = new VoteRepository(_appDbContext, _voteSortHelper);
                }
                return _vote;
            }
        }


        public ICandidateRepository Candidate
        {
            get
            {
                if (_candidate == null)
                {
                    _candidate = new CandidateRepository(_appDbContext, _candidateSortHelper);
                }
                return _candidate;
            }
        }


        public ITokenService Token
        {
            get
            {
                if (_token == null)
                {
                    _token = new TokenRepository(_appDbContext, _userManager, _roleManager, _jwtSettings);
                }
                return _token;
            }
        }


        public IVoterRepository Voter
        {
            get
            {
                if (_voter == null)
                {
                    _voter = new VoterRepository(_appDbContext, _userManager, _voterSortHelper);
                }
                return _voter;
            }
        }


        public RepositoryWrapper(
            UserManager<Voter> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IOptions<EmailSettings> mailSettings,
            IOptions<JWTSettings> jwtSettings,
            ISortHelper<Candidate> candidateSortHelper,
            ISortHelper<Category> categorySortHelper,
            ISortHelper<Voter> voterSortHelper,
            ISortHelper<Vote> voteSortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mailSettings = mailSettings;
            _jwtSettings = jwtSettings;
            _appDbContext = appDbContext;

            _voterSortHelper = voterSortHelper;
            _categorySortHelper = categorySortHelper;
            _voteSortHelper = voteSortHelper;
            _candidateSortHelper = candidateSortHelper;

            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
