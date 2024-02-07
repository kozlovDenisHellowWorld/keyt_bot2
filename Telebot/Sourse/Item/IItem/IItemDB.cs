using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item.IItem
{
    public interface IItemDB<T>
    {
        [Key]
        public int MyId { set; get; }

        [Column(TypeName = "nchar(3000)")]
        public string? MyDescription { set; get; }

        [Column(TypeName = "nchar(3000)")]
        public string? MyName { set; get; }

        public DateTime? dateTimeCreation { set; get; }

        public long? BotClientId { set; get; }

        public bool? IsDelite { set; get; }











    }
}
