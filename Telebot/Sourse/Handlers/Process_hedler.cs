using OpenAI.Files;
using File = System.IO.File;
using Telegram.Bot.Types.ReplyMarkups;
using OpenAI.Chat;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Telebot.Sourse.Item.IItem;
using Message = Telegram.Bot.Types.Message;
using Update = Telegram.Bot.Types.Update;
using Telegram.Bot;

namespace Telebot.Sourse.Handlers
{
    public  class Process_hedler
    {

        private CancellationToken CnsToken {  set; get; }

        private ITelegramBotClient Client { set; get; }







        private  bool chekingUpdate()
        { 
            bool result=true;
            using (var db= new context())
            {




            }

            return result;
        }




    }
}
