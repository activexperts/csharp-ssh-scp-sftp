using System;
using System.IO;
using AxNetwork;

namespace SshDemo
{
	/// <summary>
	/// Summary description for SshDemo.
	/// </summary>
	class SshDemo
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            AxNetwork.Ssh objSsh = new AxNetwork.Ssh(); // Create instance of COM Object
            string strDemoHost = "ssh.activexperts-labs.com", strDemoUser = "axnetwork", strDemoPassword = "topsecret";

            // A license key is required to unlock this component after the trial period has expired.
            // Call 'Activate' with a valid license key as its first parameter. Second parameter determines whether to save the license key permanently 
            // to the registry (True, so you need to call Activate only once), or not to store the key permanently (False, so you need to call Activate
            // every time the component is created). For details, see manual, chapter "Product Activation".
            /*
            objSsh.Activate( "XXXXX-XXXXX-XXXXX", false );
            */

            // Display component information
            Console.WriteLine("ActiveXperts Network Component {0}\nBuild: {1}\nModule: {2}\nLicense Status: {3}\nLicense Key: {4}\n", objSsh.Version, objSsh.Build, objSsh.Module, objSsh.LicenseStatus, objSsh.LicenseKey);

            // Set Logfile (optional, for debugging purposes)
            objSsh.LogFile = Path.GetTempPath() + "Ssh.log";
            Console.WriteLine("Log file used: {0}\n", objSsh.LogFile);

            objSsh.Host = ReadInput("Enter host/IP address", strDemoHost, false);
            Console.WriteLine("Host: {0}", objSsh.Host);
            if (objSsh.Host == strDemoHost)
            {
                objSsh.UserName = strDemoUser;
                objSsh.Password = strDemoPassword;
                objSsh.PrivateKeyFile = "";
            }
            else
            {
                objSsh.UserName = ReadInput("Enter Login", strDemoUser, false);
                objSsh.Password = ReadInput("Enter Password (Optional)", strDemoPassword, true);
                objSsh.PrivateKeyFile = ReadInput("Enter private key file (Optional)", "", true);
            }

            Console.WriteLine("UserName: {0}", objSsh.UserName);
            Console.WriteLine("Password: {0}", objSsh.Password);

            // objSsh.Port = 22;	// Default port: 22
            objSsh.AcceptHostKey = true;        // Automatically accept and store the host key if it is changed or unknown

            objSsh.ConnectTimeout = 30000;

            Console.WriteLine("OpenSession...");
            objSsh.OpenSession();
            Console.WriteLine("OpenSession, result: {0}: ({1})", objSsh.LastError.ToString(), objSsh.GetErrorDescription(objSsh.LastError));
            if (objSsh.ProtocolError != "")
                Console.WriteLine("ProtocolError: {0}", objSsh.ProtocolError);

            while (objSsh.IsSessionOpened())
            {
                objSsh.Command = ReadInput("Enter command (type 'QUIT!' to quit)", @"ls", true);
                if (objSsh.Command == "QUIT!")
                    break;

                Console.WriteLine("Command: {0}", objSsh.Command);
                Console.WriteLine("ExecuteCommand...");
                objSsh.ExecuteCommand();
                Console.WriteLine("ExecuteCommand, result: {0}: ({1})", objSsh.LastError.ToString(), objSsh.GetErrorDescription(objSsh.LastError));
                if (objSsh.ProtocolError != "")
                    Console.WriteLine("ProtocolError: {0}", objSsh.ProtocolError);
                if (objSsh.LastError == 0)
                {
                    Console.WriteLine("STDERR: ");
                    Console.WriteLine(objSsh.StdErr);
                    Console.WriteLine("STDOUT: ");
                    Console.WriteLine(objSsh.StdOut);
                }
            }

            objSsh.CloseSession();
            Console.WriteLine("CloseSession, result: {0}: ({1})", objSsh.LastError.ToString(), objSsh.GetErrorDescription(objSsh.LastError));

			Console.WriteLine("Ready.");		
			System.Threading.Thread.Sleep(3000); // Sleep for 3 second before exit
        }

        static private string ReadInput(string strTitle, string strDefaultValue, bool bAllowEmpty)
        {
            String strInput, strReturn = "";

            if (strDefaultValue == "") Console.WriteLine("{0}:", strTitle);
            else Console.WriteLine("{0}, leave blank to use: {1}", strTitle, strDefaultValue);

            do
            {
                Console.Write("  > ");
                strInput = Console.ReadLine();
                if (strInput.Length > 0)
                    strReturn = strInput;
            } while (strReturn == "" && !bAllowEmpty && strDefaultValue == "");

            if (strReturn == "") return strDefaultValue;
            else return strReturn;
        }
	}


}
