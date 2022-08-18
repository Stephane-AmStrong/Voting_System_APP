using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.AddToCategory
{
    public class AddCandidateToCategoryCommandValidator : AbstractValidator<AddCandidateToCategoryCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AddCandidateToCategoryCommandValidator(IRepositoryWrapper repository, IMapper mapper)
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
    }
}
