using System;
using Plateforme.AlloTabib.DomainLayer.DomainServices.CalendrierDomainServices;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CalendrierAppServices
{
    public class CalendrierAppServices : ICalendrierAppServices
    {
        #region Private Properties

        private readonly ICalendrierDomainServices _calendrierDomainServices;

        #endregion

        #region Constructor

        public CalendrierAppServices(ICalendrierDomainServices calendrierDomainServices)
        {
            if (calendrierDomainServices == null)
                throw new ArgumentNullException("calendrierDomainServices");
            _calendrierDomainServices = calendrierDomainServices;
        }

        #endregion

        public ResultOfType<CalendrierResultModel> GetCalendrierParPraticienAndDate(ConfigurationPraticien configurationPraticien, DateTime? dateSelected)
        {

            return _calendrierDomainServices.GetCalendrierParPraticienAndDate(configurationPraticien, dateSelected);
        }

        public ResultOfType<CalendrierPatientDataModel> GetCalendrierParPraticienForPatient(string praticien, string dateSelected)
        {
            return _calendrierDomainServices.GetCalendrierParPraticienForPatient(praticien, dateSelected);
        }


        public ResultOfType<CalendrierResultModel> GetCalendrierParPraticienForPraticien(string praticien, string dateSelected)
        {
            return _calendrierDomainServices.GetCalendrierParPraticienForPraticien(praticien, dateSelected);
        }


        public ResultOfType<System.Collections.Generic.IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSem(string praticien, string dateSelected)
        {
            return _calendrierDomainServices.GetCalendrierParPraticienForPatientParSem(praticien, dateSelected);
        }


        public ResultOfType<System.Collections.Generic.IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSemaine(string praticien, string dateSelected)
        {
            return _calendrierDomainServices.GetCalendrierParPraticienForPatientParSemaine(praticien, dateSelected);
        }


        public ResultOfType<CalendarPraticienByDay> GetPremiereDisponibilite(string praticienEmail, string dateSelected)
        {
            return _calendrierDomainServices.GetPremiereDisponibilite(praticienEmail, dateSelected);
        }
    }
}
