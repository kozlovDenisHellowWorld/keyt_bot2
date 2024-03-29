using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telebot.Sourse.Handlers
{
    public  static class concoldebuger
    {
        private static long adminId = 6021604487;

        public static async void goodMSG(string msg, ITelegramBotClient client, CancellationToken cts)
        {
          //  await client.SendTextMessageAsync(adminId,("Progra--" + msg), cancellationToken:cts);


            if (msg.Contains("true"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Progra--"+msg);
                Console.ResetColor();
            }
            else if (msg.Contains("false"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }
            else if (msg.Contains("notbad"))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }

        }

        public static async void goodMSG(string msg)
        {
            //  await client.SendTextMessageAsync(adminId,("Progra--" + msg), cancellationToken:cts);


            if (msg.Contains("true"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }
            else if (msg.Contains("false"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }
            else if (msg.Contains("notbad"))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Progra--" + msg);
                Console.ResetColor();
            }

        }



        public static async void badMSG(string msg, ITelegramBotClient client, CancellationToken cts)
        {
          //  await client.SendTextMessageAsync(adminId, ("Progra--" + msg), cancellationToken: cts);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Progra--" + msg);
            Console.ResetColor();

        }

        public static async void notifMSG(string msg, ITelegramBotClient client, CancellationToken cts)
        {
           // await client.SendTextMessageAsync(adminId, ("Progra--" + msg), cancellationToken: cts);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( msg);
            Console.ResetColor();

        }

        public static async void sistemMSG(string msg, ITelegramBotClient client, CancellationToken cts)
        {
          //  await client.SendTextMessageAsync(adminId, ("Progra--" + msg), cancellationToken: cts);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Progra--" + msg);
            Console.ResetColor();

        }


    }
}
