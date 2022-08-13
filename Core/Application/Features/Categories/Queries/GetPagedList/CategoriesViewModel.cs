using Domain.Common;

namespace Application.Features.Categories.Queries.GetPagedList
{
    public record CategoriesViewModel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
