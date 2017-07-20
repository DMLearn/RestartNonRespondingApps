using System.Timers;
using Stateless;
using System.Diagnostics;
using System;

namespace RestartNonRespondingApps
{
    public class App
    {
        private enum State
        {
            notRunning,
            Responding,
            notResponding,
            Killed,
            Started,
            ManualMode
        }

        private enum Trigger
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

        private readonly StateMachine<State, Trigger> _machine;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(2000);
        private readonly string _appName;

        private State _state = State.notRunning;

        public App(string appName)
        {
            _appName = appName;
            _timer.Elapsed += OnTimerElapsed;
            _machine = new StateMachine<State, Trigger>(_state);

            _machine.Configure(State.notRunning)
                .PermitReentry(Trigger.isNotRunning)
                .Permit(Trigger.startedByUser, State.Responding);

            _machine.Configure(State.Responding)
                .PermitReentry(Trigger.isResponding)
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

        public void UpdateAppState()
        {
            var state = GetAppState();

            switch (_machine.State)
            {
                case State.notRunning:
                    if (state == State.Responding)
                    {
                        _machine.Fire(Trigger.startedByUser);
                        ConsolePrint(ConsoleColor.Green, "läuft.");
                    }
                    break;

                case State.Responding:
                    if (state == State.notRunning)
                    {
                        _machine.Fire(Trigger.stopedByUser);
                        ConsolePrint(ConsoleColor.Green, "beendet.");
                    }
                    else if (state == State.notResponding)
                    {
                        _machine.Fire(Trigger.isNotResponding);
                        ConsolePrint(ConsoleColor.Red, "antwortet nicht.");
                    }
                    break;

                case State.notResponding:
                    if (!_timer.Enabled)
                    {
                        _machine.Fire(Trigger.isKilling);
                        KillApp();
                        StartTimmer();
                        ConsolePrint(ConsoleColor.Red, "wird beendet."); //TODO: Durch filelogger ersetzen 
                    }
                    break;

                case State.Killed:
                    if (!_timer.Enabled && state == State.notRunning)
                    {
                        _machine.Fire(Trigger.isStarting);
                        StartApp();
                        StartTimmer();
                        ConsolePrint(ConsoleColor.Green, "wird gestartet.");
                    }
                    else if (!_timer.Enabled)
                    {
                        _machine.Fire(Trigger.isNotKilling);
                        ConsolePrint(ConsoleColor.Red, "muss manuel durch Nutzer beendet werden!");
                    }
                        break;

                case State.Started:
                    if (!_timer.Enabled && state == State.Responding)
                    {
                        _machine.Fire(Trigger.isResponding);
                        ConsolePrint(ConsoleColor.Green, "läuft.");
                    }
                    else if (!_timer.Enabled)
                    {
                        _machine.Fire(Trigger.isNotStarting);
                        ConsolePrint(ConsoleColor.Red, "muss manuel durch Nutzer gestartet werden..");
                    }
                    break;

                case State.ManualMode:
                    if (state == State.notRunning) _machine.Fire(Trigger.isManuallyHandeled);
                    break;
            }
        }

        private void StartTimmer()
        {
            _timer.Enabled = true;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
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

        private void ConsolePrint(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + _appName + " " + message);
        }        
    }
}
