using System.Diagnostics;
using System.Xml;
using Telebot.Sourse;
using Telebot.Sourse.Handlers;
using Telebot.Sourse.Item;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telebot
{


    public class TeleClient
    {

        static bool needToUpdate = false;

        private string Token;

        private string BDName;

        static public TelegramBotClient myClient;

        static CancellationTokenSource myCansToken = new CancellationTokenSource();

        static public string myToken { private set; get; }

        static private ReceiverOptions myReceveOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };


        bool isstarted = false;



        public TeleClient(string token)
        {



            Token = token;

            myClient = new TelegramBotClient(token);
            //   myClient = new TelegramBotClient(token);
            myToken = token;

            MyStart();
            Console.ReadKey();

        }



        public TeleClient()
        {

            //D:\kate_bot\keyt_bot2\BeauteRoom\Inst.xml
            //E:\Den\kate_2bot\keyt_bot2\BeauteRoom\Inst.xml
            XmlMenus(@"D:\kate_bot\keyt_bot2\BeauteRoom\Inst.xml");


            //  new context2(dbName: BDName);




            myClient = new TelegramBotClient(Token);
            //   myClient = new TelegramBotClient(token);


            MyStart();
            Console.ReadKey();

        }





        public async void MyStart()
        {
            if (isstarted == true) return;

            Console.WriteLine("Необходимость в обновлении - " + needToUpdate.ToString());
            //  context.RestoreDatabase("NewStartBD_20231220000509.bak");
            
            using (var db = new context())
            {

                db.reSetBD(needToUpdate, myClient, myCansToken.Token);
                if (needToUpdate==true) XmlMenus(@"D:\kate_bot\keyt_bot2\BeauteRoom\Inst.xml");
                concoldebuger.notifMSG($"Имя базы данных: {db.databaseName}", myClient, myCansToken.Token);

            }
           
            myClient.StartReceiving(
                myUpdate,
                myErrohendler,
                myReceveOptions,
                myCansToken.Token);

            isstarted = true;
            var my = await myClient.GetMeAsync(myCansToken.Token);

            Console.WriteLine("------------------------------------------------");
            concoldebuger.goodMSG($"Стартовал\nid: {my.Id}\nUserName: {my.Username}", myClient, myCansToken.Token);

            Task.Run(CheckMessagesPeriodically);

        }

        private async void backUp(ITelegramBotClient client, Exception exception, CancellationToken arg3)
        {


        }

        private async Task CheckMessagesPeriodically()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMinutes(59));

                concoldebuger.notifMSG($"{DateTime.Now}-- Удаление собщений ", myClient, myCansToken.Token);
                // Получите текущую дату и время
                DateTime now = DateTime.Now;

                // Определите время, когда должна запускаться проверка (01:00)
                DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, 23, 0, 0);

                // Если текущее время больше или равно запланированному времени
                if (now.Hour == scheduledTime.Hour)
                {
                    // Выполните проверку сообщений
                    await CheckMessages();
                }

                // Подождите 1 минуту перед следующей проверкой
                //  concoldebuger.notifMSG($"{DateTime.Now}-- Закончил проверку старых сообщений", myClient, myCansToken.Token);
            }
        }


        private async Task CheckMessages()
        {
            // Создайте объект, показывающий время начала периода последних 12 часов
            DateTime startTime = DateTime.Now.AddHours(-12);

            // DateTime startTime = DateTime.Now.AddMinutes(-1);
            //  concoldebuger.notifMSG($"{DateTime.Now}-- получил время и дату скоторого будем удалять", myClient, myCansToken.Token);

            // Вместо "myDbContext" укажите свой контекст базы данных

            new adminHendlerHR().start_new_sasion(startTime, myClient, myCansToken.Token);
            new userHendlerHR().start_new_sasion(startTime, myClient, myCansToken.Token);




        }

        // загрузка меню 
        private void XmlMenus(string instansePath)
        {
            string curetnDirection = Directory.GetCurrentDirectory();

            string discription = Directory.GetParent(curetnDirection).FullName;
            discription = Directory.GetParent(discription).FullName;
            discription = Directory.GetParent(discription).FullName;



            //D:\kelevroRepostAssistant — Катя\BeauteRoom\Inst.xml


            //D:\kelevroRepostAssistant — Катя\BeauteRoom\Inst.xml

            // Путь к файлу XML
            string xmlFile = "Inst.xml";

            string filePath = Path.Combine(discription, xmlFile);


            filePath = instansePath;


            string debagDBname = "";

            string debugToken = "";

            string reliseToken = "";

            string reliseDBName = "";

            bool NeedToUpdate = false;

            bool isDebugDiferent = false;






            // Создаем новый XmlDocument
            XmlDocument xmlDoc = new XmlDocument();

            try
            {







                // Загружаем XML-файл
                xmlDoc.Load(filePath);


                XmlNodeList nodes = xmlDoc.GetElementsByTagName("BotInst");

                foreach (XmlNode node in nodes)
                {
                    if (node is XmlElement elem)
                    {
                        debagDBname = elem.GetAttribute("DebagDBname") ?? string.Empty;
                        debugToken = elem.GetAttribute("DebugToken") ?? string.Empty;
                        reliseToken = elem.GetAttribute("DebagDBname") ?? string.Empty;
                        reliseDBName = elem.GetAttribute("ReliseDBName") ?? string.Empty;
                        isDebugDiferent = bool.Parse(elem.GetAttribute("IsDebugDiferent") ?? "False");
                        NeedToUpdate = bool.Parse(elem.GetAttribute("NeedToUpdate") ?? "False");
                    }
                }


                if (Debugger.IsAttached)
                {
                    Token = debugToken;
                    BDName = debagDBname;
                }
                else
                {
                    Token = reliseToken;
                    BDName = reliseDBName;
                }

                using (var db = new context())
                {

                    List<User_Types> userTypes = new List<User_Types>();
                    XmlNodeList usertypeNodes = xmlDoc.SelectNodes("//UserType/Type");
                    foreach (XmlNode node in usertypeNodes)
                    {
                        var type = new User_Types();
                        type.TypeCode = node.Attributes["CodeType"]?.Value ?? string.Empty;
                        type.MyName = node.Attributes["Name"]?.Value ?? string.Empty;
                        type.MyDescription = node.Attributes["Description"]?.Value ?? string.Empty;
                        type.IsDefoult = bool.Parse(node.Attributes["isDefoult"]?.Value ?? "false");
                        //   type.BotClientId = myClient.BotId;
                        type.IsDelite = false;
                        type.dateTimeCreation = DateTime.Now;
                        userTypes.Add(type);

                    }

                    db.User_Types.AddRange(userTypes);

                    db.SaveChanges();


                    XmlNodeList menuTypesNodes = xmlDoc?.SelectNodes("//MenuType/Type");
                    List<Menu_ProcessType> menuTypes = new List<Menu_ProcessType>();

                    foreach (XmlNode node in menuTypesNodes)
                    {
                        var type = new Menu_ProcessType();

                        type.Code = node.Attributes["CodeType"]?.Value ?? string.Empty;
                        type.MyName = node.Attributes["Name"]?.Value ?? string.Empty;
                        type.dateTimeCreation = DateTime.Now;
                        type.IsDelite = false;
                        //type.BotClientId=myClient.BotId;
                        type.MyDescription = "Вид меню";

                        menuTypes.Add(type);
                    }

                    db.Menu_ProcessTypes.AddRange(menuTypes);
                    db.SaveChanges();


                    XmlNodeList buttonTypesNodes = xmlDoc.SelectNodes("//InputType/Type");
                    List<Input_Type> buttonTypes = new List<Input_Type>();
                    foreach (XmlNode node in buttonTypesNodes)
                    {
                        var type = new Input_Type();

                        type.Code = node.Attributes["CodeType"]?.Value ?? string.Empty;
                        type.MyName = node.Attributes["Name"]?.Value ?? string.Empty;
                        type.dateTimeCreation = DateTime.Now;
                        type.IsDelite = false;
                        // type.BotClientId = myClient.BotId;
                        type.MyDescription = "Вид инпута";
                        buttonTypes.Add(type);
                    }
                    db.Input_Types.AddRange(buttonTypes);
                    db.SaveChanges();

                    XmlNodeList menuProcesses = xmlDoc?.SelectNodes("//MenuProces") ?? null;
                    if (menuProcesses is null) return;

                    //List<Menu_Process> Processes = new List<Menu_Process>();

                    List<Menu_Process> MenussInProces = new List<Menu_Process>();

                    foreach (XmlNode menuProcess in menuProcesses)
                    {
                        XmlNodeList menus = menuProcess.SelectNodes("Menu") ?? null;



                        if (menus is null) break;



                        foreach (XmlNode menu in menus)
                        {

                            var _curentMenu = new Menu_Process();
                            _curentMenu.MyName = menu.Attributes["Name"]?.Value ?? string.Empty;

                            string menuType = menu.Attributes["MenuType"]?.Value ?? string.Empty;
                            _curentMenu.ProcessType = menuTypes?.FirstOrDefault(type => type.Code == menuType);

                            _curentMenu.ProcessMenuCode = menu.Attributes["MenuCode"]?.Value ?? string.Empty;

                            _curentMenu.Navigation = menu.Attributes["Navigation"]?.Value ?? string.Empty;

                            _curentMenu.MenuProcessContent = menu.Attributes["Content"]?.Value ?? string.Empty;
                            _curentMenu.MyDescription = menu.Attributes["Content"]?.Value ?? string.Empty;

                            _curentMenu.IsAwaytingText = Convert.ToBoolean(menu.Attributes["IsAwaytingText"]?.Value ?? "False");
                            _curentMenu.IsDelite = false;
                            _curentMenu.dateTimeCreation = DateTime.Now;





                            XmlNodeList inputsXML = menu?.SelectNodes("Input");

                            foreach (XmlNode input in inputsXML)
                            {
                                Process_Input _input = new Process_Input();

                                _input.MyName = input.Attributes["Name"]?.Value ?? string.Empty;
                                _input.NextProcessMenuCode = input.Attributes["NextMenuNameCode"]?.Value ?? string.Empty;


                                string _inpTypr = input.Attributes["InputType"]?.Value ?? string.Empty;
                                _input.input_Type = buttonTypes.FirstOrDefault(type => type.Code == _inpTypr);
                                _input.MenuProcessCode = _curentMenu.MenuProcessContent;
                                _input.dateTimeCreation = DateTime.Now;
                                _curentMenu.Inputs.Add(_input);


                            }


                            MenussInProces.Add(_curentMenu);



                        }



                        db.Menu_Proceses.AddRange(MenussInProces);
                        db.SaveChanges();
                    }



                    foreach (var procecc in MenussInProces)
                    {

                        foreach (var input in procecc.Inputs)
                        {
                            input.NextProcessMenu = MenussInProces.FirstOrDefault(p => p.ProcessMenuCode == input.NextProcessMenuCode);


                        }

                    }


                    db.Menu_Proceses.UpdateRange(MenussInProces);
                    db.SaveChanges();



                }
                // Выводим содержимое файла
            }
            catch (Exception ex)
            {
                // Обработка ошибок при чтении файла
            }














        }





        private async Task myErrohendler(ITelegramBotClient client, Exception exception, CancellationToken arg3)
        {
            //await myClient.SendTextMessageAsync(, exception.Message, cancellationToken: myCansToken);

            Console.WriteLine($"-------{DateTime.Now}");





            if (exception.InnerException is not null) concoldebuger.badMSG(exception.InnerException.ToString(), client, arg3);
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            //   MyStart();
            //InnerException = {"The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_myUserUpdates_myChats_ChatIdFrom\".
            //The conflict occurred in database \"NewStartBD\", table \"dbo.myChats\", column 'MyId'."}
        }

        //Постоянство  Id = 483856953

        private async Task myUpdate(ITelegramBotClient iClient, Update update, CancellationToken cancellationToken)
        {

            bool isUpdateValide = new TeleTools().checkUpdadate(iClient, update, cancellationToken);

            if (isUpdateValide == false) return;




            concoldebuger.notifMSG($"\n========================= Новое Update {DateTime.Now} =========================", iClient, cancellationToken);
            concoldebuger.notifMSG($"Update - id: {update.Id}| Type: {update.Type}", iClient, cancellationToken);




            //новый update и создание пользователей если их нет 
            string newUpdateResult = await HandlerNewUpdate.newUpdateHendler(iClient, update, cancellationToken);
            concoldebuger.sistemMSG(newUpdateResult, iClient, cancellationToken);
            if (newUpdateResult.ToLower().Contains("false")) return;
            // запись update
            string updateresult = await HandlerNewUpdate.saveUpdateInfo(iClient, update, cancellationToken);
            concoldebuger.sistemMSG(updateresult, iClient, cancellationToken);


            //Обробка пришедшего сообшения 
            string processHendler = await HandlerNewUpdate.processHendler(iClient, update, cancellationToken);






            long teleChatId = TeleTools.GetTeleChatId(update);
            long teleUserId = TeleTools.GetTeleUserId(update);
            MyUser.userType? usertype = MyUser.userTypeById(teleUserId);

            if (usertype == MyUser.userType.admin) new adminHendlerHR().adminHendler(update, iClient, cancellationToken, teleChatId, teleUserId);
            if (usertype == MyUser.userType.regulareUser) new userHendlerHR().regularUserHendler(update, iClient, cancellationToken, teleChatId, teleUserId);







            //изменение последненого update и окончание обработки
            string changeupdateId = await HandlerNewUpdate.changeLastupdateId(iClient, update, cancellationToken);
            concoldebuger.sistemMSG(changeupdateId, iClient, cancellationToken);





            HandlerNewUpdate.testPost(iClient, update, cancellationToken);


        }











    }
}
