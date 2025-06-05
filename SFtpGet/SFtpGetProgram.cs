using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AxNetwork;

namespace SFtpGetDemo
{
    class SFtpGetDemo
    {
        static void Main(string[] args)
        {
            AxNetwork.SFtp objSFtp = new AxNetwork.SFtp();
            AxNetwork.SFtpFile objSFtpFile = null;

            string strPath = "", strRemoteFilename = "", strLocalFilename = "";
            string strDemoHost = "sftp.activexperts-labs.com", strDemoUser = "axnetwork", strDemoPassword = "topsecret", strDemoPath = "/home/axnetwork/";


            // A license key is required to unlock this component after the trial period has expired.
            // Call 'Activate' with a valid license key as its first parameter. Second parameter determines whether to save the license key permanently 
            // to the registry (True, so you need to call Activate only once), or not to store the key permanently (False, so you need to call Activate
            // every time the component is created). For details, see manual, chapter "Product Activation".
            /*
            objSFtp.Activate( "XXXXX-XXXXX-XXXXX", false );
            */

            // Display component information
            Console.WriteLine("ActiveXperts Network Component {0}\nBuild: {1}\nModule: {2}\nLicense Status: {3}\nLicense Key: {4}\n", objSFtp.Version, objSFtp.Build, objSFtp.Module, objSFtp.LicenseStatus, objSFtp.LicenseKey);

            // Set Logfile (optional, for debugging purposes)
            objSFtp.LogFile = Path.GetTempPath() + "Sftp.log";
            Console.WriteLine("Log file used: {0}\n", objSFtp.LogFile);

            objSFtp.Host = ReadInput("Enter SFTP hostname", strDemoHost, false);
            Console.WriteLine("Host: {0}", objSFtp.Host);
            if(objSFtp.Host == strDemoHost)
            {
                objSFtp.UserName = strDemoUser;
                objSFtp.Password = strDemoPassword;
                strPath = strDemoPath;
            }
            else
            {
                objSFtp.UserName = ReadInput("Enter Login", "", false);
                objSFtp.Password = ReadInput("Enter Password (optional)", "", true);
                strPath = "";
            }

            Console.WriteLine("Login: {0}", objSFtp.UserName);
            Console.WriteLine("Password: {0}", objSFtp.Password);

            // Connect
            objSFtp.Port = 22;
            objSFtp.AcceptHostKey = true;        // Automatically accept and store the host key if it is changed or unknown
            //  objSftp.PrivateKeyFile = "C:\\keyfile.ppk";  // a private key file can be used instead of a password
            objSFtp.Connect();
            Console.WriteLine("Connect, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
            if (objSFtp.ProtocolError != "")
                Console.WriteLine("ProtocolError: {0}", objSFtp.ProtocolError);
            if (objSFtp.LastError == 0)
            {
                strPath = ReadInput("Enter Path", strPath, false);
                Console.WriteLine("Path: {0}", strPath);

                // Change directory
                objSFtp.ChangeDir(strPath);
                Console.WriteLine("ChangeDir, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
                if (objSFtp.LastError == 0)
                {
                    // Iterate over all files
                    objSFtpFile = (SFtpFile)objSFtp.FindFirstFile(".");
                    Console.WriteLine("FindFirstFile, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
                    while (objSFtp.LastError == 0)
                    {
                        Console.WriteLine("Name: {0}", objSFtpFile.Name);
                        Console.WriteLine("   IsDirectory: {0}", objSFtpFile.IsDirectory);
                        Console.WriteLine("   Size (bytes): {0}", objSFtpFile.SizeBytes);
                        Console.WriteLine("   Creation date (seconds): {0}", objSFtpFile.DateSeconds);
                        Console.WriteLine("   Creation date: {0}", objSFtpFile.Date);

                        /*if (objSFtpFile.IsDirectory == 0) {
                            // To save the file, call the GetFile function. 
                            strSaveAs = "C:\\temp\\" + objSFtpFile.Name;
                            objSFtp.GetFile( objSFtpFile.Name, strSaveAs ) ;                        
                            Console.WriteLine("GetFile, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));

                            // To delete a file, call the DeleteFile function. 
                            objSFtp.DeleteFile(objSFtpFile.Name);
                           Console.WriteLine("DeleteFile, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
                        } */

                        objSFtpFile = (SFtpFile)objSFtp.FindNextFile();
                        Console.WriteLine("FindNextFile, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
                    }

                    strRemoteFilename = ReadInput("Download file from current SFTP directory", "", false);
                    Console.WriteLine("Remote Filename: {0}", strRemoteFilename);

                    strLocalFilename = ReadInput("Enter the filename on the local computer", "c:\\" + strRemoteFilename, false);
                    Console.WriteLine("Local Filename: {0}", strLocalFilename);

                    objSFtp.GetFile(strRemoteFilename, strLocalFilename);
                    Console.WriteLine("GetFile, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));

                }

                objSFtp.Disconnect();
                Console.WriteLine("Disconnect, result: {0}: ({1})", objSFtp.LastError.ToString(), objSFtp.GetErrorDescription(objSFtp.LastError));
            }

            Console.WriteLine("Ready.");
            System.Threading.Thread.Sleep(5000);	// Sleep for 5 seconds before exit		
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
