//using System;
//using System.Collections.Generic;
////using System.Linq;
//using System.Net;
//using System.Text;
//using System.Net.Sockets;
//using System.Threading;

//namespace NetworkTest
//{
//    class Server
//    {
//        //Dictionary<string, Player> playerDictionary = new Dictionary<string, Player>();

//        static class GameData
//        {
//            public static Dictionary<string, Player> playerDictionary = new Dictionary<string, Player>();
//            public static Dictionary<int, GameRoom> GameRoomDictionary = new Dictionary<int, GameRoom>();
//            public static Dictionary<Player, NetworkStream> ConnectionList = new Dictionary<Player, NetworkStream>();
//            public static List<Player> GameRoomPlayersList = new List<Player>();
//            //public static List<string> ChangeQueue = new List<string>();
//            public static Queue<string> ChangeQueue = new Queue<string>();
//            public const int BUFFER_SIZE = 8192;
//        }

//        static void TestHandler(Object c)
//        {
//            TcpClient clientSocket = (TcpClient)c;
//            NetworkStream n = clientSocket.GetStream();

//            //try
//            //{
//            //    while(true)
//            //    {
//            //        Console.WriteLine(">> Trying to Read...");
//            //        byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
//            //        n.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
//            //        string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
//            //        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\r\n"));
//            //        Console.WriteLine(dataFromClient);

//            //        //string serverResponse = "Got this " + dataFromClient;
//            //        string serverResponse = "1";
//            //        serverResponse = serverResponse + "\r\n";
//            //        Console.WriteLine("My server response being sent is " + serverResponse);
//            //        Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
//            //        n.Write(sendBytes, 0, serverResponse.Length);
//            //        n.Flush();
//            //        Console.WriteLine("I think the ACK was sent.");
//            //    }
//            //}
//            //catch(Exception e)
//            //{
//            //    Console.WriteLine(">>>>> Exception Occurred.");
//            //}

//            //login
//            string str = ReadMessage(n);
//            WriteMessage("1", n);

//            //join lobby
//            str = ReadMessage(n);
//            WriteMessage("1", n);

//            //send update message room# host gamename gamestatus
//            WriteMessage("Update 1234 player1 thisgame 2", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1258 player2 thatgame 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1278 player3 bo 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1259 player4 we 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1298 player5 arg 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1251 player6 jyt 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1253 player7 wef 1", n);
//            str = ReadMessage(n);
//            WriteMessage("Update 1250 player8 ngfcv 1", n);
//            str = ReadMessage(n);

//            //try
//            //{
//            //    while (true)
//            //    {
//            //        Console.WriteLine(">> Trying to Read...");
//            //        byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
//            //        n.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
//            //        string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
//            //        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\r\n"));
//            //        Console.WriteLine(dataFromClient);

//            //        //string serverResponse = "Got this " + dataFromClient;
//            //        string serverResponse = "1";
//            //        serverResponse = serverResponse + "\r\n";
//            //        Console.WriteLine("My server response being sent is " + serverResponse);
//            //        Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
//            //        n.Write(sendBytes, 0, serverResponse.Length);
//            //        n.Flush();
//            //        Console.WriteLine("I think the ACK was sent.");
//            //    }
//            //}
//            //catch (Exception e)
//            //{
//            //    Console.WriteLine(">>>>> Exception Occurred.");
//            //}

//        }

//        static void WriteMessage(string msg, NetworkStream n)
//        {
//            Console.WriteLine(msg);
//            msg += "\r\n";
//            Byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
//            n.Write(sendBytes, 0, msg.Length);
//        }

//        static string ReadMessage(NetworkStream n)
//        {
//            byte[] bytesFrom = new byte[GameData.BUFFER_SIZE];
//            n.Read(bytesFrom, 0, GameData.BUFFER_SIZE);
//            return System.Text.Encoding.ASCII.GetString(bytesFrom);
//        }


//        /// <summary>
//        /// A function that handles each connection to a client
//        /// </summary>
//        /// <param name="c">The TcpClient object for the connection.</param>
//        static void ClientHandler(Object c)
//        {
//            TcpClient clientSocket = (TcpClient)c;
//            NetworkStream networkStream = clientSocket.GetStream();

//            //GameData.playerDictionary.Add("username", new Player("username", "password", 0));
//            Player curPlayer = null; ;

