using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSparkServer
{
    public delegate void ServerResponse(string message);
    class Server
    {
        private Socket _serverSocket;
        public event ServerResponse serverResponse;
        public Server()
        {
            this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //serverResponse("Server Started");
        }

        public void StartServer()
        {
            serverResponse("Server Started");
        }
    }
}
