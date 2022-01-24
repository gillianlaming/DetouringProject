using System;
using System.IO;
using System.Reflection;

namespace DetouringProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Reflection reflection = new Reflection("Changed");
            reflection.HookUpDelegate();

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
       
    }
}
