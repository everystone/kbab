using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
namespace Arma3Favorites
{
    public static class engine
    {
        
        //TSource Engine Query..
        static byte[] request = {
               0xff, 0xff, 0xff, 0xff, 
               0x54, 0x53, 0x6f, 0x75,
               0x72, 0x63, 0x65, 0x20, 
               0x45, 0x6e, 0x67, 0x69, 
               0x6e, 0x65, 0x20, 0x51, 
               0x75, 0x65, 0x72, 0x79, 0x00 
           };
        static int pos = 0; //read position
       static int offset = 6; //byte offset in response.

        //response:
        /*
         * String delimiter: 0x00
         * ff:ff:ff:ff:49:11:57:53:2e:41:52:4d:41:2e:53:55:20:2d:20:53:74:72:61:74:69:73:20:57:61:73:74:65:6c:61:6e:64:20:2d:20:55:4b:20:23:34:20:2d:20:4d:69:64:6e:69:67:68:74:20:47:61:6d:69:6e:67:20:2d:20:56:69:6c:61:00:53:74:72:61:74:69:73:00:41:72:6d:61:33:00:53:74:72:61:74:69:73:20:57:61:73:74:65:6c:61:6e:64:20:62:79:20:53:61:2d:4d:61:74:72:61:20:28:76:32:33:29:00:00:00:16:41:00:64:77:00:00:31:2e:33:38:2e:31:32:38:39:33:37:00:b1:1e:0c:06:e4:b6:88:84:14:40:01:62:74:2c:72:31:33:38:2c:6e:30:2c:73:37:2c:69:32:2c:6d:66:2c:6c:66:2c:76:74:2c:64:74:2c:74:73:61:6e:64:62:6f:78:2c:67:36:35:35:34:35:2c:63:32:2d:34:39:2c:70:77:2c:00:92:a3:01:00:00:00:00:00
         * ....I.WS.ARMA.SU - Stratis Wasteland - UK #4 - Midnight Gaming - Vila.Stratis.Arma3.Stratis Wasteland by Sa-Matra (v23)....A.dw..1.38.128937..........@.bt,r138,n0,s7,i2,mf,lf,vt,dt,tsandbox,g65545,c2-49,pw,.........
         *
         * Format: NAME.MAP.GAME.MISSION.PLAYERS.MAXPLAYERS
         */

        static public void debug(string msg)
        {
            using (StreamWriter file = new StreamWriter("debug.txt"))
            {
                file.WriteLine(msg);
            }

        }
        /* BYTE UTILS */
        static byte readByte(byte[] arr)
        {
            byte res = arr[pos];
            pos += 1;
            return res;
        }
        static float readFloat(byte[] arr)
        {
            float res = System.BitConverter.ToSingle(arr, pos);
            pos += 4; //float : 4 bytes
            return res;
        }
        static Char readChar(byte[] arr)
        {
            Char res = System.BitConverter.ToChar(arr, pos);
            pos += 2;
            return res;
        }
        static Int16 readInt16(byte[] arr)
        {
            Int16 res = System.BitConverter.ToInt16(arr, pos);
            pos += 2; //2 bytes
            return res;
        }
        static Int32 readInt32(byte[] arr)
        {
            Int32 res = System.BitConverter.ToInt32(arr, pos);
            pos += 4; //4 bytes
            return res;
        }
        static String readString(byte[] arr)
        {
            //Find terminater byte (0x00) .
            //Start searching from current position in array.
            int i = 0;
            for (i = pos; i < arr.Length; i++)
            {
                if (arr[i] == 0x00)
                    break;
            }
            int bytes = i - pos;// Subtract pos to get byte count ( i ).
            //Console.WriteLine(string.Format("Reading string({0}) bytes from pos {1}", bytes, pos));
            String res = System.Text.Encoding.Default.GetString(arr, pos, bytes);
            pos += bytes + 1; //Read i bytes ( and skip terminator 0x00 )
            return res;
        }

        public static Server fetch(string ip, int port)
        {

            byte[] rec;
            Stopwatch sw = new Stopwatch();
            /* Send Request to Server:IP */
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            Server server = new Server(ip, port);
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Client.ReceiveTimeout = 5000;
                udpClient.Connect(ep);
                try
                {
                    
                    sw.Start();
                    udpClient.Send(request, request.Length);
                    rec = udpClient.Receive(ref ep);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    //Console.WriteLine("Error: Server " + ip + ":" + port + " Not responding.");
                    udpClient.Close();
                    return null;
                }
                pos = offset;

                /* Create new Server obj
                    And parse the byte buffer */
                sw.Stop();
                server.setPing(sw.ElapsedMilliseconds);
                server.setName(readString(rec));
                server.setMap(readString(rec));
                server.setGame(readString(rec));
                server.setMission(readString(rec));
                pos += 2; // skip 2 next bytes ( 2x 0x00)
                server.setPlayers(readByte(rec));
                server.setMaxPlayers(readByte(rec));
                //Skip 16 bytes
                pos += 28;
                server.setTags(readString(rec));
                //pos += 1; // skip 0x00
                //int test = readbyte(rec); // 119?
                //Debug
                var str = System.Text.Encoding.Default.GetString(rec);
                debug(str);
               // udpClient.Close();
                sw.Reset();                
            }
            return server;
        }


    }
}
