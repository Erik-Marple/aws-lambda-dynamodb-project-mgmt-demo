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
using ProjectMgmtLambda.Application.Entities;

namespace ProjectMgmtLambda.Functions
{
   public class CreateRequestFunction
   {
      [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
      public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest request)
      {
         var requestModel = new CreateChangeRequest(JsonConvert.DeserializeObject<Item>(request.Body));
         var putItem = requestModel.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .ToDictionary(p => p.Name, p => new AttributeValue(p.GetValue(requestModel, null).ToString()));
         var putRequest = new PutItemRequest("ChangeRequest", putItem);
         using (var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1))
         {
            var response = await client.PutItemAsync(putRequest);
            var headers = new Dictionary<string, string>
            {
               { "Access-Control-Allow-Origin", "*" }
            };
                var body = JsonConvert.SerializeObject(requestModel, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.Created, Headers = headers, Body = body };
         }
      }      
   }
}
