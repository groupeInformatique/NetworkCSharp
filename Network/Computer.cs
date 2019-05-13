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

namespace Computer
{
    enum State { init, loaded }
    static public class ComputerInfo
    {
        static public bool hasNetwork;
        static public int ProgramState = 0;
        static public string HostName;
        public const bool DEBUG = false;
    }
}