using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class CreneauWrapper
    {
        public static CreneauResultDataModel ConvertCreneauEntityToDataModel(Creneaux creneau)
        {
            if (creneau == null)
                return null;
            var creneauResultDataModel = new CreneauResultDataModel();
            if (!string.IsNullOrEmpty(creneau.HeureDebut))
                creneauResultDataModel.HeureDebut = creneau.HeureDebut;

            if (!string.IsNullOrEmpty(creneau.HeureFin))
                creneauResultDataModel.HeureFin = creneau.HeureFin;

            if (!string.IsNullOrEmpty(creneau.Status))
                creneauResultDataModel.Status = creneau.Status;

            if (creneau.Praticien !=null && !string.IsNullOrEmpty(creneau.Praticien.Cin))
                creneauResultDataModel.PraticienCin = creneau.Praticien.Cin;
            if (!string.IsNullOrEmpty(creneau.CurrentDate))
                creneauResultDataModel.CurrentDate = creneau.CurrentDate;
            if (!string.IsNullOrEmpty(creneau.Commentaire))
                creneauResultDataModel.Commentaire = creneau.Commentaire;
            return creneauResultDataModel;
        }
    }
}
