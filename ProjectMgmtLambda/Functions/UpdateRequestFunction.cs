using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectMgmtLambda.Application.ChangeRequest;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectMgmtLambda.Functions
{
    public class UpdateRequestFunction
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> RunAsync(APIGatewayProxyRequest request)
        {
            var requestId = request.PathParameters["requestId"];
            var keyDictionary = new Dictionary<string, AttributeValue> { { "RequestId", new AttributeValue(requestId) } };
            var getItem = new GetItemRequest("ChangeRequest", keyDictionary);

            using (var client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1))
            {                
                var response = await client.GetItemAsync(getItem);
                var headers = new Dictionary<string, string>
                {
                   { "Access-Control-Allow-Origin", "*" }
                };
                if (response == null || response.Item.Count == 0)
                {
                    return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.NotFound, Headers = headers };
                }
                var body = JsonConvert.DeserializeObject<ListChangeRequest>(request.Body);
                var updateDictionary = GetUpdateDictionary(body);
                var updateRequest = new UpdateItemRequest("ChangeRequest", keyDictionary, updateDictionary);
                var result = await client.UpdateItemAsync(updateRequest);
                if (result == null || response.Item.Count == 0)
                {
                    return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.BadRequest, Headers = headers };
                }
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.NoContent, Headers = headers };
            }
        }

        private Dictionary<string, AttributeValueUpdate> GetUpdateDictionary(ListChangeRequest request)
        {
            var result = request.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .ToDictionary(p => p.Name, p => new AttributeValueUpdate(new AttributeValue(p.GetValue(request, null).ToString()), AttributeAction.PUT));
            result.Remove("RequestId");
            return result;
        }
    }
}
