using Telebot.Sourse.Item;
using Telegram.Bot;
using Telegram.Bot.Types;
using OfficeOpenXml;




using static Telebot.Sourse.TeleTools;
using OpenAI.Files;
using File = System.IO.File;
using Telegram.Bot.Types.ReplyMarkups;
using OpenAI.Chat;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Telebot.Sourse.Item.IItem;
using Message = Telegram.Bot.Types.Message;
using Update = Telegram.Bot.Types.Update;

namespace Telebot.Sourse.Handlers
{
    public  class adminHendlerHR
    {


        static string Cont_btn_back = "⬅️Назад";


        public  async void adminHendler(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId)
        {





            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var curentMenu = parsmenuType(update.CallbackQuery.Data);

                if (curentMenu == processCodeAdmin.strstMenu) {await sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.adminMenuList) { await sendUsetList(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.loadStartPhoto) { await sendLoadStartPhoto(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                if (curentMenu == processCodeAdmin.strstMenu_listuser_user) { await sendUsetList(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.strstMenu_listuser_user_userProps) { await sendUserProps(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) { await sendUserProps_changeTypUser(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.adminComment1List) { await sendReqstList(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                if (curentMenu == processCodeAdmin.adminComment1List_reqstProps) { await sendReqstProps(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                if (curentMenu == processCodeAdmin.loadStartPhoto_done) { await sendReqstProps_done(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                if (curentMenu == processCodeAdmin.loadStart_execl) { await sendExcell(update, client, cancellationToken, teleChatId, teleUserId, null, curentMenu); return; }
                if (curentMenu == processCodeAdmin.loadStart_try) { await tryButiins(update, client, cancellationToken, teleChatId, teleUserId, null, curentMenu); return; }

            }
            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {

                if (update.Message.Text == "/start") {await sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                var curentMenu = processCodeAdmin.err;

                using (var db = new context())
                {
                    var curentChat = db.myChats.FirstOrDefault(c => c.ChatId == update.Message.Chat.Id);
                    if (curentChat != null) curentMenu = parsmenuType(curentChat.processCode);
                }



                if (curentMenu == processCodeAdmin.adminMenuList) { sendUsetList(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }



                if (curentMenu == processCodeAdmin.loadStartPhoto) { sendLoadStartPhoto_get(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }
                if (curentMenu == processCodeAdmin.adminComment1List) { sendReqstList(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }


                if (curentMenu == processCodeAdmin.err) { sendStartErrl(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }


            }
            

        }




        //----- 
        private async Task tryButiins(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns, processCodeAdmin curentMenu)
        {

            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);

                //важно поментояь 
                string curetnProcessCode = setCodeMenu(curentMenu);









                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();




            ///    buttons.Add(new TeleTools.Buttons() { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });



                //  context.BackupDatabase(db);

              

            ///    List< Message> message = await new TeleTools().SendStaticMenualot(myChat, client, cancellationToken, buttons, "📃 Вот бекап:", null);

            ///    var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

           ///     myChat.PriviosMSGs.AddRange(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }



        }







        //cnfhnjdjt vty.
        public async Task sendStartMeenuexsampel(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu);


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                buttons.Add(new TeleTools.Buttons { content = "👤 Пользователи и админы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "🗄 Все запросы", callBackCode = setCodeMenu(processCodeAdmin.adminComment1List) });
                buttons.Add(new TeleTools.Buttons { content = "📗 Excel отчет", callBackCode = setCodeMenu(processCodeAdmin.loadStart_execl) });
                buttons.Add(new TeleTools.Buttons { content = "🏞 Баннер в начало", callBackCode = setCodeMenu(processCodeAdmin.loadStartPhoto) });

                //loadStart_try
               if (myChat.AllChatUsers.LastOrDefault().Id== 469825678) buttons.Add(new TeleTools.Buttons { content = "бекап", callBackCode = setCodeMenu(processCodeAdmin.loadStart_try) });
                Message message = await new  TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "<b>Меню администратора.</b> Чем помочь?", null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }

        //Запросы

        public  async Task sendReqstList(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);

                //важно поментояь 
                string curetnProcessCode = setCodeMenu(processCodeAdmin.adminComment1List);









                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                List<requst> allReqsts = null;

                if (filterInstrns is null) allReqsts = db.Requst.Where(u => u.IsDelite != true&&u.isCreated==true).ToList();
                else
                {
                    long iduser = 0;
                    long.TryParse(filterInstrns, out iduser);
                   if (iduser!=0) allReqsts = db.Requst.Where(r => r.IsDelite != true&&r.user.Id==iduser && r.isCreated == true).ToList();
                    else allReqsts = db.Requst.Where(u => u.IsDelite != true && u.isCreated == true).ToList();


                }


                buttons.Add(new TeleTools.Buttons() { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });
                if (filterInstrns is not null) buttons.Add(new TeleTools.Buttons() { content = "🔍❌ Убрать фильтр", callBackCode = setCodeMenu(processCodeAdmin.adminComment1List) });



                foreach (var reqst in allReqsts)
                {
                    string type = "";
                    if (reqst.reqstTupe.Contains("Предложение")) type = "💡";
                    else if (reqst.reqstTupe.Contains("Задать вопрос")) type = "❓";
                    else if (reqst.reqstTupe.Contains("Ошибка в материалах")) type = "🖍";

                    string newornot = "";
                    if (reqst.isNew==true) newornot = "🆕";
                    else newornot = "🆗";
                    
                    string doneis = "";
                    if (reqst.isDone == true) doneis = "✅";
                    else doneis = "⚠️";


                    buttons.Add(new TeleTools.Buttons() { content = $" {doneis}|{newornot}| {type} | {reqst.MyId} | {reqst?.user?.Username ?? reqst?.user?.FirstName ?? reqst?.user?.LastName ?? reqst?.user?.Id.ToString()} ", callBackCode = setCodeMenu(processCodeAdmin.adminComment1List_reqstProps) + reqst.SetUserIncallBack() });
                }



                List<Message> message = await new TeleTools().SendStaticMenualot(myChat, client, cancellationToken, buttons, "📃 Запросы:", null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.AddRange(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }

        //натройки заявки
        public async Task sendReqstProps(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                int reqstId = requst.GetUserIdFromCode(update.CallbackQuery.Data);
                var reqst = db.Requst.FirstOrDefault(u => u.MyId == reqstId);
                if (reqst is null) return;
                reqst.isNew = false;
                db.Requst.Update(reqst);
                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.adminComment1List_reqstProps) + reqst.SetUserIncallBack();


                //формирование кнопорей




                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();
               // if (user.UserType == MyUser.userType.regulareUser) buttons.Add(new TeleTools.Buttons { content = "Сделать администратором", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });
               // if (user.UserType == MyUser.userType.admin) buttons.Add(new TeleTools.Buttons { content = "Забрать права", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });

              if (reqst.isDone is false) buttons.Add(new TeleTools.Buttons { content = "✅ Отработана", callBackCode = setCodeMenu(processCodeAdmin.loadStartPhoto_done)+ reqst.SetUserIncallBack() });
                if (reqst.isDone is true) buttons.Add(new TeleTools.Buttons { content = "⚠️ Не отработана", callBackCode = setCodeMenu(processCodeAdmin.loadStartPhoto_done)+ reqst.SetUserIncallBack() });
                if (reqst.isDone is null) buttons.Add(new TeleTools.Buttons { content = "⚠️ Не отработана", callBackCode = setCodeMenu(processCodeAdmin.loadStartPhoto_done)+ reqst.SetUserIncallBack() });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.adminComment1List) });

                string statusReq = "✅ Отработана";
                if (reqst.isDone == false|| reqst.isDone==null) statusReq = "⚠️ не отработана";

                string menuCont = $"📃 <b>Запрос:</b> \n🆔: {reqst.MyId}\nДата создания: {reqst.dateTimeCreation}\nТип запроса: {reqst.reqstTupe}\n\n<b>Текст запроса:</b>\n<i>{reqst.reqstContent}</i>\n\n🗣 <b>Автор</b>\nUser name: {"@"+reqst.user.Username ?? ""}" +
                    $"\nLast name: {reqst.user.LastName ?? ""}\nFirst name: {reqst.user.FirstName ?? ""}" +
                    $"\nUser type: {reqst.user.UserType.ToString()}\n\nСтатус заявки: {statusReq}";

                if (reqst.Photoes.Count() > 0)
                {
                    var msgSPhoto = await new TeleTools().SendPhotoAlbum(myChat, client, cancellationToken, null, "Фото к заявке", reqst.Photoes);

                    foreach (var item in msgSPhoto)
                    {
                        var privMsg2 = PriviosMSG.createMessage(myChat.BotClientId, true, item, update);

                        myChat.PriviosMSGs.Add(privMsg2);
                    }


                }


                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, menuCont, null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }






        public async Task sendReqstProps_done(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                int reqstId = requst.GetUserIdFromCode(update.CallbackQuery.Data);
                var reqst = db.Requst.FirstOrDefault(u => u.MyId == reqstId);
                if (reqst != null)
                {

                    if (reqst.isDone is not null) reqst.isDone = !reqst?.isDone;
                    else reqst.isDone = false;
                    db.Requst.Update(reqst);
                    db.SaveChanges();
                    // удаление старого меню


                    //формирование кнопорей




                    //конец обработки и изменение статуса 


                    var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                    if (remuving is not null || remuving.Count() != 0)
                    {
                        db.priviosMSG.RemoveRange(remuving);
                    }

                    //myChat.processCode = curetnProcessCode;

                    db.myChats.Update(myChat);
                    db.SaveChanges();

                }
            }


            sendReqstProps(update, client, cancellationToken, teleChatId, teleUserId, null);


        }





        //errr важно поправить в уонцк
        public async Task sendStartErrl(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu);


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "Вопросы и заявки", callBackCode = setCodeMenu(processCodeAdmin.adminComment1List) });


                Message messageErr = await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, "⚠️ Что то пошло не так.\nПопробуйте еще раз.", null);

                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "Меню администратора", null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);
                var privMsg2 = PriviosMSG.createMessage(myChat.BotClientId, true, messageErr, update);

                myChat.PriviosMSGs.Add(privMsg2);
                myChat.PriviosMSGs.Add(privMsg);









                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }



        // список пользователей
        public async Task sendUsetList(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);

                //важно поментояь 
                string curetnProcessCode = setCodeMenu(processCodeAdmin.adminMenuList);









                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                List<MyUser> allusers = null;

                if (filterInstrns is null) allusers = db.MyUsers.Where(u => u.IsDelite != true).ToList();
                else allusers = db.MyUsers.Where(u => u.IsDelite != true && u.Username.ToLower().Contains(filterInstrns.ToLower())).ToList();

                buttons.Add(new TeleTools.Buttons() { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });
                if (filterInstrns is not null) buttons.Add(new TeleTools.Buttons() { content = "🔍❌ Убрать фильтр", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });



                foreach (var user in allusers)
                    buttons.Add(new TeleTools.Buttons() { content = $"👤 {user.Id} | {user.Username ?? user.FirstName ?? user.LastName} ", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps) + user.SetUserIncallBack() });




                List<Message> message = await new TeleTools().SendStaticMenualot(myChat, client, cancellationToken, buttons, "📃 Запросы:", null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.AddRange(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }




        //пользователь
        public async Task sendUserProps(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                int userId = MyUser.GetUserById(update.CallbackQuery.Data);
                var user = db.MyUsers.FirstOrDefault(u => u.MyId == userId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps) + user.SetUserIncallBack();


                //формирование кнопорей




                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();
                if (user.UserType == MyUser.userType.regulareUser) buttons.Add(new TeleTools.Buttons { content = "Сделать администратором", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });
                if (user.UserType == MyUser.userType.admin) buttons.Add(new TeleTools.Buttons { content = "Забрать права", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });

                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });


                string menuCont = $"Пользователь\n🆔: {user.Id}\nUser name: {user.Username ?? ""}" +
                    $"\nLast name: {user.LastName ?? ""}\nFirst name: {user.FirstName ?? ""}" +
                    $"\nUser type: {user.UserType.ToString()}";




                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, menuCont, null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);








                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }



        public async Task sendUserProps_changeTypUser(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                int userId = MyUser.GetUserById(update.CallbackQuery.Data);
                var user = db.MyUsers.FirstOrDefault(u => u.MyId == userId);

                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps) + user.SetUserIncallBack();


                //формирование кнопорей

                if (user.UserType == MyUser.userType.regulareUser) user.UserType = MyUser.userType.admin;
                else if (user.UserType == MyUser.userType.admin) user.UserType = MyUser.userType.regulareUser;

                db.MyUsers.Update(user);
                db.SaveChanges();


                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();
                if (user.UserType == MyUser.userType.regulareUser) buttons.Add(new TeleTools.Buttons { content = "Сделать администратором", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });
                if (user.UserType == MyUser.userType.admin) buttons.Add(new TeleTools.Buttons { content = "Забрать права", callBackCode = setCodeMenu(processCodeAdmin.strstMenu_listuser_user_userProps_Changeusertype) + user.SetUserIncallBack() });

                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });


                string menuCont = $"Пользователь\n🆔: {user.Id}\nUser name: {user.Username ?? ""}" +
                    $"\nLast name: {user.LastName ?? ""}\nFirst name: {user.FirstName ?? ""}" +
                    $"\nUser type: {user.UserType.ToString()}";




                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, menuCont, null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);






                Message mess2 = await new TeleTools().SendStaticMSG(user.Chats.LastOrDefault(), client, cancellationToken, "⚠️ Поменяли права. Нажмите /start", null);
                var mess = PriviosMSG.createMessage(user.Chats.LastOrDefault().BotClientId, true, mess2, update);
                user.Chats.LastOrDefault().PriviosMSGs.Add(mess);
                db.MyUsers.Update(user);
                db.SaveChanges();
                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }


