using System.Threading;
using System.Threading.Tasks;


namespace RestartNonRespondingApps
{

    class Program
    {

        static void Main(string[] args)
        {
            var observedTasks = new string[] { "Outlook" };
            var taskManager = new TaskManager(observedTasks);
            taskManager.Run();

            while (true)
            {
                Thread.Sleep(1); // TODO: find better implementaion for Thread.Sleep()
            }
        }
    }
}
//OneCommanderV2
//OUTLOOK