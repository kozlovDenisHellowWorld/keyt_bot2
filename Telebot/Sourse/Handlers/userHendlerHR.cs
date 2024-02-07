using OpenAI.Chat;
using Telebot.Sourse.Item;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Message = Telegram.Bot.Types.Message;

namespace Telebot.Sourse.Handlers
{
    public class userHendlerHR
    {


         string Cont_btn_back = "⬅️Назад";


        public  async Task regularUserHendler(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId)
        {



            try
            {
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    var curentMenu = parsmenuType(update.CallbackQuery.Data);

                    if (curentMenu == processCodeAdmin.strstMenu) { sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                    // первая тема
                    if (curentMenu == processCodeAdmin.request1_await) { await sendMenuRequest1_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request1_ok) { await sendMenuRequest1_PublishRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request1_delite) { await sendMenuRequest1_DeliteRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request1_chenge) { await sendMenuRequest1_Chage_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                    // вторая тема
                    if (curentMenu == processCodeAdmin.request2_await) { await sendMenuRequest2_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request2_ok) { await sendMenuRequest2_PublishRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request2_delite) { await sendMenuRequest2_DeliteRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request2_chenge) { await sendMenuRequest2_Chage_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                    // третяя тема 
                    if (curentMenu == processCodeAdmin.request3_await) { await sendMenuRequest3_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request3_ok) { await sendMenuRequest3_PublishRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request3_delite) { await sendMenuRequest3_DeliteRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request3_chenge) { await sendMenuRequest3_Chage_awayt(update, client, cancellationToken, teleChatId, teleUserId, null); return; }
                    if (curentMenu == processCodeAdmin.request3_photo) { await sendMenuRequest3_awaytPhoto(update, client, cancellationToken, teleChatId, teleUserId, null, processCodeAdmin.request3_photo); return; }

                    if (curentMenu == processCodeAdmin.request3_photo_end) { await sendMenuRequest3_getRequest(update, client, cancellationToken, teleChatId, teleUserId, null); return; }


                }
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {

                    if (update.Message.Text == "/start"|| update.Message.Text == "Позвать") { sendStartMeenuexsampel(update, client, cancellationToken, teleChatId, teleUserId, null); return; }

                    var curentMenu = processCodeAdmin.err;

                    using (var db = new context())
                    {
                        var curentChat = db.myChats.FirstOrDefault(c => c.ChatId == update.Message.Chat.Id);
                        if (curentChat != null) curentMenu = parsmenuType(curentChat.processCode);
                    }


                    //--- первая тема
                    if (curentMenu == processCodeAdmin.request1_await) { await sendMenuRequest1_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }
                    if (curentMenu == processCodeAdmin.request1_chenge) { await sendMenuRequest1_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }


                    //-----вторая тема

                    if (curentMenu == processCodeAdmin.request2_await) { await sendMenuRequest2_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }
                    if (curentMenu == processCodeAdmin.request2_chenge) { await sendMenuRequest2_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }

                    //---------Третяя тема
                    if (curentMenu == processCodeAdmin.request3_await) { await sendMenuRequest3_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }
                    if (curentMenu == processCodeAdmin.request3_chenge) { await sendMenuRequest3_getRequest(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }
                    if (curentMenu == processCodeAdmin.request3_photo) { await sendMenuRequest3_awaytPhoto(update, client, cancellationToken, teleChatId, teleUserId, null, processCodeAdmin.request3_photo); return; }


                    if (curentMenu == processCodeAdmin.err) { sendStartErrl(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; }


                }


            }
            catch 
            {

                    sendStartErrl(update, client, cancellationToken, teleChatId, teleUserId, update.Message.Text); return; 

            }



        }


        //cnfhnjdjt vty.
        public  async Task sendStartMeenuexsampel(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);

                var deqNeedToDelite = myChat.Requsts.Where(r => r.isCreated == false).ToList();
                if (deqNeedToDelite is not null && deqNeedToDelite.Count() != 0)
                {
                    foreach (var item in deqNeedToDelite)
                    {

                        Message message1 =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, "🤖: Ваше обращение удалено 🗑", null);
                        var privMsg1 = PriviosMSG.createMessage(myChat.BotClientId, true, message1, update);

                        myChat.PriviosMSGs.Add(privMsg1);

                        item.user = null;
                        item.chat = null;
                        item.Photoes.RemoveAll(p=>p.Reqst!=null);

                        db.Requst.Update(item);
                        db.Requst.Remove(item);

                        db.SaveChanges();
                    }

                }



                //Уведомления о новых сообщений 
                int iReq = requst.GetUserIdFromCode(myChat.processCode);

                if (iReq != 0)
                {
                    var newReqForUser = myChat.Requsts.Where(r => r.isNewForUser == true).ToList();

                    if (newReqForUser is not null && newReqForUser.Count() != 0)
                    {
                        foreach (var item in newReqForUser)
                        {
                            item.isNewForUser = false;
                        }
                        Message message1 = null;
                        if (newReqForUser.Last().reqstTupe.Contains("Предложение")) message1 =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, $"🤖: Спасибо за ваше предложение. Мы обязательно рассмотрим его и сообщим вам о результатах. Если у вас есть еще какие-то идеи или пожелания, не стесняйтесь сообщать нам. Мы всегда рады обратной связи от наших пользователей. HR отдел InfoTeCS.", null);
                        if (newReqForUser.Last().reqstTupe.Contains("Задать вопрос")) message1 =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, $"🤖: Спасибо за ваш вопрос. Чем я могу вам ещё помочь? Если у вас есть какие-то дополнительные вопросы или проблемы, не стесняйтесь обращаться к нам. Мы всегда готовы помочь нашим сотрудникам. HR отдел InfoTeCS.", null);
                        if (newReqForUser.Last().reqstTupe.Contains("Ошибка в материалах")) message1 =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, $"🤖: Благодарим вас за обращение. Мы очень ценим вашу обратную связь и будем рады исправить ошибки в наших материалах. Спасибо за вашу помощь в улучшении нашего сервиса! HR отдел InfoTeCS.", null);
                        
                        Message message2 =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, $"🤖: Вот данные по запросу.\n🆔: <b>{newReqForUser.Last().MyId}</b>\nТип: <b>{newReqForUser.Last().reqstTupe}</b>", null);


                        var privMsg1 = PriviosMSG.createMessage(myChat.BotClientId, true, message1, update);
                        var privMsg2 = PriviosMSG.createMessage(myChat.BotClientId, false, message2, update);

                        if (message1 is not null)  myChat.PriviosMSGs.Add(privMsg1);

                        myChat.PriviosMSGs.Add(privMsg2);

                        db.SaveChanges();
                    }
                }



