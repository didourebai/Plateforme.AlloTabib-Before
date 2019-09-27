using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.JourFerieDomainServices
{
    public interface IJourFerieDomainServices
    {
        ResultOfType<JourFerieResultDataModel> PostNewJourFerie(JourFerieDataModel jourFerieDto);
        ResultOfType<Null> DeleteJourFerie(string jourFerieName, string email);
        ResultOfType<JourFerieResultDataModel> EstUnJourFerie(string jourFerieName, string email);
    }
}
