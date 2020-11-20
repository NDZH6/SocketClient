using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string strIPAddress = "", strPort = "";
            IPAddress ipAddress = null;
            int nPort;

            try
            {
                Console.WriteLine("IP del server: ");
                strIPAddress = Console.ReadLine();

                Console.WriteLine("Porta del server: ");
                strPort = Console.ReadLine();

                if (!IPAddress.TryParse(strIPAddress.Trim(), out ipAddress))
                {
                    Console.WriteLine("IP non valido!");
                    return;
                }

                if (!int.TryParse(strPort, out nPort))
                {
                    Console.WriteLine("Numero porta non valido!");
                    return;
                }

                if (nPort <= 0 || nPort >= 65535)
                {
                    Console.WriteLine("Numero porta non valido!");
                    return;
                }

                Console.WriteLine("EndPoint del server: " + ipAddress.ToString() + " " + nPort);

                client.Connect(ipAddress, nPort);

                byte[] buff = new byte[128];
                string sendString = "", receiveString = "";
                int receivedBytes = 0;
                while (true)
                {
                    Console.WriteLine("Manda un messaggio: ");

                    sendString = Console.ReadLine();

                    buff = Encoding.ASCII.GetBytes(sendString);

                    client.Send(buff);

                    Console.WriteLine();

                    if (sendString.ToUpper().Trim() == "QUIT")
                    {
                        break;
                    }

                    Array.Clear(buff, 0, buff.Length);

                    receivedBytes = client.Receive(buff);

                    receiveString = Encoding.ASCII.GetString(buff, 0, receivedBytes);

                    Console.WriteLine("Server: " + receiveString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
