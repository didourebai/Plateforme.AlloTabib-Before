using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class PraticienMap : ClassMap<Praticien>
    {
        public PraticienMap()
        {
            Id(x => x.Cin);

            Map(x => x.Adresse).Column("adresse").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.NomPrenom).Column("nomprenom").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();
            Map(x => x.Telephone).Column("telephone").Not.Nullable();
            Map(x => x.Delegation).Column("delegation").Not.Nullable();
            Map(x => x.Gouvernerat).Column("gouvernerat").Not.Nullable();
            
            Map(x => x.Specialite).Column("specialite").Not.Nullable();

            Map(x => x.Fax).Column("fax").Nullable();
            Map(x => x.LanguesParles).Column("languesparles").Nullable();
            Map(x => x.Diplomes).Column("diplomes").Nullable();
            Map(x => x.Formations).Column("formations").Nullable();
            Map(x => x.Cursus).Column("cursus").Nullable();
            Map(x => x.Publication).Column("publication").Nullable();
            Map(x => x.MoyensPaiement).Column("moyenspaiement").Nullable();
            Map(x => x.ParcoursHospitalier).Column("parcourshospitalier").Nullable();
            Map(x => x.Conventionne).Column("conventionne").Not.Nullable();
            Map(x => x.InformationsPratique).Column("informationspratique").Nullable();
            Map(x => x.PrixConsultation).Column("prixconsultation").Nullable();
            Map(x => x.PresentationCabinet).Column("presentationcabinet").Nullable();
            Map(x => x.IsIndexed).Column("isIndexed").Nullable();
            Map(x => x.AdresseGoogle).Column("adresseGoogle").Nullable();

            Map(x => x.CreationDate).Column("creationdate").Not.Nullable();
            Map(x => x.LastModificationDate).Column("Lastmodificationdate").Not.Nullable();

            HasMany<Consultation>(x => x.Consultations)
              .Cascade.SaveUpdate()
              .KeyColumn("consultation_id");


            Table("praticien");
        }
    }
}
