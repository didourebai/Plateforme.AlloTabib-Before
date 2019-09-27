using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class PraticienResultModel
    {
        public List<PraticienResultDataModel> Items { get; set; }
        public PaginationHeader PaginationHeader { get; set; }

        public PraticienResultModel()
        {
            Items = new List<PraticienResultDataModel>();
            PaginationHeader = new PaginationHeader();
        }
    }
}
