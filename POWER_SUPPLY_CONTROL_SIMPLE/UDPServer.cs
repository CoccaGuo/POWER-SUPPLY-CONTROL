using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{

    public class UdpStateEventArgs : EventArgs
    {
        public IPEndPoint remoteEndPoint;
        public byte[] buffer = null;
    }

    public delegate void UDPReceivedEventHandler(UdpStateEventArgs args);

    public class UDPServer
    {
        static string locateIP = "127.0.0.1";
        IPAddress locateIpAddr = IPAddress.Parse(locateIP);
        public string port;
        public IPEndPoint locatePoint;
        public UdpClient udpClient;
        public event UDPReceivedEventHandler UDPMessageReceived;

        public void SetupServer(string port)
        {
            this.port = port;
            locatePoint = new IPEndPoint(locateIpAddr, int.Parse(port));
            udpClient = new UdpClient(locatePoint);
        }

        public void SendMsg(string msg)
        {
            if (udpClient != null && locatePoint != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                udpClient.Send(data, data.Length, locatePoint);
            }
        }

        public void SendData(byte[] prefix, double volt, double curr)
        {
            List<byte> data = new List<byte>();
            data.AddRange(prefix);
            data.AddRange(BitConverter.GetBytes(volt));
            data.AddRange(BitConverter.GetBytes(curr));
            byte[] sdata = data.ToArray();
            udpClient.Send(sdata, sdata.Length, locatePoint);
        }

        public void SendByteData(byte[] volt, byte[] curr, byte[] prefix)
        {
            // prefix: 0: remain 0xE2, for checking
            // prefix: 1: Addr code, 0x00 for main ctrller
            // prefix: 2: device code, 0x10 for power supply
            // prefix: 3: unused, currently keep it 0x00.
            System.Diagnostics.Debug.Assert(volt.Length == 2);
            System.Diagnostics.Debug.Assert(curr.Length == 2);
            System.Diagnostics.Debug.Assert(prefix.Length == 4);
            byte[] data = new byte[8];
            data[0] = prefix[0];
            data[1] = prefix[1];
            data[2] = prefix[2];
            data[3] = prefix[3];
            data[4] = volt[0];
            data[5] = volt[1];
            data[6] = curr[0];
            data[7] = curr[1];

            udpClient.Send(data, data.Length, locatePoint);

        }

        public void StartListening()

        // recv 8
        // prefix: 0: remain 0xE2, for checking
        // prefix: 1: Addr code, 0x10 for power supply
        // prefix: 2: device code, 0x00 for main
        // prefix: 3: 0b0000_0000
            // from right to left
            //bit: 5: ctrl 1 for set control on
            //bit: 0: set output on (1) off (0)
            //bit: 1: ctrl label 1 for set
            // other, set 0
            // byte 0x0*== ctrl off, 0x1* == ctrl on
            // byte 0x13 == output on && SET mode
            
            // byte 0b0001_00*0 == output off && * mode, * = 0/1
        // 4-5 volt
        // 6-7 curr

        // config on 2022.4.5 CoccaGuo
        // universal 8-byte ctrller by devices
        {
            Task.Run(() =>
            {
                while (true)
                {
                    UdpStateEventArgs udpReceiveState = new UdpStateEventArgs();

                    if (udpClient != null)
                    {
                        IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("1.1.1.1"), 1);
                        var received = udpClient.Receive(ref remotePoint);
                        udpReceiveState.remoteEndPoint = remotePoint;
                        udpReceiveState.buffer = received;
                        UDPMessageReceived?.Invoke(udpReceiveState);
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }



    }
}
