using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class CalendrierPatientDataModel
    {
        public IList<string> HeureCalendrier { get; set; }
        public string Jour { get; set; }
        public string DateCourante { get; set; } //sans afficher l'année


    }
}
