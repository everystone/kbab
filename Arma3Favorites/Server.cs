﻿using System;
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

        public String printInfo()
        {
            String info = String.Format("{0} ({1}, {2}, {3}) {4}/{5}", name, game, map, mission, players, maxplayers);
            Console.WriteLine(info);
            return info;
        }
        public String printSmall()
        {
            //if name contains #<number>, split on it.

            string shortName = name;
            if (shortName.Contains("#")) { 
                var array = shortName.Split('#');
                shortName = array[0] + array[1][0]; // Servername UK #1
            }
            if (shortName.Length > 40) shortName = shortName.Substring(0, 40);
            return(String.Format("{0} {1} {2}/{3}", shortName, map, players, maxplayers));
        }

        /* Start ARMA 3 with ip param */
        public void open()
        {
            Process game = new Process();
            game.StartInfo.FileName = config.exe;
            game.StartInfo.Arguments = "-nosplash -connect=" + ip + " -port=" + port;
            game.Start();
        }
        
    }
}
