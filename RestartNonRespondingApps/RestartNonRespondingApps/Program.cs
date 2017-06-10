using System.Collections.Generic;
using System.Diagnostics;

namespace RestartNonRespondingApps
{

    class Program
    {

        static void Main(string[] args)
        {
            var observedTasks = new string[] { "Outlook" };
            var taskManager = new TaskManager(observedTasks);
            taskManager.Run();
            
            while (true)
            {

            }
        }
    }
}
//OneCommanderV2
//OUTLOOK