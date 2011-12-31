using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Text.RegularExpressions;

namespace Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set The Console To Be Bright White
            Console.ForegroundColor = ConsoleColor.White;

            Int16 SuccessCount = 0;
            Int16 FailureCount = 0;
            Int64 AveragePingTime = 0;
            Int64 TotalCount = 0;
            Ping Ping = new Ping();
            PingReply Reply = null;
            IPHostEntry IP;
            bool IPCheck = false;
            
            try
            {
                //Setup The Output Dir
                FileSysSetup.SetupFileSys();

                //Check For Hosts.txt
                if (FileSysSetup.CheckForHosts() == false)
                {
                    Console.WriteLine("Oops: No Hosts.txt File Detected, Please Add Your Host List Into A File, One Host Per Line, Named Hosts.txt Then Try Again");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                // Create The Files
                StreamWriter success = File.CreateText("output\\success.txt");
                success.WriteLine("Successful Pings - Created On: " + DateTime.Now);
                success.WriteLine("----------------------------------------------");

                StreamWriter failure = File.CreateText("output\\failure.txt");
                failure.WriteLine("Failed Pings - Created On: " + DateTime.Now);
                failure.WriteLine("----------------------------------------------");

                StreamWriter ip_info = File.CreateText("output\\ip_info.txt");
                ip_info.WriteLine("IP Info - Created On: " + DateTime.Now);
                ip_info.WriteLine("----------------------------------------------");

                FileStream fs = File.OpenRead("hosts.txt");
                StreamReader sr = new StreamReader(fs);
                string text = sr.ReadToEnd();
                string Host = "";
                string Response = "";
                fs.Close();

                // Split file data into array of lines
                string[] lines = Regex.Split(text, "\r\n");
                TotalCount = lines.Length;
                Console.WriteLine("");
                WriteLogo();
                Console.WriteLine("");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("------ Pinger! v1.5 - By Cheyne Wallace ------------------");
                Console.WriteLine("--------- http://themonitoringguy.com --------------------");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine(lines.Length + " Hosts Detected In hosts.txt");
                Console.WriteLine("");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Options:");
            Options:
                Console.WriteLine("Press 1 To Run Single Sweep With Success & Failure Text Files");
                Console.WriteLine("Press 2 To Run Single Sweep With Success, Failure & IP Information Text Files");
                Console.WriteLine("");

                if (args.Length > 0)
                {
                    Response = args[0].ToString();
                }
                else
                {
                    Response = Console.In.ReadLine().ToString();
                }

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("Running Option #" + Response);

                if (Response == "1")
                {
                    goto Main;
                }
                else if (Response == "2")
                {
                    IPCheck = true;
                    goto Main;
                }
                else
                {
                    goto Options;
                }
            Main:

                // Loop through array
                foreach (string line in lines)
                {
                    //Skip To Next Iteration If Blank Line
                    if (string.IsNullOrEmpty(line.ToString()))
                    {
                        continue;
                    }

                    try
                    {
                        Host = line.Replace(" ", "").ToString();

                       //Resolve The IP If Selected
                        if (IPCheck == true)
                        {
                            IP = Dns.GetHostEntry(Host);
                            ip_info.WriteLine(Host.ToUpper() + " - " + GetIPString(IP));
                        }

                        //Do The Ping
                        Reply = Ping.Send(Host, 600);
                        if (Reply.Status == IPStatus.Success)
                        {
                            // --- handle reachable here
                            //Create A New Line
                            Console.WriteLine("");
                            Console.Write(Host + ": ");
                            CPrint("Success", ConsoleColor.Green);
                            Console.Write(" - " + Reply.RoundtripTime + " ms");
                            success.WriteLine(Host);
                            SuccessCount++;
                        }
                        else
                        {
                            //Create A New Line
                            Console.WriteLine("");
                            Console.Write(Host + ": ");
                            CPrint("Failure", ConsoleColor.Red);
                            failure.WriteLine(Host);
                            FailureCount++;
                        }

                        //Increment The Average Ping Time
                        AveragePingTime += Reply.RoundtripTime;

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("");
                        Console.Write(Host + ": ");
                        CPrint("Failure", ConsoleColor.Red);
                        failure.WriteLine(Host);
                        FailureCount++;
                    }
                }

                //Reset The Console Colour
                Console.ForegroundColor = ConsoleColor.White;

                //Close It Off
                success.Close();
                failure.Close();
                ip_info.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(0);

            }
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("----------------  Statistics --------------------");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Average Ping Time: " + Math.Round((decimal)AveragePingTime / TotalCount, 2) + " ms");
            Console.WriteLine("Total Hosts: " + TotalCount);
            Console.WriteLine("Failed Pings: " + FailureCount);
            Console.WriteLine("Successful Pings: " + SuccessCount);
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("-----------  Hit Any Key To Quit ----------------");
            Console.WriteLine("-------------------------------------------------");

            if (args.Length < 1)
            {
                Console.In.ReadLine();
            }

            Environment.Exit(0);
        }

        public static void CPrint(string Text, ConsoleColor Color)
        {
            //Set The Colour Of The Font, Then Set It Back To White
            Console.ForegroundColor = Color;
            Console.Write(Text);
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static string GetIPString(IPHostEntry iphostentry)
        {
            string IPString = "";
            string Divider = "";
            foreach (IPAddress IP in iphostentry.AddressList)
            {
                IPString += Divider + IP.ToString();
                Divider = ", ";
            }
            return IPString;
        }

        public static void WriteLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ########  #### ##    ##  ######   ######## ########  ####");
            Console.WriteLine(" ##     ##  ##  ###   ## ##    ##  ##       ##     ## ####");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ##     ##  ##  ####  ## ##        ##       ##     ## ####");
            Console.WriteLine(" ########   ##  ## ## ## ##   #### ######   ########   ##");
            Console.WriteLine(" ##         ##  ##  #### ##    ##  ##       ##   ##");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" ##         ##  ##   ### ##    ##  ##       ##    ##  ####");
            Console.WriteLine(" ##        #### ##    ##  ######   ######## ##     ## ####");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
