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
        int MyId { set; get; }

        [Column(TypeName = "nchar(3000)")]
        string? MyDescription { set; get; }

        [Column(TypeName = "nchar(3000)")]
        string? MyName { set; get; }

        DateTime? dateTimeCreation { set; get; }

        long? BotClientId { set; get; }

        bool? IsDelite { set; get; }

        string GetEntityTypeId();
       








    }
}
