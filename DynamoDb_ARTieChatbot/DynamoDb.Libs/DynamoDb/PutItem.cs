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
        public async Task AddNewEntry(string kindOfArtName)
        {
            var queryRequest = RequestBuilder(kindOfArtName);
            await PutItemAsync(queryRequest);
        }
        private PutItemRequest RequestBuilder(string kindOfArtName)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                
                { "KindOfArtName", new AttributeValue { N = kindOfArtName}}

            };
            return new PutItemRequest
            {
                TableName = "KindOfArtTable",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }
    }
}
