using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public  static class RendezVousWrapper
    {
        public static RendezVousResultDataModel ConvertPatientEntityToDataModel(RendezVous rendezVous)
        {
            if (rendezVous == null)
                return null;

            var rendezVousResultDataModel = new RendezVousResultDataModel();
            if (!string.IsNullOrEmpty(rendezVous.NoteConsultation))
                rendezVousResultDataModel.NoteConsultation = rendezVous.NoteConsultation;
            if (!string.IsNullOrEmpty(rendezVous.Statut))
                rendezVousResultDataModel.Statut = rendezVous.Statut;

            if (rendezVous.Patient != null)
                rendezVousResultDataModel.PatientCin = rendezVous.Patient.Email;
            
            if (rendezVous.Praticien != null)
                rendezVousResultDataModel.PraticienEmail = rendezVous.Praticien.Email;
            if (rendezVous.Creneaux != null)
            {
                rendezVousResultDataModel.CreneauId = rendezVous.Creneaux.Id.ToString();
                rendezVousResultDataModel.CurrentDate = rendezVous.Creneaux.CurrentDate;
                rendezVousResultDataModel.HeureDebut = rendezVous.Creneaux.HeureDebut;
                rendezVousResultDataModel.HeureFin = rendezVous.Creneaux.HeureFin;
            }
            
            return rendezVousResultDataModel;
        }

        public static RdvResultDataModel ConvertPatientEntityToRdvResultModel(RendezVous rendezVous)
        {
            if (rendezVous == null)
                return null;

            var rendezVousResultDataModel = new RdvResultDataModel();
            if (rendezVous.Praticien != null)
            {
                if (!string.IsNullOrEmpty(rendezVous.Praticien.NomPrenom))
                    rendezVousResultDataModel.PraticienNomPrenom = rendezVous.NoteConsultation;
                if (!string.IsNullOrEmpty(rendezVous.Praticien.Adresse) &&
                    !string.IsNullOrEmpty(rendezVous.Praticien.Gouvernerat)&& !string.IsNullOrEmpty(rendezVous.Praticien.Delegation))
                {
                    rendezVousResultDataModel.PraticienAdresse = string.Format("{0} {1} {2}",
                        rendezVous.Praticien.Adresse, rendezVous.Praticien.Delegation, rendezVous.Praticien.Gouvernerat);
                }
            }
           
            if (!string.IsNullOrEmpty(rendezVous.Statut))
                rendezVousResultDataModel.Statut = rendezVous.Statut;
            if (rendezVous.Creneaux != null)
            {
                rendezVousResultDataModel.CurrentDate = rendezVous.Creneaux.CurrentDate;
                rendezVousResultDataModel.HeureDebut = rendezVous.Creneaux.HeureDebut;
            }

            return rendezVousResultDataModel;
        }
    }
}
