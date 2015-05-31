using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma3Favorites
{
    public class Config
    {
        //public static string exe = @"D:\Programmer\Steam\steamapps\common\Arma 3\arma3.exe";
      /*  public Dictionary<string, int> favorites = new Dictionary<string, int>()
        {
                // Battleroyale EU stratis
                 {"94.23.15.45",2303}, //EU 1
                {"188.165.204.150",2303}, // EU 2
        };*/
        public List<favorite> Favorites = new List<favorite>(){
        };


        public string exe = "steam://run/107410//";
    
        //Mods
        //public string chernarus_wasteland = "-mod=@WSWeapons;@WSMaps;@WSRHS";
       // public string battleroyale = "-mod=@PUBattleRoyale";

       // static string misc = " -nosplash -skipintro -world=empty -maxMem=4095 -cpuCount=8 ";


        public void Write(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config Read(string path)
        {
            return !File.Exists(path)
                ? new Config()
                : JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
    }
}
