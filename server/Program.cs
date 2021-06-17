using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MySql.Data.MySqlClient;


namespace server
{
    class Program
    {
        static void Main(){
            Server.Run();
        }
    }

    class Server{
        public static List<ClientCon> clients = new List<ClientCon>();

        public static void Run(){

            int port = 1234; //Interface.GetInt("Listen on port: ", 65535);
            IPAddress ip = IPAddress.Any;
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();

            AcceptClients(listener);
            do{
            }while(Console.ReadLine() != "exit");
        }

        public static async void AcceptClients(TcpListener listener){
            bool isRunning = true;
            while(isRunning){
                TcpClient client = await listener.AcceptTcpClientAsync();
                ClientCon clientobj = new ClientCon(client);
                clients.Add(clientobj);

            }
        }
    }

    static class Sql{

        static string constring = $"server=127.0.0.1;userid=root;password=nBgPA6MHdp;database=library";

        public static bool AddUser(MySqlParameter[] values){
            if(values.Length == 3)
                return ExecuteSql(values, "INSERT INTO users(name, password, admin) VALUES(@name, @password, @admin)");
            else{
                Console.WriteLine("Wrong values count");
                return false;
            }
        }

        public static bool AddBook(MySqlParameter[] values){
            if(values.Length == 7)
                return ExecuteSql(values, "INSERT INTO books(isbn, title, author, publisher, genre, published, pages) VALUES(@isbn, @title, @author, @publisher, @genre, @published, @pages)");
            else{
                Console.WriteLine("Wrong values count");
                return false;
            }
        }

        public static bool BorrowBook(MySqlParameter[] values){
            if(values.Length == 2)
                return ExecuteSql(values, "UPDATE books SET fk_user_id = @id WHERE isbn = @isbn AND fk_user_id IS NULL");
            else{
                Console.WriteLine("Wrong values count");
                return false;
            }
        }

        public static bool ReturnBook(MySqlParameter[] values){
            if(values.Length == 2)
                return ExecuteSql(values, "UPDATE books SET fk_user_id = NULL WHERE isbn = @isbn AND fk_user_id = @id");
            else{
                Console.WriteLine("Wrong values count");
                return false;
            }
        }

