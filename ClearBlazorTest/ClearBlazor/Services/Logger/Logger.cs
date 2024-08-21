using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlazor
{
    public static class Logger
    {
        static List<string> Messages = new List<string>(); 

        public static void AddLog(string message)
        {
            Messages.Add($"{DateTime.Now.ToString("dd MMM yy HH:mm:ss" )} : {message}");
        }

        public static List<string> GetMessages()
        {
            return Messages;
        }
    }
}
