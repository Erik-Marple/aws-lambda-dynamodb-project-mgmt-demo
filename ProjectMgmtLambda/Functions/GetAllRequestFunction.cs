using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectMgmtLambda.Application.ChangeRequest;

namespace ProjectMgmtLambda.Functions
{
   public class GetAllRequestFunction
   {
      [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
      public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest request)
      {         
         using (var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1))
         {
            var response = await client.ScanAsync(new ScanRequest("ChangeRequest"));
            var items = DeserializeChangeRequests(response);
            var result = JsonConvert.SerializeObject(items, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var headers = new Dictionary<string, string>
            {
               { "Access-Control-Allow-Origin", "*" }
            };
            return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.OK, Headers = headers, Body = result };
         }
      }

      private List<ListChangeRequest> DeserializeChangeRequests(ScanResponse response)
      {
         var result = new List<ListChangeRequest>();
         foreach (var i in response.Items)
         {
            var dbItem = new ListChangeRequest(new Guid(i["RequestId"].S), DateTime.Parse(i["RequestDate"].S), i["ProjectName"].S, i["RequestedBy"].S, i["Practice"].S, i["ChangeType"].S, i["Description"].S);
            result.Add(dbItem);
         }
         return result;
      }
   }
}
