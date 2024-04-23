using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Telebot.Sourse.Handlers;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class Menu_Process : IItemDB<Menu_Process>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }



        public string? ProcessMenuCode { get; set; }

        public string? Navigation { get; set; }

        public string? MenuProcessContent { get; set; }

        public bool? IsAwaytingText { get; set; }

        public bool? NeedToDelite { get; set; }

        MethodInfo? OnLoadHadler { set; get; }


        MethodInfo? OnEndHadler { set; get; }




        public Menu_Process()
        {

            if (ProcessMenuCode != null && ProcessMenuCode != "" && ProcessMenuCode != string.Empty)
            {

            }
            else
            {
                OnLoadHadler = null;
                OnEndHadler = null;
            }



        }




        public void ExecuteOnLoad(Update update, ITelegramBotClient client, MyChat curentChat,context db, CancellationToken cancellationToken)
        {
            if (this.ProcessType.Code != "EditMenu")
            {

                var dinamibuttons = curentChat.DinamicButons.ToList();
                //curentChat.CurentTexrMessage = "";
                try
                {
                    db.dinamic_Butons.RemoveRange(db.dinamic_Butons.Where(b => b.ChatId == null));
                }
                catch
                {


                }

                curentChat.DinamicButons.Clear();
                db.SaveChanges();
            }
            //поиск метода

            var methods = typeof(MenuProcessor).GetMethods().Where(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnLoad")) == true);

            if (methods.Count() > 1)
            {
                
                // так если у нас  есть метод который есть у разных видов пользователя (startMenu) то мы должны вырать с учетом доступа для этого я придумал селдуеющее  сто после  _OnLoad еще добавляем тип пользоателя _amin 
                //но это в том случае если у нас уже есть несколько методов и мы выбираем, а так все по старом переходим в else
                OnLoadHadler = typeof(MenuProcessor).GetMethods().FirstOrDefault(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnLoad" + $"_{curentChat.CurentProcess.ProcessType.Code}")) == true);
            }
            else
            {
                
                OnLoadHadler = typeof(MenuProcessor).GetMethods().FirstOrDefault(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnLoad")) == true);
            }
            if (OnLoadHadler != null)
            {
                // Исполнение метода
                MenuProcessor menuProcessor = new MenuProcessor();
                object[] methodParameters = { update, client, curentChat,db,cancellationToken };
                OnLoadHadler.Invoke(menuProcessor, methodParameters);

                //Отчет об исполнении CONSOL
                string textRezult = $"Execute On Load - Process navigation: {this.Navigation}|Discription: был выполнен успешно метод при старте нового меню|true";
                Sourse.Handlers.concoldebuger.goodMSG(textRezult);
            }
            else
            {
                //Отчет об исполнении CONSOL
                string textRezult = $"Execute On Load - null |Discription: метод по атрибуту {this.Navigation} не найден|notbad";
                Sourse.Handlers.concoldebuger.goodMSG(textRezult);
            }
        }


        public void ExecuteOnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken cancellationToken)
        {
            //поиск метода 

            var methods = typeof(MenuProcessor).GetMethods().Where(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnEnd")) == true);

            if (methods.Count() > 1)
            {

                OnEndHadler = typeof(MenuProcessor).GetMethods().FirstOrDefault(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnEnd" + $"_{curentChat.CurentProcess.ProcessType.Code}")) == true);

            }
            else
            {
                OnEndHadler = typeof(MenuProcessor).GetMethods().FirstOrDefault(m => (m.GetCustomAttribute<MenuHandlerAttribute>()?.MenuCode.Contains(ProcessMenuCode + "_OnEnd")) == true);


            }

          


            if (OnEndHadler != null)
            {

                // Исполнение метода
                MenuProcessor menuProcessor = new MenuProcessor();
                object[] methodParameters = { update, client, curentChat };
                OnEndHadler.Invoke(menuProcessor, methodParameters);

                //Отчет об исполнении CONSOL
                string textRezult = $"Execute On End - Process navigation: {this.Navigation}|Discription: был выполнен успешно метод при старте нового меню|true";
                Sourse.Handlers.concoldebuger.goodMSG(textRezult);

            }
            else
            {
                //Отчет об исполнении CONSOL
                string textRezult = $"Execute On End - null |Discription: метод по атрибуту {this.Navigation} не найден|notbad";
                Sourse.Handlers.concoldebuger.goodMSG(textRezult);
            }
        }


        public int? ProcessTypeId { get; set; }
        virtual public Menu_ProcessType? ProcessType { get; set; }



        virtual public List<Process_Input> Inputs { get; set; } = new List<Process_Input>();

        virtual public List<Process_Input> CallingInputs { get; set; } = new List<Process_Input>();



        virtual public List<MyChat> Chats { get; set; } = new List<MyChat>();




        public int? UserTypeId { get; set; }
        virtual public User_Types? UserType { get; set; }






        /// <summary>
        /// Метод для получения MyId из CallbackQuery - расчитываю что в части строки будет update.CallbackQuery.Data.contains(m:(int)|) 
        /// </summary>
        /// <param name="update">Обязательно должен быть  тип CallbackQuery</param>
        /// <returns>Если не смог найти код то возвращаю null</returns>
        public static int? GetNextProcessIdByCallbak(Update update)
        {

            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) return null;

            string curentCallBakCode = update.CallbackQuery.Data;

            string curentItemCode = curentCallBakCode.Split('|').ToList().FirstOrDefault(s => s.Contains("m:")).Split(':')[1];

            int parsresult = 0;
            if (int.TryParse(curentItemCode, out parsresult))
            {
                return parsresult;
            }
            else return null;

        }
        /// <summary>
        /// Меню Id
        /// </summary>
        /// <returns>"m:{MyId}|"</returns>
        public string GetEntityTypeId()
        {
            return $"m:{MyId}|";
        }

    }
}
