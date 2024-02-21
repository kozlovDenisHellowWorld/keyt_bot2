using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Menu_Process : IItemDB<Menu_Process>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }



        public string? ProcessMenuCode { get; set; }

        public string? Navigation { get; set; }

        public string? MenuProcessContent { get; set; }

        public bool? IsAwaytingText { get; set; }






        
        public int? ProcessTypeId { get; set; }
        virtual public Menu_ProcessType? ProcessType { get; set; }



        virtual public List<Process_Input> Inputs { get; set; } = new List<Process_Input>();

        virtual public List<Process_Input> CallingInputs { get; set; } = new List<Process_Input>();



        virtual public List<MyChat> Chats { get; set; } = new List<MyChat>();




        public int? UserTypeId { get; set; }
        virtual public User_Types? UserType { get; set; }


    }
}
