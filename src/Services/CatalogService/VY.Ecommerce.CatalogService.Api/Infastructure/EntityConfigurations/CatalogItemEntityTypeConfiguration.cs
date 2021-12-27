﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.Ecommerce.CatalogService.Api.Core.Domain;
using VY.Ecommerce.CatalogService.Api.Infastructure.Context;

namespace VY.Ecommerce.CatalogService.Api.Infastructure.EntityConfigurations
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog", CatalogContext.DEFAULT_SCHEMA);

            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci=>ci.Price)
                .IsRequired(true);

            builder.Property(ci=>ci.PictureFileName)
                .IsRequired(false);

            builder.Ignore(ci=>ci.PictureUrl);

            builder.HasOne(ci => ci.CatalogBrand)
                .WithMany()
                .HasForeignKey(ci=>ci.CatalogBrandId);

            builder.HasOne(ci => ci.CatalogType)
              .WithMany()
              .HasForeignKey(ci => ci.CatalogTypeId);
        }
    }
}
