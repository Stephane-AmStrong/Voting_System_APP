using Application.Exceptions;
using Application.Features.Candidates.Queries.GetById;
using Application.Features.Categories.Queries.GetById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Candidates.Commands.AddToCategory
{
    public class AddCandidateToCategoryCommand : IRequest<CandidateViewModel>
    {
        public string CandidateId { get; set; }
        public string CategoryId { get; set; }
    }

    internal class CreateCategoryCommandHandler : IRequestHandler<AddCandidateToCategoryCommand, CandidateViewModel>
    {
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateCategoryCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<CandidateViewModel> Handle(AddCandidateToCategoryCommand command, CancellationToken cancellationToken)
        {
            var candidateEntity = await _repository.Candidate.GetByIdAsync(command.CandidateId);
            if (candidateEntity == null) throw new ApiException($"Candidate with id: {command.CandidateId}, hasn't been found.");

            var categoryEntity = await _repository.Category.GetByIdAsync(command.CategoryId);
            if (categoryEntity == null) throw new ApiException($"Category with id: {command.CategoryId}, hasn't been found.");

            candidateEntity.CategoryId = categoryEntity.Id;
            await _repository.Candidate.UpdateAsync(candidateEntity);
            await _repository.SaveAsync();

            return _mapper.Map<CandidateViewModel>(candidateEntity);
        }
    }
}
