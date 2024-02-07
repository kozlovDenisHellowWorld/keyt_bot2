using OpenAI.Chat;
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using Telebot.Sourse.Handlers;

namespace Telebot.Sourse
{
    public  class TeleTools
    {

         public async Task<Message> SendMenu1(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken)
        {
            //  ITelegramBotClient telegramBotClient= client;




            //Message resut= await iClient.SendTextMessageAsync(curentChat.ChatId, curentChat.LastUserProcess().CurentProcess.MenuContent, replyMarkup: InitInlineKeyboard(_myChat.LastUserProcess().CurentProcess.Buttons, _myChat.LastUserProcess().CurentProcess.LinesInMenu),cancellationToken: canslationToken);




            return null;
        }

         public async Task<Message> SendStaticMenu(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, List<Buttons> buttons, string menuContetn, List<myPhoto> myPhotos)
        {
            if (myPhotos is not null) { }
            List<List< InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();


            foreach (var button in buttons)
            {
                
                List<InlineKeyboardButton> lineBTN= new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: button.content, callbackData: button.callBackCode) };


                inlineKeyboardButtons.Add(lineBTN);

            }

            InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

            Message sentMessage = await client.SendTextMessageAsync(
    chatId: _myChat.ChatId, 
    text: menuContetn,
    replyMarkup: inlineKeyboardMarkup,
    parseMode: ParseMode.Html, disableNotification:true,
    cancellationToken: canslationToken);


