using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Maze_Game
{
    public class Items
    {
        [JsonIgnore]
        public IList<string> Messages { get; set; }
        [JsonRequired]
        public List<Treasure> Treasures { get; set; }
        [JsonRequired]
        public List<Threat> Threats { get; set; }

        public static Items GetItems()
        {
            Items items;

            string schemaJson = File.ReadAllText(@"..\..\ItemsSchema.json");

            JSchema schema = JSchema.Parse(schemaJson);

            JObject itemsClass;
            try
            {
                itemsClass = JObject.Parse(File.ReadAllText(@"..\..\Items.json"));
            }
            catch (JsonReaderException e)
            {
                // checking if the JSON is valid
                items = new Items();
                IList<string> error = new List<string> { e.Message };
                items.Messages = error;
                return items;
            }

            IList<string> messages;
            // Checks if the values are valid
            bool isValid = itemsClass.IsValid(schema, out messages);
            if (isValid == false)
            {
                items = new Items
                {
                    Messages = messages
                };
                return items;
            }
            else
            {
                try
                {
                    items = JsonConvert.DeserializeObject<Items>(File.ReadAllText(@"..\..\Items.json"));
                }
                catch (JsonSerializationException e)
                {
                    // Checks if the properties are valid
                    items = new Items();
                    IList<string> error = new List<string> { e.Message };
                    items.Messages = error;
                    return items;
                }
            }

            return items;
        }
    }

    public class Threat
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public int WealthOpportunity { get; set; }
        [JsonRequired]
        public Action Action { get; set; }
    }

    public class Action
    {
        [JsonRequired]
        public string Name { get; set; }
    }

    public class Treasure
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public int GainWealth { get; set; }
    }

   
}
