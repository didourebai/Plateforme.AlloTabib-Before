using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.JourFerieAppServices
{
    public interface IJourFerieAppServices
    {
        ResultOfType<JourFerieResultDataModel> PostNewJourFerie(JourFerieDTO jourFerieDto);
        ResultOfType<Null> DeleteJourFerie(string jourFerieId, string email);
        ResultOfType<JourFerieResultDataModel> EstUnJourFerie(string jourFerieName, string email);
    }
}
