using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public struct PictureBase64String
    {
        public string Picture { get; set; }

        public Guid PictureID { get; set; }
    }
}
