using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class User_Types:IItemDB<User_Types>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public string? TypeCode { get; set; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }
        public bool? IsDelite { get ; set ; }

        public bool? IsDefoult { get; set; }

        virtual public List<MyUser> Users { get; set; } = new List<MyUser>();


        virtual public List<Menu_Process> Processes { get; set; } = new List<Menu_Process>();




    }
}
