using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class CalendrierMap : ClassMap<Calendrier>
    {
        public CalendrierMap()
        {
            Id(x => x.Id)
              .GeneratedBy
              .Guid().Column("Id");
            Map(x => x.DateDebutDisponibilite).Column("datedebutdisponibilite").Not.Nullable();
            Map(x => x.DureeDecalageEntreDeuxRdvMinutes).Column("decalage").Not.Nullable();
            Map(x => x.TempsDebut).Column("tempsdebut").Not.Nullable();
            Map(x => x.TempsFin).Column("tempsfin").Not.Nullable();

            References(x => x.Praticien)
            .Column("praticien_id")
            .Cascade.SaveUpdate();

            Table("calendrier");
        }
    }
}
