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
                curentChat.DinamicButons.Add(new Item.Dinamic_Butons() { CallbackQwery = $"m:{curentChat.CurentProcess.Inputs.FirstOrDefault(p => p.input_Type.Code == "CallbackQueryList").NextProcessMenuId}|{usercode}", Content = $"{item.Username ?? curentChat.ChatId.ToString()}|", BotClientId = client.BotId, dateTimeCreation = DateTime.Now, IsDelite = false });
            }

            db.SaveChanges();

        }



        [MenuHandler("AdminPropsMenu_OnLoad")]
        public void Handle_AdminPropsMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            string message = curentChat.CurentProcess.MyDescription.Replace("{UserName}", curentChat.GetUserInline());
          
            message = message.Replace("{teleId}", $"{(curentChat.AllChatUsers.FirstOrDefault().Id.ToString()) ?? "-"}");
            message = message.Replace("{firsName}", $"{(curentChat.AllChatUsers.FirstOrDefault().FirstName) ?? "-"}");
            message = message.Replace("{userType}", $"{(curentChat.AllChatUsers.FirstOrDefault().Type.MyName) ?? "-"}");



            curentChat.CurentTexrMessage = message;

            db.SaveChanges();

        }




    }
}
