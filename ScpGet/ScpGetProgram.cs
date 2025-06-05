using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AxNetwork;

namespace ScpDemo
{
    class ScpGetDemo
    {
        static void Main(string[] args)
        {
            AxNetwork.Scp objScp = new AxNetwork.Scp();
            string strDemoHost = "scp.activexperts-labs.com", strDemoUser = "axnetwork", strDemoPassword = "topsecret", strDemoFile = "/home/axnetwork/readme.txt";
            string strRemoteFile;


            // A license key is required to unlock this component after the trial period has expired.
            // Call 'Activate' with a valid license key as its first parameter. Second parameter determines whether to save the license key permanently 
            // to the registry (True, so you need to call Activate only once), or not to store the key permanently (False, so you need to call Activate
            // every time the component is created). For details, see manual, chapter "Product Activation".
            /*
            objScp.Activate( "XXXXX-XXXXX-XXXXX", false );
            */

            // Display component information
            Console.WriteLine("ActiveXperts Network Component {0}\nBuild: {1}\nModule: {2}\nLicense Status: {3}\nLicense Key: {4}\n", objScp.Version, objScp.Build, objScp.Module, objScp.LicenseStatus, objScp.LicenseKey);

            objScp.Clear();

            // Set Logfile (optional, for debugging purposes)
            objScp.LogFile = Path.GetTempPath() + "Scp.log";
            Console.WriteLine("Log file used: {0}\n", objScp.LogFile);

            objScp.Host = ReadInput("Enter SCP hostname", strDemoHost, false);
            if (objScp.Host == strDemoHost)
            {
                objScp.UserName = strDemoUser;
                objScp.Password = strDemoPassword;
                strRemoteFile = strDemoFile;
            }
            else
            {
                objScp.UserName = ReadInput("Enter username", "", false);
                objScp.Password = ReadInput("Enter password", "", true);
                strRemoteFile = "";
            }

            objScp.AcceptHostKey = true;        // Automatically accept and store the host key if it is changed or unknown
            //  objSftp.PrivateKeyFile = "C:\\keyfile.ppk";  // a private key file can be used instead of a password


            strRemoteFile = ReadInput("Enter the filename on the remote SCP host", strRemoteFile, true);
            string strFilename = "";

            if (strRemoteFile.Contains("/"))
            {
                strFilename = strRemoteFile.Substring(strRemoteFile.LastIndexOf("/") + 1);
            }
            else
            {
                strFilename = strRemoteFile;
            }

            string strLocalFile = ReadInput("Enter the filename on the local machine", string.Format(@"{0}{1}", Path.GetTempPath(), strFilename), true);


            objScp.CopyToLocal(strRemoteFile, strLocalFile);

            Console.WriteLine("CopyToLocal, result: {0}: ({1})", objScp.LastError.ToString(), objScp.GetErrorDescription(objScp.LastError));
            if (objScp.ProtocolError != "")
                Console.WriteLine( "ProtocolError: {0}", objScp.ProtocolError );

            Console.WriteLine("Ready.");
            System.Threading.Thread.Sleep(5000); // Sleep for 5 seconds before exit
        }

        static private string ReadInput(string strTitle, string strDefaultValue, bool bAllowEmpty)
        {
            String strInput, strReturn = "";

            if (strDefaultValue == "") Console.WriteLine(strTitle);
            else Console.WriteLine(string.Format("{0}, leave blank to use: {1}", strTitle, strDefaultValue));

            do
            {
                Console.Write("  > ");
                strInput = Console.ReadLine();
                if (strInput.Length > 0)
                    strReturn = strInput;
            } while (strReturn == "" && !bAllowEmpty && strDefaultValue == "");

            if (strReturn == "") return strDefaultValue;
            return strReturn;
        }
    }
}
