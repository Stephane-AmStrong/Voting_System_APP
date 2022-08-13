using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries.GetById
{
    public class GetCategoryByIdQuery : IRequest<CategoryViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryViewModel>
    {
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var category = await _repository.Category.GetByIdAsync(query.Id);
            if (category == null) throw new ApiException($"Category with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Category with id: {query.Id}");
            return _mapper.Map<CategoryViewModel>(category);
        }
    }
}
