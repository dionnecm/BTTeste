using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;

namespace BTTeste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            connectAsServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connectAsServer()
        {
            Thread bluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            bluetoothServerThread.Start();
        }

        Guid mUUID = new Guid("00001101-0000-1000-8000-00805f9b34fb");

        public void ServerConnectThread()
        {
            updateUI("Servidor iniciado. Aguardando clientes ...");
            BluetoothListener blueListener = new BluetoothListener(mUUID);
            blueListener.Start();
            BluetoothClient conn = blueListener.AcceptBluetoothClient();
            updateUI("Cliente conectou.");

            Stream mStream = conn.GetStream();
            while (true)
            {
                byte[] received = new byte[1024];
                mStream.Read(received, 0, received.Length);
                updateUI("Received: " + Encoding.ASCII.GetString(received));
                //byte[] sent = Encoding.ASCII.GetBytes("Servidor diz Olá");
                //mStream.Write(sent, 0, sent.Length);
            }
        }

        private void updateUI(string message)
        {
            Func<int> del = delegate ()
            {
                textBox1.AppendText(message + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }
    }
}
