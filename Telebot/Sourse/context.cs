using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using Telebot.Sourse.Handlers;
using Telebot.Sourse.Item;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;

namespace Telebot.Sourse
{
    public class context : DbContext
    {
        //public string databaseName = "Hr_req_db"; // Имя базы данных

        bool backUp = false;
        string bakUpPaath = "";

       public string DbName { get; private set; }
        private bool IsloadXML=false;

        public  string databaseName { 
            get 
            {
                if (IsloadXML == true) return DbName;
                string databas = "Hr_req_db";
                if (Debugger.IsAttached) databas = "NewStartBD";
                return databas;
            }
        
        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string databaseName = "Hr_req_db"; // Имя базы данных

            //if (Debugger.IsAttached) databaseName = "NewStartBD";

         //   string databasePath = Path.Combine(Directory.GetCurrentDirectory(), $"{databaseName}.mdf"); // Полный путь к базе данных

            string databasePath = Path.Combine(Directory.GetCurrentDirectory(), $"{databaseName}.mdf"); // Полный путь к базе данных




            //optionsBuilder.UseLazyLoadingProxies().UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection=True;");

            optionsBuilder.UseLazyLoadingProxies().UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection=True;");






        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder
.Entity<User_Types>()
.HasMany(u => u.Processes)
.WithOne(p => p.UserType)
.HasForeignKey(p => p.UserTypeId);



            modelBuilder
.Entity<Menu_Process>()
.HasMany(u => u.Chats)
.WithOne(p => p.CurentProcess)
.HasForeignKey(p => p.CurentProcessId);




            modelBuilder
      .Entity<Input_Type>()
      .HasMany(u => u.All_Inputs)
      .WithOne(p => p.input_Type)
      .HasForeignKey(p => p.input_TypeId);



            modelBuilder
        .Entity<Menu_ProcessType>()
        .HasMany(u => u.Menus)
        .WithOne(p => p.ProcessType)
        .HasForeignKey(p => p.ProcessTypeId);



            modelBuilder
        .Entity<User_Types>()
        .HasMany(u => u.Users)
        .WithOne(p => p.Type)
        .HasForeignKey(p => p.Type_id);





            modelBuilder
        .Entity<Menu_Process>()
        .HasMany(u => u.Inputs)
        .WithOne(p => p.MenuProcess)
        .HasForeignKey(p => p.MenuProcessId);


            modelBuilder
        .Entity<Menu_Process>()
        .HasMany(u => u.CallingInputs)
        .WithOne(p => p.NextProcessMenu)
        .HasForeignKey(p => p.NextProcessMenuId);








            //----



            modelBuilder
        .Entity<MyChat>()
        .HasMany(u => u.Update)
        .WithOne(p => p.ChatFrom)
        .HasForeignKey(p => p.ChatFromId);




            modelBuilder
        .Entity<myMenuProcess>()
        .HasMany(u => u.Buttons)
        .WithOne(p => p.CurentMenu)
        .HasForeignKey(p => p.CurentMenuId);


            modelBuilder
    .Entity<MyChat>()
    .HasMany(u => u.Requsts)
    .WithOne(p => p.chat)
    .HasForeignKey(p => p.chatId);

            modelBuilder
.Entity<MyUser>()
.HasMany(u => u.Requsts)
.WithOne(p => p.user)
.HasForeignKey(p => p.userid);



            modelBuilder
 .Entity<requst>()
 .HasMany(u => u.Photoes)
 .WithOne(p => p.Reqst)
 .HasForeignKey(p => p.ReqstId);

            //     modelBuilder
            //.Entity<Process>()
            //.HasMany(u => u.PriviousProcess)
            //.WithOne(p => p.PriviousProcess)
            //.HasForeignKey(p => p.PriviousProcessId);

            modelBuilder.Entity<BotProperties>().HasData(
            new BotProperties[]
            {
                new BotProperties { MyId=1,  LastUpdateId=0, BotClientId=6483803548},
                new BotProperties { MyId=2,  LastUpdateId=0, BotClientId=6240171220},

            });
            //вынести в конфиг



            //------
            var startMenu_admin = new myMenuProcess()
            {
                dateTimeCreation = DateTime.Now,
                MyId = 1,
                BotClientId = 6240171220,
                MyName = "меню администратора",
                IsDelite = false,

            };
            startMenu_admin.MyDescription = $"Obj type: {startMenu_admin.GetType().Name}|Id BD: {startMenu_admin.MyId}|Text: {startMenu_admin.MyName}";





            //-------
            //var button_startmenu_createprocess = new myButtons() 
            //{ 
            //    BotClientId= 6240171220,
            //    CurentMenu= startMenu_admin,
            //     CurentMenuId= startMenu_admin.MyId,
            //     dateTimeCreation= DateTime.Now,
            //      isCreated= true,
            //       IsDelite= false,
            //        MyId=1,
            //         NextMenu=null,
            //          MyName="Создать новый опрос",

                       

            //};



            modelBuilder.Entity<myMenuProcess>().HasData(
            new myMenuProcess[]
            {
                startMenu_admin
            });


        }



