using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class ConfigurationMap : ClassMap<ConfigurationPraticien>
    {
        public ConfigurationMap()
        {
            Id(x => x.Id)
             .GeneratedBy
             .Guid().Column("Id");
        
            Map(x => x.HeureDebutMatin).Column("HeureDebutMatin").Nullable();
            Map(x => x.MinuteDebutMatin).Column("MinuteDebutMatin").Nullable();
            Map(x => x.HeureDebutMidi).Column("HeureDebutMidi").Nullable();
            Map(x => x.MinuteDebutMidi).Column("MinuteDebutMidi").Nullable();

            Map(x => x.HeureFinMatin).Column("HeureFinMatin").Nullable();
            Map(x => x.MinuteFinMatin).Column("MinuteFinMatin").Nullable();
            Map(x => x.HeureFinMidi).Column("HeureFinMidi").Nullable();
            Map(x => x.MinuteFinMidi).Column("MinuteFinMidi").Nullable();

            Map(x => x.DecalageMinute).Column("DecalageMinute").Not.Nullable();
       

            References(x => x.Praticien)
              .Column("praticien_id")
              .Cascade.SaveUpdate();
            Table("configurationPraticien");
        }
    }
}
