using System.Collections.Generic;
using System.Diagnostics;

namespace RestartNonRespondingApps
{
    class Program
    {

        static void Main(string[] args)
        {
            var taskRunnung = new DetectTaskStatus();
            taskRunnung.Run();
        }
    }
}
//OneCommanderV2
//OUTLOOK