using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Queries.ConfirmEmail
{
    public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ConfirmEmailQueryValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
