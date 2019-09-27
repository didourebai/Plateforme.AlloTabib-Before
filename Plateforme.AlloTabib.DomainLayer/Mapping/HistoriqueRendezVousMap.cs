using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class HistoriqueRendezVousMap : ClassMap<HistoriqueRendezVous>
    {
        public HistoriqueRendezVousMap()
        {
            Id(x => x.Id)
                .GeneratedBy
                .Guid().Column("Id");

            Map(x => x.CurrentDate).Column("noteconsultation").Nullable();
            Map(x => x.Statut).Column("statut").Not.Nullable();
            Map(x => x.CreneauxHeureDebut).Column("heureDebutCreneau").Not.Nullable();
            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();

            //définir les créneaux nécessaires



            References(x => x.Patient)
                .Column("patient_id")
                .Cascade.SaveUpdate();

            References(x => x.Praticien)
                .Column("praticien_id")
                .Cascade.SaveUpdate();

            Table("historiqueRendezVous");
        }
    }
}
