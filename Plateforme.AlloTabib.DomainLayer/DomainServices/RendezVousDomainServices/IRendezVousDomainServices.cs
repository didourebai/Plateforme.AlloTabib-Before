using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.RendezVousDomainServices
{
    public interface IRendezVousDomainServices
    {
        ResultOfType<RendezVousResultDataModel> PostNewRendezVous(RendezVousDataModel rendezVousDto);
        ResultOfType<RendezVousResultModel> GetRendezVousByDateAndPraticien(string praticien, string dateCurrent);
        ResultOfType<RendezVousResultDataModel> PatchNewRendezVous(RendezVousDataModel rendezVousToAdd);
        ResultOfType<Null> DeleteRendezVous(string rendezVousId);
        ResultOfType<PatientResultModel> GetPatientsParPraticien(string praticien, int take = 0, int skip = 0);
        ResultOfType<RendezVousResultDataModel> CreneauAyantRendezVous(string praticien, string dateCurrent, string heureDebut);
        ResultOfType<RdvResultModel> GetAllRendezVousParPatientEnCours(string patientEmail);
        ResultOfType<RendezVousResultModel> GetAllRendezVousNonConfirmeOuRejete(string praticienEmail);

    }
}
