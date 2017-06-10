using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
namespace RestartNonRespondingApps
{
    public class TaskManager
    {
        private enum _state
        {
            notResponding,
            isResponding,
            isStarted,
            notStarted
        }

        /*TODO: After installing C#7.0 use arraylist instead of array or list
                array needs to be intialized to a fixed size
                tuples are immutable and therofore cannot be changed durring runtime
         */
        private Tuple<string, _state, int>[] _selectedTasks = new Tuple<string, _state, int>[10];

        public TaskManager(string[] tasks)
        {
            int index = 0;
            foreach (string task in tasks)
            {                
                _selectedTasks[0] = Tuple.Create(task, _state.notStarted, 0);
                index++;
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
            int index = 0;
            foreach (var task in _selectedTasks)
            {
                GetTaskStatus(index, task.Item1);
                index ++;
            }
        }

        private void GetTaskStatus(int index, string name)
        {
            var process = Process.GetProcessesByName(name);

            //if (process.Length > 0)           
            //{
            //    if (process[0].Responding == true)
            //        _selectedTasks[index].Item2 = _state.isResponding;
            //    else if (process[0].Responding == false)
            //        _selectedTasks[index].Item2 = _state.notResponding;
            //}
            //else
            //{
            //    _selectedTasks[index].Item2 = _state.notStarted;                
            //}

        }

        private void RestartTask(int index)
        {

        }
    }
}
