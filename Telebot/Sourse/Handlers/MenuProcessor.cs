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
                string usercode =  item.GetEntityTypeId();
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

            foreach (var item in curentChat.CurentProcess.Inputs.Where(p =>p.input_Type.Code== "CallbackQueryBool").ToList())// тип кнопки
            {
                if (item.MyName == "AdminTrueFalse")// название кнопки
                {
                    var content = item.NameIfTrue;
              

                    if (user.Type.TypeCode!= "admin") content=item.NameIfFalse;

                    

                    curentChat.DinamicButons.Add(new Dinamic_Butons() { MyName= item.MyName,  Content= content ,CallbackQwery=(item.NextProcessMenu.GetEntityTypeId()+user.GetEntityTypeId())});
                
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

    }
}
