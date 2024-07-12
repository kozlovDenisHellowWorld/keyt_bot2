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
    /// Сет
    /// </summary>
    public class DateSetTime : IItem.IItemDB<DateSetTime>
    {

        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }


        public bool? IsDelite { get; set; }=false;

        /// <summary>
        /// Выбралин ли сет
        /// </summary>
        public bool? IsTarget { get; set; } = false;


        /// <summary>
        /// Дата и время сета 
        /// </summary>
        public DateTime? SetdateTime { get; set; }

        /// <summary>
        /// Чей сет
        /// </summary>
        public string? name{ get; set; }

        /// <summary>
        /// Чей сет телефоне
        /// </summary>
        public string? Telephone { get; set; }








        /// <summary>
        ///  Заказ к которому привязано  ID
        /// </summary>
        public int? OrderId { set; get; }
        /// <summary>
        /// Заказ к которому привязано 
        /// </summary>
        public virtual ReqOrderSet? Order {set;get;} 




        public string GetEntityTypeId()
        {
            return $"DST:{MyId}|";
        }
    }
}
