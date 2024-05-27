using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Telebot.Sourse.Item
{
    public class Log : IItem.IItemDB<Log>
    {
        [Key]
        public int MyId { get ; set ; }
        public string? MyDescription { get ; set ; }
        public string? MyName { get ; set ; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }
        public bool? IsDelite { get ; set ; }


        public int? MyChatId { set; get; }
        public virtual MyChat? MyChat { set; get; }


        public int? TeleMessageId { set; get; }

        public string? Callback { set; get; }


        
        



        public static int GetIdFromUpdate(Telegram.Bot.Types.Update update)
        {
            return new TeleTools().getentyIdByUpdate("L", update);
        }



        public string GetEntityTypeId()
        {
            return $"L:{MyId}|";
        }
    }
}
