using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.ConfigurationDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ConfigurationAppServices
{
    public class ConfigurationAppServices : IConfigurationAppServices
    {
        #region Private Properties

        private readonly IConfigurationDomainServices _configurationDomainServices;

        #endregion

        public ConfigurationAppServices(IConfigurationDomainServices configurationDomainServices)
        {
            if (configurationDomainServices == null)
                throw new ArgumentNullException("configurationDomainServices");
            _configurationDomainServices = configurationDomainServices;
        }

        public ResultOfType<ConfigurationResultDataModel> PostNewConfiguration(ConfigurationPraticienDto configuration)
        {
            return
                _configurationDomainServices.PostNewConfiguration(
                    EntitiesFactory.ConvertToConfigurationDataModel(configuration));
        }

        public ResultOfType<ConfigurationResultDataModel> GetConfigurationByPraticien(string praticien)
        {
            return _configurationDomainServices.GetConfigurationByPraticien(praticien);
        }


        public ResultOfType<IList<string>> AjouterDimancheFerie(string cinPraticien)
        {
            return _configurationDomainServices.AjouterDimancheFerie(cinPraticien);
        }


        public ResultOfType<IList<string>> AjouterSamediFerie(string cinPraticien)
        {
            return _configurationDomainServices.AjouterSamediFerie(cinPraticien);
        }


        public ResultOfType<IList<string>> AjouterFerie(string cinPraticien, string jour)
        {
            return _configurationDomainServices.AjouterFerie(cinPraticien, jour);
        }
    }
}
