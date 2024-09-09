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
        static int Count = 0;

        public static void AddLog(string message)
        {
            Messages.Add($"{++Count} : {DateTime.Now.ToString("dd MMM yy HH:mm:ss" )} : {message}");
        }

        public static List<string> GetMessages()
        {
            return Messages;
        }
    }
}
