﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;
using Telebot.Sourse.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace Telebot.Sourse.Item.IItem
{  
    public class MyChat : IItem.IItemDB<MyChat>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { set; get; }
        public long? BotClientId { set; get; }
        public bool? IsDelite { set; get; }




        public long ChatId { set; get; }

        /// <summary>
        /// Возможно ненужн, пробую сделать обновление
        /// </summary>
        public bool? updateMenu { set; get; } = false;

        public Telegram.Bot.Types.Enums.ChatType TeleChatType { set; get; }

      
        public string? processCode { set; get; }
        public string? processCode2 { set; get; }


        public virtual List<MyUser>? AllChatUsers { get; set; } = new List<MyUser>();

        public virtual List<PriviosMSG>? PriviosMSGs { get; set; }

        public virtual List<MyUserUpdates> Update { set; get; }=new List<MyUserUpdates>();


        public virtual List<requst> Requsts { set; get; } = new List<requst>();


        public int? CurentProcessId { set; get; }

        virtual public Menu_Process? CurentProcess { set; get; }

        /// <summary>
        /// Для текста который надо отправить 
        /// </summary>
        public  string? CurentTexrMessage { set; get; } 

        /// <summary>
        /// кноки для списков 
        /// </summary>
        public virtual List<Dinamic_Butons> DinamicButons { set; get; } = new List<Dinamic_Butons>();


        public virtual List<Log> Logs { set; get; } = new List<Log>();



        public void AddLog(Telegram.Bot.Types.Update update, context db)
        {

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                if (Logs.Count() > 50)
                {
                    var firsLog = Logs.FirstOrDefault();

                    if (firsLog != null)
                    { 
                        Logs.Remove(firsLog);
                    }
                  
                }

                Logs.Add(new Log()
                {
                    BotClientId = BotClientId,
                    dateTimeCreation = DateTime.Now,
                    IsDelite = false,
                    Callback = update.CallbackQuery.Data,
                    TeleMessageId = update.CallbackQuery.Message.MessageId,

                });
            }

            db.SaveChanges();
        }


        public string? bsckInformation { set; get; }

        /// <summary>
        /// MyChat - mc:
        /// </summary>
        /// <returns>"mc:{MyId}|"</returns>
        public string GetEntityTypeId()
        {
            return $"mc:{MyId}|";
        }
        public void SetProcess(Menu_Process nextProcess)
        {


            DinamicButons.Clear();
            CurentTexrMessage = null;
            bsckInformation = "";

            if (CurentProcess == null)
            {

              


                CurentProcess = nextProcess;

                if (processCode2 == "" || processCode2 is null)
                {
                    processCode2 = $"m:{nextProcess.MyId}|";
                }
                else if (processCode2.Contains("m:"))
                {
                    processCode2 = Regex.Replace(processCode2, @"m:\d+", $"m:{nextProcess.MyId}");
                }
                else
                {
                    processCode2 += $"m:{nextProcess.MyId}|";
                }
            }
            else {
             
                CurentProcess = nextProcess;

            }



        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает строку сосылкой на 1 пользователя этого чата чеез id</returns>
        public string GetUserLinkInline()
        {
           return  $"<a href=\"tg://user?id={(AllChatUsers.FirstOrDefault().Id.ToString()) ?? "-"}\">{AllChatUsers.FirstOrDefault().Username ?? "Пользователь"}</a>";
        }









        public static async Task <MyChat> newChatObject(ITelegramBotClient bot, Telegram.Bot.Types.Update update, CancellationToken cts)
        {
            Chat? curentChat = null;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.Message) curentChat = update.Message.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.ChannelPost) curentChat = update.ChannelPost.Chat;
            if (update.Type is Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) curentChat = update.CallbackQuery.Message.Chat;


            if (curentChat is null) return null;
           
            var loadChat = await bot.GetChatAsync(curentChat.Id,cts);

            var result = new MyChat();
            result.dateTimeCreation = DateTime.Now;
            result.ChatId = curentChat.Id;
            result.BotClientId = bot.BotId;
            result.MyName = "Пользовательский чат";
            result.IsDelite = false;
            result.TeleChatType = curentChat.Type;
            result.MyDescription = $"Obj Type: {result.GetType().Name}|ChatType: {result.TeleChatType}|BotId: {bot.BotId}|Chat id: {curentChat.Id}|DataCreation: {result.dateTimeCreation}";
          

            return result;
        }




    }
}
