using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Voters.Commands.Create
{
    public class CreateVoterCommandValidator : AbstractValidator<CreateVoterCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CreateVoterCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;


            RuleFor(p => p.FirstName)
                 .NotEmpty().WithMessage("{PropertyName} is required.")
                 .NotNull()
                 .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 charcategoryrs.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 charcategoryrs.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress();

            RuleFor(p => p.Birthday)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidDate).WithMessage("{PropertyName} is invalid.")
                .Must(BeOver18).WithMessage("You must be over age of 18 to vote.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Equal(p => p.Password).WithMessage("The password and confirmation password do not match.");
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
    }
}
