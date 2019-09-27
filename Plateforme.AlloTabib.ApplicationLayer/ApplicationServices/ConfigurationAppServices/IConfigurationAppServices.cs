using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ConfigurationAppServices
{
    public interface IConfigurationAppServices
    {
        ResultOfType<ConfigurationResultDataModel> PostNewConfiguration(ConfigurationPraticienDto configaration);
        ResultOfType<ConfigurationResultDataModel> GetConfigurationByPraticien(string praticien);
        ResultOfType<IList<string>> AjouterDimancheFerie(string cinPraticien);
        ResultOfType<IList<string>> AjouterSamediFerie(string cinPraticien);
        ResultOfType<IList<string>> AjouterFerie(string cinPraticien, string jour);
    }
}
