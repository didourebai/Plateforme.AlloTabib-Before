using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PatientAppServices
{
    public interface IPatientApplicationServices
    {
        IEnumerable<Patient> GetAll();
        void AddNewPatient(Patient patient);
        void ModifyPatient(Patient patient);
        void DeletePatient(object id);
        ResultOfType<PatientResultModel> GetPatients(int take = 0, int skip = 0);

        ResultOfType<PatientResultDataModel> PostNewPatient(PatientDTO patientDto);
        ResultOfType<PatientResultDataModel> DeletePatient(string cin);
        ResultOfType<PatientResultDataModel> GetPatientByEmail(string email);
        ResultOfType<PatientResultDataModel> PatchPatient(PatientDTO patient);
    }
}
