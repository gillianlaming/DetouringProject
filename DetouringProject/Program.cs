using System;
using System.IO;
using System.Reflection;

namespace DetouringProject
{
    class Program
    {
        static void Main(string[] args)
        {
            FSW fsw = new FSW(@"\\iisdist\privates\glaming");
            Reflection reflection = new Reflection(fsw.GetType(), "Changed");
            reflection.HookUpDelegate();
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
       
    }
}