//            try
//            {
//                while (true)
//                {
//                    Console.WriteLine(" >> The while loop started.");
//                    byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
//                    Console.WriteLine("---------READ STARTED HERE");
//                    networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
//                    Console.WriteLine("---------READ HAS ENDED");
//                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
//                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\r\n"));
//                    //int sizeOfMessage = dataFromClient.IndexOf("\r\n");
//                    //Console.WriteLine("index of end is " + sizeOfMessage);
//                    Console.WriteLine(" >> Data from client - ");
//                    Console.WriteLine(dataFromClient);

//                    // breaking apart data from client
//                    string[] splitString = dataFromClient.Split();
//                    Console.WriteLine(splitString[0]);
//                    Console.WriteLine(splitString[1]);
//                    Console.WriteLine(splitString[2]);


//                    GameData.playerDictionary.TryGetValue(splitString[1], out curPlayer);

//                    string serverResponse = "";

//                    if(curPlayer != null)
//                    {
//                        if (curPlayer.Password == splitString[2])
//                        {
//                            Console.WriteLine("The password is a match for the username!");
//                            serverResponse = "1";
//                            //this is for register. If it's log in, it needs to get the player info from dictionary and set appropriately -----------------------------------------------
//                            //Player me = new Player(splitString[0], splitString[1], 0);
//                            GameData.ConnectionList.Add(curPlayer, networkStream);
//                            GameData.GameRoomPlayersList.Add(curPlayer);
//                        }
//                        else
//                        {
//                            Console.WriteLine("The password does not match.");
//                            serverResponse = "0";
//                        }
//                    }
//                    //remember to put \r\n at the end of the reply always!

//                    //string serverResponse = "Server Response is " + dataFromClient + "\r\n";
//                    serverResponse = serverResponse + "\r\n";
//                    Console.WriteLine("My server response being sent is " + serverResponse);
//                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
//                    networkStream.Write(sendBytes, 0, serverResponse.Length);
//                    networkStream.Flush();
//                    Console.WriteLine("I think the ACK was sent.");


//                    //if(curPlayer != null)
//                    //{
//                    //    Thread curThread = new Thread(ClientHandler);
//                    //    Console.WriteLine("I think a client has been connected.");
//                    //    curThread.Start(clientSocket);
//                    //}
//                }
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine("The exception occurred and the client socket is being closed.");
//                if(curPlayer != null)
//                {
//                    GameData.ConnectionList.Remove(curPlayer);
//                }
//                clientSocket.Close();
//            }




//            //string serverResponse = "Last Message from client " + dataFromClient;
//            //Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
//            //Console.WriteLine(sendBytes.Length);

//            //networkStream.Write(sendBytes, 0, sendBytes.Length);
//            //networkStream.Flush();
//            //Console.WriteLine(" >> " + serverResponse);

//            //clientSocket.Close();
//        }

//        static void ChangePusher()
//        {
//            Console.WriteLine(" >> The Change Pusher thread has started.");
//            while(true)
//            {
//                if(GameData.ConnectionList.Count > 0)
//                {
//                    if(GameData.ChangeQueue.Count > 0)
//                    {
//                        string change = GameData.ChangeQueue.Dequeue();
//                        foreach (Player player in GameData.GameRoomPlayersList)
//                        {
//                            NetworkStream n;
//                            GameData.ConnectionList.TryGetValue(player, out n);
//                            if (n != null)
//                            {
//                                Console.WriteLine("-- " + change + " to player " + player.Username);
//                                change = change + "\r\n";
//                                Byte[] sendBytes = Encoding.ASCII.GetBytes(change);
//                                n.Write(sendBytes, 0, change.Length);
//                                //n.Flush();
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        class Player
//        {
//            public string Username { get; set; }
//            public string Password { get; set; }
//            public int GlobalScore { get; set; }

//            public Player(string usr, string pswd, int score)
//            {
//                this.Username = usr;
//                this.Password = pswd;
//                this.GlobalScore = score;
//            }

//            public void addScore(int score)
//            {
//                //doesn't trash negative numbers. considers it subtracting. 
//                GlobalScore += score;
//            }
//        }

//        class GameRoom
//        {
//            public Player Host { get; set; }
//            public Player Opponent { get; set; }
//            public string GameName { get; set; }
//            public int RoomNumber { get; set; }
//            public bool hasOpponent { get; set; }

//            public GameRoom(Player player, string roomName, int number)
//            {
//                this.Host = player;
//                this.GameName = roomName;
//                this.RoomNumber = number;
//                this.hasOpponent = false;

