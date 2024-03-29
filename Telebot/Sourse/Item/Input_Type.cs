using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Input_Type : IItem.IItemDB<Input_Type>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }

        public string? Code { get; set; }
        public bool? isDefoult { get; set; }

        virtual public List<Process_Input> All_Inputs { set; get; } = new List<Process_Input>();


        /// <summary>
        /// Input_Type - it
        /// </summary>
        /// <returns>"mp:{it:Id}|"</returns>
        public string GetEntityTypeId()
        {
            return $"it:{MyId}|";
        }

    }
}
