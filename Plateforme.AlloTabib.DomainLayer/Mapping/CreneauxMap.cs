using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class CreneauxMap : ClassMap<Creneaux>
    {
        public CreneauxMap()
        {
            Id(x => x.Id)
               .GeneratedBy
               .Guid().Column("Id");
            Map(x => x.HeureDebut).Column("heureDebut").Not.Nullable();
            Map(x => x.HeureFin).Column("heureFin").Not.Nullable();
            Map(x => x.Status).Column("status").Not.Nullable();
            Map(x => x.CurrentDate).Column("currentDate").Nullable();
            Map(x => x.Commentaire).Column("commentaire").Nullable();
            Map(x => x.DateCreation).Column("dateCreation").Nullable();

            References(x => x.Praticien)
                .Column("praticien_id")
                .Cascade.SaveUpdate();

            Table("creneaux");
        }
    }
}
