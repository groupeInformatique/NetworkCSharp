using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Computer;

namespace Interfaces
{
    public class IPV6
    {
        string sip = "127.0.0.1";
        public string ip
        {
            get => sip;
            set => sip = value;
        }
        public IPV6(string ipv6)
        {
            if (!String.IsNullOrEmpty(ipv6))
            {
                sip = ipv6;
            }
        }
    }
        public class IPV4
    {
        string sip = "127.0.0.1";
        byte[] bip = new byte[4] { 0, 0, 0, 0 };
        public string ip
        {
            get => sip;
            set => sip = value;
        }
        public IPV4(string ip)
        {
            if (ip != "0.0.0.0" && !String.IsNullOrEmpty(ip))
            {
                this.sip = ip;
                this.bip = this.sip.Split('.').Select(e => Convert.ToByte(e)).ToArray();
            } else
            {
                this.sip = "NULL";
            }
        }
        public byte[] ip2byte(string ip)
        {
            byte[] bytes = new byte[4];
            int i = 0;
            foreach (string _ip in ip.Split('.'))
            {
                byte bit;
                byte.TryParse(_ip, out bit);
                bytes[i] = bit;
                ++i;
            }
            return bytes;
        }
        public uint get_long()
        {
            return (uint)(bip[0] | ( bip[1] << 8 ) | ( bip[2] << 16 ) | ( bip[3] << 24 ));
        }
        static public string LongToIP(uint longIP)
        {
            string ip = string.Empty;
           for (int i = 0; i< 4; i++)
            {
                int num = (int)(longIP / Math.Pow(256, (3 - i)));
                longIP = longIP - (uint) (num* Math.Pow(256, (3 - i)));
                if (i == 0)
                    ip = num.ToString();
               else
                   ip  = ip + "." + num.ToString();
           }
            string[] _bip = ip.Split('.');
            Array.Reverse(_bip);
            ip = String.Join(".",_bip);
            return ip;
       }
    
       public static uint IP2Long(string ip)
       {
           string[] ipBytes;
           double num = 0;
           if(!string.IsNullOrEmpty(ip))
          {
               ipBytes = ip.Split('.');
               for (int i = ipBytes.Length - 1; i > 0; i--)
            {
                  num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
               }
           }
          return (uint) num;
       }
        public string get_string()
        {
            return this.sip;
        }
    }

    public class MAC
    {
        byte[] bmac = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        string smac = "ff:ff:ff:ff:ff:ff";
        public string mac
        {
            get => smac;
            set => smac = value;
        }
        public byte[] nsplit(string str, int n)
        {
            List<byte> cstr = new List<byte>();
            int Cutp = 0;
            for (int i = 0; i < str.Length / 2; i++)
            {
                cstr.Add(Convert.ToByte(str.Substring(Cutp, n), 16));
                Cutp += 2;
            }
            return cstr.ToArray();
        }
        public MAC(string mac)
        {
            string[] _mac = { "00", "00", "00", "00", "00", "00" };
            if (!String.IsNullOrEmpty(mac))
            {
                int i = 0;
                this.bmac = this.nsplit(mac, 2);
                foreach (byte _smac in this.bmac)
                {
                    try
                    {
                        _mac[i] = _smac.ToString("X").ToLower().PadLeft(2, '0');
                    }
                    catch (Exception e)
                    {

                    }
                    ++i;
                }
                this.smac = string.Join(":", _mac);
            }
        }
    }
    public class Iface
    {
        private NetworkInterface NIC;
        public sProperties Properties;
        public struct sProperties
        {
            public string Name;
            public IPV4 IPv4;
            public IPV4 BroadCast;
            public IPV4 Gateway;
            public IPV4 Mask;
            public static IPV4 ExternalIP;
            public MAC Mac;
            public IPV6 IPv6;
            public string Status;
            public long Speed;
            public long Breceived;
            public long Bsent;
            public long MTU;
            public string Index;
            public bool IsDhcpEnabled;
            public string Type;
            public List<Int64> lst_BReceived;
            public List<Int64> lst_BSent;
        }
        private void InitProperties()
        {
            this.Properties.Name = "";
            this.Properties.IPv4 = new IPV4("0.0.0.0");
            this.Properties.BroadCast = new IPV4("0.0.0.0");
            this.Properties.Gateway = new IPV4("0.0.0.0");
            this.Properties.Mask = new IPV4("0.0.0.0");
            if(ComputerInfo.ProgramState <=0)
                Iface.sProperties.ExternalIP = null;
            this.Properties.IPv6 = new IPV6("ff::01");
            this.Properties.Mac = new MAC("000000000000");
            this.Properties.Status = "Down";
            this.Properties.Speed = 0;
            this.Properties.Breceived = 0;
            this.Properties.Bsent = 0;
            this.Properties.MTU = 0;
            this.Properties.Index = "0";
            this.Properties.IsDhcpEnabled = false;
            this.Properties.Type = "N/A";
            this.Properties.lst_BReceived = new List<Int64>();
            this.Properties.lst_BSent = new List<Int64>();
        }

