
using System;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class PatientDataModel
    {

        public string Cin { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NomPrenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Sexe { get; set; }
        public DateTime? CreationDate { get; set; }
        public string DateNaissance { get; set; }
    }
}
