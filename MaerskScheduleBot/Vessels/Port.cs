using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Port
{
    [JsonProperty("portGeoId")]
    public string portGeoId { get; set; }

    [JsonProperty("port")]
    public string port { get; set; }

    [JsonProperty("terminal")]
    public string terminal { get; set; }

    [JsonProperty("terminalGeoId")]
    public string terminalGeoId { get; set; }

    [JsonProperty("voyageArrival")]
    public string voyageArrival { get; set; }

    [JsonProperty("voyageDeparture")]
    public string voyageDeparture { get; set; }

    [JsonProperty("arrival")]
    public DateTime arrival { get; set; }

    [JsonProperty("departure")]
    public DateTime departure { get; set; }

    [JsonProperty("serviceArr")]
    public string serviceArr { get; set; }

    [JsonProperty("serviceDep")]
    public string serviceDep { get; set; }
}
