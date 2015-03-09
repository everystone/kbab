using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma3Favorites
{
    public static class config
    {
        public static string exe = @"D:\Programmer\Steam\steamapps\common\Arma 3\arma3.exe";
    
    
        //Mods
        static string chernarus_wasteland = "-mod=@WSWeapons;@WSMaps;@WSRHS";
        static string battleroyale = "-mod=@PUBattleRoyale";

        static string misc = " -nosplash -skipintro -world=empty -maxMem=4095 -cpuCount=8 ";

    }
}
