using Newtonsoft.Json;

namespace Plateforme.AlloTabib.DomainLayer.Base.Classes
{
    public class ResultEnvelope<T> where T : class
    {
        [JsonProperty("result")]
        public ResponseEnvelope<T> Result { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
