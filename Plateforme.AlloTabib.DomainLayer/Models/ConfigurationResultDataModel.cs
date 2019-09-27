namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class ConfigurationResultDataModel
    {
        public string HeureDebutMatin { get; set; }
        public string MinuteDebutMatin { get; set; }
        public string HeureDebutMidi { get; set; }
        public string MinuteDebutMidi { get; set; }
        public string JourRepos { get; set; }
        public string DeuxiemeJourRepos { get; set; }
        public string PraticienCin { get; set; }
        public string DecalageMinute { get; set; }

        public string HeureFinMatin { get; set; }
        public string HeureFinMidi { get; set; }
        public string MinuteFinMatin { get; set; }
        public string MinuteFinMidi { get; set; }
    }
}
