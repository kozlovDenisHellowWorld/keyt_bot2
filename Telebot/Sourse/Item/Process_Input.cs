using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Process_Input : IItem.IItemDB<Process_Input>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }




        public int? input_TypeId { get; set; }
        public virtual Input_Type? input_Type { get; set; }


        public string? NameIfTrue { get; set; }

        public string? NameIfFalse { get; set; }

        public int? NextProcessMenuId { get; set; }
        public virtual Menu_Process? NextProcessMenu { get; set; }

        public string? NextProcessMenuCode { get; set; }



        public int? MenuProcessId { get; set; }
        virtual public Menu_Process? MenuProcess { get; set; }

        public string? MenuProcessCode { get; set; }


        /// <summary>
        /// Process_Input - pmsg:
        /// </summary>
        /// <returns>"pin:{MyId}|"</returns>
        public string GetEntityTypeId()
        {
            return $"pin:{MyId}|";
        }

        public static int GetIdFromUpdate(Update update)
        {

            return new TeleTools().getentyIdByUpdate("pin", update);


        }


    }
}