                using (null)
                {// Стартовое баннер

                    var startPhotoes = db.myPhotoes.Where(p => p.IsStartMSGPhoto == true).ToList();
                    var startPhoto = startPhotoes.LastOrDefault();
                    if (startPhoto is not null)
                    {
                        InputFileId photo = new InputFileId(startPhoto.FileId);
                        var pf = InputFile.FromFileId(startPhoto.FileId);
                                               

                        Message messages;
                        messages = await client.SendPhotoAsync(myChat.ChatId,pf,caption: "🤖 : <b>Привет!</b>\nЯ  –  бот HR отдела ИнфоТеКС\n У тебя есть вопросы или предложения к HR?", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                       
                            var privMsg1 = PriviosMSG.createMessage(myChat.BotClientId, true, messages, update);

                            myChat.PriviosMSGs.Add(privMsg1);

                            db.SaveChanges();


                        
                    }




                }




                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu);


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "💡 Предложение", callBackCode = setCodeMenu(processCodeAdmin.request1_await) });
                buttons.Add(new TeleTools.Buttons { content = "❓ Задать вопрос", callBackCode = setCodeMenu(processCodeAdmin.request2_await) });
                buttons.Add(new TeleTools.Buttons { content = "🖍 Ошибка в материалах", callBackCode = setCodeMenu(processCodeAdmin.request3_await) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖 : Чем я могу помочь? ⬇️", null);

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


        //errr важно поправить в уонцк
        public  async Task sendStartErrl(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                if (myChat is null) return;

                var deqNeedToDelite = myChat.Requsts.Where(r => r.isCreated == false).ToList();
                if (deqNeedToDelite is not null || deqNeedToDelite.Count() != 0)
                {
                    foreach (var item in deqNeedToDelite)
                    {
                        item.user = null;
                        item.chat = null;

                        db.Requst.Update(item);
                        db.Requst.Remove(item);

                        db.SaveChanges();
                    }

                }
                using (null)
                {// Стартовое сообщение

                    var startPhoto = db.myPhotoes.FirstOrDefault(p => p.IsStartMSGPhoto == true);

                    if (startPhoto is not null)
                    {
                        InputFileId photo = new InputFileId(startPhoto.FileId);
                        var pf = InputFile.FromFileId(startPhoto.FileId);


                        Message messages;
                        messages = await client.SendPhotoAsync(myChat.ChatId, pf, caption: "🤖 : <b>Привет!</b>\nЯ  –  бот HR отдела ИнфоТеКС", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);


                        var privMsg1 = PriviosMSG.createMessage(myChat.BotClientId, true, messages, update);

                        myChat.PriviosMSGs.Add(privMsg1);

                        db.SaveChanges();



                    }




                }


                Message messageErr =  await new TeleTools().SendStaticMSG(myChat, client, cancellationToken, "⚠️ Что то пошло не так.\nПопробуйте еще раз.", null);


                string curetnProcessCode = setCodeMenu(processCodeAdmin.strstMenu);


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "💡 Предложение", callBackCode = setCodeMenu(processCodeAdmin.request1_await) });
                buttons.Add(new TeleTools.Buttons { content = "❓ Задать вопрос", callBackCode = setCodeMenu(processCodeAdmin.request2_await) });
                buttons.Add(new TeleTools.Buttons { content = "🖍 Ошибка в материалах", callBackCode = setCodeMenu(processCodeAdmin.request3_await) });



                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖 : Чем я могу помочь? ⬇️", null);

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


        //-----------------------

        // первый запрос - тематика


        public  async Task sendMenuRequest1_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                var newRqust = requst.createRequst();
                myChat.Requsts.Add(newRqust);
                myChat.AllChatUsers.LastOrDefault().Requsts.Add(newRqust);

                db.myChats.Update(myChat);
                db.SaveChanges();




                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request1_await) + newRqust.SetUserIncallBack();


                //формирование кнопорей

                
                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });



                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖:  💡 Опишите ваше предложение ниже.", null);

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


        public  async Task sendMenuRequest1_getRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);

                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                


                string curetnProcessCode = setCodeMenu(processCodeAdmin.request1_get) + reqest.SetUserIncallBack();

                reqest.reqstTupe = " Предложение";
                reqest.reqstContent = update?.Message?.Text;
                db.Requst.Update(reqest);
                db.SaveChanges();

                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "⬅️ ✏️ Вернуться и переписать", callBackCode = setCodeMenu(processCodeAdmin.request1_chenge) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });

                buttons.Add(new TeleTools.Buttons { content = "📥  Отправить ➡️", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Ваше предложение 💡:\n\"{reqest.reqstContent}\"\n\nХотите отправить ваше предложение?", null);

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



        public  async Task sendMenuRequest1_PublishRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);


                var req = myChat.Requsts.LastOrDefault(p => p.isCreated == false);
                req.isCreated = true;


                db.Requst.Update(req);
                db.SaveChanges();

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request1_get) + req.SetUserIncallBack();


                ////формирование кнопорей

                //List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                ////buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //buttons.Add(new TeleTools.Buttons { content = "Опубликовать", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });
                //buttons.Add(new TeleTools.Buttons { content = "Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });
                //buttons.Add(new TeleTools.Buttons { content = "Редактировать", callBackCode = setCodeMenu(processCodeAdmin.request1_chenge) });


                //---

                //---



                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, buttons, "🤖- Введите текст тескт заявки (тип 1). 📝", null);
                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, $"🤖- Ваш запрос опубликован. Номер запроса №{1}", null) ;

                //var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                //myChat.PriviosMSGs.Add(privMsg);








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





        public  async Task sendMenuRequest1_DeliteRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);




                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request1_get);

                ////формирование кнопорей

                //List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                ////buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //buttons.Add(new TeleTools.Buttons { content = "Опубликовать", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });
                //buttons.Add(new TeleTools.Buttons { content = "Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });
                //buttons.Add(new TeleTools.Buttons { content = "Редактировать", callBackCode = setCodeMenu(processCodeAdmin.request1_chenge) });


                //---

                //---



                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, buttons, "🤖- Введите текст тескт заявки (тип 1). 📝", null);
                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, $"🤖- Ваш запрос опубликован. Номер запроса №{1}", null) ;

                //var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                //myChat.PriviosMSGs.Add(privMsg);








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


        public  async Task sendMenuRequest1_Chage_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);



                var reqst = myChat.Requsts.LastOrDefault(p => p.isCreated == false);


                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request1_chenge) + reqst.SetUserIncallBack();


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //  buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });
                buttons.Add(new TeleTools.Buttons { content = "📥 И так сойдет", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Хотите переписать?\n\"{reqst.reqstContent} \"\n💡 Введите заного ваше предложение ниже.", null);

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



        //--------------------- второй ------------------------------




        public  async Task sendMenuRequest2_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                var newRqust = requst.createRequst();
                myChat.Requsts.Add(newRqust);
                myChat.AllChatUsers.LastOrDefault().Requsts.Add(newRqust);

                db.myChats.Update(myChat);
                db.SaveChanges();




                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request2_await) + newRqust.SetUserIncallBack();


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });



                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖: ❓ Опишите ваш вопрос?", null);

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


        public  async Task sendMenuRequest2_getRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);

                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request2_get) + reqest.SetUserIncallBack();

                reqest.reqstTupe = "Задать вопрос";
                reqest.reqstContent = update?.Message?.Text;
                db.Requst.Update(reqest);
                db.SaveChanges();

                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = "⬅️ ✏️ Вернуться и переписать", callBackCode = setCodeMenu(processCodeAdmin.request2_chenge) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request2_delite) });

                buttons.Add(new TeleTools.Buttons { content = "📥 Отправить ➡️", callBackCode = setCodeMenu(processCodeAdmin.request2_ok) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Ваш вопрос ❓:\n<i>\"{reqest.reqstContent}</i>\"\n\n<b>Отправте ваш вопрос?</b>", null);

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



        public  async Task sendMenuRequest2_PublishRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);


                var req = myChat.Requsts.LastOrDefault(p => p.isCreated == false);
                req.isCreated = true;


                db.Requst.Update(req);
                db.SaveChanges();

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request2_get) + req.SetUserIncallBack();


                ////формирование кнопорей

                //List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                ////buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //buttons.Add(new TeleTools.Buttons { content = "Опубликовать", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });
                //buttons.Add(new TeleTools.Buttons { content = "Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });
                //buttons.Add(new TeleTools.Buttons { content = "Редактировать", callBackCode = setCodeMenu(processCodeAdmin.request1_chenge) });


                //---

                //---



                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, buttons, "🤖- Введите текст тескт заявки (тип 1). 📝", null);
                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, $"🤖- Ваш запрос опубликован. Номер запроса №{1}", null) ;

                //var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                //myChat.PriviosMSGs.Add(privMsg);








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





        public  async Task sendMenuRequest2_DeliteRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);




                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request2_get);

                ////формирование кнопорей

                //List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();


                ////buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //buttons.Add(new TeleTools.Buttons { content = "Опубликовать", callBackCode = setCodeMenu(processCodeAdmin.request1_ok) });
                //buttons.Add(new TeleTools.Buttons { content = "Забыть", callBackCode = setCodeMenu(processCodeAdmin.request1_delite) });
                //buttons.Add(new TeleTools.Buttons { content = "Редактировать", callBackCode = setCodeMenu(processCodeAdmin.request1_chenge) });


                //---

                //---



                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, buttons, "🤖- Введите текст тескт заявки (тип 1). 📝", null);
                //Message message =  await new TeleTools().SendMSG(myChat, client, cancellationToken, $"🤖- Ваш запрос опубликован. Номер запроса №{1}", null) ;

                //var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                //myChat.PriviosMSGs.Add(privMsg);








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


        public  async Task sendMenuRequest2_Chage_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);



                var reqst = myChat.Requsts.LastOrDefault(p => p.isCreated == false);


                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request2_chenge) + reqst.SetUserIncallBack();


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //  buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });
                buttons.Add(new TeleTools.Buttons { content = "📥И так сойдет", callBackCode = setCodeMenu(processCodeAdmin.request2_ok) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request2_delite) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Хотите поменять?\n\"{reqst.reqstContent} \"\nВведите заного📝:", null);

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

        //--------------------- второй ------------------------------




        public  async Task sendMenuRequest3_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);


                var newRqust = requst.createRequst();
                myChat.Requsts.Add(newRqust);
                myChat.AllChatUsers.LastOrDefault().Requsts.Add(newRqust);

                db.myChats.Update(myChat);
                db.SaveChanges();




                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request3_await) + newRqust.SetUserIncallBack();


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });



                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, "🤖: 🖍 Опишите ошибку ниже.", null);

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


        public  async Task sendMenuRequest3_getRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);

                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request3_get) + reqest.SetUserIncallBack();

                reqest.reqstTupe = "Ошибка в материалах";
                if (update?.Message?.Text != null&&update?.Message?.Text != "")
                {
                    reqest.reqstContent = update?.Message?.Text;
                    db.Requst.Update(reqest);
                    db.SaveChanges();
                }
                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
              
              buttons.Add(new TeleTools.Buttons { content = "➕📸 Добавить фото", callBackCode = setCodeMenu(processCodeAdmin.request3_photo) });
                buttons.Add(new TeleTools.Buttons { content = "⬅️ ✏️ Вернуться и переписать", callBackCode = setCodeMenu(processCodeAdmin.request3_chenge) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request3_delite) });

                buttons.Add(new TeleTools.Buttons { content = "📥 Отправить ➡️", callBackCode = setCodeMenu(processCodeAdmin.request3_ok) });

                string isConteinsPfoto = "";

                if (reqest.Photoes.Count() > 0) 
                {
                    isConteinsPfoto = $"\nФото:{reqest.Photoes.Count()}";
                }

                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Описание ошибки:\n<i>\"{reqest.reqstContent}\"{isConteinsPfoto}</i>\n\n<b>Отправить ?</b>", null);

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



        public  async Task sendMenuRequest3_PublishRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);


                var req = myChat.Requsts.LastOrDefault(p => p.isCreated == false);
                req.isCreated = true;


                db.Requst.Update(req);
                db.SaveChanges();

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request3_get) + req.SetUserIncallBack();


                ////формирование кнопорей





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





        public  async Task sendMenuRequest3_DeliteRequest(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);

                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);




                var reqest = myChat.Requsts.LastOrDefault(r => r.isCreated == false);

                string curetnProcessCode = setCodeMenu(processCodeAdmin.request3_get);










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


        public  async Task sendMenuRequest3_Chage_awayt(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns)
        {
            using (var db = new context())
            {



                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);



                var reqst = myChat.Requsts.LastOrDefault(p => p.isCreated == false);


                // удаление старого меню
                 await new TeleTools().remooveMenu(client, cancellationToken, myChat);






                string curetnProcessCode = setCodeMenu(processCodeAdmin.request3_chenge) + reqst.SetUserIncallBack();


                //формирование кнопорей

                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                //  buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.strstMenu) });
                buttons.Add(new TeleTools.Buttons { content = "📥И так сойдет", callBackCode = setCodeMenu(processCodeAdmin.request3_ok) });
                buttons.Add(new TeleTools.Buttons { content = "🗑 Забыть", callBackCode = setCodeMenu(processCodeAdmin.request3_delite) });


                Message message =  await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: Хотите поменять?\n\"{reqst.reqstContent} \"\nВведите заного📝:", null);

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


        //фото
        public  async Task sendMenuRequest3_awaytPhoto(Update update, ITelegramBotClient client, CancellationToken cancellationToken, long teleChatId, long teleUserId, string filterInstrns,processCodeAdmin CuretnProcess)
        {
            using (var db = new context())
            {


               //await Task.Delay();
                var myChat = db.myChats.FirstOrDefault(ch => ch.ChatId == teleChatId);



                // удаление старого меню
                 new TeleTools().remooveMenu(client, cancellationToken, myChat);



                // формирование кнопорей


                List<TeleTools.Buttons> buttons = new List<TeleTools.Buttons>();

                //buttons.Add(new TeleTools.Buttons { content = "Администраторы", callBackCode = setCodeMenu(processCodeAdmin.adminMenuList) });
                buttons.Add(new TeleTools.Buttons { content = Cont_btn_back, callBackCode = setCodeMenu(processCodeAdmin.request3_photo_end) });
              //  await Task.Delay(new Random().Next(100,1000));
                //await Task.Delay(new Random().Next(100, 1000));
                Message message = await new TeleTools().SendStaticMenu(myChat, client, cancellationToken, buttons, $"🤖: 🖍 📸  Загрузите фото.\nКогда закончите - \"{Cont_btn_back}\"", null);
                
                var privMsg = PriviosMSG.createMessage(myChat.BotClientId, true, message, update);

                myChat.PriviosMSGs.Add(privMsg);
                db.myChats.Update(myChat);
                db.SaveChanges();
              
              //  await Task.Delay(new Random().Next(300));

                var req = myChat.Requsts.LastOrDefault(r => r.isCreated == false);



                myPhoto photoInMsg = null;
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message.Photo is not null)
                {
                    
                    photoInMsg = db.myPhotoes.Where(p => p.FileId == update.Message.Photo.Last().FileId).ToList().Last();


                    req.Photoes.Add(photoInMsg);
                    db.Requst.Update(req);
                    db.SaveChanges();
                }




               

                string curetnProcessCode = setCodeMenu(CuretnProcess) + req?.SetUserIncallBack();











                //конец обработки и изменение статуса 

              //  await new TeleTools().remooveMenu(client, cancellationToken, myChat);
                //var remuving = db.priviosMSG.Where(msg => msg.ChatId == null);

                //if (remuving is not null || remuving.Count() != 0)
                //{
                //    db.priviosMSG.RemoveRange(remuving);
                //}

                myChat.processCode = curetnProcessCode;

                db.myChats.Update(myChat);
                db.SaveChanges();

            }


        }



        //_____________________________________________________________________________________________________________



        public async Task start_new_sasion(DateTime remooveTime, ITelegramBotClient client, CancellationToken cancellationToken)
        {
            using (var db = new context())
            {

                var adminChats = db.myChats.Where(chats => chats.PriviosMSGs.Count() != 0).ToList().Where(c => c.AllChatUsers.LastOrDefault().UserType == MyUser.userType.regulareUser && parsmenuType(c.processCode) != processCodeAdmin.awat_start).ToList().Where(c => c.PriviosMSGs.LastOrDefault().dateTimeCreation <= remooveTime).ToList();

                foreach (var adminchat in adminChats)
                {
                    new TeleTools().remooveMenu(client, cancellationToken, adminchat);


                    adminchat.processCode = setCodeMenu(processCodeAdmin.awat_start);


                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Позвать" },
})
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await client.SendTextMessageAsync(
                        chatId: adminchat.ChatId,
                        text: "🤖: Хотите сново начать общаться со мной, позовите меня. ⬇️",
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









        public async Task remooveAllAndSend()
        {
            using (var db= new context())
            {

            }
            
        }






        private  string setCodeMenu(processCodeAdmin typcode)
        {

            return $"m:{((int)typcode)}|";

        }

        private  processCodeAdmin parsmenuType(string processCode)
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
            request1_await,// заявки по тематики 1
            request1_get,
            request1_delite,
            request1_ok,
            request1_chenge,
            request2_await,// заявки по тематики 2
            request2_get,
            request2_delite,
            request2_ok,
            request2_chenge,
            request3_await,// заявки по тематики 3
            request3_get,
            request3_delite,
            request3_ok,
            request3_chenge,
            request3_photo,
            request3_photo_end,
        }



    }
}
