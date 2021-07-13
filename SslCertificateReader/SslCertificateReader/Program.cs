using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SslCertificateReader
{
    static class Program
    {
        static string certPath = @"";
        static string certFileName = "";
        static string certPassword = "";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the certificate reader.  Follow the instructions to read a certificate and see the properties");

            StartMenu();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        static void StartMenu()
        {
            ReadCertFolder();
        }

        static void ReadCertFolder()
        {
            //get user input on which folder to read
            Console.WriteLine("Where are your certificate files?");
            certPath = Console.ReadLine();

            try
            {
                //show the files in the folder
                var files = Directory.GetFiles(certPath);
                if (files != null && files.Length > 0)
                {
                    Console.WriteLine(Environment.NewLine);
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine($"- {Path.GetFileName(files[i])}");
                    }
                }

                Console.WriteLine(Environment.NewLine);
                SelectFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sorry, there was an error getting the files.");
                Console.WriteLine(Environment.NewLine);
                Restart();
            }
        }

        static void SelectFile()
        {
            Console.WriteLine("Which Certificate do you want to read?   Type in the certificate name and extension");
            certFileName = Console.ReadLine();

            Console.WriteLine(Environment.NewLine);

            GetCertPassword();
        }

        static void GetCertPassword()
        {
            Console.Write("Does this certificate require a password?  Y/N.....");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
                
                if (cki.Key == ConsoleKey.Y)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Enter the certificate password");
                    certPassword = Console.ReadLine();
                    break;
                }

                if(cki.Key == ConsoleKey.N)
                {
                    Console.WriteLine(Environment.NewLine);
                    break;
                }
            } while (cki.Key != ConsoleKey.Y || cki.Key != ConsoleKey.N);

            Console.WriteLine(Environment.NewLine);

            ReadCertificate();
        }

        static void ReadCertificate()
        {
            Console.Write("Reading Certificate...");

            X509Certificate2 certificate;

            try
            {
                certificate = string.IsNullOrWhiteSpace(certPassword)
                ? new X509Certificate2(Path.Combine(Path.GetFullPath(certPath), certFileName))
                : new X509Certificate2(Path.Combine(Path.GetFullPath(certPath), certFileName), certPassword);

                Console.Write("Done.  Printing Properties");
                Console.WriteLine(Environment.NewLine);

                //Print Properties
                if (certificate != null)
                {
                    Console.WriteLine($"Issuer Name: {certificate.Issuer}");
                    Console.WriteLine($"Subject: {certificate.Subject}");
                    Console.WriteLine($"Effective Date: {certificate.GetEffectiveDateString()}");
                    Console.WriteLine($"Expiration Date: {certificate.GetExpirationDateString()}");
                    Console.WriteLine($"Signature Algorithm Friendly Name: {certificate.SignatureAlgorithm.FriendlyName}");
                    Console.WriteLine($"Version: {certificate.Version}");
                    Console.WriteLine($"Has Private Key: {certificate.HasPrivateKey}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sorry, there was an error reading the certificate.");
                Console.WriteLine(Environment.NewLine);
            }
            finally
            {
                Restart();
            }
        }

        static void Restart()
        {
            Console.WriteLine(Environment.NewLine);
            Console.Write("Would you like to check another certificate?  Y/N.....");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    StartMenu();
                    break;
                }

                if (cki.Key == ConsoleKey.N)
                {
                    Console.WriteLine(Environment.NewLine);
                    break;
                }
            } while (cki.Key != ConsoleKey.Y || cki.Key != ConsoleKey.N);
        }
    }
}
