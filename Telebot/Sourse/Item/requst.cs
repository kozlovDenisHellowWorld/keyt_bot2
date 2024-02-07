using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace Telebot.Sourse.Item
{
    public class requst : IItem.IItemDB<requst>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }


        public string? reqstTupe { get; set; }
        public string? reqstContent { get; set; }

        public bool? isCreated { get; set; }

        public int? chatId { set; get; }

        public virtual MyChat? chat { get; set; }

        public int? userid { set; get; }
        public virtual MyUser? user { get; set; }


        public bool? isNew { set; get; }
        public bool? isNewForUser { set; get; }

        public virtual List<myPhoto>? Photoes { set; get; } = new List<myPhoto>();


        public bool? isDone { set;get; }


        public static requst createRequst()
        {
            requst result = new requst()
            {
                dateTimeCreation = DateTime.Now,
                MyName = "Запрос",
                isCreated = false,
                IsDelite = false,
                isNew = true,
                isNewForUser = true, isDone=false,
                
            };


            return result;


        }






        public string SetUserIncallBack()
        {
            return $"r:{MyId}|";

        }

        public static int GetUserIdFromCode(string processCode)
        {
            if (processCode == null) return 0;
           
            string value = processCode.Split('|')?.FirstOrDefault(cod => cod.Contains("r"))?.Split(':')[1];

            int result = 0;

            int.TryParse(value, out result);

            return result;

            

        }



    }
}
