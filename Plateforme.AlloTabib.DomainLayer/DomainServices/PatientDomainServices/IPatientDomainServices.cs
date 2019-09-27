using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.PatientDomainServices
{
    public interface IPatientDomainServices
    {
        ResultOfType<PatientResultModel> GetPatients(int take = 0, int skip = 0);
        IEnumerable<Patient> GetAll();
        void AddNewPatient(Patient patient);
        void ModifyPatient(Patient patient);
        void DeletePatient(object id);


        ResultOfType<PatientResultDataModel> PostNewPatient(PatientDataModel patient);
        ResultOfType<PatientResultDataModel> DeleteAPatient(string cin);
        ResultOfType<PatientResultDataModel> GetPatientByEmail(string email);
        ResultOfType<PatientResultDataModel> PatchPatient(PatientDataModel patient);
    }
}
