using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace _GUIProject
{
    static public class Reflection
    {
        static public Object CreateObject(string opcode)
        {
          
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly a in assemblies)
            {
                Type[] typeArray = a.GetTypes();
                foreach (Type type in typeArray)
                {
                    if (!type.FullName.StartsWith("System") && opcode.Equals(type.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return  Activator.CreateInstance(type);

                    }
                }
            }
            return null;
        }
    }
}
