using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class JourFerieMap : ClassMap<JourFerie>
    {
        public JourFerieMap()
        {
            Id(x => x.Id)
                .GeneratedBy
                .Guid().Column("Id");

            Map(x => x.JourFerieNom).Column("jourFerie").Nullable();

            References(x => x.Praticien)
                .Column("praticien_id")
                .Cascade.SaveUpdate();

            Table("jourFerie");
        }
    }
}
