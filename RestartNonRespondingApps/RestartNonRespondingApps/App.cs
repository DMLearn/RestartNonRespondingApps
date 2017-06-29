//using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using Stateless;
using System.Diagnostics;
using System.Windows.Forms;
using System;

namespace RestartNonRespondingApps
{
    public class App
    {
        public enum State
        {
            notRunning,
            Responding,
            notResponding,
            Killed,
            Started,
            ManualMode
        }

        public enum Trigger
        {
            startedByUser,
            stopedByUser,
            isResponding,
            isNotResponding,
            isKilling,
            isNotKilling,
            isStarting,
            isNotStarting,
            isManuallyHandeled,
            isNotRunning
        }

        public StateMachine<State, Trigger> _machine { get; }

        private System.Timers.Timer _timer = new System.Timers.Timer(2000);
        private readonly string _appName;
        private State _state = State.notRunning;

        public App(string appName)
        {
            _appName = appName;
            _machine = new StateMachine<State, Trigger>(_state);

            _machine.Configure(State.notRunning)
                //.InternalTransition(Trigger.isNotRunning, () => { }) //created an empty lambda since it is requiered by the internalTransisition
                .Permit(Trigger.startedByUser, State.Responding);

            _machine.Configure(State.Responding)
                //.InternalTransition(Trigger.isResponding, () => { }) //created an empty lambda since it is requiered by the internalTransisition
                .Permit(Trigger.stopedByUser, State.notRunning)
                .Permit(Trigger.isNotResponding, State.notResponding);

            _machine.Configure(State.notResponding)
                .Permit(Trigger.isKilling, State.Killed);

            _machine.Configure(State.Killed)
                .Permit(Trigger.isNotKilling, State.ManualMode)
                .Permit(Trigger.isStarting, State.Started);

            _machine.Configure(State.Started)
                .Permit(Trigger.isNotStarting, State.ManualMode)
                .Permit(Trigger.isNotResponding, State.notResponding)
                .Permit(Trigger.isResponding, State.Responding);

            _machine.Configure(State.ManualMode)
                .Permit(Trigger.isManuallyHandeled, State.notRunning);
        }

        private void UpdateAppState()
        {
            var state = GetAppState();

            switch (_machine.State)
            {
                case State.notRunning:
                    if (state == State.Responding) _machine.Fire(Trigger.startedByUser);
                    break;

                case State.Responding:
                    if (state == State.notRunning) _machine.Fire(Trigger.stopedByUser);
                    else if (state == State.notResponding) _machine.Fire(Trigger.isNotResponding);
                    break;

                case State.notResponding:
                    _machine.Fire(Trigger.isKilling);
                    break;

                case State.Killed:
                    if (!_timer.Enabled)
                    {
                        KillApp();
                        StartTimmer();
                    }
                    else if (state == State.notRunning) _machine.Fire(Trigger.isStarting);
                    /*TODO : Continue implementing the OnTimerEvent such that it checks
                           finally after the timer has elapsed whether a correct
                           app kill was performed
                           e.g : else if()
                    */

                    break;

                case State.ManualMode:
                    MessageBox.Show(_appName + "bitte manuel beenden!");
                    break;

                default:
                    break;
            }



        }

        private void StartTimmer()
        {
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
        }

        private void StartApp()
        {
            Process.Start(_appName);
        }

        private void KillApp()
        {
            var process = Process.GetProcessesByName(_appName);
            process[0].Kill();
        }

        private State GetAppState()
        {
            var process = Process.GetProcessesByName(_appName);

            if (process.Length > 0)
            {
                if (process[0].Responding) return State.Responding;
                else return State.notResponding;
            }
            else return State.notRunning;
                
        }

    }
}
