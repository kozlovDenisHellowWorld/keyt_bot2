using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Telebot.Sourse.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item.IItem
{
    public class MyChat : IItem.IItemDB<MyChat>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { set; get; }
        public long? BotClientId { set; get; }
        public bool? IsDelite { set; get; }


        public long ChatId { set; get; }

       

        public Telegram.Bot.Types.Enums.ChatType TeleChatType { set; get; }

      
        public string? processCode { set; get; }

        public virtual List<MyUser>? AllChatUsers { get; set; } = new List<MyUser>();

        public virtual List<PriviosMSG>? PriviosMSGs { get; set; }

        public virtual List<MyUserUpdates> Update { set; get; }=new List<MyUserUpdates>();


        public virtual List<requst> Requsts { set; get; } = new List<requst>();


        public int? CurentProcessId { set; get; }

        virtual public Menu_Process? CurentProcess { set; get; }















        public static async Task <MyChat> newChatObject(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            Chat? curentChat = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentChat = update.Message.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentChat = update.ChannelPost.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentChat = update.CallbackQuery.Message.Chat;


            if (curentChat is null) return null;
           
            var loadChat = await bot.GetChatAsync(curentChat.Id,cts);

            var result = new MyChat();
            result.dateTimeCreation = DateTime.Now;
            result.ChatId = curentChat.Id;
            result.BotClientId = bot.BotId;
            result.MyName = "Пользовательский чат";
            result.IsDelite = false;
            result.TeleChatType = curentChat.Type;
            result.MyDescription = $"Obj Type: {result.GetType().Name}|ChatType: {result.TeleChatType}|BotId: {bot.BotId}|Chat id: {curentChat.Id}|DataCreation: {result.dateTimeCreation}";
          

            return result;
        }




    }
}
