using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public  class EmailTest
    {
        private const string CRLF = "\r\n";
        public bool OperaStream(TcpClient tcpc, string strCmd)
        {
            try
            {
                NetworkStream TcpStream;
                if (strCmd != "") strCmd += CRLF;


                TcpStream = tcpc.GetStream();
                byte[] bWrite = Encoding.Default.GetBytes(strCmd.ToCharArray());
                TcpStream.Write(bWrite, 0, bWrite.Length);
                string sp = "";


                string returndata = getResult(tcpc);


                sp = returndata.Substring(0, 3);
                if (returndata.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length > 1) sp = returndata.Substring(returndata.IndexOf("\r\n") + 2, 3);


                if (sp == "250" || sp == "220") return true;
                return false;
            }
            catch (Exception)
            {
                //Console.WriteLine(err.Message);
                return false;
            }
        }
        public string getResult(TcpClient tcpc)
        {


            string returndata = "";
            NetworkStream tps = tcpc.GetStream();
            if (tps.CanRead)
            {
                while (tps.DataAvailable)
                {
                    byte[] bytes = new byte[tcpc.ReceiveBufferSize];
                    tps.Read(bytes, 0, (int)tcpc.ReceiveBufferSize);
                    returndata += Encoding.Default.GetString(bytes).Replace("\0", "");
                }
            }
            return returndata;
        }


        public string getLocalDnsAddresses()
        {
            string rsl = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        rsl = (dns.ToString());
                        break;
                    }
                }
            }
            return rsl;
        }
        public string getMXRecord(string s)
        {
            string rsl = "";
            try
            {
                ArrayList dnsServers = new ArrayList();
                dnsServers.Add(getLocalDnsAddresses());
                DnsLite dl = new DnsLite();
                dl.setDnsServers(dnsServers);
                ArrayList results;
                results = dl.getMXRecords(s);
                for (int i = 0; i < results.Count; i++)
                {
                    MXRecord mx = (MXRecord)results[i];
                    rsl = mx.exchange;
                }
                results = null;
            }
            catch (Exception)
            {
                //Console.WriteLine("Caught exception : " + e);
            }
            return rsl;
        }


        //Use this method to check the email address whether available
        public bool chkEmailExist(string email)
        {
            bool rsl = false;
            string strEmail;
            int intPort;
            strEmail = email;
            intPort = 25;
            string domainName, localDNS, mailMX, hostIP;
            domainName = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries)[1];


            localDNS = getLocalDnsAddresses();


            mailMX = getMXRecord(domainName);


            hostIP = Dns.GetHostAddresses(mailMX)[0].ToString();


            TcpClient tcpc = new TcpClient();
            try
            {
                tcpc.Connect(mailMX, intPort);
                string topconet = getResult(tcpc);
                while (topconet == "") { Thread.Sleep(1000); topconet = getResult(tcpc); }
                if (topconet.Substring(0, 3) != "220") { Console.Write("Connect Error"); throw new Exception("err"); }
                OperaStream(tcpc, "EHLO mydomain.com");// here is a test email domain
                OperaStream(tcpc, "NOOP");
                OperaStream(tcpc, "MAIL FROM: <contact@allotabib.net>"); // here is the email address for sending test email.
                OperaStream(tcpc, "NOOP");
                if (OperaStream(tcpc, "RCPT TO:<" + strEmail + ">"))
                {
                    //Console.WriteLine(strEmail + "is a valid email address");
                    rsl = true;
                }
                else
                {
                    //Console.WriteLine(strEmail + "is invalid");
                }
                OperaStream(tcpc, "RSET");
                OperaStream(tcpc, "QUIT");


                tcpc.Close();
            }
            catch (Exception)
            {
                rsl = false;
                tcpc.Close();
                //Console.WriteLine(err.Message);
            }
            return rsl;
        }
    }




    public class MXRecord
    {


        public int preference = -1;
        public string exchange = null;


        public override string ToString()
        {


            return "Preference : " + preference + " Exchange : " + exchange;
        }


    }


    public class DnsLite
    {


        private byte[] data;
        private int position, id, length;
        private string name;
        private ArrayList dnsServers;


        private static int DNS_PORT = 53;


        Encoding ASCII = Encoding.ASCII;


        public DnsLite()
        {


            id = DateTime.Now.Millisecond * 60;
            dnsServers = new ArrayList();


        }


        public void setDnsServers(ArrayList dnsServers)
        {


            this.dnsServers = dnsServers;


        }
        public ArrayList getMXRecords(string host)
        {


            ArrayList mxRecords = null;


            for (int i = 0; i < dnsServers.Count; i++)
            {


                try
                {


                    mxRecords = getMXRecords(host, (string)dnsServers[i]);
                    break;


                }
                catch (IOException)
                {
                    continue;
                }


            }


            return mxRecords;
        }


        private int getNewId()
        {


            //return a new id
            return ++id;
        }


        public ArrayList getMXRecords(string host, string serverAddress)
        {


            //opening the UDP socket at DNS server
            //use UDPClient, if you are still with Beta1
            UdpClient dnsClient = new UdpClient(serverAddress, DNS_PORT);


            //preparing the DNS query packet.
            makeQuery(getNewId(), host);


            //send the data packet
            dnsClient.Send(data, data.Length);


            IPEndPoint endpoint = null;
            //receive the data packet from DNS server
            data = dnsClient.Receive(ref endpoint);


            length = data.Length;


            //un pack the byte array & makes an array of MXRecord objects.
            return makeResponse();


        }


        //for packing the information to the format accepted by server
        public void makeQuery(int id, String name)
        {


            data = new byte[512];


            for (int i = 0; i < 512; ++i)
            {
                data[i] = 0;
            }


            data[0] = (byte)(id >> 8);
            data[1] = (byte)(id & 0xFF);
            data[2] = (byte)1; data[3] = (byte)0;
            data[4] = (byte)0; data[5] = (byte)1;
            data[6] = (byte)0; data[7] = (byte)0;
            data[8] = (byte)0; data[9] = (byte)0;
            data[10] = (byte)0; data[11] = (byte)0;


            string[] tokens = name.Split(new char[] { '.' });
            string label;


            position = 12;


            for (int j = 0; j < tokens.Length; j++)
            {


                label = tokens[j];
                data[position++] = (byte)(label.Length & 0xFF);
                byte[] b = ASCII.GetBytes(label);


                for (int k = 0; k < b.Length; k++)
                {
                    data[position++] = b[k];
                }


            }


            data[position++] = (byte)0; data[position++] = (byte)0;
            data[position++] = (byte)15; data[position++] = (byte)0;
            data[position++] = (byte)1;


        }


        //for un packing the byte array
        public ArrayList makeResponse()
        {


            ArrayList mxRecords = new ArrayList();
            MXRecord mxRecord;


            //NOTE: we are ignoring the unnecessary fields.
            // and takes only the data required to build
            // MX records.


            int qCount = ((data[4] & 0xFF) << 8) | (data[5] & 0xFF);
            if (qCount < 0)
            {
                throw new IOException("invalid question count");
            }


            int aCount = ((data[6] & 0xFF) << 8) | (data[7] & 0xFF);
            if (aCount < 0)
            {
                throw new IOException("invalid answer count");
            }


            position = 12;


            for (int i = 0; i < qCount; ++i)
            {
                name = "";
                position = proc(position);
                position += 4;
            }


            for (int i = 0; i < aCount; ++i)
            {


                name = "";
                position = proc(position);


                position += 10;


                int pref = (data[position++] << 8) | (data[position++] & 0xFF);


                name = "";
                position = proc(position);


                mxRecord = new MXRecord();


                mxRecord.preference = pref;
                mxRecord.exchange = name;


                mxRecords.Add(mxRecord);


            }


            return mxRecords;
        }


        private int proc(int position)
        {


            int len = (data[position++] & 0xFF);


            if (len == 0)
            {
                return position;
            }


            int offset;


            do
            {
                if ((len & 0xC0) == 0xC0)
                {
                    if (position >= length)
                    {
                        return -1;
                    }
                    offset = ((len & 0x3F) << 8) | (data[position++] & 0xFF);
                    proc(offset);
                    return position;
                }
                else
                {
                    if ((position + len) > length)
                    {
                        return -1;
                    }
                    name += ASCII.GetString(data, position, len);
                    position += len;
                }


                if (position > length)
                {
                    return -1;
                }


                len = data[position++] & 0xFF;


                if (len != 0)
                {
                    name += ".";
                }
            } while (len != 0);


            return position;
        }
    }
}
