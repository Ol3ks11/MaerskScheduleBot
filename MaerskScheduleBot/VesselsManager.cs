using Newtonsoft.Json;
using System.Text;

namespace MaerskScheduleBot
{
    public class VesselsManager
    {
        [JsonProperty("vessels")]
        public List<Ship> ships { get; set; }

        public void UpdateShipPorts(string shipName)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].ShipName == shipName)
                {
                    Ship temp = new();
                    temp = JsonConvert.DeserializeObject<Ship>(GetPortsJson(ships[i]).Result);
                    temp.ShipName = ships[i].ShipName;
                    temp.ShipCode = ships[i].ShipCode;
                    ships[i] = temp;
                    break;
                }
            }
        }

        private async Task<string> GetPortsJson(Ship ship)
        {
            string fromDateStr = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            string toDateStr = DateOnly.FromDateTime(DateTime.Now.AddDays(89)).ToString("yyyy-MM-dd");

            HttpRequestMessage requestForPortsList = new();
            string getPortsURL = "https://api.maerskline.com/maeu/schedules/vessel?vesselCode="
                +ship.ShipCode+"&fromDate="+ fromDateStr + "&toDate="+ toDateStr;
            requestForPortsList.RequestUri = new Uri(getPortsURL);
            HttpClient client = new();
            try
            {
                var maerskResponse = await client.SendAsync(requestForPortsList);
                string stringMaerskResponse = await maerskResponse.Content.ReadAsStringAsync();
                return stringMaerskResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Dispose();
            }
            return null;
        }

        public string BuildSchedule(int shipIndex)
        {
            StringBuilder builder = new();
            builder.AppendLine("Schedule for " + ships[shipIndex].ShipName +":");
            builder.AppendLine();
            foreach (var port in ships[shipIndex].Ports)
            {
                builder.AppendLine("Port call: " + port.port);
                builder.AppendLine("Terminal: " + port.terminal);
                builder.AppendLine("Arrival: " + port.arrival.ToString("dd-MM-yyyy HH:mm"));
                builder.AppendLine("Departure: " + port.departure.ToString("dd-MM-yyyy HH:mm"));
                builder.AppendLine();
                if (builder.Length > 3500)
                {
                    return builder.ToString();
                }
            }
            return builder.ToString();
        }
    }
}