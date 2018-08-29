using OpenSource.UPnP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UPnPOpenHABLocator {
    class Program {
        private static UPnPDevice upnpDevice;

        static void Main(string[] args) {
            // Wait for network UP
            while (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                Thread.Sleep(1000);
            }

            string ip = GetLocalIPAddress();

            // localhost, docker
            while(ip == "127.0.0.1" || ip.StartsWith("172.")) {
                Thread.Sleep(1000);
                ip = GetLocalIPAddress();
            }

            Console.WriteLine("Starting with IP " + ip);

            upnpDevice = UPnPDevice.CreateRootDevice(1600, 1.0, ".\\");
            upnpDevice.FriendlyName = "OpenHAB (OH URL in Model URL)";
            upnpDevice.Manufacturer = "";
            upnpDevice.ManufacturerURL = "";
            upnpDevice.ModelName = "OpenHAB";
            upnpDevice.ModelDescription = "OpenHAB";
            upnpDevice.ModelNumber = "OpenHAB";
            upnpDevice.ModelURL = new Uri("http://" + ip + ":8080");
            upnpDevice.BaseURL = new Uri("http://" + ip + ":8080");
            upnpDevice.StandardDeviceType = "OpenHAB";
            upnpDevice.UniqueDeviceName = "35fcccdc-b100-450b-bd4e-99e80fbcf6fb";

            upnpDevice.StartDevice();

            /*Console.WriteLine("Press enter to close...");
            Console.ReadLine();*/
            while (true) {
                Thread.Sleep(10000);
            }

            upnpDevice.StopDevice();
        }

        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}