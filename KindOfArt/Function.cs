using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization;
using Amazon.Lambda.LexEvents;
using Amazon.DynamoDBv2;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
//using KindOfArt.DynamoDbClient;
using Amazon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace KindOfArt
{
    public class Function
    {
        public static readonly RegionEndpoint EUWest1;
      
        AmazonDynamoDBClient client = new AmazonDynamoDBClient("", "", EUWest1);
        public LexResponse FunctionHandler(LexEvent lexEvent, ILambdaContext context)
        {
          
            IIntentProcessor process;

            if (lexEvent.CurrentIntent.Name == "kindOfArtInfo")
            {
                var result = ScanAsync(RequestBuilder());
              
                process = new KindOfArtInfoIntentProcessor(result);
                
            }
            else
            {
                throw new Exception($"Intent with name {lexEvent.CurrentIntent.Name} not supported");
            }


            return process.Process(lexEvent, context);
        }

        
        public async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var responce = await client.ScanAsync(request);
            return responce;
        }

        public ScanRequest RequestBuilder()
        {
            return new ScanRequest
            {
                TableName = "KindOfArtTable"
            };
        }
        
    }
}
