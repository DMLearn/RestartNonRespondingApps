using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RestartNonRespondingApps
{
    public class TaskManager
    {
        //TODO: implement EventHandler for the event of a task restart

        private enum _state
        {
            notResponding,
            isResponding,
            notStarted
        }
        private Tuple<string, _state>[] _selectedTasks { get; set; }
        
        public TaskManager(string[] tasks)
        {
            for (int j = 0; j < tasks.Length; j++)
            {
                _selectedTasks[j].Item1 = "dasd";
            }
        }

    }
}          

//        public void Start()
//        {

//        }

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