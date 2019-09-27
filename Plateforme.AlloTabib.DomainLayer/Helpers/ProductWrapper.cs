using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class ProductWrapper
    {
        public static ProductResultModel ConvertToProductResultDataModel(Product product)
        {
            if (product == null)
                return null;

            return new ProductResultModel
            {
                Name = product.Name,
                Description = product.Description
            };
        }
    }
}