        private static bool ExecuteSql(MySqlParameter[] values, string sqlquery){
            try{
                using(var con = new MySqlConnection(constring))
                    using(var cmd = new MySqlCommand(sqlquery, con))
                    {
                        con.Open();
                        cmd.Parameters.AddRange(values);
                        cmd.Prepare();

                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Inserted {0} row(s)");
                    }
                return true;
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static string ExecuteSc(MySqlParameter[] values, string sqlquery){
            try{
                using(var con = new MySqlConnection(constring))
                    using(var cmd = new MySqlCommand(sqlquery, con))
                    {
                        con.Open();
                        cmd.Parameters.AddRange(values);
                        cmd.Prepare();

                        object obj = cmd.ExecuteScalar();
                        if(obj == null)
                            return null;
                        else
                            return cmd.ExecuteScalar().ToString();
                    }
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public static bool UserExists(MySqlParameter[] values){
            string sqlquery = "SELECT id FROM users WHERE id = @id";
            if (ExecuteSc(values, sqlquery) != null){
                return true;
            }
            return false;
        }

        public static bool CheckPass(MySqlParameter[] values){
            string sqlquery = "SELECT id FROM users WHERE id = @id AND password = @password";
            if (ExecuteSc(values, sqlquery) != null){
                return true;
            }
            return false;
        }

        private static string GetLine(int length){
            string line = "";
            for(int i = 0; i < length; i++){
                line += "-";
            }
            return line;
        }

        private static string GetPadding(int length){
            string line = "";
            for(int i = 0; i < length; i++){
                line += " ";
            }
            return line;
        }

        private static string PrintBox(List<string>[] buffer, int columns){
            string returnString = "\n";
            string lineString = "";
            int[] columnLength = new int[columns];
            int rowcount = 0;

            // get longest field for each row to set default width for columns
            for(int i = 0; i < columns; i++){
                foreach (var item in buffer[i]){
                    if(item.Length > columnLength[i])
                        columnLength[i] = item.Length;
                    if(i == 0)
                        rowcount++;
                }
                lineString += $"+{GetLine(columnLength[i]+4)}";
            }

            lineString += "+";

            for(int i = 0; i < rowcount; i++){
                if(i == 0 || i == 1){
                    returnString += lineString+"\n";
                }
                for(int j = 0; j < columns; j++){

                    returnString += $"| {buffer[j][i]}{GetPadding(columnLength[j]-buffer[j][i].Length+2)} ";

                    if(j == 6){
                        returnString += "|\n";
                    }
                }
            }
            return returnString + lineString + "\n";
        }

        private static string ExecuteRdr(MySqlParameter[] values, string sqlquery){
            try{
                using(var con = new MySqlConnection(constring))
                    using(var cmd = new MySqlCommand(sqlquery, con))
                    {
                        con.Open();
                        cmd.Parameters.AddRange(values);
                        cmd.Prepare();
                        using(MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            int columns = rdr.FieldCount;
                            List<string>[] buffer = new List<string>[columns];

                            for(int i = 0; i < columns; i++){
                                buffer[i] = new List<string>();
                            }

                            for (int i = 0; i < rdr.FieldCount; i++){
                                buffer[i].Add(rdr.GetName(i));
                            }

                            while (rdr.Read()){
                                for (int i = 0; i < rdr.FieldCount; i++){
                                    buffer[i].Add(rdr.GetString(i));
                                }
                            }
                            return PrintBox(buffer, columns);
                        }
                    }
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public static string GetAllBooks(){
            string sqlquery = "SELECT isbn, title, author, publisher, genre, published, pages FROM books WHERE fk_user_id IS NULL";

            try{
                using(var con = new MySqlConnection(constring))
                    using(var cmd = new MySqlCommand(sqlquery, con)){
                        con.Open();
                        using(MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            int columns = rdr.FieldCount;
                            List<string>[] buffer = new List<string>[columns];

                            for(int i = 0; i < columns; i++){
                                buffer[i] = new List<string>();
                            }

                            for (int i = 0; i < rdr.FieldCount; i++){
                                buffer[i].Add(rdr.GetName(i));
                            }

                            while (rdr.Read()){
                                for (int i = 0; i < rdr.FieldCount; i++){
                                    buffer[i].Add(rdr.GetString(i));
                                }
                            }
                            return PrintBox(buffer, columns);
                        }
                    }
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public static string GetUserBorrowed(MySqlParameter[] values){
            string sqlquery = "SELECT isbn, title, author, publisher, genre, published, pages FROM books WHERE fk_user_id = @id";
            try{
                using(var con = new MySqlConnection(constring))
                    using(var cmd = new MySqlCommand(sqlquery, con)){
                        con.Open();
                        cmd.Parameters.AddRange(values);
                        cmd.Prepare();
                        using(MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            int columns = rdr.FieldCount;
                            List<string>[] buffer = new List<string>[columns];

                            for(int i = 0; i < columns; i++){
                                buffer[i] = new List<string>();
                            }

                            for (int i = 0; i < rdr.FieldCount; i++){
                                buffer[i].Add(rdr.GetName(i));
                            }

                            while (rdr.Read()){
                                for (int i = 0; i < rdr.FieldCount; i++){
                                    buffer[i].Add(rdr.GetString(i));
                                }
                            }
                            return PrintBox(buffer, columns);
                        }
                    }
            }
            catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
            return null;
        }


        // create this shit maybe
        //string sqlquery = $"SELECT isbn, title, author, publisher, genre, published, pages FROM books WHERE {column} = {findby}"
        public static void GetUsers(){

        }
        public static void RemoveUser(){

        }
        public static void RemoveBook(){

        }
    }


    public class ClientCon{

        public TcpClient client;
        public NetworkStream stream;

        public ClientCon(TcpClient clientrec){
            client = clientrec;
            stream = clientrec.GetStream();
            Con();
        }
        public async void Con(){
            byte[] buffer;
            int id = 0;

            int readbytes;
            bool login = false;
            while(!login){
                buffer = new byte[5000];
                readbytes  = await stream.ReadAsync(buffer, 0, buffer.Length);
                id = Int32.Parse(Encoding.UTF8.GetString(buffer,0,readbytes));
                MySqlParameter[] paramId = new MySqlParameter[] {new MySqlParameter("@id", id)};
                Console.WriteLine("\n" + id);
                buffer = new byte[5000];
                readbytes  = await stream.ReadAsync(buffer, 0, buffer.Length);
                string pass = Encoding.UTF8.GetString(buffer,0,readbytes);
                MySqlParameter[] paramPass = new MySqlParameter[] {new MySqlParameter("@id", id), new MySqlParameter("@password", pass)};
                if(Sql.UserExists(paramId)){
                    if(Sql.CheckPass(paramPass)){
                        stream.Write(Encoding.UTF8.GetBytes("true"));
                        login = true;

                    }else{
                        Console.WriteLine("false send to client");
                        stream.Write(Encoding.UTF8.GetBytes("false"));
                    }
                }
            }
            if(login){
                bool running = true;
                bool success;
                do{
                string returnS = "";
                buffer = new byte[5000];
                readbytes  = await stream.ReadAsync(buffer, 0, buffer.Length);
                string cmd = Encoding.UTF8.GetString(buffer,0,readbytes);
                    switch(cmd[0])
                    {
                        case '1':
                            MySqlParameter[] param1 = new MySqlParameter[]{
                                new MySqlParameter("@id", id),
                                new MySqlParameter("@isbn", Int32.Parse(cmd.Substring(1)))};
                            success = Sql.BorrowBook(param1);
                            stream.Write(Encoding.UTF8.GetBytes(success.ToString()));
                            break;
                        case '2':
                            MySqlParameter[] param2 = new MySqlParameter[]{
                                new MySqlParameter("@id", id),
                                new MySqlParameter("@isbn", Int32.Parse(cmd.Substring(1)))};
                            success = Sql.ReturnBook(param2);
                            stream.Write(Encoding.UTF8.GetBytes(success.ToString()));
                            break;
                        case '3':
                            MySqlParameter[] param3 = new MySqlParameter[]{
                                new MySqlParameter("@id", id)};
                            returnS = Sql.GetUserBorrowed(param3);
                            stream.Write(Encoding.UTF8.GetBytes(returnS));
                            break;
                        case '4':
                            returnS = Sql.GetAllBooks();
                            stream.Write(Encoding.UTF8.GetBytes(returnS));
                            break;
                        case '5':
                            MySqlParameter[] param5 = new MySqlParameter[]{
                                new MySqlParameter("@id", id)};
                            Sql.RemoveUser(param5);
                            login = false;
                            break;
                        case '6':
                            login = false;
                            running = false;
                            break;
                        default:
                            break;
                    }
                }while(running);
            }
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
    }
}
