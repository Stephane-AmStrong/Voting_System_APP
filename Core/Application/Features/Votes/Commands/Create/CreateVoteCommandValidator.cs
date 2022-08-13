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


            RuleFor(p => p.ItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(CreateVoteCommand voteCommand, CancellationToken cancellationToken)
        {
            var vote = _mapper.Map<Vote>(voteCommand);
            return !(await _repository.Vote.ExistAsync(vote));
        }
    }
}
