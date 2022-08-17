using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.ChangeAge
{
    public class ChangeAgeVoterCommandValidator : AbstractValidator<ChangeAgeVoterCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ChangeAgeVoterCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;


            RuleFor(p => Guid.Parse(p.Id))
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is not valid.");

            RuleFor(p => p.Birthday)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidDate).WithMessage("{PropertyName} is invalid.")
                .Must(BeOver18).WithMessage("You must be over age of 18 to vote.");
        }



        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime)) && date < DateTime.Now;
        }


        private bool BeOver18(DateTime givenDate)
        {
            var age = DateTime.Today.Year - givenDate.Year;
            return age > 18;
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(ChangeAgeVoterCommand voterCommand, CancellationToken cancellationToken)
        {
            var voter = _mapper.Map<Voter>(voterCommand);
            return !await _repository.Voter.ExistAsync(voter);
        }
    }
}
