using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class BroadcastServers
    {
        public static void DiscoverServer(int broadcastPort, int serverPort)
        {
            UdpClient udpClient = new UdpClient(); // Создаем экземпляр класса UdpClient
            udpClient.EnableBroadcast = true;//true чтобы разрешить отправку широковещательных сообщений
            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
            byte[] broadcastBytes = Encoding.ASCII.GetBytes("SERVER"); 
            udpClient.Send(broadcastBytes, broadcastBytes.Length, broadcastEndPoint); // Отправляем байты через UdpClient методом Send()
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            byte[] receiveBytes = udpClient.Receive(ref serverEndPoint);// Получаем ответ от сервера
            string serverInfo = Encoding.ASCII.GetString(receiveBytes);

            Console.WriteLine($"Server discovered: {serverInfo}");

            udpClient.Close();
        }
    }
}
