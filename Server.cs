using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSparkServer
{
    public delegate void ServerResponse(string message);
    class Server
    {
        private byte[] _buffer;
        private List<Socket> _clientSocket;
        private Socket _serverSocket;
        public event ServerResponse serverResponse;
        public Server()
        {
            this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._clientSocket = new List<Socket>();
            //serverResponse("Server Started");
        }

        public void StartServer()
        {
            if (serverResponse != null)
            {
                serverResponse("Setting up server...");
            }
            this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (serverResponse != null)
            {
                serverResponse("Setting up server socket...");
            }
            this._clientSocket = new List<Socket>();
            if (serverResponse != null)
            {
                serverResponse("Setting up client socket list...");
            }
            _buffer = new byte[1024];
            if (serverResponse != null)
            {
                serverResponse("Setting up buffer...");
            }
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            if (serverResponse != null)
            {
                serverResponse("Setting up bindings...");
            }
            _serverSocket.Listen(10);
            if (serverResponse != null)
            {
                serverResponse("Server started...");
            }
            StartListening();
        }

        private void StartListening()
        {
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            if (serverResponse != null)
            {
                serverResponse("Server Listening...");
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket currentSocket = _serverSocket.EndAccept(ar);
            _clientSocket.Add(currentSocket);
            if (serverResponse != null)
            {
                serverResponse("Client connected... " + currentSocket.LocalEndPoint.ToString());
            }
            currentSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), currentSocket);
            if (serverResponse != null)
            {
                serverResponse("Receiving from client... " + currentSocket.LocalEndPoint.ToString());
            }
            StartListening();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket currentSocket = (Socket)ar.AsyncState;
            int received = currentSocket.EndReceive(ar);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);
            if (serverResponse != null)
            {
                serverResponse("Message Reveived: " + Encoding.ASCII.GetString(dataBuffer));
            }
            //Check the message receive and anwser to that message
            if (Encoding.ASCII.GetString(dataBuffer).Equals("RQ:T"))
            {
                List<string> tafels = new List<string>();
                //id, name, top, right, bottom, left
                tafels.Add("1|TAFEL 1|135|15|0|0");
                tafels.Add("2|TAFEL 2|15|250|0|0");
                tafels.Add("3|TAFEL 3|35|200|0|0");
                tafels.Add("4|TAFEL 4|861|218|0|0");


                foreach (string table in tafels)
                {
                    byte[] response = Encoding.ASCII.GetBytes("T|"+table);
                    currentSocket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(SendCallBack), currentSocket);
                    if (serverResponse != null)
                    {
                        serverResponse("Message Sent: " + Encoding.ASCII.GetString(response));
                    }
                }

            }
            else
            {
                byte[] response = Encoding.ASCII.GetBytes("INVALID MESSAGE");
                currentSocket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(SendCallBack), currentSocket);
                if (serverResponse != null)
                {
                    serverResponse("Message Sent: " + Encoding.ASCII.GetString(response));
                }
            }
            currentSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), currentSocket);
            if (serverResponse != null)
            {
                serverResponse("Receiving from client... " + currentSocket.LocalEndPoint.ToString());
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            ((Socket)ar.AsyncState).EndSend(ar);
        }

        public bool ServerStarted()
        {
            if (this._serverSocket != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
