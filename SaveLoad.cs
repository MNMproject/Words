using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Words
{
    class SaveLoad : MainWindow
    {
        public static void SaveJson()
        {
            string fileName = "ForbiddenList.json";
            File.WriteAllText(fileName, JsonConvert.SerializeObject(ForbidenWordsList));
        }

        public static void LoadJsonToList(List<string> tempList)
        {
            ForbidenWordsList.Clear();
            if (File.Exists("ForbiddenList.json"))
            {
                var json = File.ReadAllText("ForbiddenList.json");
                var loading = JsonConvert.DeserializeObject<List<string>>(json);
                foreach (var list in loading)
                {
                    tempList.Add(list);
                }
            }
        }

        public static void SaveConfig()
        {
            string fileName = "config.json";
            File.WriteAllText(fileName, JsonConvert.SerializeObject(ListConfig));
        }

        public static void LoadConfig(List<ConfigList> templist)
        {
            ListConfig.Clear();
            //if (File.Exists("config.json"))
            //{
                var json = File.ReadAllText("config.json");
                var loading = JsonConvert.DeserializeObject<List<ConfigList>>(json);
                foreach (var list in loading)
                {
                    templist.Add(list);
                }
            //}
        }
    }
}
