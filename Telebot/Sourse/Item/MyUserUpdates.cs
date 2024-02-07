using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot.Sourse.Item
{
    public class MyUserUpdates : IItem.IItemDB<MyUserUpdates>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }


        public long? TeleUpdateId { set; get; }

        public UpdateType? UpdateType { get; set; }


        public string? message { set; get; }
        public long? messageId { set; get; }

        public virtual List<myPhoto>? photoInfo { set; get; } = new List<myPhoto>();


        public long? FromUserId { set; get; }

        public MessageType? TeleMessageType { set; get; }

        public ChatType? TeleChatType { get; set; }




        public async static Task<MyUserUpdates> createNewObj(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            User? curentUser = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentUser = update.Message.From;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentUser = update.ChannelPost.From;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentUser = update.CallbackQuery.Message.From;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage) curentUser = update.EditedMessage.From;

            Chat? curentChat = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentChat = update.Message.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentChat = update.ChannelPost.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentChat = update.CallbackQuery.Message.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage) curentChat = update.EditedMessage.Chat;

            string data = "";

            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) data = update.Message.Text;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) data = update.ChannelPost.Text;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) data = update.CallbackQuery.Data;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage) data = update?.EditedMessage?.EditDate?.ToString();

            if (curentChat == null) return null;

            long? curentMessageId = null;

            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentMessageId = update.Message.MessageId;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentMessageId = update.ChannelPost.MessageId;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentMessageId = update.CallbackQuery.Message.MessageId;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage) curentMessageId = update.EditedMessage.MessageId;


            MessageType? curentMessageType = null;

            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentMessageType = update.Message.Type;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentMessageType = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentMessageType = update.CallbackQuery.Message.Type;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage) curentMessageType = update.EditedMessage.Type;

            List<myPhoto> photoInfo = new List<myPhoto>();

            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message && curentMessageType is MessageType.Photo)
            {
                //  photoInfo 
                //photoInfo = $"Hight:{update.Message.Photo[3].Height}|Width:{update.Message.Photo[3].Width}|FileId:{update.Message.Photo[3].FileId}|FileUniqueId:{update.Message.Photo[3].FileUniqueId}|FileSize:{update.Message.Photo[3].FileSize}|";

                var photo = update.Message.Photo.Last();
                
                    var potoItem = new myPhoto()
                    {
                        BotClientId = bot.BotId,
                        dateTimeCreation = DateTime.Now,
                        FileId = photo.FileId,
                        FileSize = photo.FileSize,
                        FileUniqueId = photo.FileUniqueId,
                        Height = photo.Height,
                        IsDelite = false,
                        MyName = $"Photo",
                        Width = photo.Width,
                    };

                    potoItem.MyDescription = $"Obj type: {potoItem.GetType().Name}|Date cr.: {potoItem.dateTimeCreation}|" +
                        $"Tele file Id: {potoItem.FileId}|File size: {potoItem.FileSize}|File un Id:{potoItem.FileUniqueId}|hight: {potoItem.Height}|width: {potoItem.Width}";
                    photoInfo.Add(potoItem);

                

            }


            var result = new MyUserUpdates()
            {
                BotClientId = bot.BotId,
                FromUserId = curentUser?.Id,
                dateTimeCreation = DateTime.Now,
                IsDelite = false,
                message = data,
                messageId = curentMessageId,
                MyName = "update",
                TeleChatType = curentChat.Type,
                TeleMessageType = curentMessageType,
                TeleUpdateId = update.Id,
                UpdateType = update.Type,
                photoInfo = photoInfo,
            };

            result.MyDescription = $"Obj type: {result.GetType().Name}|User Id from: {result.FromUserId}|Data crea: {result.dateTimeCreation}|" +
                $"Content: {result.message}|Message Id: {result.messageId}|Chat type: {result.TeleChatType}|" +
                $"Message type: {result.TeleMessageType}|Update type: {result.UpdateType}|Update id: {result.TeleUpdateId}|Photoes: {result.photoInfo.Count()}";




            return result;
        }





        public int? ChatFromId { set; get; }


        public virtual MyChat? ChatFrom { set; get; }



    }
}
