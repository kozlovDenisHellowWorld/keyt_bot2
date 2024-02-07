using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Telebot.Sourse.Item.IItem;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class BotProperties : IItemDB<BotProperties>
    {
        [Key]
        public int MyId { get; set; }
        public string? MyDescription { get; set; }
        public string? MyName { get; set; }
        public long? BotClientId { set; get; }

        public bool? IsDelite { set; get; }
      

        public DateTime? dateTimeCreation { set; get; }

        public long? LastUpdateId { get; set; }

        public bool? NeedToUpdate { get; set; } 


    }
}
