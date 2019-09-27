using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class PatientResultModel
    {
        public List<PatientResultDataModel> Items { get; set; }
        public PaginationHeader PaginationHeader { get; set; }
        public PatientResultModel()
        {
            Items = new List<PatientResultDataModel>();
            PaginationHeader = new PaginationHeader();
        }
    }
}
