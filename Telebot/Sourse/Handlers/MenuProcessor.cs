using System.Net.WebSockets;
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace Telebot.Sourse.Handlers
{
    public class MenuProcessor
    {



        [MenuHandler("StartMenu_OnLoad")]
        public void Handle_StartMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("start - StartMenu_OnLoad");
        }




        [MenuHandler("StartMenu_OnEnd")]
        public void Handle_StartMenu_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("end - StartMenu_OnEnd");
        }



        [MenuHandler("OfferInputMenu_OnLoad")]
        public void Handle_OfferInputMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("OnLoad- OfferInputMenu_OnLoad");
        }


        [MenuHandler("ListMenuAllAdmins_OnLoad")]
        public void Handle_ListMenuAllAdmins_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            var allAdmins = db.MyUsers.Where(p => p.Type.TypeCode == "admin").ToList();

            foreach (var item in allAdmins)
            {
                string usercode = item.GetEntityTypeId();
                curentChat.DinamicButons.Add(new Item.Dinamic_Butons() { CallbackQwery = $"m:{curentChat.CurentProcess.Inputs.FirstOrDefault(p => p.input_Type.Code == "CallbackQueryList").NextProcessMenuId}|{usercode}", Content = $"{item.Username ?? curentChat.ChatId.ToString()}", BotClientId = client.BotId, dateTimeCreation = DateTime.Now, IsDelite = false });
            }

            db.SaveChanges();

        }



        [MenuHandler("AdminPropsMenu_OnLoad")]
        public void Handle_AdminPropsMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int userBDidProps = MyUser.GetUserIdFromUpdate(update);
            var user = db.MyUsers.Where(p => p.MyId == userBDidProps).ToList().FirstOrDefault();

            foreach (var item in curentChat.CurentProcess.Inputs.Where(p => p.input_Type.Code == "CallbackQueryBool").ToList())// тип кнопки
            {
                if (item.MyName == "AdminTrueFalse")// название кнопки
                {
                    var content = item.NameIfTrue;


                    if (user.Type.TypeCode != "admin") content = item.NameIfFalse;



                    curentChat.DinamicButons.Add(new Dinamic_Butons() { MyName = item.MyName, Content = content, CallbackQwery = (item.NextProcessMenu.GetEntityTypeId() + user.GetEntityTypeId()) });

                }


            }



            string message = curentChat.CurentProcess.MyDescription.Replace("{UserName}", user.GetUserLinkInline_Name());


            message = message.Replace("{teleId}", $"{(user.Id.ToString()) ?? "-"}");

            message = message.Replace("{firstName}", $"{(user.FirstName) ?? "-"}");

            message = message.Replace("{LastName}", $"{(user.LastName) ?? "-"}");
            message = message.Replace("{userType}", $"{(user.Type.MyName) ?? "-"}");





            curentChat.CurentTexrMessage = message;

            db.SaveChanges();

        }


        [MenuHandler("ChangeUserTypeInAdminProps_OnLoad")]
        public void Handle_ChangeUserTypeInAdminPropss_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int userBDidProps = MyUser.GetUserIdFromUpdate(update);
            var user = db.MyUsers.Where(p => p.MyId == userBDidProps).ToList().FirstOrDefault();

            if (user.Type.TypeCode == "admin")
            {
                var newType = db.User_Types.FirstOrDefault(t => t.TypeCode == "user");
                user.Type = newType;
            }
            else
            {
                var newType = db.User_Types.FirstOrDefault(t => t.TypeCode == "admin");
                user.Type = newType;
            }

            Console.WriteLine("меняем значение в бд");



            db.SaveChanges();

        }





        [MenuHandler("ListMenuAllUsers_OnLoad")]
        public void Handle_ListMenuAllUsers_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            var allAdmins = db.MyUsers.ToList();

            foreach (var item in allAdmins)
            {
                string usercode = item.GetEntityTypeId();
                curentChat.DinamicButons.Add(new Item.Dinamic_Butons() { CallbackQwery = $"m:{curentChat.CurentProcess.Inputs.FirstOrDefault(p => p.input_Type.Code == "CallbackQueryList").NextProcessMenuId}|{usercode}", Content = $"{item.Username ?? curentChat.ChatId.ToString()}", BotClientId = client.BotId, dateTimeCreation = DateTime.Now, IsDelite = false });
            }

            db.SaveChanges();

        }



        [MenuHandler("UserPropsMenu_OnLoad")]
        public void Handle_UserPropsMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int userBDidProps = MyUser.GetUserIdFromUpdate(update);
            var user = db.MyUsers.Where(p => p.MyId == userBDidProps).ToList().FirstOrDefault();



            foreach (var item in curentChat.CurentProcess?.Inputs.Where(i => i.input_Type.Code == "CallbackQueryBool"))
            {

                if (item.MyName == "AdminTrueFalse_userProps")
                {
                    string content = item.NameIfTrue;
                    if (user.Type.TypeCode != "admin")
                    {
                        content = item.NameIfFalse;
                    }

                    curentChat.DinamicButons.Add(new Dinamic_Butons()
                    {
                        BotClientId = client.BotId,
                        CallbackQwery = item.NextProcessMenu.GetEntityTypeId() + user.GetEntityTypeId(),
                        Content = content,
                        dateTimeCreation = DateTime.Now,
                        IsDelite = false,
                        MyName = item.MyName

                    });
                }


            }



            string message = curentChat.CurentProcess.MyDescription.Replace("{UserName}", user.GetUserLinkInline_Name());


            message = message.Replace("{teleId}", $"{(user.Id.ToString()) ?? "-"}");

            message = message.Replace("{firstName}", $"{(user.FirstName) ?? "-"}");

            message = message.Replace("{LastName}", $"{(user.LastName) ?? "-"}");
            message = message.Replace("{userType}", $"{(user.Type.MyName) ?? "-"}");





            curentChat.CurentTexrMessage = message;

            db.SaveChanges();

        }



        [MenuHandler("ChangeUserTypeInUserProps_OnLoad")]
        public void Handle_ChangeUserTypeInUserProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int userBDidProps = MyUser.GetUserIdFromUpdate(update);
            var user = db.MyUsers.Where(p => p.MyId == userBDidProps).ToList().FirstOrDefault();

            if (user.Type.TypeCode == "admin")
            {
                var newType = db.User_Types.FirstOrDefault(t => t.TypeCode == "user");
                user.Type = newType;
            }
            else
            {
                var newType = db.User_Types.FirstOrDefault(t => t.TypeCode == "admin");
                user.Type = newType;
            }



            db.SaveChanges();

        }



        [MenuHandler("MenuPropsListMenu_OnLoad")]
        public void Handle_MenuPropsListMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            var allMenus = db.Menu_Proceses.ToList();



            var menuBtn = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.input_Type.Code == "CallbackQueryList");

            int i = 0;
            foreach (var item in allMenus)
            {
                i++;
                string context = item.MyName.Replace("{MenuName}", (item.MyName));
                context = $"{item.MyId}) " + context;
                string callBack = menuBtn.NextProcessMenu.GetEntityTypeId() + $"em:{item.MyId}|";
                curentChat.DinamicButons.Add(new Dinamic_Butons()
                {
                    BotClientId = client.BotId,
                    MyName = item.MyName,
                    dateTimeCreation = DateTime.Now,
                    Content = context,
                    CallbackQwery = callBack

                });


            }



            db.SaveChanges();

        }




        [MenuHandler("MenuAndActionProps_OnLoad")]
        public void Handle_MenuAndActionProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int mDbId = new TeleTools().getentyIdByUpdate("em", update);

            var menu = db.Menu_Proceses.FirstOrDefault(m => m.MyId == mDbId);


            string cont = curentChat.CurentProcess.MenuProcessContent;


            curentChat.CurentTexrMessage = new TeleTools().FormateMenuPropsText(curentChat.CurentProcess.MenuProcessContent, menu);




            db.SaveChanges();

        }






        [MenuHandler("InputPropsListMenu_OnLoad")]
        public void Handle_InputPropsListMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            var btnTempleyt = curentChat.CurentProcess.Inputs.FirstOrDefault(inp => inp.NextProcessMenuCode == "InputProps");

            foreach (var item in db.Inputs.ToArray())
            {

                string callback = btnTempleyt.NextProcessMenu.GetEntityTypeId() + item.GetEntityTypeId();
                string btnContent =item.MyId.ToString()+")"+ btnTempleyt.MyName.Replace("{InputName}", item.MyName);
                curentChat.DinamicButons.Add(new Dinamic_Butons()
                {
                    BotClientId = client.BotId,
                    CallbackQwery = callback,
                    Content = btnContent,
                    MyName = item.MyName,
                    dateTimeCreation = DateTime.Now,
                    IsDelite = false,
                    MyDescription = item.MyDescription,
                });
            }



            db.SaveChanges();

        }



        [MenuHandler("InputProps_OnLoad")]
        public void Handle_InputProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);


            int myDbId = Process_Input.GetIdFromUpdate(update);

            var targetUnput = db.Inputs.FirstOrDefault(inp => inp.MyId == myDbId);

            string text = curentChat.CurentProcess.MenuProcessContent;


            curentChat.bsckInformation = targetUnput.GetEntityTypeId();

            text = text.Replace("{InputName}",targetUnput.MyName);
            text = text.Replace("{InputType}", targetUnput.input_Type.Code);
            text = text.Replace("{MenuBelow}", targetUnput.MenuProcess.MyName);
            text = text.Replace("{NextMenu}", targetUnput.NextProcessMenu.MenuProcessContent);



            string btnContent1 = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "Родитель - {MenuName}").MyName.Replace("{MenuName}", targetUnput.MenuProcess.ProcessMenuCode);

            curentChat.DinamicButons.Add(
                new Dinamic_Butons()
                {
                    CallbackQwery = targetUnput.GetEntityTypeId() + $"em:{targetUnput.MenuProcess.MyId}|",
                    BotClientId = client.BotId,
                    Content = btnContent1,
                    MyName = targetUnput.MyName,
                    IsDelite = false,
                    dateTimeCreation = DateTime.Now
                });
            curentChat.DinamicButons.Add(
            new Dinamic_Butons()
            {
                CallbackQwery = targetUnput.GetEntityTypeId() + $"em:{targetUnput.NextProcessMenu.MyId}|",
                BotClientId = client.BotId,
                Content = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "Call - {CallingMenu}").MyName.Replace("{CallingMenu}", targetUnput.NextProcessMenu.ProcessMenuCode),
                MyName = targetUnput.MyName,
                IsDelite = false,
                dateTimeCreation = DateTime.Now
            });


            curentChat.CurentTexrMessage=text;






            db.SaveChanges();

        }

    }
}
