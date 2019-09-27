namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class RendezVousResultDataModel
    {
        public string NoteConsultation { get; set; }
        public string Statut { get; set; }
        public string CreneauId { get; set; }
        public string PatientCin { get; set; }
        public string PatientNomPrenom { get; set; }
        public string PatientTelephone { get; set; }
        public string PatientDateNaissance { get; set; }
        public string PatientAdresse { get; set; }


        public string PraticienEmail { get; set; }
        public string CurrentDate { get; set; }
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }

        //Information lié au praticien pour envoyer l'email :)

        public string PraticinNomPrenom { get; set; }
        public string PraticienSpecialite { get; set; }
        public string PraticienAdresseDetaille { get; set; }
        public string PraticienTelephone { get; set; }

        
    }
}
