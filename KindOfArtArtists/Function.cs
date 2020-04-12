using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer))]

namespace KindOfArtArtists
{
    public class Function
    {
        public static readonly RegionEndpoint EUWest1;

        AmazonDynamoDBClient client = new AmazonDynamoDBClient("", "", EUWest1);
        /// <summary>
        /// Then entry point for the Lambda function that looks at the current intent and calls 
        /// the appropriate intent process.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public LexResponse FunctionHandler(LexEvent lexEvent, ILambdaContext context)
        {

            IIntentProcessor process;

            if (lexEvent.CurrentIntent.Name == "KindOfArtArtists")
            {
                var result = ScanAsync(RequestBuilder());

                process = new KindOfArtArtistsIntentProcessor(result);

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
