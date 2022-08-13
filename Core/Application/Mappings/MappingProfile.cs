using Application.Models;
using Application.Features.Account.Commands.Authenticate;
using Application.Features.Account.Commands.RefreshAccessToken;
using Application.Features.Account.Commands.ResetPassword;
using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Queries.GetById;
using Application.Features.Categories.Queries.GetPagedList;
using AutoMapper;
using Domain.Entities;
using Application.Features.Account.Commands.RegisterVoter;
using Application.Features.Candidates.Commands.Create;
using Application.Features.Candidates.Queries.GetById;
using Application.Features.Candidates.Queries.GetPagedList;
using Application.Features.Candidates.Commands.Update;
using Application.Features.Votes.Commands.Create;
using Application.Features.Votes.Queries.GetById;
using Application.Features.Votes.Queries.GetPagedList;
using Application.Features.Voters.Commands.Create;
using Application.Features.Voters.Queries.GetById;
using Application.Features.Voters.Queries.GetPagedList;
using Application.Features.Voters.Commands.Update;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Voter, RegisterVoterCommand>().ReverseMap();
            CreateMap<UserToken, UserTokenViewModel>().ReverseMap();
            CreateMap<RefreshTokens, RefreshTokensViewModel>().ReverseMap();
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>().ReverseMap();

            CreateMap<LoginModel, AuthenticationCommand>().ReverseMap();
            CreateMap<AuthenticationModel, AuthenticationViewModel>().ReverseMap();

            CreateMap<Candidate, CreateCandidateCommand>().ReverseMap();
            CreateMap<Candidate, CandidateViewModel>().ReverseMap();
            CreateMap<Candidate, CandidatesViewModel>().ReverseMap();
            CreateMap<Candidate, UpdateCandidateCommand>().ReverseMap();
            
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoriesViewModel>().ReverseMap();

            CreateMap<Vote, CreateVoteCommand>().ReverseMap();
            CreateMap<Vote, VoteViewModel>().ReverseMap();
            CreateMap<Vote, VotesViewModel>().ReverseMap();

            CreateMap<Voter, CreateVoterCommand>().ReverseMap();
            CreateMap<Voter, VoterViewModel>().ReverseMap();
            CreateMap<Voter, VotersViewModel>().ReverseMap();
            CreateMap<Voter, UpdateVoterCommand>().ReverseMap();

        }
    }
}
