using ProjectMgmtLambda.Application.Entities;
using System;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Text;
using ProjectMgmtLambda.Application.Enums;

namespace ProjectMgmtLambda.Application.ChangeRequest
{
   public class CreateChangeRequest : Item
   {
        public CreateChangeRequest()
        {

        }

        public CreateChangeRequest(Item item)
        {
            RequestId = Guid.NewGuid();
            RequestDate = item.RequestDate;
            ProjectName = item.ProjectName;
            RequestedBy = item.RequestedBy;
            Description = item.Description;
            Practice = item.Practice;
            ChangeType = item.ChangeType;
        }
      
        [DynamoDBHashKey]
        public Guid RequestId { get; set; }
        [DynamoDBProperty]
        public new DateTime RequestDate { get; set; }
        [DynamoDBProperty]
        public new string ProjectName { get; set; }
        [DynamoDBProperty]
        public new string RequestedBy { get; set; }
        [DynamoDBProperty]
        public new string Description { get; set; }
        [DynamoDBProperty]
        public new Practice Practice { get; set; }
        [DynamoDBProperty]
        public new ChangeType ChangeType { get; set; }

    }
}
