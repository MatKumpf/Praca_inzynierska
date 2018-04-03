using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public abstract class CommInterface
    {
        public enum Messages
        {
            Connect, OK
        };

        protected int _port;


        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        protected NetworkStream _netStream;

        protected BinaryWriter _writer;

        public BinaryWriter Writer
        {
            get { return _writer; }
            set { _writer = value; }
        }

        protected BinaryReader _reader;

        public BinaryReader Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        protected TcpClient _client;

        public TcpClient Client
        {
            get { return _client; }
            set { _client = value; }
        }

        public bool Connected
        {
            get { return _client.Connected; }
        }

        public void Send(Messages communique)
        {
            try
            {
                if (_writer != null)
                    _writer.Write(communique.ToString());
            }
            catch { }
        }

        public void Send(string communique)
        {
            try
            {
                if (_writer != null)
                    _writer.Write(communique);
            }
            catch { }
        }

        public void Send(char[] communique)
        {
            try
            {
                if (_writer != null)
                    _writer.Write(communique);
            }
            catch { }
        }

        public string Read()
        {
            try
            {
                if (_reader != null)
                {
                    return _reader.ReadString();
                }
                return null;
            }
            catch
            {
                return null; 
            }
        }
    }
}
