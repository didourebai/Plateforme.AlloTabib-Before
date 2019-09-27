using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class UserAccountMap : ClassMap<UserAccount>
    {

        public UserAccountMap()
        {
            Id(x => x.Email);

            Map(x => x.Password).Column("password").Not.Nullable();
            Map(x => x.Type).Column("type").Not.Nullable();
            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();
            Map(x => x.EstActive).Column("estactive").Not.Nullable();

            Table("account");
        }
    }
}
