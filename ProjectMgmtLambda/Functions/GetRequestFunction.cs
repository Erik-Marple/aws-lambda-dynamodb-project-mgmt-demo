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
using ProjectMgmtLambda.Application.Enums;

namespace ProjectMgmtLambda.Functions
{
    public class GetRequestFunction
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest request)
        {
            using (var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1))
            {
                var requestId = request.PathParameters["requestId"];
                var attributeDictionary = new Dictionary<string, AttributeValue> { { "RequestId", new AttributeValue(requestId) } };
                var getItem = new GetItemRequest("ChangeRequest", attributeDictionary);
                var response = await client.GetItemAsync(getItem);
                var headers = new Dictionary<string, string>
                {
                   { "Access-Control-Allow-Origin", "*" }
                };
                if (response == null || response.Item.Count == 0)
                {
                    return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.NotFound, Headers = headers };
                }
                var item = DeserializeChangeRequest(response);
                var result = JsonConvert.SerializeObject(item, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });                
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.OK, Headers = headers, Body = result };
            }
        }

        private ListChangeRequest DeserializeChangeRequest(GetItemResponse response)
        {
            var result = new ListChangeRequest(
                new Guid(response.Item["RequestId"].S),
                DateTime.Parse(response.Item["RequestDate"].S),
                response.Item["ProjectName"].S,
                response.Item["RequestedBy"].S,
                response.Item["Practice"].S,
                response.Item["ChangeType"].S,
                response.Item["Description"].S);
            return result;
        }
    }
}
