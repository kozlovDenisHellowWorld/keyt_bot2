using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Handlers
{
    public static class HandlerNewUpdate
    {


        public static async Task<string> newUpdateHendler(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            if (update is null) return "update is NULL  первая проверка безуспешна|false";

            long chatId = 0;
            long userId = 0;
            using (null)
            {
                //chatId, userId
                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    chatId = update.Message.Chat.Id;
                    if (update.Message.From != null) userId = update.Message.From.Id;
                }
                else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost)
                {
                    chatId = update.ChannelPost.Chat.Id;
                    if (update.ChannelPost.From != null) userId = update.ChannelPost.From.Id;
                }
                else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    if (update.CallbackQuery.Message.From.Id != bot.BotId) userId = update.CallbackQuery.Message.From.Id;
                }
                else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
                {
                    chatId = update.EditedMessage.Chat.Id;
                    if (update.EditedMessage.From != null) userId = update.EditedMessage.From.Id;
                }
                else if (chatId == 0) return $"В update не найден чат, возможно нет типа. Текущий тип update - {update.Type}|false";
            }




            //====== Собщения ========

            //чат 
            using (var db = new context())
            {
                var botProps = db.BotProperties.FirstOrDefault(i => i.BotClientId == bot.BotId);
                if (update.Id == botProps.LastUpdateId) return "update id==privious update Id пропускаем|false";


                MyChat curnchat = null;

                if (db.myChats.Count() != 0) curnchat = db.myChats.FirstOrDefault(i => i.ChatId == chatId);
                // Проверка есть ли такой чат 
                if (curnchat is null)
                {
                    curnchat = await MyChat.newChatObject(bot, update, cts);
                    db.myChats.Add(curnchat);
                    db.SaveChanges();
                     concoldebuger.goodMSG($"Добавлен новый чат (id BD: {curnchat.MyId}): {curnchat.MyDescription}", bot,cts);
                }
            }


            if (userId == 0) return $"update ({update.Id})  Проверка пользователей и чата заночена. Пользователя нет.  Возможно тип update tupe == callback или тип чата == \"Канал\"|notbad";


            //пользователь 
            using (var db = new context())
            {
                var botProps = db.BotProperties.FirstOrDefault(i => i.BotClientId == bot.BotId);
                if (update.Id == botProps.LastUpdateId) return "update id==privious update Id пропускаем|false";

                MyUser curentUser = null;
                MyChat curntchat = null;
                if (db.myChats.Count() != 0) curntchat = db.myChats.FirstOrDefault(i => i.ChatId == chatId);

                if (db.MyUsers.Count() != 0) curentUser = db.MyUsers.FirstOrDefault(i => i.Id == userId);

                


                if (curentUser is null)
                {
                    curentUser = await MyUser.newUserObject(bot, update, cts);
                    if (curentUser == null) return $"update (id: {update.Id})  Проверка пользователей и чата заночена. Пользователя нет или тип update tupe == {update.Type} (возможно тип чата {curntchat.TeleChatType}) |notbad";

                    //var eeee=db.User_Types.ToList();
                    curentUser.Type = db.User_Types.FirstOrDefault(p => p.IsDefoult == true)??null;
                    
                    if(curentUser.Id == 469825678) curentUser.Type= db.User_Types.FirstOrDefault(p => p.TypeCode == "admin");


                    db.MyUsers.Add(curentUser);
                    db.SaveChanges();

                    concoldebuger.goodMSG($"Добавлен новый пользователь (Id BD: {curentUser.MyId}):  {curentUser.MyDescription}", bot, cts);
                }



            }



            using (var db=new context())
            {
                var curentChat = db.myChats.FirstOrDefault(i=>i.ChatId==chatId);
                var curetnUser = db.MyUsers.FirstOrDefault(i => i.Id == userId);

                if (curentChat.AllChatUsers.FirstOrDefault(i => i.Id == curetnUser.Id)is not null) return "update ({update.Id})  Проверка пользователей и чата заночена успешна|true";
                curentChat.AllChatUsers.Add(curetnUser);
                concoldebuger.goodMSG($"Был добавлен пользователь (id db: {curetnUser.MyId}) c Tele user id:{curetnUser.Id} в чат (id db: {curentChat.MyId}) c Tele chat id:{curentChat.ChatId}", bot, cts);
                db.SaveChanges();
             }




            return $"update ({update.Id})  Проверка пользователей и чата заночена успешна|true";
        }


        public static async Task<string> changeLastupdateId(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            try
            {
                using (var db= new context())
                {
                    
                    var botProps = db.BotProperties.FirstOrDefault(i => i.BotClientId == bot.BotId);
                    botProps.LastUpdateId=update.Id;
                    db.SaveChanges();
                    return $"last updateId  новый {botProps.LastUpdateId} |true";

                }


            }
            catch 
            {
                return "last updateId не изменен|false"; 
            }
                    
        }


        public static async Task<string> saveUpdateInfo(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            Chat? curentChat = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentChat = update.Message.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentChat = update.ChannelPost.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentChat = update.CallbackQuery.Message.Chat;


            var myupdate = await MyUserUpdates.createNewObj(bot, update, cts);

            using (var db= new context())
            {
                db.myUserUpdates.Add(myupdate);
                db.SaveChanges();

                var chat= db.myChats.FirstOrDefault(i=>i.ChatId==curentChat.Id);// вот тут фиаско при изменение сообщения 
                chat.Update.Add(myupdate);
                db.SaveChanges();

            }

            concoldebuger.goodMSG($"Новый update {myupdate.MyDescription}", bot, cts);

            return $"Update успешно записан |true";
        }




        public static async Task<string> incumingMessageHendler(ITelegramBotClient bot, Update update, CancellationToken cts)
        {



            return "";
        }







        public static async void testPost(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            //        -1001987659195 - группа
            //-1001817308789 - канал 
            try
            {
                if (update.Type is not Telegram.Bot.Types.Enums.UpdateType.ChannelPost) return;

                await bot.ForwardMessageAsync(-1001987659195, -1001817308789, update.ChannelPost.MessageId);

            }
            catch
            {

                return;
            }


        }

        public static async Task<string> processHendler(ITelegramBotClient iClient, Update update, CancellationToken cancellationToken)
        {
            string result = "";

            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery || update.Type is Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                Chat curentChat = null;
                Message curentMessage = null;
                if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    curentChat = update.CallbackQuery.Message.Chat;
                    curentMessage = update.CallbackQuery.Message;
                }
                else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    curentChat = update.Message.Chat;
                    curentMessage = update.Message;
                }
              if (curentChat is null||curentMessage is null)  return "Не очень |false";
            }




            return result;
        }
    }
}
