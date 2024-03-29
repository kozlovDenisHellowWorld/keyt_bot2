using System.Diagnostics;
using System.Xml;
using Telebot.Sourse.Handlers;
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;

namespace Telebot.Sourse
{
    public class TeleTools
    {


        /// <summary>
        /// CallbackQuery
        /// </summary>
        private string InputType_1 = "CallbackQuery";
        /// <summary>
        /// AwaytText
        /// </summary>
        private string InputType_2 = "AwaytText";
        /// <summary>
        /// ЧТо то делаем DoSmth
        /// </summary>
        private string InputType_3 = "DoSmth";

        /// <summary>
        /// CallbackQueryList
        /// </summary>
        private string InputType_4 = "CallbackQueryList";




        public async Task<Message> SendMenu1(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken)
        {
            //  ITelegramBotClient telegramBotClient= client;




            //Message resut= await iClient.SendTextMessageAsync(curentChat.ChatId, curentChat.LastUserProcess().CurentProcess.MenuContent, replyMarkup: InitInlineKeyboard(_myChat.LastUserProcess().CurentProcess.Buttons, _myChat.LastUserProcess().CurentProcess.LinesInMenu),cancellationToken: canslationToken);




            return null;
        }

        public async Task<Message> SendStaticMenu(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, List<Buttons> buttons, string menuContetn, List<myPhoto> myPhotos)
        {
            if (myPhotos is not null) { }
            List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();


            foreach (var button in buttons)
            {

                List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: button.content, callbackData: button.callBackCode) };


                inlineKeyboardButtons.Add(lineBTN);

            }

            InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

            Message sentMessage = await client.SendTextMessageAsync(
    chatId: _myChat.ChatId,
    text: menuContetn,
    replyMarkup: inlineKeyboardMarkup,
    parseMode: ParseMode.Html, disableNotification: true,
    cancellationToken: canslationToken);


