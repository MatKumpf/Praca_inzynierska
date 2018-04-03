using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Listening : CommInterface
    {
        private BackgroundWorker BWListening;
        private bool _autoResume;
        private TcpListener Listener;
        private bool _connected;

        public Listening(int port, bool autoResume)
        {
            _port = port;
            _autoResume = autoResume;
            _connected = false;

            BWListening = new BackgroundWorker();
            BWListening.WorkerReportsProgress = true;
            BWListening.WorkerSupportsCancellation = true;
            BWListening.DoWork += BWListening_DoWork;
            BWListening.RunWorkerCompleted += BWListening_RunWorkerCompleted;
        }

        public bool AutoResume
        {
            get { return _autoResume; }
            set { _autoResume = value; }
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
        }

        public void StartListening()
        {
            BWListening.RunWorkerAsync();
        }

        public void StopListeningOrBreakConnection()
        {
            try
            {
                BWListening.CancelAsync();
            }
            catch { }
            _netStream = null;
            _writer = null;
            _reader = null;
            _connected = false;
            try { Client.Close(); }
            catch { }
            try { Listener.Stop(); }
            catch { }

            if (_autoResume)
            {
                BWListening.RunWorkerAsync();
            }
        }

        public void ChangePort(int port)
        {
            _port = port;
            BWListening.CancelAsync();

            if (_autoResume)
            {
                BWListening.RunWorkerAsync();
            }
        }

        private void BWListening_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_connected == false && _autoResume == true)
            {
                BWListening.RunWorkerAsync();
            }
        }

        private void BWListening_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, _port);
                Listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Listener.Start();

                while (!Listener.Pending())
                {
                    Thread.Sleep(100);
                    if (BWListening.CancellationPending)
                    {
                        return;
                    }
                }
            }
            catch
            {
                try { Listener.Stop(); }
                catch { }
                return;
            }

            try
            {
                Client = Listener.AcceptTcpClient(); // Przypisanie klienta który połączył się z naszym listenerem
                _netStream = Client.GetStream(); // Stworzenie połączenia przez który wysyłamy i odbieramy dane
                _writer = new BinaryWriter(_netStream); // Obiekt do wysyłania danych
                _reader = new BinaryReader(_netStream); // Obiekt do odbierania danych

                while (true)
                {
                    Thread.Sleep(100);
                    if (BWListening.CancellationPending)
                    {
                        return;
                    }
                    Thread.Sleep(100);
                    if (_reader.ReadString() == Messages.Connect.ToString()) // Sprawdzamy czy otrzymaliśmy komunikat o chęci połączenia się
                    {
                        Send(Messages.OK);
                        _connected = true;
                        return;
                    }
                }
            }
            catch
            {
                _netStream = null;
                _writer = null;
                _reader = null;
                _connected = false;

                try { Client.Close(); }
                catch { }
                try { Listener.Stop(); }
                catch { }
            }
        }

    }
}
