using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace Telebot.Sourse.Item
{
    public class menuPropsHR : IItem.IItemDB<menuPropsHR>
    {
        public int MyId { get ; set ; }
        public string? MyDescription { get ; set ; }
        public string? MyName { get ; set ; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }
        public bool? IsDelite { get ; set ; }

        public string? menuContent { set; get; }

        List<myPhoto> photos { set; get; } = new List<myPhoto>();

        /// <summary>
        /// menuPropsHR - phr
        /// </summary>
        /// <returns>"mp:{phr:Id}|"</returns>
        public string GetEntityTypeId()
        {
            return $"phr:{MyId}|";
        }


    }
}
