using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VY.Ecommerce.CatalogService.Api.Core.Application.ViewModels
{
    public class PaginateItemsViewModel<TEntity> where TEntity : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public IEnumerable<TEntity> Data { get; set; }

        public PaginateItemsViewModel(int pageIndex,int pageSize,long count,IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
