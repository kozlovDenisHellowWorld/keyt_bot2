using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;
//play right
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot.Sourse.Handlers
{
    public class MenuProcessor
    {

        #region Dawinchi

        [MenuHandler("StartMenu_OnLoad")]
        public async Task Handle_StartMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("start - StartMenu_OnLoad");
        }




        [MenuHandler("StartMenu_OnEnd")]
        public async Task Handle_StartMenu_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("end - StartMenu_OnEnd");
        }



        [MenuHandler("OfferInputMenu_OnLoad")]
        public async Task Handle_OfferInputMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            Console.WriteLine("OnLoad- OfferInputMenu_OnLoad");
        }


        [MenuHandler("ListMenuAllAdmins_OnLoad")]
        public async Task Handle_ListMenuAllAdmins_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_AdminPropsMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_ChangeUserTypeInAdminPropss_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_ListMenuAllUsers_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_UserPropsMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_ChangeUserTypeInUserProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_MenuPropsListMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
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
        public async Task Handle_MenuAndActionProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            int mDbId = new TeleTools().getentyIdByUpdate("em", update);

            var menu = db.Menu_Proceses.FirstOrDefault(m => m.MyId == mDbId);


            string cont = curentChat.CurentProcess.MenuProcessContent;


            curentChat.CurentTexrMessage = new TeleTools().FormateMenuPropsText(curentChat.CurentProcess.MenuProcessContent, menu);




            db.SaveChanges();

        }






        [MenuHandler("InputPropsListMenu_OnLoad")]
        public async Task Handle_InputPropsListMenu_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);

            var btnTempleyt = curentChat.CurentProcess.Inputs.FirstOrDefault(inp => inp.NextProcessMenuCode == "InputProps");

            foreach (var item in db.Inputs.ToArray())
            {

                string callback = btnTempleyt.NextProcessMenu.GetEntityTypeId() + item.GetEntityTypeId();
                string btnContent = item.MyId.ToString() + ")" + btnTempleyt.MyName.Replace("{InputName}", item.MyName);
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
        public async Task Handle_InputProps_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            db.myChats.Update(curentChat);


            int myDbId = Process_Input.GetIdFromUpdate(update);

            var targetUnput = db.Inputs.FirstOrDefault(inp => inp.MyId == myDbId);

            string text = curentChat.CurentProcess.MenuProcessContent;


            curentChat.bsckInformation = targetUnput.GetEntityTypeId();

            text = text.Replace("{InputName}", targetUnput.MyName);
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


            curentChat.CurentTexrMessage = text;






            db.SaveChanges();

        }



        [MenuHandler("ListMenuDate_park_OnLoad")]
        public async Task Handle_ListMenuDate_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            var nedtoBreak = curentChat.ReqOrderSet.Where(r => r.IsCreate == false).ToList();

            foreach (var item in nedtoBreak)
            {
                item.IsCreate = true;
                item.IsDelite = true;
            }
            db.SaveChanges();

            var btn = curentChat.CurentProcess.Inputs.FirstOrDefault(p => p.input_Type.Code == "CallbackQueryList");


            if (curentChat.user_Reg_Telephone == null || curentChat.user_Reg_Name == null || curentChat.user_Reg_Telephone == "" || curentChat.user_Reg_Name == "")
            {
                curentChat.CurentTexrMessage = "🤖: Прости но я не могу тебя записать. Нужно расказать свои секретики.";
                db.SaveChanges();
                return;
            }


            for (int i = 0; i < 10; i++)
            {

                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.AddDays(i);



                curentChat.DinamicButons.Add(new Dinamic_Butons()
                {
                    BotClientId = client.BotId,
                    dateTimeCreation = DateTime.Now,
                    IsDelite = false,
                    MyName = dateTime.ToString("d MMMM - dddd"),
                    CallbackQwery = btn.NextProcessMenu.GetEntityTypeId() + $"day-{dateTime.ToString("MM-d")}",
                    Content = dateTime.ToString("d MMMM - dddd"),
                });
            }


            db.SaveChanges();

            //    string startPath = "https://dawinchiwakepark.ru/";


            //    string startPath1 = "https://dawinchiwakepark.ru/dashboard";


            //    var getDawinch = new WebReqGet(startPath);
            //    getDawinch.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            //    getDawinch.Host = "dawinchiwakepark.ru";
            //    getDawinch.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36";
            //    getDawinch.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            //    getDawinch.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9");
            //    getDawinch.Headers.Add("sec-ch-ua", " \"Google Chrome\";v=\"125\", \"Chromium\";v=\"125\", \"Not.A/Brand\";v=\"24\"");
            //    getDawinch.Headers.Add("sec-ch-ua-mobile", "?0");
            //    getDawinch.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            //    getDawinch.Headers.Add("Sec-Fetch-Dest", "document");
            //    getDawinch.Headers.Add("Sec-Fetch-Mode", "navigate");
            //    getDawinch.Headers.Add("Sec-Fetch-Site", "none");
            //    getDawinch.Headers.Add("Sec-Fetch-User", "?1");
            //    getDawinch.Headers.Add("Upgrade-Insecure-Requests", "1");
            //    getDawinch.Headers.Add("Connection", "keep-alive");


            //    getDawinch.Run(new CookieContainer());


            //    //byte[] bytes = Encoding.UTF8.GetBytes(getDawinch.Response);
            //    //string decodedString = Encoding.UTF8.GetString(bytes);



            //    ChromeOptions options = new ChromeOptions();
            //    AutoResetEvent waitHandle = new AutoResetEvent(false);


            //    IWebDriver driver = new ChromeDriver(options);



            //    // Создание и запуск задачи для загрузки сайта в отдельном потоке
            //    Task.Run(() => driver.Navigate().GoToUrl(startPath1));

            //    // Здесь можно продолжать выполнение других действий в основном потоке

            //    // Пример: ожидание выполнения задачи загрузки в течение 5 секунд


            //    for (int i = 0; i < 10; i++)
            //    {
            //        concoldebuger.goodMSG(i.ToString() + " ыыыы");

            //    }
            //    // Работа с элементами, которые уже загружены
            //    //IWebElement element = driver.FindElement(By.XPath("//xpath_of_element"));
            //    //string text = element.Text;


            //    // Продолжение работы с другими элементами или действиями



            //    await Task.Delay(5000);
            //    Console.WriteLine("******* вышли ");
            //}






        }






        [MenuHandler("ListMenuTime_park_OnLoad")]
        public async Task ListMenuTime_park_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            //concoldebuger.goodMSG("ждем 10 секунд ------------------------1-");
            string data = update.CallbackQuery.Data.ToString();


            //concoldebuger.goodMSG("ListMenuTime_park_OnLoad- start   4_____________________________________________________________________________ --- OnLoadHadler");



            // string _dateM = data.Split('|').FirstOrDefault(d => d.Contains("day")).Split('-')[1];
            //string _dateD = data.Split('|').FirstOrDefault(d => d.Contains("day")).Split('-')[2];

            int _dateM = 0;
            int _dateD = 0;


            if (int.TryParse(data.Split('|').FirstOrDefault(d => d.Contains("day")).Split('-')[1], out _dateM))
            {

            }
            else
            {
                _dateM = 0;

            }

            if (int.TryParse(data.Split('|').FirstOrDefault(d => d.Contains("day")).Split('-')[2], out _dateD))
            {

            }
            else
            {
                _dateD = 0;

            }

            DateTime taregetDateTime = new DateTime(DateTime.Now.Year, _dateM, _dateD);


            string startPath1 = "https://dawinchiwakepark.ru/dashboard";

            ChromeOptions options = new ChromeOptions();
            //   options.AddArgument("--headless"); // Запуск браузера в "тихом" режиме (без открытия окна)
            IWebDriver driver = new ChromeDriver(options);
            // concoldebuger.badMSG("ждем 10 секунд -------------------------2");

            //   concoldebuger.badMSG("ListMenuTime_park_OnLoad- await   5  await using_____________________________________________________________________________ --- OnLoadHadler");

            using (null)
            {
                //  concoldebuger.badMSG("ListMenuTime_park_OnLoad- await   6  await Task.Run(() => driver.Navigate().GoToUrl(startPath1))_____________________________________________________________________________ --- OnLoadHadler");

                await Task.Run(() => driver.Navigate().GoToUrl(startPath1));
                // concoldebuger.badMSG("ждем 10 секунд -------------------------4");

                await Task.Delay(4000);

                IWebElement button_park = driver.FindElement(By.XPath("//a[contains(text(), 'С трамплинами (Park)')]"));
                button_park.Click();
                await Task.Delay(500);


                IWebElement nextElement = driver.FindElement(By.XPath("//div[@class='inline-block']/h2[@class='text-center']"));
                string curentDateText = nextElement.Text;
                string format = "dd MMMM";
                DateTime currentDate;

                if (DateTime.TryParseExact(curentDateText, format, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out currentDate))
                {

                }
                else return;

                if (currentDate != taregetDateTime)
                {
                    IWebElement button_park_Calender = driver.FindElement(By.CssSelector("button.bg-transparent"));
                    button_park_Calender.Click();
                    await Task.Delay(200);

                    //-------------


                    string currentDateText_Calendar = driver.FindElement(By.CssSelector("span.day__month_btn")).Text;
                    string format_calendar = "MMMM yyyy"; // ВОТ ТУТ ВОЗМОЖНО ОШИБКА БУДЕТ ММММ!!!
                    DateTime currentDate_Calendar;
                    DateTime.TryParseExact(currentDateText_Calendar, /*format  - тут изменил но вроде неправельно было*/format_calendar, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out currentDate_Calendar);

                    if (currentDate_Calendar.Month != taregetDateTime.Month)
                    {
                        driver.FindElement(By.CssSelector("span.next")).Click();

                    }
                    driver.FindElement(By.XPath($"//span[contains(@class, 'cell day') and text()='{taregetDateTime.Day}']")).Click();

                }

                await Task.Delay(2000);


                List<DateTime> EmptyTime = new List<DateTime>();
                List<DateSetTime> Datesettimes = new List<DateSetTime>();

                //  IList<IWebElement> divElements = driver.FindElements(By.XPath("//div[contains(@class, 'border-r')   and contains(@class, 'border-b') and contains(@class, 'p-1') and contains(@class, 'text-xs') and contains(@class, 'align-middle') and contains(@class, 'flex-1')  and contains(@class, 'sm:h-32') and contains(@class, 'md:h-24')]"));

                //IList<IWebElement> divElements = driver.FindElements(By.CssSelector(".border-r.border-b.p-1.text-xs.align-middle.flex-1.sm\\:h-32.md\\:h-24"));
                //IList<IWebElement> divElements = driver.FindElements(By.CssSelector("[class='border-r border-b p-1 text-xs align-middle flex-1 sm:h-32 md:h-24']"));

                //IList<IWebElement> divElements = driver.FindElements(By.XPath("//div[not(contains(@class, 'booked')) and not(contains(@class, 'reservation')) and contains(@class, 'border-r') and contains(@class, 'border-b') and contains(@class, 'p-1') and contains(@class, 'text-xs') and contains(@class, 'align-middle') and contains(@class, 'flex-1') and contains(@class, 'sm:h-32') and contains(@class, 'md:h-24')]"));

                IList<IWebElement> divElements = driver.FindElements(By.XPath("//div[contains(@class, 'border-r') and contains(@class, 'border-b') and contains(@class, 'p-1') and contains(@class, 'text-xs') and contains(@class, 'align-middle') and contains(@class, 'flex-1') and contains(@class, 'sm:h-32') and contains(@class, 'md:h-24')]"));
                await Task.Delay(200);

                foreach (var item in divElements)
                {
                    string classText = item.GetAttribute("class");

                    if (classText.Contains("booked")) continue;
                    if (classText.Contains("temporary")) continue;
                    if (classText.Contains("temporary-long")) continue;
                    if (classText.Contains("reservation")) continue;
                    if (classText.Contains("past")) continue;




                    string dateTimeValue = item.GetAttribute("datetime");

                    int hours = int.Parse(dateTimeValue.Split(':')[0]);
                    int minutes = int.Parse(dateTimeValue.Split(':')[1]);
                    EmptyTime.Add(new DateTime(DateTime.Now.Year, taregetDateTime.Month, taregetDateTime.Day, hours, minutes, 0));


                    Datesettimes.Add(new DateSetTime()
                    {
                        BotClientId = client.BotId,
                        dateTimeCreation = DateTime.Now,
                        SetdateTime = new DateTime(DateTime.Now.Year, taregetDateTime.Month, taregetDateTime.Day, hours, minutes, 0)
                    });

                }

                //1 необходимо сделать объект кторый вберет в себя завку на регистрирацию. у этого объеката будет два типа поля выбранные даты и даты которые вообще есть (два масива в каждой завке тоесть получится что при оформлении у нас остануться данныве о том какая читуация была на момент регистрации)

                // 2 вот ту надо создать это завку и заполнить мачив с возможными сетами 

                //3 после чего перейти к в новый процесс и там уже ебашить  таблицу с динаимк батонз 

                //4 при изменение будут перезаполнятся динамик батонз 



                driver.Close();


                var newOrder = new ReqOrderSet()
                {
                    Date = new DateTime(taregetDateTime.Year, taregetDateTime.Month, taregetDateTime.Day),
                    BotClientId = client.BotId,
                    dateTimeCreation = DateTime.Now,
                    IsCreate = false,
                    IsDelite = false,
                    TimeSets = Datesettimes,
                    MyName = "Заявка на бронь сетов"
                };

                db.myChats.Update(curentChat);

                curentChat.ReqOrderSet.Add(newOrder);


                var nextProcess = curentChat.CurentProcess?.Inputs.FirstOrDefault(i => i.MyName == "Next_menu")?.NextProcessMenu;



                // concoldebuger.badMSG("ListMenuTime_park_OnLoad- await   7     await curentChat.CurentProcess.ExecuteOnEnd_____________________________________________________________________________ --- OnLoadHadler");


                await curentChat.CurentProcess.ExecuteOnEnd(update, client, curentChat, db, ctl);

                curentChat.SetProcess(nextProcess);

                db.SaveChanges();

                await new TeleTools().remooveMenu(client, ctl, curentChat);

                await curentChat.CurentProcess.ExecuteOnLoad(update, client, curentChat, db, ctl);

                var messages = await new TeleTools().SendStaticMenu_forXMLLoad(curentChat, client, ctl, update, db);


            }
        }





        [MenuHandler("ListMenuTime_park_Coose_OnLoad")]
        public async Task ListMenuTime_park_Coose_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            bool debugRule = false;// для того чтобы попробовать сделать эдит. когда мы в другом месте исправляем 



            if (debugRule == true)
            {
                if (update.CallbackQuery.Data.Contains("set:"))
                {

                    string set_text = update?.CallbackQuery?.Data.Split('|').FirstOrDefault(d => d.Contains("set:"));
                    string dateTimeSet_text = set_text.Split(':').FirstOrDefault(s => !s.Contains("set"));
                    string format_ = "HH-m-d-MM-y";
                    DateTime targetDateTime;

                    DateTime.TryParseExact(dateTimeSet_text, format_, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out targetDateTime);

                    foreach (var item in curentChat.ReqOrderSet.FirstOrDefault(r => r.IsCreate == false).TimeSets)
                    {
                        if (item.SetdateTime == targetDateTime)
                        {

                            item.IsTarget = !item.IsTarget;

                        }


                    }

                    db.SaveChanges();

                }

            }
            foreach (var item in curentChat?.ReqOrderSet?.FirstOrDefault(r => r.IsCreate == false).TimeSets)
            {
                var btn = new Dinamic_Butons()
                {
                    dateTimeCreation = DateTime.Now,
                    BotClientId = item.BotClientId,
                };

                if (item.IsTarget.Value == true)
                {
                    btn.CallbackQwery = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "TargetTime").NextProcessMenu.GetEntityTypeId() + $"set:{item.SetdateTime.Value.ToString("HH-m-d-MM-y")}|T|";

                    string content = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "TargetTime").NameIfTrue;
                    content = content.Replace("{Time}", item.SetdateTime.Value.ToString("HH:mm"));

                    btn.Content = content;
                }
                else
                {
                    btn.CallbackQwery = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "TargetTime").NextProcessMenu.GetEntityTypeId() + $"set:{item.SetdateTime.Value.ToString("HH-m-d-MM-y")}|";

                    string content = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "TargetTime").NameIfFalse;
                    content = content.Replace("{Time}", item.SetdateTime.Value.ToString("HH:mm"));

                    btn.Content = content;
                }

                curentChat.DinamicButons.Add(btn);


            }

            db.SaveChanges();

        }

        [MenuHandler("ChooseTimeGo_OnLoad")]
        public async Task ChooseTimeGo_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.CallbackQuery.Data.Contains("set:"))
            {

                string set_text = update?.CallbackQuery?.Data.Split('|').FirstOrDefault(d => d.Contains("set:"));
                string dateTimeSet_text = set_text.Split(':').FirstOrDefault(s => !s.Contains("set"));
                string[] format_ = { "HH-m-d-MM-y" };


                DateTime targetDateTime;

                DateTime.TryParseExact(dateTimeSet_text, format_, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out targetDateTime);




                foreach (var item in curentChat.ReqOrderSet.FirstOrDefault(r => r.IsCreate == false).TimeSets)
                {
                    if (item.SetdateTime == targetDateTime)
                    {

                        item.IsTarget = !item.IsTarget;

                    }


                }

                db.SaveChanges();

            }

        }

        [MenuHandler("StopAndRemuveRegistration_OnLoad")]
        public async Task StopAndRemuveRegistration_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;

            var reserd = curentChat.ReqOrderSet.FirstOrDefault(r => r.IsCreate == false);
            if (reserd is not null)
            {
                reserd.IsCreate = true;
                reserd.IsDelite = true;
            }
            db.SaveChanges();

        }



        [MenuHandler("StartRegistration_sets_toPark_OnLoad")]
        public async Task StartRegistration_sets_toPark_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            string setTimeTexst = "";

            var targetSet = curentChat.ReqOrderSet.FirstOrDefault(r => r.IsCreate == false).TimeSets.Where(t => t.IsTarget == true);

            var dateTimeSets = new List<DateTime>();

            foreach (var item in targetSet)
            {
                dateTimeSets.Add(item.SetdateTime ?? new DateTime());

            }


            if (targetSet is not null && targetSet.Count() > 0)
            {

                //if (curentChat.user_Reg_Abon is null || curentChat.user_Reg_Name is null || curentChat.user_Reg_Telephone is null)
                //{
                //    curentChat.CurentTexrMessage = "🤖: Я не могу тебя за регистрировать. В начале нужно расказать мне твой телефон и имя";
                //    return;

                //}


                setTimeTexst += "🗓:" + targetSet.FirstOrDefault().SetdateTime.Value.ToString("dd.MM") + "\nПарк";

                foreach (var item in targetSet)
                {
                    setTimeTexst += $"\n🕟 {item.SetdateTime.Value.ToString("HH:mm")}";
                }
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{sets}", setTimeTexst);
                curentChat.CurentTexrMessage += "\n#SETRegistration";


                List<string> registration_result = new List<string>();

                foreach (var item in targetSet)
                {
                    Task.Run(async () =>
                    {
                      using (null)
                                {

                                    string answer = (await new TeleTools().SetRegistration("С трамплинами (Park)", item.SetdateTime ?? new DateTime(), curentChat.user_Reg_Name, curentChat.user_Reg_Telephone, curentChat.user_Reg_Abon));



                                    registration_result.Add(answer);
                                    if (answer.Contains("Ошибка"))
                                    {

                                        string messageErr = $"🤖:Ошибка ⚠️\nПрости у меня не получилось зарегистрировать вот этот сет <pre>🗓:{item.SetdateTime.Value.ToString("dd.MM - dddd")}\n🕟:{item.SetdateTime.Value.ToString("HH:mm")}</pre> \nПроблемы могут быть с номером телефона. Проверь введеный телефон и попробуй заного.";
                                        Message sentMessage = await client.SendTextMessageAsync(chatId: curentChat.ChatId, text: messageErr, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: ctl);



                                        //Task.Run(async () =>
                                        //{
                                        //     using (null)
                                        //    {

                                        //    }
                                        //});


                                        //    string messageErr = $"🤖:Ошибка ⚠️\nПрости у меня не получилось зарегистрировать вот этот сет <pre>🗓:{item.SetdateTime.Value.ToString("dd.MM - dddd")}\n🕟:{item.SetdateTime.Value.ToString("HH:mm")}</pre> \nПроблемы могут быть с номером телефона. Проверь введеный телефон и попробуй заного.";
                                        //Message sentMessage = await client.SendTextMessageAsync(chatId: curentChat.ChatId, text: messageErr, parseMode: ParseMode.Html, disableNotification: true, cancellationToken: ctl);

                                    }

                                }

                            });


                }





            }
            else
            {
                curentChat.CurentTexrMessage = "🤖: Что то пошло не так. Попробуй заного. Или ты не выбрал ни одно сета";
            }







            var reserd = curentChat.ReqOrderSet.FirstOrDefault(r => r.IsCreate == false);
            if (reserd is not null)
            {
                reserd.IsCreate = true;
                reserd.IsDelite = false;// было труе 
            }
            db.SaveChanges();

        }









        [MenuHandler("InputTelePHForRegistration_OnEnd")]
        public async Task InputTelePHForRegistration_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {

                curentChat.user_Reg_Telephone = update.Message.Text;
                db.SaveChanges();
            }



        }


        [MenuHandler("InputNikName_OnEnd")]
        public async Task InputNikName_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {

                curentChat.user_Reg_Name = update.Message.Text;
                db.SaveChanges();
            }



        }

        [MenuHandler("InputAbonNumber_OnEnd")]
        public async Task InputAbonNumber_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {
                
                curentChat.user_Reg_Abon = update.Message.Text;
                db.SaveChanges();


            }



        }



        [MenuHandler("RegInformationRemember_OnLoad")]
        public async Task RegInformationRemember_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{tel}", curentChat.user_Reg_Telephone + "   ")??"-";
            curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{name}", curentChat.user_Reg_Name + "   ")??"-";
            curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{abon}", curentChat.user_Reg_Abon + "   ")??"-";

        }

        [MenuHandler("ParInfo_AllSets_OnLoad")]
        public async Task ParInfo_AllSets_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var allSetsInDAy =  await new TeleTools().GetAllSets("С трамплинами (Park)", client, DateTime.Now.AddDays(4),false);

           var last_msg= curentChat.PriviosMSGs.LastOrDefault();

            string assets = "<b>{SetTime}</b> - <code>{infoUser}</code>\n";

            string textMsg = "";

            if (last_msg != null)
            {
                foreach (var item in allSetsInDAy)
                {
                    textMsg += assets.Replace("{SetTime}", item.SetdateTime.Value.ToString("HH:mm")).Replace("infoUser", item.name);
                }

            
            }

            client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, last_msg.MessageId??00, textMsg, ParseMode.Html);



        }

        #endregion

        #region  Катя
        [MenuHandler("StartMenu_regularUser_OnLoad")]
        public async Task Handle_StartMenu_regularUser_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            using (var db1 = new context())
            {
                var botProps = db1.BotProperties.FirstOrDefault(b => b.BotClientId == client.BotId);
                if (botProps.startPhoto != null)
                {
                    Message message = await client.SendPhotoAsync(curentChat.ChatId, photo: InputFile.FromFileId(botProps.startPhoto.FileId), caption: "Приветственное фото", parseMode: ParseMode.Html, cancellationToken: ctl);

                    curentChat.PriviosMSGs.Add(PriviosMSG.createMessage(botClientId: client.BotId, true, message, update));
                    while (true)
                    {
                        try
                        {
                            db1.SaveChanges();
                            break;
                        }
                        catch
                        {
                            concoldebuger.badMSG("Ошибка тут !! 2");
                        }
                    }
                }
            }
        }


        #region regularUser
        #region  Предложение
        [MenuHandler("offer_report_start_OnLoad")]
        public async Task Handle_offer_report_start_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {



            if (curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "💡 Предложение") == null)
            {
                var newReqwest = requst.newRequst("💡 Предложение", client.BotId);
                curentChat.Requsts.Add(newReqwest);
                db.SaveChanges();
            }
            
            
        }

        [MenuHandler("offer_report_start_OnEnd")]
        public async Task Handle_offer_report_start_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {
                var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false);
                if (curentReq != null)
                {
                    curentReq.reqstContent = update.Message?.Text;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    concoldebuger.badMSG("Method exeeption : offer_report_startStartMenu_regularUse_OnEnd -  curentReq is null | ");
                }
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : offer_report_startStartMenu_regularUse_OnEnd -  is not UpdateType.Message ");
            }
        }


        [MenuHandler("offer_confirmation_OnLoad")]
        public async Task Handle_offer_confirmation_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "💡 Предложение");
            if (curentReq != null)
            {
                string text = curentReq.reqstContent;
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", text);
            }
            else
            {
                curentChat.CurentTexrMessage = "Что то пошло не так начни заного";
                concoldebuger.badMSG("Method exeeption : offer_confirmation_OnLoad  -  curentReq is null | ");
            }
        }

        [MenuHandler("offer_report_input_photot_OnEnd")]
        public async Task Handle_offer_report_input_photot_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "💡 Предложение");
            if (curentReq != null)
            {
                var photo = myPhoto.createPhot(update.Message.Photo.LastOrDefault()) ;
                curentReq.Photoes.Add(photo);

            }
            else
            {
              
            }

        }

        [MenuHandler("offer_delite_OnLoad")]
        public async Task Handle_offer_delite_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "💡 Предложение");
            if (curentReq != null)
            {
                curentChat.Requsts.Remove(curentReq);
                db.Requst.Remove(curentReq);
                db.SaveChanges();
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : ofer_delite_OnLoad -  curentReq is null | ");
            }
        }


        [MenuHandler("offer_save_OnLoad")]
        public async Task Handle_offer_save_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "💡 Предложение");
            if (curentReq != null)
            {
               curentReq.isCreated=true;
                curentReq.user = curentChat.AllChatUsers.FirstOrDefault();

                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{IdReq}", curentReq.MyId.ToString());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{TypeReq}", curentReq.reqstType);
                db.SaveChanges();

                

            }
            else
            {
                concoldebuger.badMSG("Method exeeption : offer_save_OnLoad -  curentReq is null");
            }
        }



        #endregion


        #region  Вопрос
        [MenuHandler("question_report_start_OnLoad")]
        public async Task Handle_question_report_start_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {



            if (curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "❓ Задайте вопрос") == null)
            {
                var newReqwest = requst.newRequst("❓ Задайте вопрос", client.BotId);
                curentChat.Requsts.Add(newReqwest);
                db.SaveChanges();
            }


        }

        [MenuHandler("question_report_start_OnEnd")]
        public async Task Handle_question_report_start_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {
                var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false);
                if (curentReq != null)
                {
                    curentReq.reqstContent = update.Message?.Text;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    concoldebuger.badMSG("Method exeeption : question_report_startStartMenu_regularUse_OnEnd -  curentReq is null | ");
                }
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : question_report_startStartMenu_regularUse_OnEnd -  is not UpdateType.Message ");
            }
        }


        [MenuHandler("question_confirmation_OnLoad")]
        public async Task Handle_question_confirmation_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "❓ Задайте вопрос");
            if (curentReq != null)
            {
                string text = curentReq.reqstContent;
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", text);
            }
            else
            {
                curentChat.CurentTexrMessage = "Что то пошло не так начни заного";
                concoldebuger.badMSG("Method exeeption : question_confirmation_OnLoad  -  curentReq is null | ");
            }
        }

        [MenuHandler("question_report_input_photot_OnEnd")]
        public async Task Handle_question_report_input_photot_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "❓ Задайте вопрос");
            if (curentReq != null)
            {
                var photo = myPhoto.createPhot(update.Message.Photo.LastOrDefault());
                curentReq.Photoes.Add(photo);

            }
            else
            {

            }

        }

        [MenuHandler("question_delite_OnLoad")]
        public async Task Handle_question_delite_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "❓ Задайте вопрос");
            if (curentReq != null)
            {
                curentChat.Requsts.Remove(curentReq);
                db.Requst.Remove(curentReq);
                db.SaveChanges();
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : ofer_delite_OnLoad -  curentReq is null | ");
            }
        }


        [MenuHandler("question_save_OnLoad")]
        public async Task Handle_question_save_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "❓ Задайте вопрос");
            if (curentReq != null)
            {
                curentReq.isCreated = true;
                curentReq.user = curentChat.AllChatUsers.FirstOrDefault();

                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{IdReq}", curentReq.MyId.ToString());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{TypeReq}", curentReq.reqstType);
                db.SaveChanges();



            }
            else
            {
                concoldebuger.badMSG("Method exeeption : question_save_OnLoad -  curentReq is null");
            }
        }



        #endregion


        #region  Ошибка
        [MenuHandler("error_report_start_OnLoad")]
        public async Task Handle_error_report_start_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {



            if (curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "🖍 Ошибка в материалах") == null)
            {
                var newReqwest = requst.newRequst("🖍 Ошибка в материалах", client.BotId);
                curentChat.Requsts.Add(newReqwest);
                db.SaveChanges();
            }


        }

        [MenuHandler("error_report_start_OnEnd")]
        public async Task Handle_error_report_start_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.Message)
            {
                var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false);
                if (curentReq != null)
                {
                    curentReq.reqstContent = update.Message?.Text;
                    db.SaveChanges();
                    return;
                }
                else
                {
                    concoldebuger.badMSG("Method exeeption : error_report_startStartMenu_regularUse_OnEnd -  curentReq is null | ");
                }
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : error_report_startStartMenu_regularUse_OnEnd -  is not UpdateType.Message ");
            }
        }


        [MenuHandler("error_confirmation_OnLoad")]
        public async Task Handle_error_confirmation_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "🖍 Ошибка в материалах");
            if (curentReq != null)
            {
                string text = curentReq.reqstContent;
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", text);
            }
            else
            {
                curentChat.CurentTexrMessage = "Что то пошло не так начни заного";
                concoldebuger.badMSG("Method exeeption : error_confirmation_OnLoad  -  curentReq is null | ");
            }
        }

        [MenuHandler("error_report_input_photot_OnEnd")]
        public async Task Handle_error_report_input_photot_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "🖍 Ошибка в материалах");
            if (curentReq != null)
            {
                var photo = myPhoto.createPhot(update.Message.Photo.LastOrDefault());
                curentReq.Photoes.Add(photo);

            }
            else
            {

            }

        }

        [MenuHandler("error_delite_OnLoad")]
        public async Task Handle_error_delite_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "🖍 Ошибка в материалах");
            if (curentReq != null)
            {
                curentChat.Requsts.Remove(curentReq);
                db.Requst.Remove(curentReq);
                db.SaveChanges();
            }
            else
            {
                concoldebuger.badMSG("Method exeeption : ofer_delite_OnLoad -  curentReq is null | ");
            }
        }


        [MenuHandler("error_save_OnLoad")]
        public async Task Handle_error_save_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            var curentReq = curentChat.Requsts.FirstOrDefault(r => r.isCreated == false && r.reqstType == "🖍 Ошибка в материалах");
            if (curentReq != null)
            {
                curentReq.isCreated = true;
                curentReq.user = curentChat.AllChatUsers.FirstOrDefault();

                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{IdReq}", curentReq.MyId.ToString());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{TypeReq}", curentReq.reqstType);
                db.SaveChanges();



            }
            else
            {
                concoldebuger.badMSG("Method exeeption : error_save_OnLoad -  curentReq is null");
            }
        }



        #endregion


        #region Мои заявки
        [MenuHandler("list_report_OnLoad")]
        public async Task Handle_list_report_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            var reqs = curentChat.Requsts.Where(r => r.isDone == false).ToList();

            var button = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "Id:{IdReq} - {Type}");

            foreach (var item in reqs)
            {

                string text_btn = button.MyName;
                text_btn = text_btn.Replace("{IdReq}", item.MyId.ToString());
                if (item.reqstType.Contains("💡"))   text_btn = text_btn.Replace("{Type}", "💡");
                if (item.reqstType.Contains("❓")) text_btn = text_btn.Replace("{Type}", "❓");
                if (item.reqstType.Contains("🖍")) text_btn = text_btn.Replace("{Type}", "🖍 ");
                curentChat.DinamicButons.Add(new Dinamic_Butons() { 
                    Content= text_btn,
                    CallbackQwery=(button.NextProcessMenu.GetEntityTypeId()+item.GetEntityTypeId())});

            }
            db.SaveChanges();


        }


        [MenuHandler("report_info_OnLoad")]
        public async Task Handle_report_info_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                int iReq = requst.GetUserIdFromCode(update.CallbackQuery.Data);

                var req = curentChat.Requsts.FirstOrDefault(r => r.MyId == iReq);


                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{IdReq}", req.MyId.ToString());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{TypeReq}", req.reqstType);
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", req.reqstContent);
                db.SaveChanges();


            }



        }


        #endregion


        #endregion



        #region admin


        #region Стартовое фото
        [MenuHandler("get_start_photo_start_OnLoad")]
        public async Task Handle_get_start_photo_start_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            using (var db1=new context())
            {
                var botProps = db1.BotProperties.FirstOrDefault(b => b.BotClientId == client.BotId);
                if (botProps.startPhoto != null)
                {
                    Message message = await client.SendPhotoAsync(curentChat.ChatId, photo: InputFile.FromFileId(botProps.startPhoto.FileId), caption: "Приветственное фото", parseMode: ParseMode.Html, cancellationToken: ctl);

                    curentChat.PriviosMSGs.Add(PriviosMSG.createMessage(botClientId: client.BotId, true, message, update));
                    while (true)
                    {
                        try
                        {
                            db1.SaveChanges();
                            break;
                        }
                        catch
                        {
                            concoldebuger.badMSG("Ошибка тут !! 2");
                        }
                    }
                }
            }
            
          
        }
        [MenuHandler("get_start_photo_start_OnEnd")]
        public async Task Handle_get_start_photo_start_OnEnd(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            if (update.Type == UpdateType.Message)
            {
                var botProps = db.BotProperties.FirstOrDefault(b => b.BotClientId == client.BotId);
                botProps.startPhoto = myPhoto.createPhot(update.Message.Photo.LastOrDefault());
                
                db.SaveChanges();
            }
        }
        #endregion


        #region список заявков

        [MenuHandler("offer_menu_start_OnLoad")]
        public async Task Handle_offer_menu_start_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {
            


        }


        [MenuHandler("offer_menu_list_new_offer_OnLoad")]
        public async Task Handle_offer_menu_list_new_offer_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            var offers = db.Requst.Where(r => r.isNew == true).ToList();
            var buttex = curentChat.CurentProcess.Inputs.FirstOrDefault(i=>i.MyName== "{date}|{user}");
            foreach (var offer in offers)
            {
                string userName = offer.user.FirstName??offer.user.Username??offer.user.LastName??offer.user.Id.ToString();
                string type = "💡";
                if (offer.reqstType.Contains("❓")) type = "❓";
                if (offer.reqstType.Contains("🖍")) type = "🖍";
                string btnName = type +" | "+ offer.dateTimeCreation.Value.ToString("dd.MM")+" | "+ userName;

                curentChat.DinamicButons.Add(new Dinamic_Butons() { Content=btnName,CallbackQwery= (buttex.NextProcessMenu.GetEntityTypeId()+offer.GetEntityTypeId())});

            }
            db.SaveChanges();

        }

        [MenuHandler("offer_menu_new_offer_info_OnLoad")]
        public async Task Handle_offer_menu_new_offer_info_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            if (update.Type is UpdateType.CallbackQuery)
            {
                int reqId = requst.GetUserIdFromCode(update.CallbackQuery.Data);

                if (reqId == 0) return;
                var req =db.Requst.FirstOrDefault(r=>r.MyId==reqId);

                if (req == null)
                {
                    curentChat.CurentTexrMessage = "🤖:  Не нашел ничего. Что то пошло не так.";
                    return;
                }
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{name}", req.user.GetUserLinkInline_Name());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{type}", req.reqstType);
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", req.reqstContent);


                var btn = curentChat.CurentProcess.Inputs.FirstOrDefault(i=>i.input_Type.Code== "CallbackQueryList");

                curentChat.DinamicButons.Add(new Dinamic_Butons()
                {
                    CallbackQwery = (btn.NextProcessMenu.GetEntityTypeId() + req.GetEntityTypeId()),
                    Content = btn.MyName

                }) ;


                db.SaveChanges();


            }

            

        }

        [MenuHandler("offer_menu_new_offer_info_getInworke_OnLoad")]
        public async Task Handle_offer_menu_new_offer_info_getInworke_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            if (update.Type is UpdateType.CallbackQuery)
            {
                int reqId = requst.GetUserIdFromCode(update.CallbackQuery.Data);

                if (reqId == 0) return;
                var req = db.Requst.FirstOrDefault(r => r.MyId == reqId);

                req.isNew = false;

                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent.Replace("{Idreq}",req.MyId.ToString());

                db.SaveChanges();


            }



        }



        [MenuHandler("offer_menu_list_in_worke_offer_OnLoad")]
        public async Task Handle_offer_menu_list_in_worke_offer_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            var offers = db.Requst.Where(r => r.isNew != true&&r.isDone==false&&r.IsDelite!=true).ToList();
            var buttex = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.MyName == "{date}|{user}");
            foreach (var offer in offers)
            {
                string userName = offer.user.FirstName ?? offer.user.Username ?? offer.user.LastName ?? offer.user.Id.ToString();
                string type = "💡";
                if (offer.reqstType.Contains("❓")) type = "❓";
                if (offer.reqstType.Contains("🖍")) type = "🖍";
                string btnName = type + " | " + offer.dateTimeCreation.Value.ToString("dd.MM") + " | " + userName;

                curentChat.DinamicButons.Add(new Dinamic_Butons() { Content = btnName, CallbackQwery = (buttex.NextProcessMenu.GetEntityTypeId() + offer.GetEntityTypeId()) });

            }
            db.SaveChanges();

        }

        [MenuHandler("offer_menu_in_worke_offer_info_OnLoad")]
        public async Task Handle_offer_menu_in_worke_offer_info_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            if (update.Type is UpdateType.CallbackQuery)
            {
                int reqId = requst.GetUserIdFromCode(update.CallbackQuery.Data);

                if (reqId == 0) return;
                var req = db.Requst.FirstOrDefault(r => r.MyId == reqId);

                if (req == null)
                {
                    curentChat.CurentTexrMessage = "🤖:  Не нашел ничего. Что то пошло не так.";
                    return;
                }
                curentChat.CurentTexrMessage = curentChat.CurentProcess.MenuProcessContent;
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{name}", req.user.GetUserLinkInline_Name());
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{type}", req.reqstType);
                curentChat.CurentTexrMessage = curentChat.CurentTexrMessage.Replace("{text}", req.reqstContent);


                var btn = curentChat.CurentProcess.Inputs.FirstOrDefault(i => i.input_Type.Code == "CallbackQueryList");

                curentChat.DinamicButons.Add(new Dinamic_Butons()
                {
                    CallbackQwery = (btn.NextProcessMenu.GetEntityTypeId() + req.GetEntityTypeId()),
                    Content = btn.MyName

                });


                db.SaveChanges();


            }



        }

        [MenuHandler("offer_menu_in_worke_offer_info_getInworke_OnLoad")]
        public async Task Handle_offer_menu_in_worke_offer_info_getInworke_OnLoad(Update update, ITelegramBotClient client, MyChat curentChat, context db, CancellationToken ctl)
        {

            if (update.Type is UpdateType.CallbackQuery)
            {
                int reqId = requst.GetUserIdFromCode(update.CallbackQuery.Data);

                if (reqId == 0) return;
                var req = db.Requst.FirstOrDefault(r => r.MyId == reqId);

                req.isDone = true;

                db.SaveChanges();


            }



        }







        #endregion



        #endregion



        #endregion






    }
}
