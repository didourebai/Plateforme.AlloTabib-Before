using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
   public class UserAccountResultModel
    {
       public List<UserAccountDataModel> Items { get; set; }

       public UserAccountResultModel()
       {
           Items = new List<UserAccountDataModel>();
       }
    }
}
