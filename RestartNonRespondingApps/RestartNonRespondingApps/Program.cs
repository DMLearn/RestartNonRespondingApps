using System.Threading;
using System.Collections.Generic;


namespace RestartNonRespondingApps
{

    class Program
    {

        static void Main(string[] args)
        {
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