using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Text;

namespace DetouringProject
{
    class Reflection
    {
        private string _eventOverride;

        public Reflection(string eventToOverride)
        {
            _eventOverride = eventToOverride;
        }

        public void HookUpDelegate()
        {
            Assembly assembly = typeof(FSW).Assembly;
            Type type = assembly.GetType("DetouringProject.FSW");

            //Create our own instance of this type
            // There needs to be a default constructor for this to work
            Object assemblyAsObj = Activator.CreateInstance(type);

            //Get the event we want to update
            var events = type.GetEvents();
            EventInfo eventInfo = type.GetEvent(_eventOverride);
            Type eventHandlingDelegate = eventInfo.EventHandlerType;

            //Get the method which handles this event
            MethodInfo mehtodHandler = typeof(Reflection).GetMethod("OnChangedMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate d = Delegate.CreateDelegate(eventHandlingDelegate, this, mehtodHandler);
            
            //Testing -- this part succeeds in setting the field for the FSW class
            var field = type.GetField("enableRaisingEvents");
            field.SetValue(assemblyAsObj, true);
            MethodInfo method = type.GetMethod("Init");
            method.Invoke(assemblyAsObj, null);

            //Get the add accessing method to actually hook up our overridden method
            MethodInfo addHandler = eventInfo.GetAddMethod(); 
            Object[] addHandlerArgs = { d };
            addHandler.Invoke(assemblyAsObj, addHandlerArgs);

            //Test change notification by writing text to a file
            TestChangeNotification();
        }
            
        private void TestChangeNotification()
        {
            FileInfo fi = new FileInfo(@"\\iisdist\privates\glaming\Testing.txt");

            if (!fi.Exists)
            {
                fi.Create();
            }

            using (FileStream fs = File.Open(fi.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.\n");
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }

        private void OnChangedMethod(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"[This is using the reflection] Changed: {e.FullPath}");
        }
    }
}
