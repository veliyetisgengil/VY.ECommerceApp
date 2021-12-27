using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using VY.Ecommerce.CatalogService.Api.Core.Domain;

namespace VY.Ecommerce.CatalogService.Api.Infastructure.Context
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context,IWebHostEnvironment env,ILogger<CatalogContextSeed> logger)
        {
            var policy = Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount:3,
                    sleepDurationProvider:retry =>TimeSpan.FromSeconds(5),
                    onRetry: (exception,timeSpan,retry,ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {message} detected on attemp {retry} of");
                    }
                );
            var setupDir = Path.Combine(env.ContentRootPath,"Infrastructure","Setup","SeedFiles");
            var picturePath = "Pics";

            await policy.ExecuteAsync(()=> ProcessSeeding(context,setupDir,picturePath,logger));
        }

        private async Task ProcessSeeding(CatalogContext context, string setupDir,string picturePath,ILogger logger)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(GetCatalogBrandFromFile(setupDir));

                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetCatalogTypeFromFile(setupDir));

                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetCatalogItemsFromFile(setupDir,context));

                await context.SaveChangesAsync();

                GetCatalogItemPictures(setupDir, picturePath);
            }

        }
        private IEnumerable<CatalogBrand> GetCatalogBrandFromFile(string contentPath)
        {
            IEnumerable<CatalogBrand> GetPreconfiguredBrands()
            {
                return new List<CatalogBrand>()
                {
                    new CatalogBrand {Brand = "Azure" },
                    new CatalogBrand {Brand = ".Net" },
                    new CatalogBrand {Brand = "Visual Studio" },
                    new CatalogBrand {Brand = "VS Code" },
                };
            }

            string fileName = Path.Combine(contentPath, "BrandsTextFile.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredBrands();
            }

            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i.Trim('"')
            }).Where(i => i != null);
            return list ?? GetPreconfiguredBrands();
        }
        private IEnumerable<CatalogType> GetCatalogTypeFromFile(string contentPath)
        {
            IEnumerable<CatalogType> GetPreconfiguredTypes()
            {
                return new List<CatalogType>()
                {
                    new CatalogType {Type = "Mug" },
                    new CatalogType {Type = "T-Shirt" },
                    new CatalogType {Type = "Sheet" },
                    new CatalogType {Type = "Usb Memory" },
                };
            }

            string fileName = Path.Combine(contentPath, "CatalogTypesTextFile.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredTypes();
            }

            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i=>new CatalogType() { 
            Type =i.Trim('"')
            }).Where(i=>i != null);
            return list ?? GetPreconfiguredTypes();
        }
        private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentPath,CatalogContext context)
        {
            IEnumerable<CatalogItem> GetPreconfiguredItems()
            {
                return new List<CatalogItem>() 
                {
                    new CatalogItem {CatalogTypeId = 2 ,CatalogBrandId =2,Description="test acıklama1",Name="deneme name1"},
                    new CatalogItem {CatalogTypeId = 3 ,CatalogBrandId =3,Description="test acıklama2",Name="deneme name2"},
                    new CatalogItem {CatalogTypeId = 2 ,CatalogBrandId =4,Description="test acıklama3",Name="deneme name3"},
                    new CatalogItem {CatalogTypeId = 3 ,CatalogBrandId =5,Description="test acıklama4",Name="deneme name4"},
                    new CatalogItem {CatalogTypeId = 2 ,CatalogBrandId =4,Description="test acıklama5",Name="deneme name5"},
                    new CatalogItem {CatalogTypeId = 3 ,CatalogBrandId =2,Description="test acıklama6",Name="deneme name6"}
                };
            }

            string fileName = Path.Combine(contentPath,"CatalogItemsTextFile.txt");

            if (!File.Exists(fileName))
            {
                return GetPreconfiguredItems();
            }

            var catalogTypeIdLookUp = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
            var catalogBrandIdLookUp = context.CatalogBrands.ToDictionary(ct => ct.Brand, ct => ct.Id);

            var fileContent = File.ReadAllLines(fileName)
                .Skip(1)
                .Select(i => i.Split(','))
                .Select(i => new CatalogItem
                {
                    CatalogTypeId = catalogTypeIdLookUp[i[0]],
                    CatalogBrandId = catalogBrandIdLookUp[i[1]],
                    Description = i[2].Trim('"').Trim(),
                    Name = i[3].Trim('"').Trim(),
                    Price = Decimal.Parse(i[4].Trim('"').Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                    PictureFileName = i[5].Trim('"').Trim(),
                }) ;
            return fileContent;
        }
        private void GetCatalogItemPictures(string contentPath, string picturePath)
        {
            picturePath ??= "pics";

            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileCatalogItemPictures = Path.Combine(contentPath, "CatalogItems.zip");
                ZipFile.ExtractToDirectory(zipFileCatalogItemPictures,picturePath);
            }
        }

    }
}