        public void Refresh_Sr(NetworkInterface _NIC)
        {
            long AvgBr = _NIC.GetIPv4Statistics().BytesReceived - this.Properties.Breceived;
            long AvgBs = _NIC.GetIPv4Statistics().BytesSent - this.Properties.Bsent;
            this.Properties.lst_BReceived.Add(AvgBr);
            this.Properties.lst_BSent.Add(AvgBs);
            this.Properties.Breceived = _NIC.GetIPv4Statistics().BytesReceived;
            this.Properties.Bsent = _NIC.GetIPv4Statistics().BytesSent;
        }

        /* private void External_IP()
         {
             if (Iface.sProperties.ExternalIP == null)
             {
                 string url = "http://checkip.dyndns.org";
                 System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                 System.Net.WebResponse resp = req.GetResponse();
                 System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                 string response = sr.ReadToEnd().Trim();
                 string[] a = response.Split(':');
                 string a2 = a[1].Substring(1);
                 string[] a3 = a2.Split('<');
                 string a4 = a3[0];
                 Iface.sProperties.ExternalIP = new IPV4(a4);
             }
         }*/
        public string GetExternalIP()
        {
            string externalIP = "";
            try
            {
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
                ComputerInfo.hasNetwork = true;
            }
            catch
            {
                ComputerInfo.hasNetwork = false;
            }
            ComputerInfo.ProgramState = 1;
            return externalIP;
        }
        public Iface(NetworkInterface _NIC)
        {
            // IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            NIC = _NIC;
            InitProperties();
            this.Properties.Name = _NIC.Name;
        }
        public void LoadIface()
        {
            string ip = "192.168.0.0";
            int i = 0;
            try
            {
                UnicastIPAddressInformationCollection ipa = NIC.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation _ipa in ipa)
                {
                    switch (_ipa.Address.AddressFamily)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            // we have IPv4
                            this.Properties.IPv4 = new IPV4(NIC.GetIPProperties().UnicastAddresses[i].Address.ToString());
                            break;
                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            // we have IPv6
                            this.Properties.IPv6 = new IPV6(NIC.GetIPProperties().UnicastAddresses[i].Address.ToString());
                            break;
                        default:
                            // umm... yeah... I'm going to need to take your red packet and...
                            break;
                    }
                    ++i;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());//code for any other type of exception
            }
            this.Properties.Breceived = NIC.GetIPv4Statistics().BytesReceived;
            this.Properties.Bsent = NIC.GetIPv4Statistics().BytesSent;
            this.Properties.Speed = NIC.Speed;
            try
            {
                this.Properties.IsDhcpEnabled = NIC.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
                this.Properties.MTU = NIC.GetIPProperties().GetIPv4Properties().Mtu;
            } catch(Exception ex)
            {

            }
            //NIC.GetIPv4Statistics().External_IP();
            
            try
            {
                {
                    UnicastIPAddressInformationCollection UnicastIPInfoCol = NIC.GetIPProperties().UnicastAddresses;
                    if (NIC.GetIPProperties().UnicastAddresses.Count - 1 > 0)
                    {
                        string mask = UnicastIPInfoCol[NIC.GetIPProperties().UnicastAddresses.Count - 1].IPv4Mask.ToString();
                        this.Properties.Mask = new IPV4(mask);
                    }
                }
                GatewayIPAddressInformationCollection gateway = NIC.GetIPProperties().GatewayAddresses;
                foreach (GatewayIPAddressInformation address in gateway)
                {
                    if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        string Gateway = address.Address.ToString();
                        this.Properties.Gateway = new IPV4(address.Address.ToString());
                        break;
                    }
                }
                this.Properties.BroadCast = new IPV4(IPV4.LongToIP(~this.Properties.Mask.get_long() | this.Properties.IPv4.get_long()));
                this.Properties.Index = NIC.GetIPProperties().GetIPv4Properties().Index.ToString();
                this.Properties.Type = NIC.NetworkInterfaceType.ToString();
                this.Properties.Status = NIC.OperationalStatus.ToString();
            }
            catch (Exception ex)
            {

            }

            this.Properties.Mac = new MAC(NIC.GetPhysicalAddress().ToString());
            if (ComputerInfo.ProgramState <= 0) { Iface.sProperties.ExternalIP = new IPV4(this.GetExternalIP()); }
            
            //External_IP();
        }
        override
        public string ToString()
        {
            return this.Properties.Name + Environment.NewLine + this.Properties.IPv4.ip + Environment.NewLine + this.Properties.Mask.ip + Environment.NewLine + this.Properties.Mac.mac + Environment.NewLine + this.Properties.Gateway.ip + Environment.NewLine + this.Properties.BroadCast.ip + Environment.NewLine + Environment.NewLine;
        }
        public Iface()
        {
        }
    }
}