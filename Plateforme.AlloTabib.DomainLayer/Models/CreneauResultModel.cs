using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class CreneauResultModel
    {
        public List<CreneauResultDataModel> Items { get; set; }
        public PaginationHeader PaginationHeader { get; set; }

        public CreneauResultModel()
        {
            Items = new List<CreneauResultDataModel>();
            PaginationHeader = new PaginationHeader();
        }
    }
}
