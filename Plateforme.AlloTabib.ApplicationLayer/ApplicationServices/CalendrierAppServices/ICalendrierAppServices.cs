using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CalendrierAppServices
{
    public interface ICalendrierAppServices
    {
        ResultOfType<CalendrierResultModel> GetCalendrierParPraticienAndDate(
            ConfigurationPraticien configurationPraticien, DateTime? dateSelected);
        ResultOfType<CalendrierPatientDataModel> GetCalendrierParPraticienForPatient(string praticien, string dateSelected);
        ResultOfType<CalendrierResultModel> GetCalendrierParPraticienForPraticien(string praticien, string dateSelected);

        ResultOfType<IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSem(
            string praticien,
            string dateSelected);

        ResultOfType<IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSemaine(
            string praticien,
            string dateSelected);

        ResultOfType<CalendarPraticienByDay> GetPremiereDisponibilite(string praticienEmail, string dateSelected);
    }
}
