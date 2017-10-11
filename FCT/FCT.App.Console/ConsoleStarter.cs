using FCT.Bootstrapper; 

namespace FCT.App.Console
{
    public class ConsoleStarter
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Staring Console App");

            var bootstrapper = new Bootstrappy();
            bootstrapper.RunInConsole();           
        }
    }
}
