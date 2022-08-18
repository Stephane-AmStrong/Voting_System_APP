using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Votes.Commands.Create
{
    public class CreateVoteCommandValidator : AbstractValidator<CreateVoteCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateVoteCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;


            RuleFor(p => p.CandidateId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(CandidateMustExist).WithMessage("Candidate with id: {PropertyName}, hasn't been found.");
            
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(CategoryMustExist).WithMessage("Category with id: {PropertyName}, hasn't been found.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.")
                .MustAsync(VoterBelongsToCategoryAsync).WithMessage("This candidate does not belong to this category.");

        }

        private async Task<bool> IsUnique(CreateVoteCommand voteCommand, CancellationToken cancellationToken)
        {
            var vote = _mapper.Map<Vote>(voteCommand);
            return !await _repository.Vote.ExistAsync(vote);
        }

        private async Task<bool> CandidateMustExist(string id, CancellationToken cancellationToken)
        {
            var candidate = await _repository.Candidate.GetByIdAsync(id);
            return candidate != null;
        }

        private async Task<bool> CategoryMustExist(string id, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetByIdAsync(id);
            return category != null;
        }

        private async Task<bool> VoterBelongsToCategoryAsync(CreateVoteCommand voteCommand, CancellationToken cancellationToken)
        {
            if (!await CandidateMustExist(voteCommand.CandidateId, cancellationToken) || !await CategoryMustExist(voteCommand.CategoryId, cancellationToken)) return false;

            var category = await _repository.Category.GetByIdAsync(voteCommand.CategoryId);
            return category.Candidates.Any(x => x.Id == voteCommand.CandidateId);
        }
    }
}
