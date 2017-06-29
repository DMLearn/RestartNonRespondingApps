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
            wasStarted,
            notStarted,
            wasShutdown
        }

        private readonly List<Tuple<string, _state>> _selectedTasks = new List<Tuple<string, _state>>();
        
        public TaskManager(string[] tasks)
        {
            for (int i = 0; i < tasks.Length; i++)
            {                
                _selectedTasks.Insert(i, Tuple.Create(tasks[i], _state.notStarted));
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
            Console.WriteLine("{2}\t Name: {0}\t State: {1}\t ", _selectedTasks[0].Item1, _selectedTasks[0].Item2,  DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// Method checks the states of the selected tasks and saves the values to the field "_selectedTasks".
        /// Based on the state for each process, different procedures are followed.
        /// <para>  
        /// 1) State = isResponding:
        /// While application is running, no actions are taken due to the normal operation. 
        /// </para>
        /// <para>
        /// 2) State = notResponding:
        /// Kill application. Set state to "notStarted" and increment counter item3 by one.
        /// In the next iteration check if application is shutdown and restart process. Incerement item3 by one.
        /// In the next iteration check if application is aup and running, the set state to "isResponding" and set item3 to zero.
        /// </para>   
        /// </summary>
        private void CheckTasks()
        {
            for (int j = 0; j < _selectedTasks.Count; j++)
            {
                //Read task state and update the field _selectedTasks
                GetTaskStatus(j, _selectedTasks[j].Item1);

                //switch (_selectedTasks[j].Item2)
                //{
                //    case _state.notResponding:
                //        ShutdownTask(_selectedTasks[j].Item1);
                //        break;
                //    case _state.wasStarted:
                //        break;
                //    case _state.notStarted:
                //        break;
                //    case _state.wasShutdown:
                //        break;
                //    default:
                //        //Default case represents => _state.isResponding
                //        break;
                //}
            }
        }

        /// <summary>
        /// Reads the current state of a process.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        private void GetTaskStatus(int index, string name)
        {
            var process = Process.GetProcessesByName(name);

            if (process.Length > 0)
            {
                if (process[0].Responding)
                {
                    var tuple = Tuple.Create(name, _state.isResponding);
                    UpdateSelectedList(tuple, index);
                }
                else if (!process[0].Responding)
                {
                    var tuple = Tuple.Create(name, _state.notResponding);
                    UpdateSelectedList(tuple, index);
                }
            }
            else
            {
                var tuple = Tuple.Create(name, _state.notStarted);
                UpdateSelectedList(tuple, index);
            }
        }

        /// <summary>
        /// Update the field _selectedTasks .
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="index"></param>
        private void UpdateSelectedList(Tuple<string, _state> tuple, int index)
        {
            _selectedTasks.RemoveAt(index);
            _selectedTasks.Insert(index, tuple);
        }

        /// <summary>
        /// Restart a process after shutdown.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        private void RestartTask(string name)
        {
             
        }

        /// <summary>
        /// Kill a non responding process. 
        /// </summary>
        /// <param name="name"></param>
        private void ShutdownTask(string name)
        {
            
        }
    }
}
