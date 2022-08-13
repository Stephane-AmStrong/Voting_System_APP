using Application.Enums;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries.GetPagedList
{
    public class GetCategoriesQuery : QueryParameters, IRequest<PagedListResponse<CategoriesViewModel>>
    {
        public GetCategoriesQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedListResponse<CategoriesViewModel>>
    {
        private readonly ILogger<GetCategoriesQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetCategoriesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<CategoriesViewModel>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var categories = await _repository.Category.GetPagedListAsync(query);
            var categoriesViewModel = _mapper.Map<List<CategoriesViewModel>>(categories);
            _logger.LogInformation($"Returned Paged List of Categories from database.");
            return new PagedListResponse<CategoriesViewModel>(categoriesViewModel, categories.MetaData);
        }
    }
}
