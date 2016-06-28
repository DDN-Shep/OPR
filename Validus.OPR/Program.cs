using Microsoft.Owin.Hosting;
using Serilog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Validus.OPR
{
    public class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPosition(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        private const string DefaultHostUrl = "http://localhost:12345";

        public static void Main(string[] args)
        {
            ConfigureConsole(args);

            var url = ConfigurationManager.AppSettings["HostUrl"];

            if (string.IsNullOrEmpty(url)) url = DefaultHostUrl;

            using (WebApp.Start<Startup>(url))
            {
                Log.Information("{0} listening on {1}{2}", Console.Title, url, Environment.NewLine);

                Log.Information("Enter a number and hit 'Enter' to load an existing OPR page...");
                Log.Information("Hit 'Spacebar' key to open the OPR landing page...");
                Log.Information("Hit 'Escape' key to terminate OPR server...");

                ConsoleKeyInfo cki;

                var id = string.Empty;

                while ((cki = Console.ReadKey(true)).Key != ConsoleKey.Escape)
                {
                    if (cki.Key == ConsoleKey.Spacebar) Process.Start(url);
                    else if (cki.Key == ConsoleKey.Enter)
                    {
                        Process.Start(string.Concat(url, "?id=", id));

                        id = string.Empty;
                    }
                    else if ((cki.Key >= ConsoleKey.D0 && cki.Key <= ConsoleKey.D9)
                        || (cki.Key >= ConsoleKey.NumPad0 && cki.Key <= ConsoleKey.NumPad9))
                    {
                        id = string.Concat(id, cki.KeyChar);
                    }
                }
            }
        }

        private static void ConfigureConsole(string[] args)
        {
            Console.Title = "Operational Performance Report Server";

            Console.SetBufferSize(150, 50);
            Console.SetWindowSize(150, 50);

            var window = GetConsoleWindow();

            SetWindowPosition(window, 0, 25, 25, 0, 0, 0x0001);
        }
    }
}