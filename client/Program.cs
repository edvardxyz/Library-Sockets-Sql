using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace client
{
    class Program
    {
        static void Main(){
            int port = 1234; //Interface.GetInt("Server port: ", 65535);
            TcpClient client = new TcpClient();
            IPAddress ip = IPAddress.Parse("127.0.0.1");  //Interface.GetIp());
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            byte[] buffer = new byte[5000];
            int readbytes;
            string loginStatus;

            client.Connect(endPoint);
            NetworkStream stream = client.GetStream();
            while(client.Connected){
                Command(Interface.GetInput("Indtast bruger id: "), stream, false);
                Command(Interface.GetPass(), stream, false);
                readbytes = stream.Read(buffer);

                loginStatus = Encoding.UTF8.GetString(buffer, 0, readbytes);

                if(loginStatus == "user"){
                    UserMenu(stream);
                    stream.Close();
                    client.Close();
                }else if(loginStatus == "admin"){
                    AdminMenu(stream);
                    stream.Close();
                    client.Close();
                }else{
                    Console.WriteLine("Login failed");
                }
            }
            client.Close();
        }

        static void Command(string cmd, NetworkStream stream, bool printresult){
            byte[] buffer = new byte[5000];
            stream.Write(Encoding.UTF8.GetBytes(cmd));
            if(printresult){
                int readbytes = stream.Read(buffer);
                string result = Encoding.UTF8.GetString(buffer, 0, readbytes);
                Console.WriteLine(result + "\nTryk en tast for at gå tilbage");
                Console.ReadKey();
            }
        }

        private static void UserMenu(NetworkStream stream){
            bool running = true;
            do{
                int menu = Interface.GetDigit("1) Lån bog\n2) Aflever bog\n3) Se hvilke bøger du har lånt\n4) Se hvilke bøger biblioteket har\n5) Slet bruger\n6) Log ud", 6);
                switch (menu)
                {
                    case 1:
                        Command("1" + Interface.GetInput("Indtast ISBN på bogen du vil låne: "), stream, true);
                        break;
                    case 2:
                        Command("2" + Interface.GetInput("Indtast ISBN på bogen du vil aflevere: "), stream, true);
                        break;
                    case 3:
                        Command("3", stream, true);
                        break;
                    case 4:
                        Command("4", stream, true);
                        break;
                    case 5:
                        do{
                            Console.WriteLine("\nEr du sikker på du vil slette din bruger?");
                            Console.WriteLine("Tryk 'y' for at bekræfte, tryk en anden tast for at fortryde");
                            if (Console.ReadKey(true).Key == ConsoleKey.Y){
                                Command("5", stream, false);
                                goto case 6;
                            }else{
                                break;
                            }
                        }while(true);
                        break;
                    case 6:
                        Command("6", stream, false);
                        running = false;
                        break;
                    default:
                        break;
                }
            }while(running);
        }

        private static void AdminMenu(NetworkStream stream){
            bool running = true;
            do{
                int menu = Interface.GetDigit("1) Opret bog\n2) Slet bog\n3) Slet bruger\n4) Vis alle bøger\n5) Vis alle brugere\n6) Log ud", 6);
                switch (menu)
                {
                    case 1:
                        Command("1", stream, false);
                        Command(Interface.GetInput("Indtast ISBN på bogen: "), stream, false);
                        Command(Interface.GetString("Indtast Title på bogen: "), stream, false);
                        Command(Interface.GetString("Indtast Author  på bogen: "), stream, false);
                        Command(Interface.GetString("Indtast Publisher på bogen: "), stream, false);
                        Command(Interface.GetString("Indtast Genre på bogen: "), stream, false);
                        Command(Interface.GetInput("Indtast Published på bogen: "), stream, false);
                        Command(Interface.GetInput("Indtast Pages på bogen: "), stream, true);
                        break;
                    case 2:
                        Command("2", stream, false);
                        Command(Interface.GetInput("Indtast ISBN på bogen du vil slette: "), stream, true);
                        break;
                    case 3:
                        Command("3", stream, false);
                        Command(Interface.GetInput("Indtast ID paa brugeren du vil slette: "), stream, true);
                        break;
                    case 4:
                        Command("4", stream, true);
                        break;
                    case 5:
                        Command("5", stream, true);
                        break;
                    case 6:
                        Command("6", stream, false);
                        running = false;
                        break;
                    default:
                        break;
                }
            }while(running);
        }
    }


    public class Interface
    {
        public static int GetInt(string message, int max){
            uint number;
            do
            {
                Console.Write(message);
            } while (!uint.TryParse(Console.ReadLine(), out number) || number > max);
            return (int)number;
        }

        public static string GetIp(){
            int octet1 = GetInt("Type IP first octet: ", 255);
            int octet2 = GetInt($"Type IP second octet: {octet1}.", 255);
            int octet3 = GetInt($"Type IP third octet: {octet1}.{octet2}.", 255);
            int octet4 = GetInt($"Type IP fourth octet: {octet1}.{octet2}.{octet3}.", 255);
            return $"{octet1}.{octet2}.{octet3}.{octet4}";
        }
        public static string GetString(string message)
        {
            Console.Clear();
            Console.Write(message);
            return Console.ReadLine();
        }

        public static string GetInput(string message)
        {
            int number;
            do
            {
                Console.Clear();
                Console.Write(message);
            } while (!int.TryParse(Console.ReadLine(), out number));
            return number.ToString();
        }

        public static int GetDigit(string message, int max)
        {
            int number;
            do
            {
                Console.Clear();
                Console.Write(message);
                number = Console.ReadKey(true).KeyChar - '0';
            } while (number > max || number < 1);
            return number;
        }

        // funktion til at lave stjerne GetPass
        public static string PrintStars(int starCount)
        {
            string stars = "";
            for(int i = 0; i < starCount; i++){
                stars += "*";
            }
            return stars;
        }

        public static string GetPass()
        {
            int starCount = 0;
            // tom string hvor hver char bliver appended
            string password = "";
            do
            {
                Console.Clear();
                Console.Write("Indtast kode: {0}", PrintStars(starCount));
                // tager keypress fra user putter ConsoleKeyInfo ind i keyinfo
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);

                // hvis backspace fjern sidste char og fjern stjerne, hvis password.Length er 0 continue for at undgå crash
                if(keyinfo.Key == ConsoleKey.Backspace){
                    if(password.Length == 0)
                        continue;
                    password = password.Remove(password.Length-1);
                    starCount--;
                    continue;
                }
                starCount++;
                // hvis enter så break
                if(keyinfo.Key == ConsoleKey.Enter)
                    break;

                // append keyinfo lavet til char
                password += keyinfo.KeyChar;
            }while(true);
            // hasher stringen med pass.hash function og returere derefter
            return Pass.Hash(password);
        }
    }

    // laver SHA256 objekt
    public class Pass
    {
        public static string Hash(string passString)
        {
            string passHashString = "";
            SHA256 sha256obj = SHA256.Create();
            // SHA256 ComputeHash tager kun byte array, derfor konverteres string her;
            byte[] passBytes = sha256obj.ComputeHash(Encoding.UTF8.GetBytes(passString));

            // for loop igennem hver byte of lav til string med argument "x2"
            // x2 er format specifier så en byte altid bliver til 2char lang string
            for ( int i =0; i < passBytes.Length; i++){

                passHashString += passBytes[i].ToString("x2");
            }
            return passHashString;
        }
    }
}

