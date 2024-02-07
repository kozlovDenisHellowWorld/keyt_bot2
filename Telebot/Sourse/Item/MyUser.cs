using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;


namespace Telebot.Sourse.Item
{
    public class MyUser : User, IItem.IItemDB<MyUser>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }

        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { set; get; }

        public long? BotClientId { set; get; }

        public bool? IsDelite { set; get; }

        public userType? UserType { set; get; }



        [Column(TypeName = "nchar(100)")]
        public string? Phone { get; set; }

        [Column(TypeName = "nchar(100)")]
        public string? RealName { get; set; }


        public virtual List<requst> Requsts { set; get; } = new List<requst>();

        virtual public List<MyChat> Chats { set; get; } = new List<MyChat>();



        public MyUser()
        { }



        public async static Task< MyUser> newUserObject(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            User? curentUser = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentUser = update.Message.From;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentUser = update.ChannelPost.From;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentUser = update.CallbackQuery.Message.From;

            if (curentUser == null) return null;
            var user = new MyUser();
            user.Id = curentUser.Id;
            user.FirstName=curentUser.FirstName;
            user.LastName=curentUser.LastName;
            user.dateTimeCreation = DateTime.Now;
            user.Username=curentUser.Username;
            user.UserType=userType.regulareUser;
            user.IsBot=curentUser.IsBot;
            user.IsDelite = false;
            user.AddedToAttachmentMenu=curentUser.AddedToAttachmentMenu;
            user.BotClientId = bot.BotId;
            user.CanJoinGroups=curentUser.CanJoinGroups;
            user.CanReadAllGroupMessages=curentUser.CanReadAllGroupMessages;
            

            if (curentUser.Id== 469825678) user.UserType = userType.admin;

            user.MyDescription = $"Obj type: {user.GetType().Name}|teleId:{user.Id}|User name: {user.Username ?? "-"}|First name: {user.FirstName ?? "-"}|" +
                $"Last name: {user.LastName ?? "-"}|Type: {user.UserType}|isDelite: {user.IsDelite}";

            return user; 
            
           
        }






        public static userType? userTypeById(long teleUserId)
        {
            userType? result = userType.errr;

            using (var bd=new context())
            {
                result = bd.MyUsers.FirstOrDefault(us => us.Id == teleUserId).UserType;
            }

            return result;
        }





        public string SetUserIncallBack()
        {
            return $"u:{MyId}|"; 
        
        }

        public static int GetUserById(string processCode)
        {
            if(!processCode.Contains("u:")) return 0;

            string value = processCode.Split('|').FirstOrDefault(cod => cod.Contains("u")).Split(':')[1];
          
            int result = 0;

            int.TryParse(value, out  result);

            return result;

        }





        public enum userType
        {
            admin,
            regulareUser,
            manager,
           errr

        }

      
    }
}
