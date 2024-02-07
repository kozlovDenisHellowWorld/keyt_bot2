using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class myButtons : IItem.IItemDB<myMenuProcess>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public DateTime? dateTimeCreation { get; set; }
        public long? BotClientId { get; set; }
        public bool? IsDelite { get; set; }


        public bool? isCreated { set; get; }

        public MyUser.userType? userType {set;get;}



        public int CurentMenuId { get; set; }
        [ForeignKey("CurentMenuId")]
        public virtual myMenuProcess? CurentMenu { set; get; }



    
        public virtual myMenuProcess? NextMenu { set; get; }




    }
}
