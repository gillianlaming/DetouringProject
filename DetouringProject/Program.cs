using System;
using System.IO;
using System.Reflection;

namespace DetouringProject
{
    class Program
    {
        static void Main(string[] args)
        {
            FSW fsw = new FSW();
            Reflection reflection = new Reflection("Changed");
            reflection.HookUpDelegate();
            fsw.Init();
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
       
    }
}
