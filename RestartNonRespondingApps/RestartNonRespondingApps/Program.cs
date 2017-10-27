using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;


namespace RestartNonRespondingApps
{

    class Program
    {

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        static void Main(string[] args)
        {

        var handle = GetConsoleWindow();
        ShowWindow(handle, 6 ); //Start app minimized

        var apps = new[] { new App("OUTLOOK") }; //new App("OneCommanderV2")

            while (true)
            {
                foreach(App app in apps)
                {
                    app.UpdateAppState();
                }

                Thread.Sleep(100); // TODO: find better implementaion for Thread.Sleep()
            }
        }
    }
}
//OneCommanderV2
//OUTLOOK