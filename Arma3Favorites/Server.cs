using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma3Favorites
{
    public class Server
    {
        String name;
        String ip;
        int port;
        String game;
        String map;
        String mission;
        Int16 players;
        Int16 maxplayers;
        long ping;
        String gametags;

        /*Game tags */
        bool battleeye;
        string platform;
        bool locked;
        bool dedicated;

        public Server(string ip, int port)
        {
            //Fetch data from server and assign
            this.ip = ip;
            this.port = port;
        }
        public void setName(String name) { this.name = name; }
        public void setPlayers(Int16 players) { this.players = players; }
        public void setMaxPlayers(Int16 max) { this.maxplayers = max; }
        public void setMap(String map) { this.map = map; }
        public void setGame(String game) { this.game = game; }
        public void setMission(String mission) { this.mission = mission; }
        public void setPing(long ping) { this.ping = ping; }
        public long getPing() { return ping; }
        public String getIp() { return ip;}
        public int getPort() { return port; }
        public String getName() { return name; }
        public bool isLocked() { return locked; }
        public String getTags() { return gametags; }
        public String getMission() { return mission; }
        /*bt,r140,n0,s7,i2,mf,lf,vt,dt,tsandbox,g65545,c4194303-4194303,pw,
         https://community.bistudio.com/wiki/STEAMWORKSquery
         */
        public void setTags(String tags) { 
            this.gametags = tags;
            string[] arr = tags.Split(',');
            battleeye = arr[0].Equals("bt");
            locked = arr[6].Equals("lt"); //lock true
        
        }
        public String printInfo()
        {
            String info = String.Format("{0} ({1}, {2}, {3}) {4}/{5}", name, game, map, mission, players, maxplayers);
            Console.WriteLine(info);
            return info;
        }
        public void printSmall()
        {
            //if name contains #<number>, split on it.

            string shortName = name;
            if (shortName.Contains("#")) { 
                var array = shortName.Split('#');
                shortName = array[0] + array[1][0]; // Servername UK #1
            }
            if (shortName.Length > 40) shortName = shortName.Substring(0, 40);
           // return(String.Format("{0} {1} {2}/{3}", shortName, map, players, maxplayers));
            Console.Write(String.Format("{0} {1} {2}/{3}", shortName, map, players, maxplayers));
            //Check if server is locked ( password protected ).
            if (this.isLocked())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" [LOCKED]");
                Console.ResetColor();


            }

            //Check ping
            if (ping > 150) Console.ForegroundColor = ConsoleColor.Red;
            else if (ping > 90) Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" " + ping + "ms");

            Console.ResetColor();

        }

        /* Start ARMA 3 with ip param */
        public void open()
        {
            Process game = new Process();

           // game.StartInfo.FileName = "steam://run/107410//";
            String param = "steam://run/107410//";
            //Check mission type, add correct mod param
            if (mission.Equals("battleroyale"))
                param += "mod=@PUBattleRoyale ";
            if (mission.Equals("Epoch Mod"))
                param += "mod=@Epoch ";

            param += "-nosplash -connect=" + ip + " -port=" + (port - 1); //Gameport is one below steam query port

            game.StartInfo.FileName = param;
            //game.StartInfo.Arguments = param; 
            game.Start();

            
            Console.WriteLine("debug param: " + param);
        }
        
    }
}
