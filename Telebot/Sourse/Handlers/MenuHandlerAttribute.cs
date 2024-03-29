using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Sourse.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MenuHandlerAttribute : Attribute
    {
        public string MenuCode { get; }

        

        public MenuHandlerAttribute(string menuCode)
        {
            MenuCode = menuCode;
          
        }
    }
}
