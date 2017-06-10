using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
namespace RestartNonRespondingApps
{
    public class TaskManager
    {
        //TODO: implement EventHandler for the event of a task restart

        private enum _state
        {
            notResponding,
            isResponding,
            isStarted,
            notStarted
        }

        private List<Tuple<string, _state, int>> _selectedTasks = new List<Tuple<string, _state, int>> { };

        public TaskManager(string[] tasks)
        {
            foreach (string task in tasks)
            {
                var tupleElement = Tuple.Create(task, _state.notStarted, 0);
                _selectedTasks.Add(tupleElement);
            }
        }
        
        public void Run()
        {
            var timer = new Timer(3000);
            timer.Elapsed += OnTimerElapsed;
            timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CheckTasks();
        }

        private void CheckTasks()
        {
            //int index = 0;
            foreach (var task in _selectedTasks)
            {
                try
                {
                    var taskResponding = Process.GetProcessesByName(task.Item1)[0].Responding;
                    Console.WriteLine(task.Item1 + ": " + taskResponding.ToString());
                }
                catch (Exception)
                {
                    Console.WriteLine(task.Item1 + ": not started!");
                }
                

            }
        }

        private void GetStatus(int index)
        {
         
        }

        private void SetStatus(int index)
        {

        }
    }
}


//        public void Stop()
//        {

//        }

//        private void GetStatus()
//        {
//            for (int i = 0; i < _selectedTasks.Count; i++)
//            {
//                var process = Process.GetProcessesByName(_selectedTasks[i].Item1);

//                if (process.Length > 0 || process[0].Responding)
//                    _selectedTasks[i].Item2 = _state.isResponding;
//            }

//            foreach (Tuple<string, _state> task in _selectedTasks)
//            {

//                if (process.Length > 0 && process[0].Responding)


//            }
//                task.Item2 = Process.GetProcessesByName(task.Item1)[0].Responding;



//        }

//        private void RestartTask()
//        {

//        }


//    }
//}
////OneCommanderV2
////OUTLOOK