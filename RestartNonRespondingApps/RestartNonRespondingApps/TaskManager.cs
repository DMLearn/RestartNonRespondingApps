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

        private readonly List<Tuple<string, _state, int>> _selectedTasks = new List<Tuple<string, _state, int>>();
        
        public TaskManager(string[] tasks)
        {
            for (int i = 0; i < tasks.Length; i++)
            {                
                _selectedTasks.Insert(i, Tuple.Create(tasks[i], _state.notStarted, 0));
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
            Console.WriteLine("{3}\t Name: {0}\t State: {1}\t Count: {2}", _selectedTasks[0].Item1, _selectedTasks[0].Item2, _selectedTasks[0].Item3, DateTime.Now.ToLongTimeString());
        }

        private void CheckTasks()
        {
            for (int j = 0; j < _selectedTasks.Count; j++)
            {
                GetTaskStatus(j, _selectedTasks[j].Item1);

                //if (_selectedTasks[j].Item2 == _state.notResponding)
                //{
                //    RestartTask(j, _selectedTasks[j].Item1);
                //}
            }
        }

        private void GetTaskStatus(int index, string name)
        {
            var process = Process.GetProcessesByName(name);

            if (process.Length > 0)
            {
                if (process[0].Responding)
                {
                    var tuple = Tuple.Create(name, _state.isResponding, 0);
                    UpdateSelectedList(tuple, index);
                }
                else if (!process[0].Responding)
                {
                    var tuple = Tuple.Create(name, _state.notResponding, 0);
                    UpdateSelectedList(tuple, index);
                }
            }
            else
            {
                var tuple = Tuple.Create(name, _state.notStarted, 0);
                UpdateSelectedList(tuple, index);
            }
        }

        private void UpdateSelectedList(Tuple<string, _state, int> tuple, int index)
        {
            _selectedTasks.RemoveAt(index);
            _selectedTasks.Insert(index, tuple);
        }

        private void RestartTask(int index, string name)
        {
            //var process = Process.GetProcessesByName(name);
            //process[0].Kill();

            //var tuple = Tuple.Create(name, _state.i, 0);    //TODO: Continue implementation here
        }
    }
}
