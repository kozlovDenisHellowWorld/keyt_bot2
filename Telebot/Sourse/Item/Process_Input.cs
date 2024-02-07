using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telebot.Sourse.Item
{
    public class Process_Input : IItem.IItemDB<Process_Input>
    {
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }

        
        public int? NextProcessMenuId { get; set; }
        public virtual Menu_Process? NextProcessMenu { get; set; }





        public int? MenuProcessId { get; set; }
        virtual public Menu_Process? MenuProcess { get; set; }




    }
}
