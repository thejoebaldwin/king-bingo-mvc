using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace KingBingo.Models
{
    public class Badge
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BadgeID { get; set; }
        public string Name { get; set; }
      

        public byte[] Image { get; set; }

        public string Description { get; set; }

        public static byte[] GetImage(string name)
        {
            System.Drawing.Image img =  System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Images/Badges/" + name));
            MemoryStream ms = new MemoryStream();
            if (name.ToLower().Contains(".jpg") || name.ToLower().Contains(".jpeg"))
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else if (name.ToLower().Contains(".png"))
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }

            return ms.ToArray();
        }

    }
}