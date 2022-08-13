using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class PagedListResponse<T>
    {
        public MetaData MetaData { get; set; }
        public List<T> PagedList { get; set; }
        
        public PagedListResponse(List<T> pagedList, MetaData metaData)
        {
            MetaData = new MetaData
            {
                TotalCount = metaData.TotalCount,
                PageSize = metaData.PageSize,
                CurrentPage = metaData.CurrentPage,
                TotalPages = metaData.TotalPages
            };

            PagedList = pagedList;
        }
    }
}
