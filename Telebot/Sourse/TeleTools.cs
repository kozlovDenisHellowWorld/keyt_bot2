using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Telebot.Sourse.Handlers;
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Linq;

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

        /// <summary>
        /// CallbackQueryList
        /// </summary>
        private string InputType_5 = "CallbackQueryBack";



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


        public string FormateMenuPropsText(string textline, Menu_Process menu)
        {

            string cont = textline;

            cont = cont.Replace("{MenuCode}", menu.ProcessMenuCode);
            cont = cont.Replace("{Name}", menu.MyName);
            cont = cont.Replace("{Navigation}", menu.Navigation);
            cont = cont.Replace("{IsAwaytingText}", menu.IsAwaytingText.ToString());
            cont = cont.Replace("{NeedToDelite}", menu.NeedToDelite.ToString());
            cont = cont.Replace("{Ninput}", menu.Inputs.Count().ToString());
            cont = cont.Replace("{Content}", menu.MenuProcessContent);
            cont = cont.Replace("{MenuType}", menu.ProcessType.MyName);
            cont = cont.Replace("{UserType}", menu.UserType.MyName);

            return cont;
        }


        public async Task<Message[]> SendStaticMenu_forXMLLoad(MyChat _myChat, ITelegramBotClient client, CancellationToken canslationToken, Update update, context db)
        {
            List<Message> msgResult = new List<Message>();


            if (update == null)
            { 
            
            
            }



            if (_myChat.CurentProcess.ProcessType.Code == "StaticListButtonsCallbackQuery")
            {

                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
                foreach (var item in _myChat.CurentProcess.Inputs)
                {
                    if (item.input_Type.Code == "CallbackQuery")
                    {
                        db.Inputs.Update(item);
                        if (item.input_Type.Code != InputType_1) continue;
                        //   var callingprocess = _myChat.CurentProcess.ProcessType.Menus.FirstOrDefault(m =>m.ProcessMenuCode == (item?.NextProcessMenu?.ProcessMenuCode?? "StartMenu"));
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                        inlineKeyboardButtons.Add(lineBTN);
                    }
                    else if (item.input_Type.Code == "CallbackQueryList")
                    {
                        var dinamicBtn = _myChat.DinamicButons.FirstOrDefault(p => p.MyName == item.MyName);
                        if (dinamicBtn == null)
                        {
                            db.Inputs.Update(item);
                            if (item.input_Type.Code != InputType_1) continue;
                            //   var callingprocess = _myChat.CurentProcess.ProcessType.Menus.FirstOrDefault(m =>m.ProcessMenuCode == (item?.NextProcessMenu?.ProcessMenuCode?? "StartMenu"));
                            var callingprocess = item?.NextProcessMenu;
                            if (callingprocess == null) continue;
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                            inlineKeyboardButtons.Add(lineBTN);

                        }
                        else
                        {
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: dinamicBtn.Content, callbackData: dinamicBtn.CallbackQwery) };
                            inlineKeyboardButtons.Add(lineBTN);

                        }
                    }
                    else if (item.input_Type.Code == "CallbackQueryBack")
                    {
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, item.NextProcessMenu.GetEntityTypeId()) };
                        inlineKeyboardButtons.Add(lineBTN);


                    }


                }
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText(_myChat.CurentTexrMessage ?? _myChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);
                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));



            }
            else if (_myChat.CurentProcess.ProcessType.Code == "Message")
            {

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText(_myChat.CurentTexrMessage ?? _myChat.CurentProcess.MenuProcessContent), parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);

                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));

                _myChat.SetProcess(_myChat.CurentProcess.Inputs.FirstOrDefault().NextProcessMenu);
                //вот тут поправил 
                await    _myChat.CurentProcess.ExecuteOnLoad(update, client,_myChat,db,canslationToken);
                await  SendStaticMenu_forXMLLoad(_myChat, client, canslationToken, update, db);
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
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: "Убрать фильтр", callbackData: _myChat.CurentProcess.GetEntityTypeId()) };
                            inlineKeyboardButtons.Add(lineBTN);
                        }


                        inlineKeyboardButtons.AddRange(InlineKeyCreate(_myChat.DinamicButons, 1));

                        //foreach (var dBut in _myChat.DinamicButons)
                        //{
                        //    if (filteroptions != null && (!dBut.Content.ToLower().Contains(filteroptions.ToLower()))) continue;

                        //    List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: dBut.Content, callbackData: $"{dBut.CallbackQwery}") };
                        //    inlineKeyboardButtons.Add(lineBTN);


                        //}



                        if (_myChat.DinamicButons.Count > 0)
                        {

                            // db.dinamic_Butons.RemoveRange(db.dinamic_Butons.Where(p => p.Chat == null).ToList());

                            db.dinamic_Butons.RemoveRange(_myChat.DinamicButons);

                            _myChat.DinamicButons.Clear();


                            while (true)
                            {
                                try
                                {
                                    db.SaveChanges();
                                    break;
                                }
                                catch
                                {
                                    concoldebuger.badMSG("Ошибка тут !! 1");
                                }
                            }
                        }
                        //   List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}") };
                        //  inlineKeyboardButtons.Add(lineBTN);

                    }
                    else if (item.input_Type.Code == InputType_5)
                    {
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;

                        //string LogBack = _myChat.Logs
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, item.NextProcessMenu.GetEntityTypeId()) };
                        inlineKeyboardButtons.Add(lineBTN);
                    }
                    else if (item.input_Type.Code == "CallbackQueryBool")
                    {
                        if (_myChat.DinamicButons.Count() > 0)
                        {
                            int maxRows = 20;

                            int colmns = (int)Math.Ceiling((double)(_myChat.DinamicButons.Count() / maxRows));
                            //int colmns2 = (int)Math.Ceiling((double)(_myChat.DinamicButons.Count()/72));
                            //int colmns3 = (int)Math.Ceiling((double)(_myChat.DinamicButons.Count() / 76));


                            int remainingRows = _myChat.DinamicButons.Count() % maxRows;
                            //int remainingRows2 = _myChat.DinamicButons.Count() % 72;
                            //int remainingRows3 = _myChat.DinamicButons.Count() % 76;
                            if (remainingRows > 0)
                            {
                                colmns++;
                            }
                            //if (remainingRows2 > 0)
                            //{
                            //    colmns2++;
                            //}
                            //if (remainingRows3 > 0)
                            //{
                            //    colmns3++;
                            //}



                            //int _BTN_40 = _myChat.DinamicButons.Count()%40 ;

                            //  if (colmns==1&&_myChat.DinamicButons.)

                            inlineKeyboardButtons.AddRange(InlineKeyCreate(_myChat.DinamicButons, colmns));


                        }
                    }


                    //Start
                    //Отослал кнопки
                    //Пользователь ажал кнопку
                    //Пришел Update
                    //Записал Log
                    //Обработал обтейт и получае Mext menu
                    //Change menu -  изменяю меню и затераю dinamic btns и curent text
                    //on end
                    // on load
                    // отпраяляю 



                }
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                Message sentMessage = await client.SendTextMessageAsync(chatId: _myChat.ChatId, text: FormateText(_myChat.CurentTexrMessage??_myChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                msgResult.Add(sentMessage);
                _myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));


                bool issaved = false;
                while (true)
                {
                    try
                    {
                        db.SaveChanges();
                        break;
                    }
                    catch
                    {
                        concoldebuger.badMSG("Ошибка тут !! 1");
                    }
                }
                
                

            }


            return msgResult.ToArray();
        }








        /// <summary>
        /// без : - важна символ двоеточея уже есть
        /// </summary>
        /// <param name="entyCode"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public int getentyIdByUpdate(string entyCode, Update update)
        {

            string parsLine = (update.CallbackQuery?.Data) ?? "";

            if (parsLine == "") return (-1);

            string pattern = @$"{entyCode}:(\d+)";

            // Находим все совпадения с помощью Regex
            MatchCollection matches = Regex.Matches(parsLine, pattern);

            // Выводим найденные числа

            string numberString = matches.Last().Groups[1].Value; // Получаем значение числа из группы захвата
            int number;
            if (int.TryParse(numberString, out number))
            {
                Console.WriteLine("Найденное число: " + number);
            }
            else
            {
                Console.WriteLine("Не удалось преобразовать строку в число.");
            }
            return number;
        }

        public async Task<Message[]> EditStaticMenu_forXMLLoad(MyChat thisChat, ITelegramBotClient iClient, CancellationToken cancellationToken, Update update, context db)
        {
            // await thisChat.CurentProcess.ExecuteOnLoad(update, iClient, thisChat, db, cancellationToken);
            List<Message> msgResult = new List<Message>();


            if (thisChat.CurentProcess.ProcessType.Code == "StaticListButtonsCallbackQuery")
            {

                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
                foreach (var item in thisChat.CurentProcess.Inputs)
                {
                    if (item.input_Type.Code == "CallbackQuery")
                    {
                        db.Inputs.Update(item);
                        if (item.input_Type.Code != InputType_1) continue;
                        //   var callingprocess = _myChat.CurentProcess.ProcessType.Menus.FirstOrDefault(m =>m.ProcessMenuCode == (item?.NextProcessMenu?.ProcessMenuCode?? "StartMenu"));
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                        inlineKeyboardButtons.Add(lineBTN);
                    }
                    else if (item.input_Type.Code == InputType_5)
                    {
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;

                        //string LogBack = _myChat.Logs
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, item.NextProcessMenu.GetEntityTypeId()) };
                        inlineKeyboardButtons.Add(lineBTN);
                    }
                    else if (item.input_Type.Code == "CallbackQueryBool")
                    {
                        var dinamicBtn = thisChat.DinamicButons.FirstOrDefault(p => p.MyName == item.MyName);
                        if (dinamicBtn == null)
                        {
                            db.Inputs.Update(item);
                            if (item.input_Type.Code != InputType_1) continue;
                            //   var callingprocess = _myChat.CurentProcess.ProcessType.Menus.FirstOrDefault(m =>m.ProcessMenuCode == (item?.NextProcessMenu?.ProcessMenuCode?? "StartMenu"));
                            var callingprocess = item?.NextProcessMenu;
                            if (callingprocess == null) continue;
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}|") };
                            inlineKeyboardButtons.Add(lineBTN);

                        }
                        else
                        {
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: dinamicBtn.Content, callbackData: dinamicBtn.CallbackQwery) };
                            inlineKeyboardButtons.Add(lineBTN);

                        }
                    }


                }
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                if (thisChat.PriviosMSGs.LastOrDefault() is null) return null;





                bool isDifferntText = update.CallbackQuery.Message.Text.SequenceEqual(thisChat.CurentTexrMessage);


                var curentKeyBord = update.CallbackQuery?.Message.ReplyMarkup;
                bool IsDifferentKeyBord = curentKeyBord.InlineKeyboard.SequenceEqual(inlineKeyboardMarkup.InlineKeyboard);


                if (isDifferntText == false && IsDifferentKeyBord == false)
                {
                    await iClient.EditMessageTextAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, FormateText(thisChat.CurentTexrMessage), ParseMode.Html, replyMarkup: inlineKeyboardMarkup);

                }
                else if (isDifferntText == false && IsDifferentKeyBord == true)
                {
                    await iClient.EditMessageTextAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, FormateText(thisChat.CurentTexrMessage), ParseMode.Html);

                }
                else if (isDifferntText == true && IsDifferentKeyBord == false)
                {
                    await iClient.EditMessageReplyMarkupAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, replyMarkup: inlineKeyboardMarkup);
                }



                //  if (IsDifferent == true) await iClient.EditMessageReplyMarkupAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, replyMarkup: inlineKeyboardMarkup);

                //Message sentMessage = await iClient.SendTextMessageAsync(chatId: thisChat.ChatId, text: FormateText(_myChat.CurentTexrMessage ?? _myChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: canslationToken);
                //msgResult.Add(sentMessage);
                //_myChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(_myChat.BotClientId, _myChat.CurentProcess.NeedToDelite ?? true, msgResult, update));



            }
            else if (thisChat.CurentProcess.ProcessType.Code == "DinamickListButtonsCallbackQuery")
            {
                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();



                foreach (var item in thisChat.CurentProcess.Inputs)
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
                            List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: "Убрать фильтр", callbackData: thisChat.CurentProcess.GetEntityTypeId()) };
                            inlineKeyboardButtons.Add(lineBTN);
                        }


                        inlineKeyboardButtons.AddRange(InlineKeyCreate(thisChat.DinamicButons, 1));

                        //foreach (var dBut in _myChat.DinamicButons)
                        //{
                        //    if (filteroptions != null && (!dBut.Content.ToLower().Contains(filteroptions.ToLower()))) continue;

                        //    List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: dBut.Content, callbackData: $"{dBut.CallbackQwery}") };
                        //    inlineKeyboardButtons.Add(lineBTN);


                        //}



                        if (thisChat.DinamicButons.Count > 0)
                        {

                            // db.dinamic_Butons.RemoveRange(db.dinamic_Butons.Where(p => p.Chat == null).ToList());

                            db.dinamic_Butons.RemoveRange(thisChat.DinamicButons);

                            thisChat.DinamicButons.Clear();


                            db.SaveChanges();
                        }
                        //   List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, callbackData: $"m:{callingprocess.MyId}") };
                        //  inlineKeyboardButtons.Add(lineBTN);

                    }
                    else if (item.input_Type.Code == InputType_5)
                    {
                        var callingprocess = item?.NextProcessMenu;
                        if (callingprocess == null) continue;

                        //string LogBack = _myChat.Logs
                        List<InlineKeyboardButton> lineBTN = new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(text: item.MyName, item.NextProcessMenu.GetEntityTypeId()) };
                        inlineKeyboardButtons.Add(lineBTN);
                    }
                    else if (item.input_Type.Code == "CallbackQueryBool")
                    {
                        if (thisChat.DinamicButons.Count() > 0)
                        {
                            int maxRows = 20;

                            int colmns = (int)Math.Ceiling((double)(thisChat.DinamicButons.Count() / maxRows));
                            //int colmns2 = (int)Math.Ceiling((double)(_myChat.DinamicButons.Count()/72));
                            //int colmns3 = (int)Math.Ceiling((double)(_myChat.DinamicButons.Count() / 76));


                            int remainingRows = thisChat.DinamicButons.Count() % maxRows;
                            //int remainingRows2 = _myChat.DinamicButons.Count() % 72;
                            //int remainingRows3 = _myChat.DinamicButons.Count() % 76;
                            if (remainingRows > 0)
                            {
                                colmns++;
                            }
                            //if (remainingRows2 > 0)
                            //{
                            //    colmns2++;
                            //}
                            //if (remainingRows3 > 0)
                            //{
                            //    colmns3++;
                            //}



                            //int _BTN_40 = _myChat.DinamicButons.Count()%40 ;

                            //  if (colmns==1&&_myChat.DinamicButons.)

                            inlineKeyboardButtons.AddRange(InlineKeyCreate(thisChat.DinamicButons, colmns));


                        }
                    }


                    //Start
                    //Отослал кнопки
                    //Пользователь ажал кнопку
                    //Пришел Update
                    //Записал Log
                    //Обработал обтейт и получае Mext menu
                    //Change menu -  изменяю меню и затераю dinamic btns и curent text
                    //on end
                    // on load
                    // отпраяляю 



                }


                // InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboardButtons);

               // Message sentMessage = await iClient.SendTextMessageAsync(chatId: thisChat.ChatId, text: FormateText(thisChat.CurentProcess.MenuProcessContent), replyMarkup: inlineKeyboardMarkup, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: cancellationToken);
                //msgResult.Add(sentMessage);
                // thisChat.PriviosMSGs.AddRange(PriviosMSG.createMessage(thisChat.BotClientId, thisChat.CurentProcess.NeedToDelite ?? true, msgResult, update));


                bool isDifferntText = update.CallbackQuery.Message.Text.SequenceEqual(thisChat.CurentTexrMessage??thisChat.CurentProcess.MenuProcessContent);


                var curentKeyBord = update.CallbackQuery?.Message.ReplyMarkup;
                bool IsDifferentKeyBord = curentKeyBord.InlineKeyboard.SequenceEqual(inlineKeyboardMarkup.InlineKeyboard);


                if (isDifferntText == false && IsDifferentKeyBord == false)
                {
                    await iClient.EditMessageTextAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, FormateText(thisChat.CurentTexrMessage ?? thisChat.CurentProcess.MenuProcessContent), ParseMode.Html, replyMarkup: inlineKeyboardMarkup);

                }
                else if (isDifferntText == false && IsDifferentKeyBord == true)
                {
                    await iClient.EditMessageTextAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, FormateText(thisChat.CurentTexrMessage ?? thisChat.CurentProcess.MenuProcessContent), ParseMode.Html);

                }
                else if (isDifferntText == true && IsDifferentKeyBord == false)
                {
                    await iClient.EditMessageReplyMarkupAsync(thisChat.ChatId, thisChat.PriviosMSGs.LastOrDefault().MessageId ?? 0, replyMarkup: inlineKeyboardMarkup);
                }


                db.SaveChanges();

            }

            return null;
        }



        //List<List<InlineKeyboardButton>>
        //InlineKeyboardMarkup
        public List<List<InlineKeyboardButton>> InlineKeyCreate(List<Dinamic_Butons> buttons, int nColomns)
        {
            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            for (int i = 0; i < buttons.Count; i++)
            {
                // Вычисляем номер строки для текущей кнопки
                int row = i / nColomns;
                // Если такой строки еще нет в списке, добавляем новую строку
                if (inlineKeyboard.Count <= row)
                {
                    inlineKeyboard.Add(new List<InlineKeyboardButton>());
                }

                // Добавляем кнопку в текущую строку с текстом и данными обратного вызова
                inlineKeyboard[row].Add(new InlineKeyboardButton(buttons[i].Content)
                {
                    Text = buttons[i].Content,
                    CallbackData = buttons[i].CallbackQwery
                });
            }

            return inlineKeyboard;
            //   return new InlineKeyboardMarkup(inlineKeyboard);
        }

        public async Task<string> SetRegistration(string SetTipe, DateTime setDateTime, string UserName, string PhoneNum, string SetNum)
        {
                      concoldebuger.badMSG("ListMenuTime_park_OnLoad- await   6  await Task.Run(() => driver.Navigate().GoToUrl(startPath1))_____________________________________________________________________________ --- OnLoadHadler");

            bool conrol_registraation = true;

            string result = $"Сет зарегисрирован|{setDateTime.ToString("HH:mm dd-MM-yy")}|";
            
            //if (UserName == null) UserName = "Убрать";
            //if (PhoneNum == null) PhoneNum = "9117631807";
            //if (SetNum == null) SetNum = "2001412";

            if (UserName == null) UserName = "Зареган через бот";
            if (PhoneNum == null) PhoneNum = "9111111111";
            if (SetNum == null) SetNum = "";

            PhoneNum = PhoneNum.Trim();
            
            PhoneNum = PhoneNum.Replace("-", "");
            PhoneNum = PhoneNum.Replace("+7", "");
          if (PhoneNum[0]=='8')  PhoneNum = PhoneNum.Remove(0,1);





            string startPath1 = "https://dawinchiwakepark.ru";

            

            int trying = 0;
            while (trying != 10)
            {
                ChromeOptions options = new ChromeOptions();
                //  options.AddArgument("--headless"); // Запуск браузера в "тихом" режиме (без открытия окна)

                options.AddArgument("ignore-certificate-errors");
                options.AddArgument("--ignore-certificate-errors-spki-list");
                options.AddArgument("--ignore-ssl-errors");
                options.AddArgument("test-type");
                options.AddArguments("-incognito");
                options.AddArgument("no-sandbox");
                options.AddArgument("--start-maximized");
                options.AddArgument("log-level=3");
                IWebDriver driver = new ChromeDriver(options);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(6);

                try
                {
                    using (null)
                    {




                        conrol_registraation = false;

                        try
                        {
                            await Task.Run(() => driver.Navigate().GoToUrl(startPath1));

                        }
                        catch (WebDriverTimeoutException)
                        {
                            // Обрабатываем ошибку "таймаута", но продолжаем работу с тем, что уже загружено
                        }


                        // concoldebuger.badMSG("ждем 10 секунд -------------------------4");

                        //await Task.Delay(4000);



                        IWebElement nameInput = driver.FindElement(By.XPath("//input[@id='name']"));
                        nameInput.SendKeys(UserName); // replace "Your Name" with the desired input

                        await Task.Delay(200);

                        IWebElement phoneInput = driver.FindElement(By.XPath("//input[@id='--7']"));
                        phoneInput.SendKeys(PhoneNum);
                        foreach (var item in PhoneNum)
                        {
                            phoneInput.SendKeys(item.ToString());

                            await Task.Delay(40);
                        }



                        IWebElement dropdown = driver.FindElement(By.XPath("//select[@id='-']"));
                        SelectElement select = new SelectElement(dropdown);
                        select.SelectByText(SetTipe);
                        await Task.Delay(500);


                        if (SetNum != null || SetNum == "")
                        {
                            IWebElement abonInput = driver.FindElement(By.XPath("//input[@id='promo']"));
                            abonInput.SendKeys(SetNum); // replace "Your Name" with the desired input
                            await Task.Delay(500);
                        }

                        IWebElement buttonAbonTypeGONext = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext.Click();
                        await Task.Delay(500);

                        // Находим элемент "Сет по абонементу" по XPath
                        IWebElement setAbonementu = driver.FindElement(By.XPath("//div[@class='time-slot']/label[contains(text(), 'Сет по абонементу')]"));

                        // Выбираем элемент "Сет по абонементу"
                        setAbonementu.Click();
                        await Task.Delay(1000);
                        IWebElement buttonAbonTypeGONext2 = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext2.Click();
                        await Task.Delay(1000);


                        //IWebElement button = driver.FindElement(By.ClassName("wizard-btn-wd"));
                        //button.Click();
                        //await Task.Delay(500);

                        //IWebElement buttonAbonType = driver.FindElement(By.ClassName("time-slot"));
                        //button.Click();
                        //await Task.Delay(500);

                        //IWebElement button_abon_Set = driver.FindElement(By.Id("7"));

                        //button.Click();
                        //await Task.Delay(500);


                        IWebElement dateElementMonth= driver.FindElement(By.XPath($"//span[contains(@class, 'day__month_btn') and contains(@class, 'up')]"));
                        if (!dateElementMonth.Text.Trim().ToLower().Contains($"{setDateTime.ToString("MMM")}"))
                        {
                            IWebElement dateElementMonthnext = driver.FindElement(By.XPath($"//span[contains(@class, 'next')]"));
                            dateElementMonthnext.Click();

                        }

                        string date = setDateTime.ToString("d");
                        if (date[0] == '0') date.Remove(0, 1);
                        string date1 = setDateTime.Day.ToString();

                        // IWebElement dateElement = driver.FindElement(By.XPath($"//span[contains(@class, 'cell day')   and text()='{setDateTime.ToString("dd")}']"));
                        IWebElement dateElement = driver.FindElement(By.XPath($"//span[contains(@class, 'cell') and contains(@class, 'day') and text()='{date1}']"));

                        //and contains(@class, 'day')


                        // Теперь кликнем на эту дату в календаре
                        dateElement.Click();
                        await Task.Delay(1000);



                        // IWebElement timeElement = driver.FindElement(By.XPath("//label[contains(@for, '" + setDateTime.ToString("HH:mm") + "')]"));
                        //  IWebElement timeElement = driver.FindElement(By.XPath("//span[contains(@for, '" + setDateTime.ToString("HH:mm") + "')]"));



                        //IWebElement timeElement = driver.FindElement(By.XPath($"//div[@class='time-slot']/label[contains(text(), '{setDateTime.ToString("HH:mm")}')]"));
                        //// Теперь кликнем на это время
                        //timeElement.Click();
                        //await Task.Delay(1000);


                        IList<IWebElement> timeSlots = driver.FindElements(By.ClassName("time-slot"));
                        string targetTime = setDateTime.ToString("HH:mm");

                        foreach (var slot in timeSlots)
                        {
                            string labelText = slot.FindElement(By.TagName("label")).Text.Trim();
                            if (labelText == targetTime)
                            {
                                // IWebElement radioButton = slot.FindElement(By.CssSelector("input[type='radio']"));
                                IWebElement radioButton = slot.FindElement(By.TagName("label"));

                                radioButton.Click();
                                break;
                            }
                        }
                        await Task.Delay(1000);




                        IWebElement buttonAbonTypeGONext3 = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext3.Click();
                        await Task.Delay(1000);

                        


                        using (null)
                        {
                            IWebElement errorElement = null;
                            try
                            {
                                errorElement = driver.FindElement(By.XPath("//span[@class='error' and text()='No query results for model [App\\Promocode].']"));
                                if (errorElement != null)
                                {
                                    driver.Close();
                                    result= $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";
                                    
                                    break;
                                }

                            }
                            catch
                            {

                            }






                            try
                            {
                               
                                errorElement = driver.FindElement(By.XPath("//span[@class='error' and text()=''No query results for model [App\\Promocode]."));
                                if (errorElement != null)
                                {
                                    driver.Close();
                                    result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                                    break;
                                }



                            }
                            catch
                            {

                            }





                            try
                            {


                                errorElement = driver.FindElement(By.XPath($"//span[contains(@class, 'error')]"));
                                if (errorElement != null)
                                {
                                    string text = errorElement.Text;

                                    driver.Close();

                                    

                                    result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                                    break;
                                }

                            }
                            catch
                            {

                            }


                        }

                        conrol_registraation = true;


                    }
                }
                catch 
                {
                    conrol_registraation = false;

                    trying++;
                    if (trying==10) result= $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                }
                driver.Close();
                if (conrol_registraation == true) break;

            }


           





            return result;

        }




        public async Task start_new_sasion(string user_Types_code,DateTime? remooveTime, ITelegramBotClient client, CancellationToken cancellationToken)
        {
            using (var db = new context())
            {

                var allChats = db.myChats.Where(c=>c.AllChatUsers.FirstOrDefault().Type.TypeCode== user_Types_code).ToList();

                allChats = allChats.Where(c => c.PriviosMSGs?.LastOrDefault()?.dateTimeCreation<=remooveTime).ToList();


                foreach (var chat in allChats)
                {
                    if (chat.CurentProcess.ProcessMenuCode.Contains("AwaytingUser_")) continue;

                    await new TeleTools().remooveMenu(client, cancellationToken, chat);

                    var process = db.Menu_Proceses.FirstOrDefault(m => m.ProcessMenuCode == $"AwaytingUser_{user_Types_code}");

                    chat.SetProcess(process);
                    db.SaveChanges();
                    await chat.CurentProcess.ExecuteOnLoad(null,client,chat,db,cancellationToken);
                    await new TeleTools().SendStaticMenu_forXMLLoad(chat, client, cancellationToken, null, db);





                }

                db.SaveChanges();




            }

        }



        public async Task <List<DateSetTime>> GetAllSets(string DashBordType, ITelegramBotClient client, DateTime dateTimeTarget, bool IsFreeSets)
        { 

            

            List<DateSetTime> result = new List<DateSetTime>();

            ChromeOptions options = new ChromeOptions();
            //  options.AddArgument("--headless"); // Запуск браузера в "тихом" режиме (без открытия окна)

            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("--ignore-certificate-errors-spki-list");
            options.AddArgument("--ignore-ssl-errors");
            options.AddArgument("test-type");
            options.AddArguments("-incognito");
            options.AddArgument("no-sandbox");
            options.AddArgument("--start-maximized");
            options.AddArgument("log-level=3");
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(8);



            string startPath1 = "https://dawinchiwakepark.ru/dashboard";
            try
            {
                 Task.Run(()=> driver.Navigate().GoToUrl(startPath1));

            }
            catch (WebDriverTimeoutException)
            {
                // Обрабатываем ошибку "таймаута", но продолжаем работу с тем, что уже загружено
            }
            await Task.Delay(6000);

            //  concoldebuger.badMSG("ListMenuTime_park_OnLoad- await   6  await Task.Run(() => driver.Navigate().GoToUrl(startPath1))_____________________________________________________________________________ --- OnLoadHadler");

            IWebElement dateElemrnt = driver.FindElement(By.XPath("//h2[@class='text-center']"));

            string textDateel = dateElemrnt.Text.Trim() ;

            string dateTimeCurent = dateTimeTarget.ToString("dd MMMM");

            if (!textDateel.ToLower().Contains( dateTimeCurent))
            {
                IWebElement calendareButton = driver.FindElement(By.XPath("//button[contains(@class, 'bg-transparent') and contains(@class, 'hover:bg-blue-500') and contains(text(), 'Выбрать дату')]"));
                calendareButton.Click();
                await Task.Delay(100);

               
                while (true)
                {
                    IWebElement monthInfo = driver.FindElement(By.XPath("//span[contains(@class, 'day__month_btn')]"));
                    string textMonth = monthInfo.Text.Trim();
                    string texttargetMonth = (dateTimeTarget.ToString("MMM"));

                    string jdfskj = dateTimeTarget.AddMonths(2).ToString("MMM");



                    if (textMonth.ToLower().Contains(texttargetMonth)) break;

                    driver.FindElement(By.CssSelector("span.next")).Click();
                   
                }

                driver.FindElement(By.XPath($"//span[contains(@class, 'cell day') and text()='{dateTimeTarget.Day}']")).Click();




            }

            await Task.Delay(1000);



            await Task.Delay(4000);

            IWebElement button_park = driver.FindElement(By.XPath($"//a[contains(text(), '{DashBordType}')]"));
            button_park.Click();
            await Task.Delay(500);


           


            IList<IWebElement> divElements = driver.FindElements(By.XPath("//div[contains(@class, 'border-r') and contains(@class, 'border-b') and contains(@class, 'p-1') and contains(@class, 'text-xs') and contains(@class, 'align-middle') and contains(@class, 'flex-1') and contains(@class, 'sm:h-32') and contains(@class, 'md:h-24')]"));
            await Task.Delay(200);

            foreach (var item in divElements)
            {
                string classText = item.GetAttribute("class");

                if (IsFreeSets)
                {
                    if (classText.Contains("booked")) continue;
                    if (classText.Contains("temporary")) continue;
                    if (classText.Contains("temporary-long")) continue;
                    if (classText.Contains("reservation")) continue;
                    if (classText.Contains("past")) continue;

                }

                IWebElement infoUser = item.FindElement(By.XPath(".//span[@class='block details-top']"));

                string userInfoText = infoUser?.Text??"-";

                IWebElement infoSet = item.FindElement(By.XPath(".//span[@class='block details-bottom']"));

                string Infoset = infoSet?.Text??"-";


                string dateTimeValue = item.GetAttribute("datetime");

                string dateTimeValue2 = item.GetAttribute("block details-top");

                

                int hours = int.Parse(dateTimeValue.Split(':')[0]);
                int minutes = int.Parse(dateTimeValue.Split(':')[1]);

                if (userInfoText == "") userInfoText = "Имя и тел.: - ";
                if (Infoset == "") Infoset = "Сет 10 мин";

              var datetimeSet=  new DateTime(DateTime.Now.Year, dateTimeTarget.Month, dateTimeTarget.Day, hours, minutes, 0);



                string discription = $"{datetimeSet.ToString("HH:mm")}\n Инф:{userInfoText}\n{Infoset}";



                result.Add(new DateSetTime()
                {
                    BotClientId = client.BotId,
                    IsDelite = false,
                    MyDescription = discription,
                       name = userInfoText,
                    Telephone = Infoset,
                    dateTimeCreation = DateTime.Now,
                    SetdateTime = datetimeSet
                }) ;

            }

    


            driver.Close();




            return result;
        }





        public async Task<string> CreateTunel(  string UserID)
        {
            
            bool conrol_registraation = true;

           



            string startPath1 = "https://dawinchiwakepark.ru";



            int trying = 0;
            while (trying != 10)
            {
                ChromeOptions options = new ChromeOptions();
                //  options.AddArgument("--headless"); // Запуск браузера в "тихом" режиме (без открытия окна)

                options.AddArgument("ignore-certificate-errors");
                options.AddArgument("--ignore-certificate-errors-spki-list");
                options.AddArgument("--ignore-ssl-errors");
                options.AddArgument("test-type");
                options.AddArguments("-incognito");
                options.AddArgument("no-sandbox");
                options.AddArgument("--start-maximized");
                options.AddArgument("log-level=3");
                IWebDriver driver = new ChromeDriver(options);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(6);

                try
                {
                    using (null)
                    {




                        conrol_registraation = false;

                        try
                        {
                            await Task.Run(() => driver.Navigate().GoToUrl(startPath1));

                        }
                        catch (WebDriverTimeoutException)
                        {
                            // Обрабатываем ошибку "таймаута", но продолжаем работу с тем, что уже загружено
                        }


                        // concoldebuger.badMSG("ждем 10 секунд -------------------------4");

                        //await Task.Delay(4000);



                        IWebElement nameInput = driver.FindElement(By.XPath("//input[@id='name']"));
                       // nameInput.SendKeys(UserName); // replace "Your Name" with the desired input

                        await Task.Delay(200);

                        IWebElement phoneInput = driver.FindElement(By.XPath("//input[@id='--7']"));
                      


                        IWebElement dropdown = driver.FindElement(By.XPath("//select[@id='-']"));
                        SelectElement select = new SelectElement(dropdown);
                        select.SelectByText("kk");
                        await Task.Delay(500);



                        IWebElement buttonAbonTypeGONext = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext.Click();
                        await Task.Delay(500);

                        // Находим элемент "Сет по абонементу" по XPath
                        IWebElement setAbonementu = driver.FindElement(By.XPath("//div[@class='time-slot']/label[contains(text(), 'Сет по абонементу')]"));

                        // Выбираем элемент "Сет по абонементу"
                        setAbonementu.Click();
                        await Task.Delay(1000);
                        IWebElement buttonAbonTypeGONext2 = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext2.Click();
                        await Task.Delay(1000);


                        





                        IWebElement buttonAbonTypeGONext3 = driver.FindElement(By.XPath("//button[@class='wizard-btn btn-fill wizard-btn-wd btn-next']"));
                        buttonAbonTypeGONext3.Click();
                        await Task.Delay(1000);




                        using (null)
                        {
                            IWebElement errorElement = null;
                            try
                            {
                                errorElement = driver.FindElement(By.XPath("//span[@class='error' and text()='No query results for model [App\\Promocode].']"));
                                if (errorElement != null)
                                {
                                    driver.Close();
                                  // result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                                    break;
                                }

                            }
                            catch
                            {

                            }






                            try
                            {

                                errorElement = driver.FindElement(By.XPath("//span[@class='error' and text()=''No query results for model [App\\Promocode]."));
                                if (errorElement != null)
                                {
                                    driver.Close();
                                  //  result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                                    break;
                                }



                            }
                            catch
                            {

                            }





                            try
                            {


                                errorElement = driver.FindElement(By.XPath($"//span[contains(@class, 'error')]"));
                                if (errorElement != null)
                                {
                                    string text = errorElement.Text;

                                    driver.Close();



                                   // result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                                    break;
                                }

                            }
                            catch
                            {

                            }


                        }

                        conrol_registraation = true;


                    }
                }
                catch
                {
                    conrol_registraation = false;

                    trying++;
                   // if (trying == 10) result = $"Ошибка при регистрации|{setDateTime.ToString("HH:mm dd-MM-yy")}|";

                }
                driver.Close();
                if (conrol_registraation == true) break;

            }








            return "";

        }




    }

}
