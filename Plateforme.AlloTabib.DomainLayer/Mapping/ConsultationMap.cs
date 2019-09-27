using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class ConsultationMap : ClassMap<Consultation>
    {
        public ConsultationMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Label).Column("label").Not.Nullable();
            Map(x => x.Specialite).Column("specialite").Not.Nullable();

            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();

           

            Table("consultation");
        }
    }
}