        //--------------------------------------Стартовое фото-----


        //пользователь
        public async Task sendLoadStartPhoto(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

              
                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.loadStartPhoto) ;


                //формирование кнопорей



                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });



                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖: Загрузи баннер 🏞:", null);

                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);







                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }


        public async Task sendLoadStartPhoto_get(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {

                if (update.Message?.Photo is null)
                {
                   

                    sendLoadStartPhoto(update,client,cancellationToken,teleChatId, teleUserId, filterInstrns);

                    return;
                }
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.loadStartPhoto);


                //формирование кнопорей

                var newPHoto = update.Message.Photo;
                List<myPhoto> newMyPhotos = new List<myPhoto>();

                foreach (var item in newPHoto)
                {
                    var createdPhoto = myPhoto.createPhot(item);

                    createdPhoto.BotClientId = client.BotId;
                    createdPhoto.IsStartMSGPhoto = true;
                    newMyPhotos.Add(createdPhoto);

                }
                db.myPhotoes.Add(newMyPhotos.LastOrDefault());
                db.SaveChanges();
                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();
                sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null);
            }

            
        }


        //--------------------------------------Excell-------------------------------------------------------------------------------------------------

        public async Task sendExcell(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns, processCodeAdmin processCodeAdmin)
        {
            using (var db = new context())
            {
                string curetnDir = System.IO.Directory.GetCurrentDirectory();
                string filePath = curetnDir + $"\\{DateTime.Now.ToString().Replace(".", " ").Replace(":", " ")}.xlsx";


                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                // удаление старого меню
                new TeleTools().remooveMenu(client, cancellationToken, myChat);


                string curetnProcessCode = setCodeMenu(processCodeAdmin);

                var allreports = db.Requst.Where(p => p.isCreated == true && p.IsDelite == false).ToList();
                //формирование кнопорей
               


                //-----------------формирование 
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                using (ExcelPackage excelPackage = new ExcelPackage())
                {



                    // Добавляем новый лист
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Отчет");

                    // Записываем текстовую строку в ячейку A1 с разделителями ";"





                    worksheet.Cells[1, 1].Value = "Дата создания";
                    worksheet.Cells[1, 2].Value = "Тип";
                    worksheet.Cells[1, 3].Value = "Имя";
                    worksheet.Cells[1, 4].Value = "Фамилия";
                    worksheet.Cells[1, 5].Value = "Login";
                    worksheet.Cells[1, 6].Value = "User Id";

                    worksheet.Cells[1, 7].Value = "Кол-во фото";
                    worksheet.Cells[1, 8].Value = "Текст запроса";
                    worksheet.Cells[1, 9].Value = "Новая";
                    worksheet.Cells[1, 10].Value = "Отработана";


                    int line = 2;
                    foreach (var item in allreports)
                    {

                        worksheet.Cells[line, 1].Value = item?.dateTimeCreation.Value.ToString("d");
                        worksheet.Cells[line, 2].Value = item?.reqstTupe;
                        worksheet.Cells[line, 3].Value = item?.user?.FirstName ?? "-";
                        worksheet.Cells[line, 4].Value = item?.user?.LastName ?? "-";
                        worksheet.Cells[line, 5].Value = "@" + item?.user?.Username ?? "-";
                        worksheet.Cells[line, 6].Value = item?.user?.Id.ToString();


                        worksheet.Cells[line, 7].Value = item?.Photoes?.Count();
                        worksheet.Cells[line, 8].Value = item?.reqstContent;
                        worksheet.Cells[line, 9].Value = item?.isNewForUser.ToString();
                        worksheet.Cells[line, 10].Value = item?.isDone.ToString();

                        line++;
                    }

                  

                    excelPackage.SaveAs(filePath);
                   

                    // Сохраняем пакет Excel в файл


                }

                await using Stream stream = System.IO.File.OpenRead(filePath);
                Message message = await client.SendDocumentAsync(
                    chatId: myChat.ChatId,
                    document: InputFile.FromStream(stream: stream, fileName: $"{DateTime.Now.ToString().Replace(".", " ").Replace(":", " ")}.xlsx"),
                    caption: "Отчет");
                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, false, message, update);

                stream.Close();
                //конец обработки и изменение статуса 


                var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                if (remuving is not null || remuving.Count() != 0)
                {
                    db.priviosMSG.RemoveRange(remuving);
                }

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

                //удаление ненужного файла 
                try
                {
                    // Проверяем, существует ли файл
                    if (File.Exists(filePath))
                    {
                        // Удаляем файл
                        File.Delete(filePath);
                        Console.WriteLine("Файл успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Файл не существует.");
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
                }

                sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null);
            }


        }



        //_____________________________________________________________________________________________________________



        public async Task start_new_sasion(DateTime remooveTime, ITelegramBotClient client, CancellationToken cancellationToken)
        {
            using (var db=new context())
            {

               var adminChats = db.myChats.Where(chats => chats.PriviosMSGs.Count() != 0).ToList().Where(c=>c.AllChatUsers.LastOrDefault().UserType== MyUser.userType.admin&&parsmenuType(c.processCode)!= processCodeAdmin.awat_start).ToList().Where(c=>c.PriviosMSGs.LastOrDefault().dateTimeCreation<= remooveTime).ToList();

                foreach (var adminchat in adminChats)
                {
                    new TeleTools().remooveMenu(client,cancellationToken,adminchat);


                    adminchat.processCode = setCodeMenu(processCodeAdmin.awat_start);


                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "/start" },
})
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await client.SendTextMessageAsync(
                        chatId: adminchat.ChatId,
                        text: "Нажмите старт чтобы начать - /start",
                    replyMarkup: replyKeyboardMarkup, disableNotification: true,
                    cancellationToken: cancellationToken);
                   
                    
                    var privMsg = PriviosMSG.createMessage(client.BotId, true, sentMessage, null);
                    adminchat.PriviosMSGs.Add(privMsg);

                }

                db.SaveChanges();



                // var updates = db.myUserUpdates.Where(update => update.dateTimeCreation <= remooveTime.AddHours(2)).ToList();

                //  var updates = db.myUserUpdates.Where(update => update.ChatFrom.PriviosMSGs.Count()!=00&&update.ChatFrom.PriviosMSGs.LastOrDefault().dateTimeCreation <= remooveTime.AddMinutes(5)).ToList();




                //foreach (var update in updates.Where(update=>update.ChatFrom.AllChatUsers.FirstOrDefault().UserType== MyUser.userType.admin))
                //{
                //    if (update.ChatFrom == null) continue;
                //    var processCodeChat = parsmenuType(update?.ChatFrom?.processCode);
                //    if (update.ChatFrom.PriviosMSGs.LastOrDefault().dateTimeCreation > remooveTime) continue;


                //    if (processCodeChat != processCodeAdmin.awat_start)
                //    {
                //         new TeleTools().remooveMenu(client, cancellationToken, update.ChatFrom);


                //    }



                //}

            }
            
        }





        private string setCodeMenu(processCodeAdmin typcode)
        {

            return $"m:{((int)typcode)}|";

        }

        private static processCodeAdmin parsmenuType(string processCode)
        {
            processCodeAdmin reuslt = processCodeAdmin.err;
            if (processCode is null) return reuslt;
            string value = processCode.Split('|').FirstOrDefault(cod => cod.Contains("m")).Split(':')[1];


            if (Enum.TryParse<processCodeAdmin>(value, out reuslt))
            {
                return reuslt;
            }
            return reuslt;


        }



        public enum processCodeAdmin
        {
            err,
            awat_start,
            strstMenu,//- стартовое меню
            adminMenuList,// - настройка администраторов
            
            adminComment1List,// заявки по тематики 1
            adminComment1List_reqstProps,

            strstMenu_listuser_user,
            strstMenu_listuser_user_userProps,
            strstMenu_listuser_user_userProps_Changeusertype,
            loadStartPhoto,
            loadStartPhoto_done,
            loadStart_execl,
            loadStart_try,

        }


    }
}
