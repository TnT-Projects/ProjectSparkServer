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

            if(true)
            {
                byte[] response = Encoding.ASCII.GetBytes("MESSAGE");
                currentSocket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(SendCallBack), currentSocket);
                if (serverResponse != null)
                {
                    serverResponse("Message Sent: " + Encoding.ASCII.GetString(response));
                }
            }

        }

        private void SendCallBack(IAsyncResult ar)
        {
            ((Socket)ar.AsyncState).EndSend(ar);
        }

    }
}
