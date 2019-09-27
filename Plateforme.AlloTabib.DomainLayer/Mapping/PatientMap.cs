using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class PatientMap : ClassMap<Patient>
    {
        public PatientMap()
        {
            Id(x => x.Cin);

            Map(x => x.Adresse).Column("adresse").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.NomPrenom).Column("nomprenom").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();
            Map(x => x.Telephone).Column("telephone").Not.Nullable();
            Map(x => x.DateNaissance).Column("datenaissance").Not.Nullable();
            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();
            Map(x => x.Sexe).Column("sexe").Nullable();
            Map(x => x.IsIndexed).Column("isIndexed").Nullable();

           Table("patient");
        }
    }
}
