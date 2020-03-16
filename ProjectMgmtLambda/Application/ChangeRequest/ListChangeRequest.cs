using ProjectMgmtLambda.Application.Entities;
using ProjectMgmtLambda.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectMgmtLambda.Application.ChangeRequest
{
   public class ListChangeRequest
   {
        public ListChangeRequest(Guid requestId, DateTime requestDate, string projectName, string requestedBy, string practice, string changeType, string description)
        {
            RequestId = requestId;
            RequestDate = requestDate;
            ProjectName = projectName;
            RequestedBy = requestedBy;
            Practice = practice;
            ChangeType = changeType;
            Description = description;
        }
        public Guid RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string ProjectName { get; set; }
        public string RequestedBy { get; set; }
        public string Practice { get; set; }
        public string ChangeType { get; set; }
        public string Description { get; set; }
   }
}
