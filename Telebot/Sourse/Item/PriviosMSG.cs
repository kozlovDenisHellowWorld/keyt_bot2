using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class PriviosMSG : IItemDB<PriviosMSG>
    {
        [Key]
        public int MyId { get ; set ; }
        public string? MyDescription { get ; set ; }
        public string? MyName { get ; set ; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }

        public bool? IsDelite { set; get; }
        public int? MessageId { set; get; }

        public long? UupdateId { set; get; }

        public bool? NedTodelite { set; get; }


        public int? ChatId { set; get; }
        [ForeignKey("ChatId")]
        public virtual MyChat? Chat { set; get; }





        static public PriviosMSG createMessage(long? botClientId, bool needToDelete, Message message,Update update)
        {
            PriviosMSG priviosMSG = new PriviosMSG()
            { 
                 


            };

            priviosMSG.BotClientId = botClientId;
            priviosMSG.dateTimeCreation = DateTime.Now;
            priviosMSG.IsDelite = false;
            priviosMSG.MessageId = message.MessageId;
            priviosMSG.NedTodelite = needToDelete;
            priviosMSG.UupdateId = update?.Id;
            priviosMSG.MyDescription = "Сообщение";


            return priviosMSG;
        }


        static public List< PriviosMSG> createMessage(long? botClientId, bool needToDelete, List<Message> messages, Update update)
        {

            List<PriviosMSG> myMessagess = new List<PriviosMSG>();

            foreach (var message in messages)
            {

                PriviosMSG priviosMSG = new PriviosMSG()
                {



                };

                priviosMSG.BotClientId = botClientId;
                priviosMSG.dateTimeCreation = DateTime.Now;
                priviosMSG.IsDelite = false;
                priviosMSG.MessageId = message.MessageId;
                priviosMSG.NedTodelite = needToDelete;
                priviosMSG.UupdateId = update.Id;
                priviosMSG.MyDescription = "Сообщение";

                myMessagess.Add(priviosMSG);

            }

        


            return myMessagess;
        }


    }
}
