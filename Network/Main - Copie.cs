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
    public partial class Main : Form
    {
        public List<Iface> ifaces = new List<Iface>();
        public void net_adapters()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Iface _if = new Iface(nic);
                //txt_debug.Text += _if.ToString();
                this.ifaces.Add(_if);
                comboBox1.Items.Add(_if.Properties.Name);
            }
        }
        public Main()
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
            Iface iface = this.ifaces.ElementAt(comboBox1.SelectedIndex);
            txt_broadcast.Text = iface.Properties.BroadCast.ip;
            txt_ipv4.Text = iface.Properties.IPv4.ip;
            txt_gateway.Text = iface.Properties.Gateway.ip;
            txt_mask.Text = iface.Properties.Mask.ip;
            txt_mac.Text = iface.Properties.Mac.mac;
            //txt_externe.Text = Iface.sProperties.ExternalIP.ip;
            txt_ipv6.Text = iface.Properties.IPv6.ip;
            txt_status.SelectedText = iface.Properties.Status;
            txt_mtu.Text = iface.Properties.MTU.ToString();
            txt_speed.Text = iface.Properties.Speed.ToString();
            txt_sent.Text = iface.Properties.Bsent.ToString();
            txt_received.Text = iface.Properties.Breceived.ToString();
            txt_index.Text = iface.Properties.Index.ToString();
            cmb_isdhcp.SelectedItem = (iface.Properties.IsDhcpEnabled ? "DHCP" : "STATIC");
            /* foreach (Control ctl in Controls)
             {
                 ctl.Enabled = false;
             }*/
            txt_mask.Enabled = false;
            txt_gateway.Enabled = false;
            txt_ipv4.Enabled = false;
            txt_mtu.Enabled = false;
            txt_status.Enabled = false;
            cmb_isdhcp.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* foreach (Control ctl in Controls)
             {
                 ctl.Enabled = true;
             }*/
            txt_mask.Enabled = true;
            txt_gateway.Enabled = true;
            txt_ipv4.Enabled = true;
            txt_mtu.Enabled = true;
            txt_status.Enabled = true;
            cmb_isdhcp.Enabled = true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string command = "netsh interface ip set address " + comboBox1.SelectedItem.ToString() + " static " + txt_ipv4.Text + " " + txt_mask.Text + " " + txt_gateway.Text + " 1";
            if(cmb_isdhcp.SelectedItem.ToString() == "STATIC")
            {
                Library.ExecuteCommandSync("netsh interface ip set address \"" + comboBox1.SelectedItem.ToString() + "\" static " + txt_ipv4.Text + " " + txt_mask.Text + " " + txt_gateway.Text + " 1");
            }
            else
            {
                Library.ExecuteCommandSync("netsh interface ip set address \"" + comboBox1.SelectedItem.ToString() + "\" dhcp");
            }
            Library.ExecuteCommandSync("netsh interface ipv4 set subinterface \"" + comboBox1.SelectedItem.ToString() + "\" mtu=" + txt_mtu.Text + " store=persistent");
            //ProcessStartInfo ProcessInfo = new ProcessStartInfo(command);
            try
            {
                string isenabled = txt_status.SelectedItem.ToString() == "Up" ? "enable" : "disable";
                Library.ExecuteCommandSync("netsh interface set interface \"" + comboBox1.SelectedItem.ToString() + "\" " + isenabled);
            }
            catch (Exception ex)
            {

            }
        }

        private void cmb_isdhcp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

//netsh interface set interface "Local Area Connection" disable