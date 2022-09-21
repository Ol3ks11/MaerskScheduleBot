using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ship
{
    [JsonProperty("name")]
    public string ShipName { get; set; }

    [JsonProperty("code")]
    public string ShipCode { get; set; }

    [JsonProperty("ports")]
    public List<Port> Ports { get; set; }
}
