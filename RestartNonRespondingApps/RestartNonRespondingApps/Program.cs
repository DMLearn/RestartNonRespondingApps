using System.Collections.Generic;
using System.Diagnostics;

namespace RestartNonRespondingApps
{

    class Program
    {

        static void Main(string[] args)
        {
            var observedTasks = new string[] { "Outlook", "Word" };
            var taskManager = new TaskManager(observedTasks);
        }
    }
}
//OneCommanderV2
//OUTLOOK