            return sentMessage;
        }


        public async Task<List< Message>> SendStaticMenualot(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, List<Buttons> buttons, string menuContetn, List<myPhoto> myPhotos)
        {

            int i_buttons = 0;


            List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
            InlineKeyboardMarkup inlineKeyboardMarkup=null;
            List< Message > messages = new List<Message>();



            foreach (var button in buttons)
            {
             


                List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: button.content, callbackData: button.callBackCode) };


                inlineKeyboardButtons.Add(lineBTN);


                i_buttons++;


                if (i_buttons == 90 || buttons.Count()-1 == buttons.IndexOf(button))
                {
                    i_buttons = 0;
                    inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                    Message sentMessage = await client.SendTextMessageAsync(
            chatId: _myChat.ChatId,
            text: menuContetn,
            replyMarkup: inlineKeyboardMarkup,
            parseMode: ParseMode.Html, disableNotification: true,
            cancellationToken: canslationToken);

                    messages.Add(sentMessage);
                    inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
                }

            }

         


            return messages;
        }








        public async Task<List<Message>> SendPhotoAlbum(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, List<Buttons> buttons, string menuContetn, List<myPhoto> myPhotos)
        {
            if (myPhotos is null) return null;

           // List<InputFileId> photoes = new List<InputFileId>();
          
            List<IAlbumInputMedia> albums = new List<IAlbumInputMedia>();
            List<Message> messages = new List<Message>();
            foreach (var photo in myPhotos)
            {
               // photoes.Add( new InputMediaPhoto( InputFile.FromFileId(photo.FileId)));

                albums.Add(new InputMediaPhoto(InputFile.FromFileId(photo.FileId)));

                if (albums.Count() == 10|| myPhotos.Last()== photo)
                {
                    Message[] sentMessage = await client.SendMediaGroupAsync(_myChat.ChatId, media: albums, disableNotification: true, cancellationToken: canslationToken);
                    messages.AddRange(sentMessage.ToList());
                    albums = new List<IAlbumInputMedia>();
                }
            }








            //6021604487 | Data


            return messages;
        }






         public async Task<Message> SendStaticMSG(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken,  string menuContetn, List<myPhoto> myPhotos)
        {
            if (myPhotos is not null) { }
         


            Message sentMessage = await client.SendTextMessageAsync(
    chatId: _myChat.ChatId,
    text: menuContetn,
    parseMode: ParseMode.Html,
    cancellationToken: canslationToken);


            return sentMessage;
        }





        public  async Task remooveMenu(ITelegramBotClient client, CancellationToken cancellation, MyChat curentChat)
        {
            if (curentChat.PriviosMSGs is null || curentChat.PriviosMSGs.Count() == 0) return;
            List<PriviosMSG> priviosMSGs = curentChat.PriviosMSGs.Where(msg => msg.NedTodelite == true).ToList();


            while (priviosMSGs.Count() > 0)
            {
                var lastmsg = priviosMSGs.Last();
                try
                {
                    

                    int mesgID = lastmsg.MessageId ?? 0;
                    if (mesgID == 0) continue;
                    await client.DeleteMessageAsync(curentChat.ChatId, mesgID);

                    curentChat.PriviosMSGs.Remove(lastmsg);

                    priviosMSGs.Remove(lastmsg);
                }
                catch {
                    curentChat.PriviosMSGs.Remove(lastmsg);

                    priviosMSGs.Remove(lastmsg);
                }
               
               

            }


        }






         public static long GetTeleUserId(Update update)
        {
            long result = 0;

            if (update.Type == UpdateType.CallbackQuery)
            {
                using (var db = new context())
                {
                    result = db.myChats.FirstOrDefault(ch => ch.ChatId == update.CallbackQuery.Message.Chat.Id).AllChatUsers.FirstOrDefault().Id;

                }

            }
            if (update.Type == UpdateType.Message)
            {
                result = update.Message.From.Id;


            }


            return result;
        }

         public static long GetTeleChatId(Update update)
        {
            long result = 0;

            if (update.Type == UpdateType.CallbackQuery)
            {
                result = update.CallbackQuery.Message.Chat.Id;
            }
            if (update.Type == UpdateType.Message)
            {
                result = update.Message.From.Id;
            }
            return result;
        }



        public static string GetXMLToken(string fileName)
        {
            string curetnDir = System.IO.Directory.GetCurrentDirectory();

            string filePath=Path.Combine(curetnDir, fileName);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(filePath);
            XmlNode botInstNode = xmlDoc.SelectSingleNode("BotInst");

            string Token = "";
            if (Debugger.IsAttached) Token = botInstNode.Attributes["DebugToken"].Value;
            else Token = botInstNode.Attributes["ReliseToken"].Value;

            return Token;
        }



        public struct Buttons
        {
            public string content;
            public string callBackCode;


        }


        public bool checkUpdadate(ITelegramBotClient iClient, Update update, CancellationToken cancellationToken)
        {
            bool result = true;

            if (update.Type == UpdateType.CallbackQuery)// проверка при нажати енопки есть ли чат
            {
                using (var db = new context())
                {
                    //проверка есть личат у меня в бд при нажатие кнопки из колл бек меню
                    var meg = db.myUserUpdates.ToList();

                    var chats = db.myChats.ToList();

                    if (db.myChats.FirstOrDefault(p => p.ChatId == update.CallbackQuery.Message.Chat.Id) is null)
                    {
                        result = false;
                        concoldebuger.badMSG($"Update - id: {update.Id}| type: {update.Type}|  - НЕ НАШЕЛ ЧАТ В бд, но тип адейта CallbackQuery", iClient,cancellationToken);
                    }
                }
            }
            else if (update.Type == UpdateType.Message)// проверка чат ли это
            {
                //проверка что это не чат 
                if (update.Message.Chat.Id < 0)
                {
                    result = false;
                    concoldebuger.badMSG($"Update - id: {update.Id}| type: {update.Type}|  Этот апдейт из чата ", iClient,cancellationToken);
                }
            }
           else  if (update.Type == UpdateType.EditedMessage)// тут нажо дописывать  при исправлении месаджа 
            {
                result=false;// изменить сообщение
                //using (var db = new context())
                //{
                //    if (db.myChats.FirstOrDefault(p => p.ChatId == update.EditedMessage.Chat.Id) is null) return;
                //}
            }


            return result;

        }




    }
    //{

    //     public Item.TeleClientUser GetSetClientFromUpdate(Update update, TelegramBotClient clietn)
    //    {

    //        var chat = getChatFromUpdate(update);

    //        if (IsNewClient(update))
    //            using (var db = new context())
    //            {

    //                if (update.Type == UpdateType.MyChatMember) return null;
    //                db.TeleUsers.Add(new Item.TeleClientUser(update, Item.TeleClientUser.type.regulareClient));
    //                db.SaveChanges();


    //            }


    //        var DB = new context();

    //        foreach (var user in DB.TeleUsers.ToList())
    //        {
    //            if (user.TeleChatId == chat.Id) return user;
    //        }



    //        return null;
    //    }






    //    public  bool IsNewClient(Update update)
    //    {
    //        var db = new context();

    //        TeleClient user = null;

    //        Chat chat = getChatFromUpdate(update);

    //        foreach (var teleUser in db.TeleUsers)
    //        {
    //            if (teleUser.TeleChatId == chat.Id) return false;

    //        }

    //        return true;
    //    }


    //    public  Chat getChatFromUpdate(Update update)
    //    {
    //        Chat result = null;

    //        if (update.Type == UpdateType.Message) result = update.Message.Chat;

    //        if (update.Type == UpdateType.CallbackQuery) result = update.CallbackQuery.Message.Chat;



    //        return result;
    //    }


    //    public  Telegram.Bot.Types.Message getMassageFromUpdate(Update update)
    //    {
    //        Telegram.Bot.Types.Message result = null;

    //        if (update.Type == UpdateType.Message) result = update.Message;

    //        if (update.Type == UpdateType.CallbackQuery) result = update.CallbackQuery.Message;



    //        return result;
    //    }


    //    public  async Task SendBtnInChat(TelegramBotClient client, Item.TeleClientUser user, string text_priv, Dictionary<string, string> BtnContent, int btnInl, bool needToDelete, CancellationToken cancellationToken)
    //    {

    //        Message msg = await client.SendTextMessageAsync(user.TeleChatId, text_priv, replyMarkup: initMenuInlineKeyB(BtnContent, btnInl), cancellationToken: cancellationToken);

    //        Item.Massage.SetNewMassage(client, msg, needToDelete, text_priv);

    //    }


    //    public  async Task<bool> SendAlbum(TelegramBotClient client, Item.TeleClientUser user, List<IAlbumInputMedia> mediaAlbum, bool needToDelete, CancellationToken cancellationToken)
    //    {



    //      var msgs = await client.SendMediaGroupAsync(user.TeleChatId, media:mediaAlbum,cancellationToken:cancellationToken);

    //        foreach (var msg in msgs)
    //        {
    //            Item.Massage.SetNewMassage(client, msg, needToDelete, "альбом фоток");
    //        }

    //        return true;

    //    }


    //    public  async Task<bool> Sendphoto(TelegramBotClient client, Item.TeleClientUser user, PhotoSize photo, bool needToDelete, CancellationToken cancellationToken)
    //    {



    //        var msg = await client.SendPhotoAsync(user.TeleChatId, photo: new Telegram.Bot.Types.InputFiles.InputOnlineFile(photo.FileId), cancellationToken: cancellationToken);


    //        Item.Massage.SetNewMassage(client, msg, needToDelete, "фотока");


    //        return true;

    //    }




    //    public  InlineKeyboardMarkup initMenuInlineKeyB(Dictionary<string, string> BtnContent, int btnInl)
    //    {
    //        int i = 1;
    //        List<InlineKeyboardButton[]> btnMain = new List<InlineKeyboardButton[]>();
    //        var lineBtn = new List<InlineKeyboardButton>();

    //        var last = BtnContent.Last();

    //        foreach (var item in BtnContent)
    //        {
    //            var bt = InlineKeyboardButton.WithCallbackData(item.Key, callbackData: item.Value);

    //            lineBtn.Add(bt);

    //            if (i == btnInl || last.Equals(item))
    //            {
    //                btnMain.Add(lineBtn.ToArray());
    //                lineBtn = new List<InlineKeyboardButton>();
    //                i = 1;
    //                continue;
    //            }


    //            i++;


    //        }





    //        return new InlineKeyboardMarkup(btnMain);
    //    }




    //    public  async void SendMsg(TelegramBotClient client, Item.TeleClientUser user, string text,  bool needToDelete, CancellationToken cancellationToken)
    //    {

    //        Message msg = await client.SendTextMessageAsync(user.TeleChatId, text,  cancellationToken: cancellationToken);

    //        Item.Massage.SetNewMassage(client, msg, needToDelete, text);

    //    }
















    //    /// <summary>
    //    /// если вернулся 0 то ошибка 
    //    /// </summary>
    //    /// <param name="update"></param>
    //    /// <returns></returns>
    //    public  long GetUserIdFromCallBakc(Update update)
    //    {
    //        long result = 0;

    //        string maincontent = getMassageFromUpdate(update).Text;


    //        if (maincontent.Contains("user:"))
    //        {

    //            foreach (var item in maincontent.Split('|'))
    //            {
    //                if (!item.Contains("user:")) continue;

    //                string content = item.Split(':')[1];

    //                result = Convert.ToInt64(content);

    //            }


    //        }




    //        return result;
    //    }


    //    public  string SetChatIdInCallBack(Item.TeleClientUser user)
    //    {
    //        return $"user:{user.TeleChatId.ToString()}|";


    //    }

    //    //public  string SetQustenId(Item.Question question)
    //    //{

    //    //    if (question == null) return $"qu:{0}|"; 
    //    //    return $"qu:{question.Id.ToString()}|";

    //    //}

    //    public  int GetQustenId(Update update)
    //    {


    //        if (update.Type == UpdateType.CallbackQuery)
    //        {
    //            var content = update.CallbackQuery.Data;

    //            if (!content.Contains("qu:")) return 0;

    //             foreach (var item in content.Split('|'))
    //            {


    //                if (item.Contains("qu:")) return Convert.ToInt32(item.Split(':')[1]);


    //            }

    //        }
    //        else 
    //        {
    //            var content = Item.TeleClientUser.GetClientByUpdate(update).CurentProcessCode;

    //            if (!content.Contains("qu:")) return 0;

    //            foreach (var item in content.Split('|'))
    //            {

    //                if (item.Contains("qu:")) return Convert.ToInt32(item.Split(':')[1]);


    //            }

    //        }





    //        return 0;
    //    }






    //    //public  string SetRequestId(Item.Request question)
    //    //{

    //    //    if (question == null) return $"re:{0}|";
    //    //    return $"re:{question.id.ToString()}|";

    //    //}

    //    public  int GetRequestId(Update update)
    //    {


    //        if (update.Type == UpdateType.CallbackQuery)
    //        {
    //            var content = update.CallbackQuery.Data;

    //            if (!content.Contains("re:")) return 0;

    //            foreach (var item in content.Split('|'))
    //            {


    //                if (item.Contains("re:")) return Convert.ToInt32(item.Split(':')[1]);


    //            }

    //        }
    //        else
    //        {
    //            var content = Item.TeleClientUser.GetClientByUpdate(update).CurentProcessCode;

    //            if (!content.Contains("re:")) return 0;

    //            foreach (var item in content.Split('|'))
    //            {

    //                if (item.Contains("re:")) return Convert.ToInt32(item.Split(':')[1]);


    //            }

    //        }





    //        return 0;
    //    }



    //}
}
