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

        /// <summary>
        /// MyUserUpdates - mph:
        /// </summary>
        /// <returns>"muu:{MyId}|"</returns>
        public string GetEntityTypeId()
        {
            return $"muu:{MyId}|";
        }


        public async static Task<MyUserUpdates> createNewObj(ITelegramBotClient bot, Update update, CancellationToken cts)
        {
            User? curentUser = null;
            Chat? curentChat = null;
            string data = "";
            long? curentMessageId = null;
            MessageType? curentMessageType = null;
            List<myPhoto> photoInfo = new List<myPhoto>();


            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                curentUser = update.Message?.From;
                curentChat = update.Message?.Chat;
                data = update.Message?.Text ?? string.Empty;
                curentMessageId = update.Message.MessageId;
                
                curentMessageType = update.Message.Type;


                if (curentMessageType is MessageType.Photo)
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

            }
            else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                curentUser = update.CallbackQuery?.Message?.From;
                curentChat = update.CallbackQuery?.Message?.Chat;
                data = update.CallbackQuery?.Data ?? string.Empty;
                
                curentMessageId = update.CallbackQuery.Message.MessageId;
                curentMessageType = update.CallbackQuery.Message.Type;

            }
            else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
            {
                curentUser = update.EditedMessage.From;
                curentChat = update.EditedMessage.Chat;
                data = update?.EditedMessage?.Text;
                curentMessageId = update.EditedMessage.MessageId;
                curentMessageType = update.EditedMessage.Type;

            }
            else if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost)
            {
                curentUser = update.ChannelPost?.From;
                curentChat = update.ChannelPost?.Chat;
                data = update.ChannelPost?.Text ?? string.Empty;
                curentMessageId = update.ChannelPost.MessageId;
               
                curentMessageType = update.ChannelPost.Type;

            }




            if (curentChat == null) return null;


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

            result.MyDescription = $"Obj type: {result.GetType().Name}|User name: {curentUser.Username??"-"}|User Id from: {result.FromUserId}|Data crea: {result.dateTimeCreation}|" +
                $"Content: {result.message}|Message Id: {result.messageId}|Chat type: {result.TeleChatType}|" +
                $"Message type: {result.TeleMessageType}|Update type: {result.UpdateType}|Update id: {result.TeleUpdateId}|Photoes: {result.photoInfo.Count()}";




            return result;
        }





        public int? ChatFromId { set; get; }


        public virtual MyChat? ChatFrom { set; get; }



    }
}
