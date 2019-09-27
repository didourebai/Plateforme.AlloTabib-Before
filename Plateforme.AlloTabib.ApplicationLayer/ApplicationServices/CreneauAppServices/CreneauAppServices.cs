using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.CreneauDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CreneauAppServices
{
    public class CreneauAppServices : ICreneauAppServices
    {

        #region Private Properties

        private readonly ICreneauDomainServices _creneauDomainServices;

        #endregion

        public CreneauAppServices(ICreneauDomainServices creneauDomainServices)
        {
            if (creneauDomainServices == null)
               throw new ArgumentNullException("creneauDomainServices");

            _creneauDomainServices = creneauDomainServices;
        }

        public ResultOfType<IList<CreneauResultDataModel>> PostNewCreneau(IList<CreneauDTO> creneauToAdd)
        {
            IList<CreneauDataModel> creneaux = new List<CreneauDataModel>();
                creneauToAdd.ToList().ForEach(cre=>
                creneaux.Add(EntitiesFactory.ConvertToJourFerieDataModel(cre))
                );

                return _creneauDomainServices.PostNewCreneau(creneaux);
        }

        public ResultOfType<CreneauResultModel> GetCreneauxByPraticien(string cin, int take = 0, int skip = 0)
        {
            return _creneauDomainServices.GetCreneauxByPraticien(cin,take,skip);
        }

        public ResultOfType<CreneauResultDataModel> GetCreneauByHeureDebutAndDate(string heureDebut, string praticien, string dateCurrent)
        {
            return _creneauDomainServices.GetCreneauByHeureDebutAndDate(heureDebut,praticien,dateCurrent);
        }

        public ResultOfType<Null> DeleteCreneau(string praticien, string dateCurrent, string heureDebut)
        {
            return _creneauDomainServices.DeleteCreneau(praticien,dateCurrent,heureDebut);
        }

        public ResultOfType<CreneauResultDataModel> UpdateCreneauPraticienByDate(string praticien, string dateCurrent, CreneauDataModel creneau)
        {
            return _creneauDomainServices.UpdateCreneauPraticienByDate(praticien, dateCurrent, creneau);
        }


        public ResultOfType<Null> PostCreneaux(string from, string to, string cinPraticien, string dateSelected)
        {
            return _creneauDomainServices.PostCreneaux(from, to, cinPraticien, dateSelected);
        }


        public ResultOfType<Null> PostCreneauxJour(string from, string to, string cinPraticien, string jour)
        {
            return _creneauDomainServices.PostCreneauxJour(from, to, cinPraticien, jour);
        }
    }
}
