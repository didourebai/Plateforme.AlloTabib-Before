using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.RendezVousAppServices
{
    public interface IRendezVousAppServices
    {
        ResultOfType<RendezVousResultDataModel> PostNewRendezVous(RendezVousDTO rendezVousDto);
        ResultOfType<RendezVousResultModel> GetRendezVousByDateAndPraticien(string praticien, string dateCurrent);
        ResultOfType<RendezVousResultDataModel> PatchNewRendezVous(RendezVousDTO rendezVousDto);
        ResultOfType<Null> DeleteRendezVous(string rendezVousId);
        ResultOfType<PatientResultModel> GetPatientsParPraticien(string praticien);
        ResultOfType<RendezVousResultDataModel> CreneauAyantRendezVous(string praticien, string dateCurrent, string heureDebut);
        ResultOfType<RdvResultModel> GetAllRendezVousParPatientEnCours(string patientEmail);
        ResultOfType<RendezVousResultModel> GetAllRendezVousNonConfirmeOuRejete(string praticienEmail);

    }
}
