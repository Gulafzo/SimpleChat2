using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Utilities;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var socket = ConnectClientToServer(new IPEndPoint(IPAddress.Loopback, 10111));

            var chatContent = ReceiveChatContent(socket);

            ShowChatContent(chatContent);

            var message = GetClientMessage();

            SendMessageToServer(socket, message);
            
            /*
             * Потенциально будет нужна в ходе дальнейшей разработки
             * В текущей версии строку ожидания Enter заменяет ожидание в
             * 1 секунду ниже
             */
            //WaitForEnterPressedToCloseApplication();

            DisconnectClientFromServer(socket);
            
            Thread.Sleep(TimeSpan.FromSeconds(1));
            
            DisposeClientSocket(socket);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
          
        }

        private static void DisposeClientSocket(Socket socket)
        {
            socket.Close();
            socket.Dispose();
        }

        private static void DisconnectClientFromServer(Socket socket)
        {
            socket.Disconnect(false);
            Console.WriteLine("Client disconnected from server");
        }

        private static void WaitForEnterPressedToCloseApplication()
        {
            Console.Write("Press [Enter] to close client console application");
            Console.ReadLine();
        }

        private static void SendMessageToServer(Socket socket, string message)
        {
            Console.WriteLine("Sending message to server");
            SocketUtility.SendString(socket, message,
                () => { Console.WriteLine($"Send string to server data check client side exception"); });
            Console.WriteLine("Message sent to server");
        }

        private static string GetClientMessage()
        {
            Console.Write("Your message:");
            var message = Console.ReadLine();
            return message;
        }

        private static void ShowChatContent(string chatContent)
        {
            Console.WriteLine("---------------Chat content--------------------");
            Console.WriteLine(chatContent);
            Console.WriteLine("------------End of chat content----------------");
            Console.WriteLine();
        }

        private static string ReceiveChatContent(Socket socket)
        {
            string chatContent = SocketUtility.ReceiveString(socket,
                () => { Console.WriteLine($"Receive string size check from server client side exception"); },
                () => { Console.WriteLine($"Receive string data check from server client side exception"); });
            return chatContent;
        }

        private static Socket ConnectClientToServer(IPEndPoint serverEndPoint)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);
            
            socket.Connect(serverEndPoint);

            Console.WriteLine($"Client connected Local {socket.LocalEndPoint} Remote {socket.RemoteEndPoint}");
            
            return socket;
        }


        //-------------------------------------
        public static void ShowServerMenu(List<string> servers)//метод принимает список серверов
        {
            Console.WriteLine("Available servers:");// Вывод доступных серверов
            for (int i = 0; i < servers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {servers[i]}");
            }

            int serverIndex = -1;
            while (serverIndex < 0 || serverIndex >= servers.Count)// Цикл запрашивает у пользователя номер сервера
            {
                Console.Write("Enter server number to connect: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int index))
                {
                    serverIndex = index - 1;
                }
            }

            string serverInfo = servers[serverIndex];
            string[] parts = serverInfo.Split(':');
            string serverAddress = parts[0];
            int serverPort = int.Parse(parts[1]);

            ConnectToServer(serverAddress, serverPort);// подключение по  адресу и порту
        }

        public static void ConnectToServer(string serverAddress, int serverPort)
        {
            TcpClient tcpClient = new TcpClient(serverAddress, serverPort);//подключение к серверу  
            
        }

        
    }
}
