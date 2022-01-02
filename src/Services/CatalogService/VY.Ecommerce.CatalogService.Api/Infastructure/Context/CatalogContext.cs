using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.Ecommerce.CatalogService.Api.Core.Domain;
using VY.Ecommerce.CatalogService.Api.Infastructure.EntityConfigurations;

namespace VY.Ecommerce.CatalogService.Api.Infastructure.Context
{
    public class CatalogContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "catalog";

        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        public DbSet<CatalogItem> CatalogItems {get;set;}
        public DbSet<CatalogBrand> CatalogBrands {get;set;}
        public DbSet<CatalogType> CatalogTypes {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());

        }
    }
}
