using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VY.Ecommerce.CatalogService.Api.Core.Application.ViewModels;
using VY.Ecommerce.CatalogService.Api.Core.Domain;
using VY.Ecommerce.CatalogService.Api.Infastructure;
using VY.Ecommerce.CatalogService.Api.Infastructure.Context;

namespace VY.Ecommerce.CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;

        public CatalogController(CatalogContext catalogContext,IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _settings = settings.Value;

            catalogContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking; 
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginateItemsViewModel<CatalogItem>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0,string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIdsAsync(ids);

                if (!items.Any())
                {
                    return BadRequest("ids value invalid. Must be comma-separeted list of numbers");
                }

                return Ok(items);
            }
            var totalItems = await _catalogContext.CatalogItems.LongCountAsync();

            var itemsOnPage = await _catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginateItemsViewModel<CatalogItem>(pageIndex,pageSize,totalItems,itemsOnPage);

            return Ok(model);
        }

        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id,out int x),Value:x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<CatalogItem>();
            }

            var idsToSelect = numIds
                .Select(id=>id.Value);

            var items = ChangeUriPlaceholder(items);

            return items;
        }
    }
}
