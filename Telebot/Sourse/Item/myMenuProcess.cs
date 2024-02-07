using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace Telebot.Sourse.Item
{
    public class myMenuProcess : IItem.IItemDB<myMenuProcess>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }

        

        public virtual List<myButtons> Buttons { get; set; }=new List<myButtons>();
        public menuTypes? MenuType { set; get; }


        public enum menuTypes
        {
            regularButtonMenu,
            listButtonsMenu,

            checkBox,
            onChose,



        }


    }
}
