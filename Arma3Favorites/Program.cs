﻿using System;
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
 */

namespace Arma3Favorites
{
    class Program
    {
        

         
        static void Main(string[] args)
        {

          Dictionary<string, int> favorites = new Dictionary<string, int>()
            {
            {"31.3.230.66", 3103},// UK #4   
           
            {"81.19.208.102",2403}, // UK #2
            {"81.19.208.101", 2603}, //UK #1
            {"37.187.77.180", 2403}, //FR 1 Chernarus
            {"81.19.208.113", 2703}, //UK #6
            {"81.19.208.112", 2603}, // UK#3 hc
            {"5.39.85.45", 2303}, // UK #8
            {"37.59.53.188", 2303} // UK #5
            
        };
            String ascii = @" __  __ ______ _______ ______      ____      ______ 
|  |/  |   __ \   _   |   __ \    |_   |    |      |
|     <|   __ <       |   __ <     _|  |_ __|  --  |
|__|\__|______/___|___|______/    |______|__|______|
                                                    ";
            Console.WriteLine(ascii);
            Console.WriteLine("  KBAB BEATS ARMA'S BROWSER root@eirik.pw 03/15\n\n");
            List<Server> servers = new List<Server>();
           // engine e = new engine();
            //Console.ReadKey();

            int i=0;
            foreach (KeyValuePair<string,int> server in favorites)
            {
               Server s = engine.fetch(server.Key, server.Value);
                if (s != null)
                {
                    servers.Add(s);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("#" + i + " ");
                    Console.ResetColor();
                    Console.Write(s.printSmall());
                    
                    //Check ping
                    long ping = s.getPing();
                    if (ping > 150) Console.ForegroundColor = ConsoleColor.Red;
                    else if (ping > 90) Console.ForegroundColor = ConsoleColor.Yellow;
                    else Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" " + ping+"ms");

                    Console.ResetColor();
                    i++;
                }
            }
            int choice = 0;
            Console.Write("Enter ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("#");
            Console.ResetColor();
            Console.Write(" To Launch: ");
            if (Int32.TryParse(Console.ReadLine(), out choice))
            {
                if ((choice > -1) && (choice < servers.Count))
                {
                    servers[choice].open();
                }
           }
        }
     }
             
}