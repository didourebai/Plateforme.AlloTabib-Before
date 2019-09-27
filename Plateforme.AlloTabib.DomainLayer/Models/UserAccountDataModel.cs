using System;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class UserAccountDataModel
    {
        public string Password { get; set; }
        public  string Email { get; set; }
        //Type : doctor or patient
        public  string Type { get; set; }
        public  DateTime? CreationDate { get; set; }
        public  DateTime LastModificationDate { get; set; }
    }

}
