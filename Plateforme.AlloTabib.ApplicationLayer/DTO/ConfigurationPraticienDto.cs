namespace Plateforme.AlloTabib.ApplicationLayer.DTO
{
    public class ConfigurationPraticienDto
    {
        public string HeureDebutMatin { get; set; }
        public string MinuteDebutMatin { get; set; }

        public string HeureDebutMidi { get; set; }
        public string MinuteDebutMidi { get; set; }


        public string HeureFinMatin { get; set; }
        public string MinuteFinMatin { get; set; }

        public string HeureFinMidi { get; set; }
        public string MinuteFinMidi { get; set; }

        
        public int DecalageMinute { get; set; }
        public string  PraticienCin { get; set; }
    }
}