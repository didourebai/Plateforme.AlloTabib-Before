using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class MapCoordinationsMap : ClassMap<GoogleMapsCoordinations>
    {
        public MapCoordinationsMap()
        {
            Id(x => x.Id)
                .GeneratedBy
                .Guid().Column("Id");
            Map(x => x.FormattedAddress).Column("adresseformatte").Not.Nullable();
            Map(x => x.Latitude).Column("latitude").Not.Nullable();
            Map(x => x.Longitude).Column("longitude").Not.Nullable();

            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();

            References(x => x.UserAccount)
                .Column("userAccount_id")
                .Cascade.SaveUpdate();

            Table("mapcoordinations");
        }
    }
}
