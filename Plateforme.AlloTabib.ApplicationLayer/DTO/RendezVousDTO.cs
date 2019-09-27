using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.ApplicationLayer.DTO
{
    public class RendezVousDTO
    {
        public string NoteConsultation { get; set; }
        public string Statut { get; set; }
        public string CreneauId { get; set; }
        public string PatientCin { get; set; }
        public string PraticienCin { get; set; }
        public string CurrentDate { get; set; }
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }
        public string Id { get; set; }
    }
}