//            }

//            public bool addOpponent(Player opp)
//            {
//                if (hasOpponent) return false;
//                if (opp == null) return false;
//                this.Opponent = opp;
//                this.hasOpponent = true;
//                return true;

//            }
//        }

//        static void Main(string[] args)
//        {

//            Console.WriteLine(" >> Main fuction started.");

//            //create a thread for updating game lists here

//            GameData.playerDictionary.Add("a305", new Player("a305", "godie123", 58));

//            //Player tempPlayer;
//            //string tempUsername = "a305";
//            //GameData.playerDictionary.TryGetValue(tempUsername, out tempPlayer);
//            //Console.WriteLine(tempPlayer.password);
//            //Console.WriteLine(tempPlayer.globalScore);
//            //tempPlayer.addScore(45);
//            //Console.WriteLine(tempPlayer.globalScore);


//            //IPAddress ipAddress = IPAddress.Parse("205.175.106.230");
//            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

//            //TcpListener serverSocket = new TcpListener(IPAddress.Any, 2018);
//            TcpListener serverSocket = new TcpListener(localIPs[1], 8888);
//            int requestCount = 0;
//            serverSocket.Start();
//            Console.WriteLine(" >> Server Started");

//            requestCount = 0;

//            //Thread createClient = new Thread(CreateNewClient);
//            //createClient.Start(serverSocket);

//            //Thread changesThread = new Thread(ChangePusher);
//            //changesThread.Start();

//            //GameData.ChangeQueue.Enqueue("This is change 1 from the queue.");
//            //GameData.ChangeQueue.Enqueue("This is change 2 from the queue.");
//            //GameData.ChangeQueue.Enqueue("This is change 3 from the queue.");
//            //GameData.ChangeQueue.Enqueue("This is change 4 from the queue.");
//            //GameData.ChangeQueue.Enqueue("This is change 5 from the queue.");
//            //GameData.ChangeQueue.Enqueue("This is change 6 from the queue.");


//            while (true)
//            {
//                try
//                {
//                    TcpClient clientSocket = serverSocket.AcceptTcpClient();
//                    Console.WriteLine(" >> Accept connection from client");
//                    requestCount = requestCount + 1;

//                    Thread t = new Thread(TestHandler);
//                    t.Start(clientSocket);

//                    //Thread curThread = new Thread(ClientHandler);
//                    //Console.WriteLine("I think a client has been connected.");
//                    //curThread.Start(clientSocket);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.ToString());
//                    return;
//                }
//            }
//        }

