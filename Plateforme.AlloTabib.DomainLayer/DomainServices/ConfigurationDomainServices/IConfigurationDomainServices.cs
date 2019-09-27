using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.ConfigurationDomainServices
{
    public interface IConfigurationDomainServices
    {
        ResultOfType<ConfigurationResultDataModel> PostNewConfiguration(ConfigurationDataModel configaration);
        ResultOfType<ConfigurationResultDataModel> GetConfigurationByPraticien(string praticien);
        ResultOfType<IList<string>> AjouterDimancheFerie(string cinPraticien);
        ResultOfType<IList<string>> AjouterSamediFerie(string cinPraticien);
        ResultOfType<IList<string>> AjouterFerie(string cinPraticien, string jour);
    }
}
