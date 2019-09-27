using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.JourFerieDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.JourFerieAppServices
{
    public class JourFerieAppServices : IJourFerieAppServices
    {
        #region Private Properties

        private readonly IJourFerieDomainServices _jourFerieDomainServices;

        #endregion

        public JourFerieAppServices(IJourFerieDomainServices jourFerieDomainServices)
        {
            if (jourFerieDomainServices == null)
                throw new ArgumentNullException("jourFerieDomainServices");

            _jourFerieDomainServices = jourFerieDomainServices;
        }

        public ResultOfType<JourFerieResultDataModel> PostNewJourFerie(JourFerieDTO jourFerieDto)
        {
            return _jourFerieDomainServices.PostNewJourFerie(EntitiesFactory.ConvertToJourFerieResultDataModel(jourFerieDto));
        }

        public ResultOfType<Null> DeleteJourFerie(string jourferieId, string email)
        {
            return _jourFerieDomainServices.DeleteJourFerie(jourferieId,email);
        }


        public ResultOfType<JourFerieResultDataModel> EstUnJourFerie(string jourFerieName, string email)
        {
            return _jourFerieDomainServices.EstUnJourFerie(jourFerieName, email);
        }
    }
}
