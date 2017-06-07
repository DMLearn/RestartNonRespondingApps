using System.Collections.Generic;
using System.Diagnostics;


namespace RestartNonRespondingApps
{
    public class DetectTaskStatus
    {
        //TODO: Break down Run() in several methods each responsible for one task (e.g. check running tasks, start application,...)
        //TODO: Erase while loop in Run() and leave the task of repating to the calling class Program.cs or a controller class
        //TODO: Add async for the case of starting several applications at the same time => wait for start event
        //TODO: Add thread delay if program is started or detect start event
        //TODO: Implement error handling
        //TODO: Run app in autostart of windows  

        private Process[] _runningTasks = Process.GetProcesses();
        private List<string> _observedTasks = new List<string>();
        private int _timerThreadDelay = 5000;

        public DetectTaskStatus()
        {
            _observedTasks.Add("OUTLOOK");
            _observedTasks.Add("OneCommanderV2");
        }

        public void Run()
        {
            while (true)
            {
                foreach (string task in _observedTasks)
                {
                    try
                    {
                        var process = Process.GetProcessesByName(task);

                        if (process.Length == 0 || process[0].Responding == false)
                            Process.Start(task);

                    }
                    catch (System.Exception)
                    {

                        throw;
                    }


                }

                _runningTasks = Process.GetProcesses();
            }
        }
    }
}
