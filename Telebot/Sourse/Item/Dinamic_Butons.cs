using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Dinamic_Butons : IItem.IItemDB<myMenuProcess>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }


        public string? CallbackQwery { set; get; }

        public string? Content { set; get; }


        public virtual int? ChatId { set; get; }
        public virtual MyChat? Chat { set; get; }

        /// <summary>
        /// Динамические кнопки - db
        /// </summary>
        /// <returns>"mp:{db:Id}|"</returns>
        public string GetEntityTypeId()
        {
            return $"db:{MyId}|";
        }

    }
}
