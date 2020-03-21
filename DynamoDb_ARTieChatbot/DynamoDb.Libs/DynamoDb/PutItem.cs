using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public class PutItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public PutItem(IAmazonDynamoDB dynamoDB)
        {
            _dynamoClient = dynamoDB;
        }
        public async Task AddNewEntry(int id, string replyDateTime)
        {
            var queryRequest = RequestBuilder(id, replyDateTime);
            await PutItemAsync(queryRequest);
        }
        private PutItemRequest RequestBuilder(int id, string replyDateTime)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { N = id.ToString()}},
                { "ReplyDateTime", new AttributeValue { N = replyDateTime}}

            };
            return new PutItemRequest
            {
                TableName = "Table",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }
    }
}
