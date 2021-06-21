using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MinMax_Algorithm
{
    /// <summary>
    /// Clase que encapsula una conexión TCP a una dirección IP remota mediante un Socket.
    /// </summary>
    class Connection
    {
        public string RemoteIPAddress;
        public int RemotePort;
        public Socket RemoteSocket;
        private int Listen;

        // Constructor.
        public Connection() {}

        /// <summary>
        /// Método que establece la dirección IP a la cual se desea conectar.
        /// </summary>
        /// <param name="address">La dirección IP dentro de una cadena (i.e. "127.0.0.1").</param>
        /// <param name="port">El puerto al cual se envían los datos.</param>
        public void SetAddress(string address, int port)
        {
            RemoteIPAddress = address;
            RemotePort = port;
            Listen = 0;
        }

        public void Set_Listen(int Lis)
        {
            Listen= Lis;
        }

        /// <summary>
        /// Función que intenta establecer la conexión mediante un Socket utilizando
        /// la dirección y el puerto establecidos anteriormente.
        /// </summary>
        /// <returns>Devuelve una cadena vacía si la conexión se realizó con éxito
        /// o una cadena describiendo el error si hubo alguno.</returns>
        public string Connect()
        {
            // Verificar si los parámetros de conexión ya han sido establecidos.
            if ((this.RemoteIPAddress == "") || (this.RemotePort < 1))
            {
                return "Error: no se han definido los parámetros de conexión (IP:Puerto).";
            }

            // Crear el socket y el endpoint a partir de los parámetros de conexión.
            RemoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            IPAddress RemoteAddress = System.Net.IPAddress.Parse(this.RemoteIPAddress);
            IPEndPoint RemEndPoint = new IPEndPoint(RemoteAddress, this.RemotePort);

            if (Listen == 100)
            {
                try
                {
                    RemoteSocket.Bind(RemEndPoint);
                    RemoteSocket.Listen(1000);
                        //.Bind(RemEndPoint);
                    RemoteSocket = RemoteSocket.Accept();
                }
                catch (SocketException se)
                {
                    return se.ErrorCode.ToString();
                }
            }
            else
            {
                try
                {
                    // Intentar establecer una conexión con el endpoint remoto.
                    RemoteSocket.Connect(RemEndPoint);
                }
                catch (SocketException se)
                {
                    // Devolver el código de error de C# con su descripción.
                    return "Error: " + se.ErrorCode + "\n" + se.Message + "\n\n";
                }
            }

            /*if (Listen == 100)
            {
                RemoteSocket.Bind(RemEndPoint);
                RemoteSocket.Listen(100);
                Socket handler = RemoteSocket.Accept();
                //RemoteSocket = RemoteSocket.Accept();
            }*/

            // Devolver una cadena vacía, indicando una conexión exitosa.
            return "";
        }

        public void Disconnect()
        {
            if (RemoteSocket.Connected == true)
            {
                try
                {
                    RemoteSocket.Disconnect(false);
                    RemoteSocket.Close();
                }
                catch { }
            }
        }

        public byte[] GetReceivedData()
        {
            int recv;
            byte[] data = new byte[2]; 
            recv = this.RemoteSocket.Receive(data);
            return data;
        }

        public bool SendData(string Data)
        {
          
            int sent;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            sent = this.RemoteSocket.Send(enc.GetBytes(Data));
            if (sent == Data.Length)
                return true;
            else
                return false;
        }
    }

}
