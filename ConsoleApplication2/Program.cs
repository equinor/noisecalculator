using System.Configuration;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //IKernel kernel = new StandardKernel(new NoiseCalculatorModule());

            string connectionString = ConfigurationManager.AppSettings["Lol"];
            string lol = "lol";

        }
    }
}