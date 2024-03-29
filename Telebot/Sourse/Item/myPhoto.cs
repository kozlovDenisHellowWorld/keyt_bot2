
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telebot.Sourse.Item
{
    public class myPhoto : PhotoSize, IItem.IItemDB<myPhoto>
    {
        [Key]
        public int MyId { get ; set ; }
        public string? MyDescription { get ; set ; }
        public string? MyName { get ; set ; }
        public DateTime? dateTimeCreation { get ; set ; }
        public long? BotClientId { get ; set ; }
        public bool? IsDelite { get ; set ; }

        
        public bool? IsStartMSGPhoto { set; get; }

        public string? contetnt { set; get; }



        public static myPhoto createPhot(PhotoSize photoSize)
        {
            myPhoto result = new myPhoto()
            {
                dateTimeCreation = DateTime.Now,
                FileId = photoSize.FileId,
                FileSize = photoSize.FileSize,
                FileUniqueId = photoSize.FileUniqueId,
                IsStartMSGPhoto = false,
                 Width=photoSize.Width,
                  Height =photoSize.Height,
                   IsDelite=false,

            
            };


            return result;
        
        }


        public int? ReqstId { set; get; }
        public virtual requst? Reqst { set; get; }


        /// <summary>
        /// myPhoto - mph:
        /// </summary>
        /// <returns>"mph:{MyId}|"</returns>
        public string GetEntityTypeId()
        {
            return $"mph:{MyId}|";
        }
    }
}