            return sentMessage;
        }


        public async Task<List<Message>> SendStaticMenualot(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, List<Buttons> buttons, string menuContetn, List<myPhoto> myPhotos)
        {

            int i_buttons = 0;


            List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
            InlineKeyboardMarkup inlineKeyboardMarkup = null;
            List<Message> messages = new List<Message>();



            foreach (var button in buttons)
            {



                List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: button.content, callbackData: button.callBackCode) };


                inlineKeyboardButtons.Add(lineBTN);


                i_buttons++;


                if (i_buttons == 90 || buttons.Count() - 1 == buttons.IndexOf(button))
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

                if (albums.Count() == 10 || myPhotos.Last() == photo)
                {
                    Message[] sentMessage = await client.SendMediaGroupAsync(_myChat.ChatId, media: albums, disableNotification: true, cancellationToken: canslationToken);
                    messages.AddRange(sentMessage.ToList());
                    albums = new List<IAlbumInputMedia>();
                }
            }








            //6021604487 | Data


            return messages;
        }






        public async Task<Message> SendStaticMSG(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, string menuContetn, List<myPhoto> myPhotos)
        {
            if (myPhotos is not null) { }



            Message sentMessage = await client.SendTextMessageAsync(
    chatId: _myChat.ChatId,
    text: menuContetn,
    parseMode: ParseMode.Html,
    cancellationToken: canslationToken);


            return sentMessage;
        }





        public async Task remooveMenu(ITelegramBotClient client, CancellationToken cancellation, MyChat curentChat)
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
                catch
                {
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

            string filePath = Path.Combine(curetnDir, fileName);

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

            if (update.Type == UpdateType.CallbackQuery)// проверка при нажати енопки есть ли чат - в том случае если я обновил бд 
            {
                using (var db = new context())
                {
                    //проверка есть личат у меня в бд при нажатие кнопки из колл бек меню
                    var meg = db.myUserUpdates.ToList();

                    var chats = db.myChats.ToList();

                    if (db.myChats.FirstOrDefault(p => p.ChatId == update.CallbackQuery.Message.Chat.Id) is null)
                    {
                        result = false;
                        concoldebuger.badMSG($"Update - id: {update.Id}| type: {update.Type}|  - НЕ НАШЕЛ ЧАТ В бд, но тип адейта CallbackQuery", iClient, cancellationToken);
                    }
                }
            }
            else if (update.Type == UpdateType.Message)// проверка чат ли это
            {
                //проверка что это не чат 
                if (update.Message.Chat.Id < 0)
                {
                    result = false;
                    concoldebuger.badMSG($"Update - id: {update.Id}| type: {update.Type}|  Этот апдейт из чата ", iClient, cancellationToken);
                }
            }
            else if (update.Type == UpdateType.EditedMessage)// тут нажо дописывать  при исправлении месаджа 
            {
                result = false;// изменить сообщение
                //using (var db = new context())
                //{
                //    if (db.myChats.FirstOrDefault(p => p.ChatId == update.EditedMessage.Chat.Id) is null) return;
                //}
            }


            return result;

        }




        private string FormateText(string textline)
        {

            string message = textline;

             message = message.Replace("{n}", "\n");
            message = message.Replace("{t}", "\t");
            message = message.Replace("{b}", "<b>");
            message = message.Replace("{eb}", "</b>");
          
            message = message.Replace("{code}", "<code>");
            message = message.Replace("{ecode}", "</code>");

            message = message.Replace("{code}", "<code>");
            message = message.Replace("{ecode}", "</code>");
          
            message = message.Replace("{blockquote}", "<blockquote>");
            message = message.Replace("{eblockquote}", "</blockquote>");

            message = message.Replace("{pre}", "<pre>");
            message = message.Replace("{epre}", "</pre>");

            return message;
        }





        public async Task<Message[]> SendStaticMenu_forXMLLoad(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, Update update, context db)
        {
            List<Message> msgResult = new List<Message>();

            if (_myChat.CurentProcess.ProcessType.Code == "StaticListButtonsCallbackQuery")
            {

                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
                foreach (var item in _myChat.CurentProcess.Inputs)
                {
                    db.Inputs.Update(item);
                    if (item.input_Type.Code != InputType_1) continue;
                    //   var callingprocess = _myChat.CurentProcess.ProcessType.Menus.FirstOrDefault(m =>m.ProcessMenuCode == (item?.NextProcessMenu?.ProcessMenuCode?? "StartMenu"));
                    var callingprocess = item?.NextProcessMenu;
                    if (callingprocess == null) continue;
                    List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                    inlineKeyboardButtons.Add(lineBTN);
                }
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText( _myChat.CurentTexrMessage ?? _myChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);
                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));



            }
            else if (_myChat.CurentProcess.ProcessType.Code == "Message")
            {

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText(_myChat.CurentTexrMessage ?? _myChat.CurentProcess.MenuProcessContent), parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);

                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));

                _myChat.SetProcess(_myChat.CurentProcess.Inputs.FirstOrDefault().NextProcessMenu, db);

                await SendStaticMenu_forXMLLoad(_myChat, client, canslationToken, update, db);
            }
            else if (_myChat.CurentProcess.ProcessType.Code == "DinamickListButtonsCallbackQuery")
            {
                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

                foreach (var item in _myChat.CurentProcess.Inputs)
                {
                    if (item.input_Type.Code == InputType_1)
                    {
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                        inlineKeyboardButtons.Add(lineBTN);

                    }
                    else if (item.input_Type.Code == InputType_4)
                    {

                        string filteroptions = update.Message?.Text;


                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;

                        if (filteroptions != null)
                        {
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: "Убрать фильтр", callbackData: _myChat.CurentProcess.GetEntityTypeId())};
                            inlineKeyboardButtons.Add(lineBTN);
                        }

                        foreach (var dBut in _myChat.DinamicButons)
                        {
                            if (filteroptions != null && (!dBut.Content.ToLower().Contains(filteroptions.ToLower()))) continue;

                                List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: dBut.Content, callbackData: $"{dBut.CallbackQwery}") };
                                inlineKeyboardButtons.Add(lineBTN);
                         

                        }



                        if (_myChat.DinamicButons.Count > 0)
                        {

                            // db.dinamic_Butons.RemoveRange(db.dinamic_Butons.Where(p => p.Chat == null).ToList());

                            db.dinamic_Butons.RemoveRange(_myChat.DinamicButons);

                            _myChat.DinamicButons.Clear();


                            db.SaveChanges();
                        }
                        //   List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}") };
                        //  inlineKeyboardButtons.Add(lineBTN);

                    }








                }
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText(_myChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);
                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));


            }


            return msgResult.ToArray();
        }
























    }

}
