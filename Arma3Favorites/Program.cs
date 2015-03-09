using System;
using System.Collections.Generic;
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
        static Dictionary<string, int> favorites;
        static void refresh()
        {
            int i = 0;
            foreach (KeyValuePair<string, int> server in favorites)
            {
                Server s = engine.fetch(server.Key, server.Value);
                if (s != null)
                {
                    servers.Add(s);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("#" + i + " ");
                    Console.ResetColor();
                    Console.Write(s.printSmall());

                    //Check if server is locked ( password protected ).
                    if (s.isLocked())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" [LOCKED]");
                        Console.ResetColor();
                    }
                    
                    //Check ping
                    long ping = s.getPing();
                    if (ping > 150) Console.ForegroundColor = ConsoleColor.Red;
                    else if (ping > 90) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" " + ping + "ms");

                    Console.ResetColor();


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
             favorites = new Dictionary<string, int>()
               {
               
               {"37.59.53.188", 2303}, // UK #5
               {"81.19.208.102",2403}, // UK #2
               {"81.19.208.101", 2603}, //UK #1 //03? 02?
             
               {"81.19.208.112", 2603}, // UK#3 hc
               {"37.187.77.180", 2403}, //FR 1 Chernarus
               {"5.39.85.45", 2303}, // UK #8            
               {"85.159.42.196", 2303}, //RU #1
               {"31.3.230.66", 3103},// UK #4  
                {"81.19.208.113", 2703 },//UK #6

                // Battleroyale EU stratis
                 {"94.23.15.45",2303}, //EU 1
                {"188.165.204.150",2303}, // EU 2
                {"81.19.216.152", 2311}, // UK 3
               // {"81.19.216.152", 2320}, // UK 7
                {"109.70.148.112",2301} // UK 7 HC

           };
             
             
            String ascii = @" __  __ ______ _______ ______      ____      ______ 
|  |/  |   __ \   _   |   __ \    |_   |    |      |
|     <|   __ <       |   __ <     _|  |_ __|  --  |
|__|\__|______/___|___|______/    |______|__|______|
                                                    ";
            Console.WriteLine(ascii);
            Console.WriteLine("  KBAB BEATS ARMA'S BROWSER root@eirik.pw 03/15\n\n");
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
                String input = Console.ReadLine();
                if (Int32.TryParse(input, out choice))
                {
                    if ((choice > -1) && (choice < servers.Count))
                    {
                        servers[choice].open();
                    }
                }
                else if (input.Equals("r"))
                {
                    Console.Clear();
                    refresh();
                }
                else if (input.Equals("q"))
                {
                    running = false; //exit
                }
            }
        }
     }
             
}
