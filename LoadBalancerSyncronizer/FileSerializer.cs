using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LoadBalancerSyncronizer
{
    public static class FileSerializer
    {
        private static string getJsonPath()
        {
            return Application.ExecutablePath.ToLowerInvariant().Replace(".exe", ".json");
        }

        public static API Load()
        {
            string jsonPath = getJsonPath();
            if (!File.Exists(jsonPath))
            {
                API api = new API();
                
                string json = JsonConvert.SerializeObject(api, Formatting.Indented);
                File.WriteAllText(jsonPath, json, Encoding.UTF8);
            }

            return JsonConvert.DeserializeObject<API>(File.ReadAllText(jsonPath, Encoding.UTF8));
        }

        public static void Save(API api)
        {
            string json = JsonConvert.SerializeObject(api, Formatting.Indented);
            File.WriteAllText(getJsonPath(), json, Encoding.UTF8);
        }
    }
}
