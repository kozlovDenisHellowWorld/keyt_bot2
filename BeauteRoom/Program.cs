// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using Telebot;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;



internal class Program
{
    private static void Main(string[] args)
    {
       

        Console.WriteLine("Hello, World!");
        Console.WriteLine("___________________________");
        TeleClient Hr_bot;
        TeleClient Apelsin;
        //GrupeAdmin
        //TeleClient bot = new TeleClient("6378750640:AAFISGXTu7dK6L9yaHA_lHfncvE8dpD4tyc");

        Hr_bot = new TeleClient();

        AppDomain.CurrentDomain.ProcessExit += new EventHandler(Hr_bot.OnProcessExit);
        Console.CancelKeyPress += new ConsoleCancelEventHandler(Hr_bot.OnCancelKeyPress);


        Console.ReadKey();



    }


 
}