//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace NetworkTest
{
    class Program
    {

        static Object LockObject = new object();

        public static class GameData
        {
            public static Dictionary<string, Player> PlayerDictionary = new Dictionary<string, Player>();
            public static Dictionary<int, GameRoom> GameDictionary = new Dictionary<int, GameRoom>();
            public static Dictionary<Player, NetworkStream> ConnectionList = new Dictionary<Player, NetworkStream>();
            //public static Dictionary<string, Func<string, bool>> FunctionDictionary = new Dictionary<string, Func<string, bool>>();
            public static List<Player> LobbyPlayersList = new List<Player>();
            public static Queue<string> ChangeQueue = new Queue<string>();
            public const int BUFFER_SIZE = 8192;
            public static int RoomNumberCounter = 0;
        }

        public class Player
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public int GlobalScore { get; set; }

            public Player(string user, string pswd, int score)
            {
                this.Username = user;
                this.Password = pswd;
                this.GlobalScore = score;
            }

            public void AddScore(int score)
            {
                GlobalScore += score;
            }
        }

        public class GameRoom
        {
            public Player Host { get; set; }
            public Player Opponent { get; set; }
            public string GameName { get; set; }
            public int RoomNumber { get; set; }
            public bool HasOpponent { get; set; }
            public int GameStatus { get; set; }
            public bool HostReady { get; set; }
            public bool OppReady { get; set; }

            public GameRoom(Player player, string roomName, int roomNum)
            {
                this.Host = player;
                this.GameName = roomName;
                this.RoomNumber = roomNum;
                this.HasOpponent = false;
                this.GameStatus = 1;
                this.HostReady = false;
                this.OppReady = false;
            }

            public bool AddOpponent(Player opp)
            {
                if (HasOpponent) return false;
                if (opp == null) return false;
                this.Opponent = opp;
                this.HasOpponent = true;
                this.GameStatus = 2;
                return true;
            }
        }

        static void ClientHandler(Object c)
        {
            TcpClient clientSocket = (TcpClient)c;
            NetworkStream networkStream = clientSocket.GetStream();
            //System.Object lockThis = new System.Object();

        Player curPlayer = null;
            while (true)
            {
                try
                {
                    if (curPlayer != null)
                    {
                        Console.WriteLine(curPlayer.Username + " >> Trying to read next command");
                    }
                    else
                    {
                        Console.WriteLine(" >> Trying to read next command");
                    }

                    Console.WriteLine("Thread username is " + Thread.CurrentThread.ManagedThreadId);
                    //string message = ReadMessage(networkStream);
                    string message = ReadAsyncMessage(networkStream);
                    if(message == null || message == "")
                    {
                        continue;
                    }
                    if (message == "0" || message == "1") goto end_of_loop;
                    string command = message.Substring(0, message.IndexOf(' '));
                    string parameters = message.Substring(message.IndexOf(' ') + 1);
                    string success;
                    if (command == "reg")
                    {
                        success = Register(parameters);
                        WriteMessage(success, networkStream);
                        if (GameData.PlayerDictionary.TryGetValue(parameters.Substring(0, parameters.IndexOf(' ')), out curPlayer))
                        {
                            if (GameData.ConnectionList.ContainsKey(curPlayer))
                            {
                                GameData.ConnectionList.Remove(curPlayer);
                            }
                            GameData.ConnectionList.Add(curPlayer, networkStream);
                        }

                        goto end_of_loop;
                    }
                    if (command == "login")
                    {
                        success = Login(parameters);
                        WriteMessage(success, networkStream);
                        //WriteMessage(success, networkStream);
                        GameData.PlayerDictionary.TryGetValue(parameters.Substring(0, parameters.IndexOf(' ')), out curPlayer);
                        //Console.WriteLine("Player username is " + curPlayer.Username);
                        if (GameData.ConnectionList.ContainsKey(curPlayer))
                        {
                            GameData.ConnectionList.Remove(curPlayer);
                        }
                        GameData.ConnectionList.Add(curPlayer, networkStream);
                        goto end_of_loop;
                    }
                    if (command == "joinlobby")
                    {
                        success = EnterLobby(parameters);
                        WriteMessage(success, networkStream);
                        GameData.LobbyPlayersList.Add(curPlayer);
                        //NetworkStream n;
                        //GameData.ConnectionList.TryGetValue(curPlayer, out n);
                        for (int i = 0; i < GameData.GameDictionary.Count; i++)
                        {
                            WriteMessage($"gamelist update {GameData.GameDictionary.ElementAt(i).Value.RoomNumber}" +
                                $" {GameData.GameDictionary.ElementAt(i).Value.Host.Username}" +
                                $" {GameData.GameDictionary.ElementAt(i).Value.GameName}" +
                                $" {GameData.GameDictionary.ElementAt(i).Value.GameStatus}",
                                networkStream);
                        }
                        // send all data 
                        //foreach (KeyValuePair<int, GameRoom> game in GameData.GameDictionary)
                        //{
                        //    WriteMessage($"gamelist update {game.Key} {game.Value.Host.Username} {game.Value.GameName} {game.Value.GameStatus}", networkStream);
                        //}
                        goto end_of_loop;
                    }
                    if (command == "creategame")
                    {
                        success = CreateGame(parameters);
                        WriteMessage(success, networkStream);
                        GameData.PlayerDictionary.TryGetValue(parameters.Substring(0, parameters.IndexOf(' ')), out curPlayer);
                        //GameData.ConnectionList.Remove(curPlayer);
                        goto end_of_loop;
                    }
                    if (command == "globalchat")
                    {
                        success = SendGlobalChat(parameters);
                        WriteMessage(success, networkStream);
                        goto end_of_loop;
                    }
                    if (command == "joingame")
                    {
                        success = JoinGame(parameters);
                        WriteMessage(success, networkStream);
                        GameData.PlayerDictionary.TryGetValue(parameters.Substring(0, parameters.IndexOf(' ')), out curPlayer);
                        //GameData.ConnectionList.Remove(curPlayer);
                        goto end_of_loop;
                    }
                    if (command == "exitlobby")
                    {
                        success = ExitLobby(parameters);
                        WriteMessage(success, networkStream);
                        GameData.PlayerDictionary.TryGetValue(parameters.Substring(0, parameters.IndexOf(' ')), out curPlayer);
                        //GameData.ConnectionList.Remove(curPlayer);
                        clientSocket.Close();
                        return;
                        //goto end_of_loop;
                    }
                    if (command == "gameready")
                    {
                        success = GameReady(parameters);
                        //WriteMessage(success, networkStream);
                        string[] param = parameters.Split();
                        GameRoom tempRoom;
                        if (GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
                        {
                            if (tempRoom.Host == curPlayer)
                            {
                                //NetworkStream n;
                                //GameData.ConnectionList.TryGetValue(tempRoom.Host, out n);
                                GameData.ConnectionList.Remove(curPlayer);
                                GameData.ConnectionList.Add(curPlayer, networkStream);
                                WriteMessage("makemove", networkStream);
                                
                            }
                            if (tempRoom.Opponent == curPlayer)
                            {
                                GameData.ConnectionList.Remove(curPlayer);
                                GameData.ConnectionList.Add(curPlayer, networkStream);
                                //WriteMessage(success, networkStream);
                            }
                        }
                        goto end_of_loop;
                    }
                    if (command == "clearmessage")
                    {

                        ReadMessage(networkStream);
                        WriteMessage("1", networkStream);
                        goto end_of_loop;
                    }
                    if(command == "unregister")
                    {
                        GameData.PlayerDictionary.Remove(curPlayer.Username);
                        return;
                    }

                    if(command == "fire")
                    {
                        //param is username roomnum letter number
                        string[] param = parameters.Split();
                        GameRoom tempRoom;
                        if(GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
                        {
                            if(param[0] == tempRoom.Host.Username)
                            {
                                NetworkStream oppNetwork;
                                GameData.ConnectionList.TryGetValue(tempRoom.Opponent, out oppNetwork);
                                if (oppNetwork == null) Console.WriteLine("The opponent network is null");

                                WriteMessage($"fire {param[2]} {param[3]}", oppNetwork);
                            }
                            else
                            {
                                NetworkStream hostNetwork;
                                GameData.ConnectionList.TryGetValue(tempRoom.Host, out hostNetwork);
                                if (hostNetwork == null) Console.WriteLine("The host network is null");

                                WriteMessage($"fire {param[2]} {param[3]}", hostNetwork);
                            }
                        }
                        goto end_of_loop;
                    }
                    if(command == "move")
                    {
                        //param is username roomnum hit/miss cont/lose
                        string[] param = parameters.Split();

                        //if(param[0] == "hit")
                        //{
                        //    if(param[1] == "continue")
                        //    {
                        //        WriteMessage(parameters, networkStream);
                        //    }
                        //}
                        GameRoom tempRoom;
                        if (GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
                        {
                            NetworkStream oppNetwork;
                            GameData.ConnectionList.TryGetValue(tempRoom.Opponent, out oppNetwork);
                            NetworkStream hostNetwork;
                            GameData.ConnectionList.TryGetValue(tempRoom.Host, out hostNetwork);
                            if (param[0] == tempRoom.Host.Username)
                            {
                                
                                if (oppNetwork == null) Console.WriteLine("The opponent network is null");
                                if (param[2] == "hit")
                                {
                                    if(param[3] == "continue")
                                    {
                                        
                                        WriteMessage($"move {param[2]} {param[3]}", oppNetwork);
                                        
                                        WriteMessage($"makemove", hostNetwork);
                                        ReadMessage(oppNetwork);
                                    }
                                    else
                                    {
                                        //player lost, let other person know they won
                                        WriteMessage($"move hit win", oppNetwork);
                                        WriteMessage($"{tempRoom.Host.GlobalScore}", hostNetwork);
                                    }
                                }
                                else
                                {
                                    //it is a miss
                                    WriteMessage($"move {param[2]} {param[3]}", oppNetwork);
                                    
                                    WriteMessage("makemove", hostNetwork);
                                    ReadMessage(oppNetwork);
                                }
                            }
                            else
                            {
                                //you're the opponent
                                if (hostNetwork == null) Console.WriteLine("The host network is null");
                                if (param[2] == "hit")
                                {
                                    if (param[3] == "continue")
                                    {

                                        WriteMessage($"move {param[2]} {param[3]}", hostNetwork);
                                        
                                        WriteMessage($"makemove", oppNetwork);
                                        ReadMessage(hostNetwork);
                                    }
                                    else
                                    {
                                        WriteMessage($"move hit win", hostNetwork);
                                        WriteMessage($"{curPlayer.GlobalScore}", oppNetwork);
                                    }
                                }
                                else
                                {
                                    //it is a miss
                                    WriteMessage($"move {param[2]} {param[3]}", hostNetwork);
                                    
                                    WriteMessage($"makemove", oppNetwork);
                                    ReadMessage(hostNetwork);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("The room wasn't foundddddddddddddddd");
                        }
                        goto end_of_loop;
                    }
                    if(command == "localscore")
                    {
                        // param is username score
                        string[] param = parameters.Split();

                        Player thisPlayer;
                        GameData.PlayerDictionary.TryGetValue(param[0], out thisPlayer);
                        thisPlayer.AddScore(Int32.Parse(param[1]));
                        WriteMessage(thisPlayer.GlobalScore + "", networkStream);
                        goto end_of_loop;
                    }
                    if(command == "getleaderboard")
                    {
                        WriteMessage("", networkStream);
                        goto end_of_loop;
                    }
                    if(command == "unregister")
                    {

                    }


                    end_of_loop:
                        continue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" >> Exception occurred and client socket is being closed.");
                        if (curPlayer != null)
                        {
                            GameData.LobbyPlayersList.Remove(curPlayer);
                            GameData.ConnectionList.Remove(curPlayer);
                            Console.WriteLine($"The player that caused exception is {curPlayer.Username}");
                        }
                        Console.WriteLine(e.StackTrace);
                        clientSocket.Close();
                        return;
                    }
                
            }
        }



        public static void ChangePusher()
        {
            Console.WriteLine(" >> The change pusher thread has started");
            //while(true)
            //{
            //Console.WriteLine(" >>> ChangePusher while loop started.");
            if (GameData.ConnectionList.Count > 0)
            {
                //Console.WriteLine(" >>> The connection list is not empty.");
                if (GameData.ChangeQueue.Count > 0)
                {
                    //Console.WriteLine($"The change queue has {GameData.ChangeQueue.Count} elements inside.");
                    string change = GameData.ChangeQueue.Dequeue();
                    //foreach(Player player in GameData.LobbyPlayersList)
                    //{
                    //    NetworkStream n;
                    //    GameData.ConnectionList.TryGetValue(player, out n);
                    //    if(n != null)
                    //    {
                    //        if(GameData.LobbyPlayersList.Contains(player))
                    //        {
                    //            Console.WriteLine($"--{change} to player {player.Username}");

                    //            WriteMessage(change, n);
                    //            string clientResponse = ReadMessage(n); //don't need this information much
                    //            //ReadMessage(n);
                    //        }
                    //    }
                    //}
                    for (int i = 0; i < GameData.LobbyPlayersList.Count; i++)
                    {
                        NetworkStream n;
                        Player player;
                        GameData.PlayerDictionary.TryGetValue(GameData.LobbyPlayersList.ElementAt(i).Username, out player);
                        GameData.ConnectionList.TryGetValue(player, out n);
                        if (n != null)
                        {
                            Console.WriteLine("Connection is not null and trying to push change.");

                            if (GameData.LobbyPlayersList.Contains(player))
                            {
                                Console.WriteLine($"--{change} to player {player.Username}");

                                WriteMessage(change, n);
                                Console.WriteLine(player.Username + " Change pushed.");
                                Console.WriteLine("Change pushed message again.");
                                string res = ReadMessage(n);
                                Console.WriteLine("Message finished reading in change pusher.");
                                Console.WriteLine(player.Username + " the push change response is " + res);
                            }
                        }
                    }
                }
            }
            //}
        }

        static void WriteMessage(string msg, NetworkStream n)
        {
            try
            {
                Console.WriteLine("Writing message " + msg);
                msg += "\r\n";
                Byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
                if (n == null) Console.WriteLine("The networkstream is null.");
                else
                {
                    Console.WriteLine("The networkstream is not null.");
                }
                n.Write(sendBytes, 0, msg.Length);
                Console.WriteLine("Finished writing message" + msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("Write Exception happeneddddddddddd");
                Console.WriteLine(e.StackTrace);
            }
        }

        static string ReadMessage(NetworkStream n)
        {
            try
            {
                Console.WriteLine("Trying to read...");
                Console.WriteLine("Thread id is " + Thread.CurrentThread.ManagedThreadId);

                byte[] bytesFrom = new byte[GameData.BUFFER_SIZE];
                n.Read(bytesFrom, 0, GameData.BUFFER_SIZE);
                byte k = 3;
                for (int i = 0; i < GameData.BUFFER_SIZE; i++)
                    bytesFrom[i] = (byte)(bytesFrom[i] - k);
                string msg = System.Text.Encoding.ASCII.GetString(bytesFrom);
                Console.WriteLine("The message is" + msg);
                msg = msg.Substring(0, msg.IndexOf("\r\n"));
                //msg = msg.Substring(0, msg.Length - 2); //this should remove \r\n
                Console.WriteLine("Read message " + msg);
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine("Read Exception happenedddddddddddd");
                Console.WriteLine(e.StackTrace);
                return "";
            }
        }

        static string ReadAsyncMessage(NetworkStream n)
        {
            try
            {
                Console.WriteLine("Trying to read...");
                Console.WriteLine("Thread id is " + Thread.CurrentThread.ManagedThreadId);
                byte[] bytesFrom = new byte[GameData.BUFFER_SIZE];
                while (n.DataAvailable == false);
                n.ReadAsync(bytesFrom, 0, GameData.BUFFER_SIZE);
                byte k = 3;
                for (int i = 0; i < GameData.BUFFER_SIZE; i++)
                    bytesFrom[i] = (byte)(bytesFrom[i] - k);
                string msg = "";
                msg = System.Text.Encoding.ASCII.GetString(bytesFrom);
                
                msg = msg.Substring(0, msg.IndexOf("\r\n"));
                //msg = msg.Substring(0, msg.Length - 2); //this should remove \r\n
                Console.WriteLine("Read message " + msg);
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine("Read Exception happenedddddddddddd");
                Console.WriteLine(e.StackTrace);
                return "";
            }
        }



        static void Main(string[] args)
        {
            Console.WriteLine(" >> Main function started.");

            // just initialized player for testing purposes
            //GameData.PlayerDictionary.Add("a305", new Player("a305", "godie123", 58));

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            TcpListener serverSocket = new TcpListener(localIPs[1], 8888);
            serverSocket.Start();
            Console.WriteLine(" >> Server Started");

            // populate function dictionary
            //GameData.FunctionDictionary.Add();


            // start queue pusher thread right away
            //Thread changesThread = new Thread(ChangePusher);
            //changesThread.Start();

            while (true)
            {
                try
                {
                    Console.WriteLine("The connection while loop happened.");
                    TcpClient clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> Accepted connection from client");
                    Thread curThread = new Thread(ClientHandler);
                    curThread.Start(clientSocket);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.ReadKey();
                    return;
                }
            }

        }



        static public string Register(string msg)
        {
            Console.WriteLine("Registration happened.");
            //msg is username password
            string[] param = msg.Split();
            if (param.Length < 2) return "1";

            //check if username already exists
            if (GameData.PlayerDictionary.ContainsKey(param[0]))
            {
                return "0";
            }

            // add new player to PlayerDictionary
            Player newPlayer = new Player(param[0], param[1], 0);
            GameData.PlayerDictionary.Add(param[0], newPlayer);
            Console.WriteLine("Param[0] is " + param[0]);
            Console.WriteLine("Param[1] is " + param[1]);
            return "1";
        }

        static public string Login(string msg)
        {
            //msg is username password
            string[] param = msg.Split();
            if (param.Length < 2) return "0";

            if (GameData.PlayerDictionary.ContainsKey(param[0]))
            {
                Player tempPlayer;
                GameData.PlayerDictionary.TryGetValue(param[0], out tempPlayer);
                if (param[1] == tempPlayer.Password)
                {
                    return "1";
                }
            }
            return "0";
        }

        static public string EnterLobby(string msg)
        {
            //msg is just username
            Player tempPlayer;
            GameData.PlayerDictionary.TryGetValue(msg, out tempPlayer);
            if (tempPlayer != null)
            {
                if (GameData.LobbyPlayersList.Contains(tempPlayer)) return "0";
                //NetworkStream n;
                //GameData.ConnectionList.TryGetValue(tempPlayer, out n);
                //for (int i = 0; i < GameData.GameDictionary.Count; i++)
                //{
                //    WriteMessage($"gamelist update {GameData.GameDictionary.ElementAt(i).Value.RoomNumber}" +
                //        $" {GameData.GameDictionary.ElementAt(i).Value.Host.Username}" +
                //        $" {GameData.GameDictionary.ElementAt(i).Value.GameName}" +
                //        $" {GameData.GameDictionary.ElementAt(i).Value.GameStatus}",
                //        n);
                //}
                //GameData.LobbyPlayersList.Add(tempPlayer);
                return "1";
            }
            return "0";
        }

        static public string CreateGame(string msg)
        {
            //msg is host gamename
            string[] param = msg.Split();
            Player creator;
            GameData.PlayerDictionary.TryGetValue(param[0], out creator);
            if (creator == null) return "0";
            int roomNumber = ++GameData.RoomNumberCounter;
            GameRoom room = new GameRoom(creator, param[1], roomNumber);
            GameData.GameDictionary.Add(roomNumber, room);

            //GameData.ChangeQueue.Enqueue($"gamelist update {roomNumber} {room.Host.Username} {room.GameName} {room.GameStatus}");
            //ChangePusher();

            return "1";
        }

        static public string SendGlobalChat(string msg)
        {
            //GameData.ChangeQueue.Enqueue($"globalchat {msg}");
            //ChangePusher();
            return "1";
        }

        static public string JoinGame(string msg)
        {
            //msg is username roomnumber
            string[] param = msg.Split();
            GameRoom tempRoom;
            if (GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
            {
                if (tempRoom.HasOpponent) return "0";

                Player opp;
                if (GameData.PlayerDictionary.TryGetValue(param[0], out opp))
                {
                    tempRoom.AddOpponent(opp);
                    return "1";
                }
            }
            return "0";
        }

        //it's like logging out
        static public string ExitLobby(string msg)
        {
            // msg is username
            Player tempPlayer;
            if (GameData.PlayerDictionary.TryGetValue(msg, out tempPlayer))
            {
                GameData.LobbyPlayersList.Remove(tempPlayer);
                //GameData.ConnectionList.Remove(tempPlayer);
                return "1";
            }
            return "0";
        }

        static public string GameReady(string msg)
        {
            //msg is username roomnumber
            string[] param = msg.Split();
            GameRoom tempRoom;
            if (GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
            {
                if (tempRoom == null)
                {
                    Console.WriteLine(">>>>>>>>>>>>>>game room is null");
                }
                if (tempRoom.Opponent == null)
                {
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>opponent is null");
                }
                if (param[0] == tempRoom.Host.Username)
                {
                    tempRoom.HostReady = true;
                    while (true)
                    {
                        if (tempRoom.OppReady)
                        {
                            return "1";
                        }
                    }
                }
                else
                {
                    if (param[0] == tempRoom.Opponent.Username)
                    {
                        tempRoom.OppReady = true;
                        while (true)
                        {
                            if (tempRoom.HostReady)
                            {
                                return "1";
                            }
                        }
                    }
                }
            }
            return "0";
        }

        //static public string FireMove(string[] param)
        //{
        //    //msg is username roomnum letter number
        //    //going to split it outside for this case
        //    GameRoom tempRoom;
        //    if(GameData.GameDictionary.TryGetValue(Int32.Parse(param[1]), out tempRoom))
        //    {

        //    }

        //    return "";
        //}

        static public void GameOver(Player player, NetworkStream networkStream)
        {
            WriteMessage($"{player.GlobalScore}", networkStream);
            ReadMessage(networkStream); //get leaderboard request
            List<Player> players = new List<Player>();
            foreach (var p in GameData.PlayerDictionary)
            {
                players.Add(p.Value);
            }
            List<Player> SortedList = players.OrderByDescending(o => o.GlobalScore).ToList<Player>();
            if (SortedList.Count < 10)
            {
                WriteMessage($"{SortedList.Count}", networkStream);
                ReadMessage(networkStream); //just acknowledgement
                for (int i = 0; i < SortedList.Count; i++)
                {
                    WriteMessage($"{SortedList.ElementAt(i).Username} {SortedList.ElementAt(i).GlobalScore}", networkStream);
                    ReadMessage(networkStream); // just acknowledgement
                }
            }
            else
            {
                WriteMessage("10", networkStream);
                ReadMessage(networkStream); //just acknowledgement
                for (int i = 0; i < 10; i++)
                {
                    WriteMessage($"{SortedList.ElementAt(i).Username} {SortedList.ElementAt(i).GlobalScore}", networkStream);
                    ReadMessage(networkStream); // just acknowledgement
                }
            }
        }


    }
}