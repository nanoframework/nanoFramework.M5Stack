// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Stack;
using nanoFramework.Networking;
using nanoFramework.Tough;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Console = nanoFramework.M5Stack.Console;

Tough.InitializeScreen();

Debug.WriteLine("Hello from Tough!");

Console.WriteLine("Hello from Tough!");

//const string Ssid = "SSID";
//const string Password = "YourWifiPasswordHere";
//// Give 60 seconds to the wifi join to happen
CancellationTokenSource cs = new(60000);
var success = WifiNetworkHelper.Reconnect(requiresDateTime: true, token: cs.Token);
if (!success)
{
    // Something went wrong, you can get details with the ConnectionError property:
    Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
    if (WifiNetworkHelper.HelperException != null)
    {
        Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
    }
}

Tough.TouchEvent += TouchEventCallback;

/////////////////////////////////////////////////////////////////////////////////////
// add certificate in PEM format (as a string in the app)
//X509Certificate letsEncryptCACert = new X509Certificate(_dstRootCAX3);
/////////////////////////////////////////////////////////////////////////////////////


// get host entry for How's my SSL test site
//IPHostEntry hostEntry = Dns.GetHostEntry("www.howsmyssl.com");
// get host entry for Global Root test site
IPHostEntry hostEntry = Dns.GetHostEntry("global-root-ca.chain-demos.digicert.com");

// need an IPEndPoint from that one above
IPEndPoint ep = new IPEndPoint(hostEntry.AddressList[0], 443);

Debug.WriteLine("Opening socket...");
using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
{
    try
    {
        Debug.WriteLine("Connecting...");

        // connect socket
        mySocket.Connect(ep);

        Debug.WriteLine("Authenticating with server...");

        // setup SSL stream
        using (SslStream sslStream = new SslStream(mySocket))
        {
            ///////////////////////////////////////////////////////////////////////////////////
            // Authenticating using a client certificate stored in the device is possible by
            // setting the UseStoredDeviceCertificate property. 
            // 
            // In practice it's equivalent to providing a client certificate in
            // the 'clientCertificate' parameter when calling AuthenticateAsClient(...)
            //
            /////////////////////////////////////////////////////////////////////////////////// 
            //stream.UseStoredDeviceCertificate = true;

            ///////////////////////////////////////////////////////////////////////////////////
            // Authenticating the server can be handled in one of three ways:
            //
            // 1. By providing the root CA certificate of the server being connected to.
            // 
            // 2. Having the target device preloaded with the root CA certificate.
            // 
            // !! NOT SECURED !! NOT RECOMENDED !!
            // 3. Forcing the authentication workflow to NOT validate the server certificate.
            //
            /////////////////////////////////////////////////////////////////////////////////// 

            // option 1 
            // setup authentication (add CA root certificate to the call)
            // Let's encrypt test certificate
            //stream.AuthenticateAsClient("www.howsmyssl.com", null, letsEncryptCACert, SslProtocols.Tls11);
            // GlobalRoot CA cert from resources
            sslStream.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", null, null, SslProtocols.Tls12);

            // option 2
            // setup authentication (without providing root CA certificate)
            // this requires that the trusted root CA certificates are available in the device certificate store
            //stream.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.Tls11);
            //stream.AuthenticateAsClient("global-root-ca.chain-demos.digicert.com", SslProtocols.Tls12);

            // option 3
            // disable certificate validation
            //stream.SslVerification = SslVerification.NoVerification;
            //stream.AuthenticateAsClient("www.howsmyssl.com", SslProtocols.TLSv11);

            Debug.WriteLine("SSL handshake OK!");

            // write an HTTP GET request to receive data
            byte[] buffer = Encoding.UTF8.GetBytes("GET / HTTP/1.0\r\n\r\n");
            sslStream.Write(buffer, 0, buffer.Length);

            Debug.WriteLine($"Wrote {buffer.Length} bytes");

            int bytesCounter = 0;

            do
            {
                var bufferLenght = sslStream.Length;

                // if available length is 0, need to read at least 1 byte to get it started
                if (bufferLenght == 0)
                {
                    bufferLenght = 1;
                }

                // setup buffer to read data from socket
                buffer = new byte[bufferLenght];

                // trying to read from socket
                int bytes = sslStream.Read(buffer, 0, buffer.Length);

                bytesCounter += bytes;

                if (bytes > 0)
                {
                    // data was read!
                    // output as string
                    // mind to use only the amount of data actually read because it could be less than the requested count
                    Debug.Write(new String(Encoding.UTF8.GetChars(buffer, 0, bytes)));
                }
            }
            while (sslStream.DataAvailable);

            Debug.WriteLine($"Read {bytesCounter} bytes");
        }
    }
    catch (SocketException ex)
    {
        Debug.WriteLine($"** Socket exception occurred: {ex.Message} error code {ex.ErrorCode}!**");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"** Exception occurred: {ex.Message}!**");
    }
}


Thread.Sleep(Timeout.Infinite);

void TouchEventCallback(object sender, TouchEventArgs e)
{
    const string StrXY1 = "TOUCHED at X= ";
    const string StrXY2 = ",Y= ";

    Console.CursorLeft = 0;
    Console.CursorTop = 0;

    Debug.WriteLine(StrXY1 + e.X + StrXY2 + e.Y);
    Console.WriteLine(StrXY1 + e.X + StrXY2 + e.Y + "  ");

    Console.WriteLine("                                      ");
}
