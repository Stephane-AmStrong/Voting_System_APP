﻿using Application.Enums;
using Application.Features.Categories.Queries.GetById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<CategoryViewModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly Nest.ElasticClient _nestClient;
        private readonly IMapper _mapper;


        public CreateCategoryCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger, Nest.ElasticClient nestClient)
        {
            _repository = repository;
            _nestClient = nestClient;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<CategoryViewModel> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(command);

            var response = await _nestClient.IndexAsync(category,
                x => x.Index(EnumElasticIndexes.Categories.ToString())
            );
            category.Id = response.Id;

            await _repository.Category.CreateAsync(category);
            await _repository.SaveAsync();

            return _mapper.Map<CategoryViewModel>(category);
        }
    }
}
