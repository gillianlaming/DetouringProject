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
        private Type _targetType;
        private string _eventOverride;
      //  private string _handlerToOverride;
        public Reflection(Type type, string eventToOverride)
        {
            _targetType = type;
            _eventOverride = eventToOverride;
           // _handlerToOverride = handlerToOverride;
        }

        public void HookUpDelegate()
        {
            //Assembly assembly = _targetType.Assembly;
            //Assembly assembly = Assembly.GetExecutingAssembly();
            Assembly assembly = typeof(FSW).Assembly;
            
            Console.WriteLine("Currently executing assembly:");
            Console.WriteLine("   {0}\n", assembly.FullName);

            Type type = assembly.GetType("FSW"); //todo: unhardcode this?

            //Create our own instance of this type
            // There needs to be a default constructor for this to work
            Object assemblyAsObj = Activator.CreateInstance(_targetType);

            //Get the event we want to update
            EventInfo eventInfo = _targetType.GetEvent(_eventOverride);
            Type eventHandlingDelegate = eventInfo.EventHandlerType;

            //Get the method which handles this event
            //Todo -- what if we don't know what handles the event/**/
            MethodInfo mehtodHandler = typeof(Reflection).GetMethod("OnChangedMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate d = Delegate.CreateDelegate(eventHandlingDelegate, this, mehtodHandler);

            //Get the add accessing method to actually hook up our overriDden method
            MethodInfo addHandler = eventInfo.GetAddMethod();
            Object[] addHandlerArgs = { d };
            addHandler.Invoke(assemblyAsObj, addHandlerArgs);
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
