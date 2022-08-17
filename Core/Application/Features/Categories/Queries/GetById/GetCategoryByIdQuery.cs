using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
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
        private readonly Nest.ElasticClient _nestClient;

        public GetCategoryByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _nestClient = nestClient;
        }

        public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var results = _nestClient.Search<Category>(s => 
                s.Index("Categories").Query(q => q.MatchAll()));






            var category = await _repository.Category.GetByIdAsync(query.Id);
            if (category == null) throw new ApiException($"Category with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Category with id: {query.Id}");
            return _mapper.Map<CategoryViewModel>(category);
        }
    }
}
