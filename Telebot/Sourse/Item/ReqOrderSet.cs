using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    /// <summary>
    /// заказ. в него входят список сетов которые можно забронить и которые может выбрать пользователь
    /// </summary>
    public class ReqOrderSet : IItem.IItemDB<ReqOrderSet>
    {
        [Key]
        public int MyId { get ; set ; }
        public string? MyDescription { get ; set ; }
        public string? MyName { get ; set ; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }
        public bool? IsDelite { get ; set ; }  = false;


        public bool? IsCreate { get; set; } = false;



        public virtual List<DateSetTime> TimeSets { set; get; } = new List<DateSetTime>();

      


        public virtual DateTime? Date { set; get; }




        public int? ChatId { set; get; }
        public virtual MyChat? Chat { get; set; }


        public string GetEntityTypeId()
        {
            return $"Or:{MyId}|";
        }
    }
}
