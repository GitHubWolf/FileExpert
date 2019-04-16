using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace Library
{
    public partial class NetworkInterfaceListBox : ComboBox

    {
        public NetworkInterfaceListBox()
        {
            InitializeComponent();
        }
        public NetworkInterfaceListBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void ListAllNetworkInterfaces()
        {
            this.Items.Clear();//Clean all first!

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in networkInterfaces)
            {
                if (OperationalStatus.Up != adapter.OperationalStatus)
                {
                    //Skip disconnected network.
                    continue;
                }

                if (NetworkInterfaceType.Loopback == adapter.NetworkInterfaceType)
                {
                    //We don't want the loopack interface.
                    continue;
                }

                //Get network with IPV4.
                IPInterfaceProperties ipProperties = adapter.GetIPProperties();
                IPv4InterfaceProperties ipv4Properties = ipProperties.GetIPv4Properties();
                if (null != ipv4Properties)
                {
                    //foreach( ipProperties.UnicastAddresses.Count
                    foreach (UnicastIPAddressInformation unicastIpAddress in ipProperties.UnicastAddresses)
                    {
                        if (unicastIpAddress.Address.IsIPv6LinkLocal
                            | unicastIpAddress.Address.IsIPv6Multicast
                            | unicastIpAddress.Address.IsIPv6SiteLocal
                            | unicastIpAddress.Address.IsIPv6Teredo)
                        {
                            //Sorry. We don't support IPV6 currently.
                        }
                        else
                        {
                            KeyValuePair<IPAddress, int> networkIndexIpPair = new KeyValuePair<IPAddress, int>(unicastIpAddress.Address, ipv4Properties.Index);
                            this.Items.Add(networkIndexIpPair);
                        }
                    }
                }
            }

            //Select the first one by default.
            if (this.Items.Count != 0)
            {
                this.SelectedIndex = 0;
            }
        }


        /*
        private void GetAllNetworkInterfaces()
        {

            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.IsIPv6LinkLocal | ip.IsIPv6Multicast | ip.IsIPv6SiteLocal | ip.IsIPv6Teredo)
                    {
                        //Sorry. We don't support IPV6 currently.
                    }
                    else
                    {
                        listBoxAllNetworkInterfaces.Items.Add(ip);
                    }
                }
            }
            catch (Exception)
            { 
            }

        }
        */
    }
}
