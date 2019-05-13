using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Interfaces;
using System.Diagnostics;

namespace Network
{
    public partial class FormIface : Form
    {
        public List<Iface> ifaces = new List<Iface>();
        Iface iface = new Iface();
        public void net_adapters()
        {
           NetworkInterface[] nic2 =NetworkInterface.GetAllNetworkInterfaces();
           // NetworkInterface[] nic1 = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Iface _if = new Iface(nic);
                //txt_debug.Text += _if.ToString();
                this.ifaces.Add(_if);
                comboBox1.Items.Add(_if.Properties.Name);
            }
            this.iface = null;
        }
        public void refresh()
        {
            this.ifaces.Clear();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Iface _if = new Iface(nic);
                //txt_debug.Text += _if.ToString();
                this.ifaces.Add(_if);
            }
        }
        public FormIface()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            net_adapters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            this.iface = this.ifaces.ElementAt(comboBox1.SelectedIndex);
            this.refresh();
            iface.LoadIface();

            txt_mask.Enabled = false;
            txt_gateway.Enabled = false;
            txt_ipv4.Enabled = false;
            txt_mtu.Enabled = true;
            txt_status.Enabled = true;
            txt_mac.Enabled = true;

            txt_broadcast.Text = iface.Properties.BroadCast.ip;
            txt_ipv4.Text = iface.Properties.IPv4.ip;
            txt_gateway.Text = iface.Properties.Gateway.ip;
            txt_mask.Text = iface.Properties.Mask.ip;
            txt_mac.Text = iface.Properties.Mac.mac;
            txt_externe.Text = Iface.sProperties.ExternalIP.ip;
            txt_ipv6.Text = iface.Properties.IPv6.ip;
            txt_status.SelectedItem = iface.Properties.Status;
            txt_mtu.Text = iface.Properties.MTU.ToString();
            txt_speed.Text = iface.Properties.Speed.ToString();
            txt_sent.Text = iface.Properties.Bsent.ToString();
            txt_received.Text = iface.Properties.Breceived.ToString();
            txt_index.Text = iface.Properties.Index.ToString();
            txt_ethernet.Text = iface.Properties.Type.ToString();
            string DHCPVal = (iface.Properties.IsDhcpEnabled ? "DHCP" : "STATIC");
            cmb_isdhcp.SelectedItem = DHCPVal;
            cmb_isdhcp.Enabled = true;
            if(DHCPVal == "STATIC")
            {
                txt_ipv4.Enabled = true;
                txt_mask.Enabled = true;
                txt_gateway.Enabled = true;
            }
            /* foreach (Control ctl in Controls)
             {
                 ctl.Enabled = false;
             }*/
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
            if (cmb_isdhcp.SelectedItem.ToString() == "STATIC")
            {
                Library.ExecuteCommandSync("netsh interface ip set address \"" + comboBox1.SelectedItem.ToString() + "\" static " + txt_ipv4.Text + " " + txt_mask.Text + " " + txt_gateway.Text + " 1");
            }
            else
            {
                Library.ExecuteCommandSync("netsh interface ip set address \"" + comboBox1.SelectedItem.ToString() + "\" dhcp");
            }
            Library.ExecuteCommandSync("netsh interface ipv4 set subinterface \"" + comboBox1.SelectedItem.ToString() + "\" mtu=" + txt_mtu.Text + " store=persistent");
            //ProcessStartInfo ProcessInfo = new ProcessStartInfo(command);
            if (txt_status.SelectedItem.ToString() == "Up")
            {
                Library.ExecuteCommandSync("netsh interface set interface \"" + comboBox1.SelectedItem.ToString() + "\" admin=disable");
            }
            Int32 i32 = 0;
            Int32.TryParse(txt_index.Text, out i32);
            String key = (i32 - 1).ToString().PadLeft(4, '0');
            Library.ExecuteCommandSync("reg add HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\"+ key +" /v NetworkAddress /d " + txt_mac.Text + " /f");
            try
            {
                string isenabled = txt_status.SelectedItem.ToString() == "Up" ? "enable" : "disable";
                Library.ExecuteCommandSync("netsh interface set interface \"" + comboBox1.SelectedItem.ToString() + "\" admin=" + isenabled);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmb_isdhcp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_isdhcp.SelectedItem.ToString() == "STATIC")
            {
                txt_gateway.Enabled = true;
                txt_ipv4.Enabled = true;
                txt_mask.Enabled = true;
            }
            else //DHCP
            {
                txt_gateway.Enabled = false;
                txt_ipv4.Enabled = false;
                txt_mask.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txt_gateway_TextChanged(object sender, EventArgs e)
        {

        }

        private void tmr_refresh_Tick(object sender, EventArgs e)
        {
            if (this.iface != null)
            {
                Int32 x = 0;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    //txt_debug.Text += _if.ToString();
                    if (nic.Name == this.iface.Properties.Name)
                    {
                        this.ifaces[x].Refresh_Sr(nic);
                        this.iface = this.ifaces.ElementAt(x);
                        txt_received.Text = this.iface.Properties.Breceived.ToString();
                        txt_sent.Text = this.iface.Properties.Bsent.ToString();
                        break;
                    }
                    ++x;
                }
                txt_upload.Text = (this.iface.Properties.lst_BSent.Last()).ToString();
                txt_download.Text = (this.iface.Properties.lst_BReceived.Last()).ToString();
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            formRouteur frmR = new formRouteur();
            frmR.Show();
        }
    }
}

//netsh interface set interface "Local Area Connection" disable