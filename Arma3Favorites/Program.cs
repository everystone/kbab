using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * Superfast Arma3 Server browser
 * by Eirik Kvarstein eirik.kvarstein@gmail.com
 * 
 * @TODO: Load favorites and exe path from file
 * Export/import as json.
 * WEll WELL WELL. Works with All Steam servers(?).
 * Let user add servers from all games that uses steam server protocol.
 * In list, sort by game. If user selects # from another game, launch that one.
 * 
 * Timeout on udp requests.
 */

namespace Arma3Favorites
{
    class Program
    {
        static List<Server> servers;
        //static Dictionary<string, int> favorites;
        private static String configPath = Directory.GetCurrentDirectory() + "\\kbab.conf";
        public static Config config = Config.Read(configPath);
        
        public static void autoJoin(Server serv)
        {
            int delay = 30; //seconds
            while (true)
            {
                Console.Clear();
                Console.Write("Ip: " + serv.getIp() + " port: " + serv.getPort());
                Console.WriteLine("Server Status: ");
                Server auto = engine.fetch(serv.getIp(), serv.getPort());
                if (auto == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("OFFLINE");
                    Console.ResetColor();
                }
                else
                {
                    auto.printSmall();
                    if (!auto.isLocked())
                    {
                        Console.WriteLine("\nJoining server...");
                        auto.open();
                        break;
                    }
                }
                Console.WriteLine("Autojoin active");
                Console.WriteLine("Refresh every " + delay + " seconds.");
                System.Threading.Thread.Sleep(delay * 1000);
            }
        }


        static void refresh()
        {
            int i = 0;
            string mission = "";
            servers.Clear();

            foreach (KeyValuePair<string, int> server in config.favorites)
            {
                Server s = engine.fetch(server.Key, server.Value);
                if (s != null)
                {
                    servers.Add(s);
                    /* Delimiter between different missions / mods */
                    if (!s.getMission().Equals(mission))
                    {
                        mission = s.getMission();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(mission);
                        Console.ResetColor();
                    }
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("#" + i + " ");
                    Console.ResetColor();


                    s.printSmall();


                    i++;

                    //debug
                    //Console.WriteLine("Tags: " + s.getTags());
                }
            }
        }

         
        static void Main(string[] args)
        {
            //STEAM ports are now linked to game-port as +1 for query and +2 to-master
            // So game server is port -1
            /*   ARMA SA-MATRA WASTELAND SERVERS */
             bool running = true;
             Console.WriteLine("configPath: " + configPath);

             String ascii = @" __   ___.         ___.      ____     ____ 
|  | _\_ |__ _____ \_ |__   /_   |   /_   |
|  |/ /| __ \\__  \ | __ \   |   |    |   |
|    < | \_\ \/ __ \| \_\ \  |   |    |   |
|__|_ \|___  (____  /___  /  |___| /\ |___|
     \/    \/     \/    \/         \/      ";
            Console.WriteLine(ascii);
            Console.WriteLine("  KBAB BEATS ARMA'S BROWSER eirik.kvarstein@gmail.com 03/15\n\n");
            servers = new List<Server>();

           // engine e = new engine();
            //Console.ReadKey();
            
            refresh(); //Refresh Server list

            while (running)
            {
                int choice = 0;
                Console.Write("Enter ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("#");
                Console.ResetColor();
                Console.Write(" To Launch: ");
                var input = Console.ReadLine();
                if (Int32.TryParse(input, out choice))
                {
                    if ((choice > -1) && (choice < servers.Count))
                    {
                        /* If server is locked, offer to autojoin when open */
                        if (servers[choice].isLocked())
                        {
                            Console.WriteLine("Server is locked. Autojoin when open? y/n");
                                var yesno = Console.ReadKey();
                                if(yesno.KeyChar == 'y'){
                                    autoJoin(servers[choice]);
                                }
                            
                        }else {
                        servers[choice].open();
                        }
                    }
                }
                else if (input.Equals("r"))
                {
                    Console.Clear();
                    refresh();
                }
                else if (input.Equals("q"))
                {
                    config.Write(configPath);
                    running = false; //exit
                }
            }
        }
     }
             
}
