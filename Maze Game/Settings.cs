using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Maze_Game
{
    public class Settings
    {
        [JsonIgnore]
        public IList<string> Messages { get; set; }
        [JsonRequired]
        public Player Player { get; set; }
        [JsonRequired]
        public int NumberOfRooms { get; set; }        
        
        public static Settings GetConfiguration()
        {
            Settings settings;
            // Schema for the JSON values
            string schemaJson = File.ReadAllText(@"..\..\ConfigSchema.json");

            JSchema schema = JSchema.Parse(schemaJson);

            JObject settingsClass;
            try
            {
                settingsClass = JObject.Parse(File.ReadAllText(@"..\..\MazeConfiguration.json"));
            }
            catch (JsonReaderException e)
            {
                // checking if the JSON is valid
                settings = new Settings();
                IList<string> error = new List<string>{ e.Message };
                settings.Messages = error;
                return settings;
            }           

            IList<string> messages;
            // Checks if the values are valid
            bool isValid = settingsClass.IsValid(schema, out messages);
            if (isValid == false)
            {
                settings = new Settings
                {
                    Messages = messages
                };
                return settings;
            }
            else
            {
                try
                {
                    settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(@"..\..\MazeConfiguration.json"));
                }
                catch (JsonSerializationException e)
                {             
                    // Checks if the properties are valid
                    settings = new Settings();
                    IList<string> error = new List<string> { e.Message };
                    settings.Messages = error;
                    return settings;
                }                
            }            
            settings.Player.Wealth = 0;
            return settings;
        }
    }

    public class Player
    {
        [JsonRequired]        
        public string Name { get; set; }
        [JsonIgnore]
        public int Wealth { get; set; }
        [JsonIgnore]
        public Room Location { get; set; }
    }




}
