using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class RendezVousResultModel
    {
       public List<RendezVousResultDataModel> Items { get; set; }
        public PaginationHeader PaginationHeader { get; set; }

        public RendezVousResultModel()
        {
            Items = new List<RendezVousResultDataModel>();
            PaginationHeader = new PaginationHeader();
        }
    }
}
