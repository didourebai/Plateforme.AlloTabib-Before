using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class RdvResultModel
    {
        public List<RdvResultDataModel> Items { get; set; }
      
        public RdvResultModel()
        {
            Items = new List<RdvResultDataModel>();
          
        }
    }
}
