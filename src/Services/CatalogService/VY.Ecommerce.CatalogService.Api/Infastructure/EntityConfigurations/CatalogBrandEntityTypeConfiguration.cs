using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.Ecommerce.CatalogService.Api.Core.Domain;
using VY.Ecommerce.CatalogService.Api.Infastructure.Context;

namespace VY.Ecommerce.CatalogService.Api.Infastructure.EntityConfigurations
{
    class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand", CatalogContext.DEFAULT_SCHEMA);

            builder.HasKey(ci=>ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_brand_hilo")
                .IsRequired();

            builder.Property(ci => ci.Brand)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
