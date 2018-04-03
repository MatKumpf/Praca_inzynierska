using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class Connection : CommInterface
    {
        public Connection(int port)
        {
            _port = port;
        }

        public bool Connect(IPAddress ipAddress, int waitTime)
        {
            try
            {
                Client = new TcpClient();
                Client.Connect(ipAddress, _port);
                _netStream = Client.GetStream();
                _writer = new BinaryWriter(_netStream);
                _reader = new BinaryReader(_netStream);

                Send(Messages.Connect);
                Stopwatch timer = Stopwatch.StartNew();
                timer.Start();
                while (true)
                {
                    Thread.Sleep(100);
                    if (timer.ElapsedMilliseconds > waitTime)
                    {
                        return false;
                    }
                    if (_reader.ReadString() == Messages.OK.ToString())
                    {
                        return true;
                    }
                }
            }
            catch
            {
                try { Client.Close(); }
                catch { }
                return false;
            }
        }

        public bool Disconnect()
        {
            if (Client.Connected)
            {
                Client.Close();
                _netStream = null;
                _writer = null;
                _reader = null;
                return true;
            }
            else
                return false;


        }
    }
}
