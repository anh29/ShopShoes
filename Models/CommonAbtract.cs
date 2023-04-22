using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopLearning.Models
{
    public abstract class CommonAbtract
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedrDate { get; set; }
        public string ModifiedrBy { get; set; }
    }
}