        public context()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();




        }


        public context(string dbName)
        {
          

            DbName = dbName;
            IsloadXML = true;

            Database.EnsureCreated();





        }


        public void reSetBD(bool needDelete, ITelegramBotClient client, CancellationToken cts)
        {

            if (needDelete)
            {

                Database.EnsureDeleted();
                Console.WriteLine("------------------------------------------------");
                 concoldebuger.goodMSG("бд обнавлена", client, cts);
                Console.WriteLine("------------------------------------------------");
            }
            if (Debugger.IsAttached) Database.EnsureCreated();


        }





        public static void BackupDatabase(DbContext dbContext)
        {
            string backupPath = Directory.GetCurrentDirectory();

            string connectionString = dbContext.Database.GetConnectionString();
            string databaseName = GetDatabaseNameFromConnectionString(connectionString);
            string backupFileName = $"{databaseName}_{DateTime.Now:yyyyMMddHHmmss}.bak";
            string fullBackupPath = Path.Combine(backupPath, backupFileName);

            string sqlCommand = $"BACKUP DATABASE [{databaseName}] TO DISK = '{fullBackupPath}'";

            dbContext.Database.ExecuteSqlRaw(sqlCommand);
        }

        private static string GetDatabaseNameFromConnectionString(string connectionString)
        {
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }






        public DbSet<MyUser> MyUsers { set; get; }
        public DbSet<MyChat> myChats { set; get; }

        public DbSet<MyUserUpdates> myUserUpdates { set; get; }


        public DbSet<BotProperties> BotProperties { set; get; }

        public DbSet<myMenuProcess> myProcessMenues { set; get; }

        public DbSet<myButtons> myButtons { set; get; }

        public DbSet<PriviosMSG> priviosMSG { set; get; }


        public DbSet<requst> Requst { set; get; }

        public DbSet<myPhoto> myPhotoes { set; get; }


        public DbSet<Dinamic_Butons> dinamic_Butons { set; get; }





        public DbSet<User_Types> User_Types { set; get; }
        public DbSet<Input_Type> Input_Types { set; get; }
        public DbSet<Menu_Process> Menu_Proceses { set; get; }
        public DbSet<Menu_ProcessType> Menu_ProcessTypes { set; get; }
        public DbSet<Process_Input> Inputs { set; get; }

        //public DbSet<myPhoto> myPhotoes { set; get; }

        //public DbSet<PriviosMSG> MessagesNeeTodelite { set; get; }

        // public DbSet<Process> Processes { set; get; }
        //public DbSet<MyBtns>  MyBtns { set; get; }




        //
        //  public DbSet<Item.WorckItems.ItemType> ObjTypes  { set; get; }


    }
}
