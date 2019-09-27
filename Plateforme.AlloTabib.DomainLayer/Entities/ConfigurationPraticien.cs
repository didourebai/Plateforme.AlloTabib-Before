using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class ConfigurationPraticien : BasicEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string HeureDebutMatin { get; set; }
        public virtual string MinuteDebutMatin { get; set; }

        public virtual string HeureDebutMidi { get; set; }
        public virtual string MinuteDebutMidi { get; set; }


        public virtual string HeureFinMatin { get; set; }
        public virtual string MinuteFinMatin { get; set; }

        public virtual string HeureFinMidi { get; set; }
        public virtual string MinuteFinMidi { get; set; }

        public virtual int DecalageMinute { get; set; }
        public virtual Praticien Praticien { get; set; }

        public ConfigurationPraticien()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }
    }
}
