using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Menu_ProcessType : IItem.IItemDB<Menu_ProcessType>
    {
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }

        public string? Code { get; set; }
        virtual public List<Menu_Process> Menus { get; set; }= new List<Menu_Process>();



    }
}
