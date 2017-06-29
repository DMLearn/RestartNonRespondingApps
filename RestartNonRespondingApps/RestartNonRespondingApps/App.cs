using System.Threading;
using System.Threading.Tasks;
using Stateless;

namespace RestartNonRespondingApps
{
    class App
    {
        enum State { notRunning, Responding, notResponding, Killed, Started, ManualMode}
        enum Trigger { startedByUser, stopedByUser, isResponding, isNotResponding, isKilling, isNotKilling, isStarting, isNotStarting, isManuallyHandeled, isNotRunning}

        State _state = State.notRunning;
        StateMachine<State, Trigger> _machine;

        public App()
        {
            _machine = new StateMachine<State, Trigger>(_state);

            _machine.Configure(State.notRunning)
                .Permit(Trigger.isNotRunning, State.notRunning)
                .Permit(Trigger.startedByUser, State.Responding);

            _machine.Configure(State.Responding)
                .Permit(Trigger.stopedByUser, State.notRunning)
                .Permit(Trigger.isResponding, State.Responding)
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
    }
}
//OneCommanderV2
//OUTLOOK