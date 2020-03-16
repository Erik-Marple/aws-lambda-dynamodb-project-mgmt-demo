using ProjectMgmtLambda.Application.Enums;
using System;
using System.Collections.Generic;

namespace ProjectMgmtLambda.Application.Entities
{
   public class Item
   {
        public DateTime RequestDate { get; set; }
        public string ProjectName { get; set; }
        public string RequestedBy { get; set; }
        public string Description { get; set; }
        public Practice Practice { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}
