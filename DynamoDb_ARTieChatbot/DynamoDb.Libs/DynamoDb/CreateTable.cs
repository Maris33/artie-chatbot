using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.Libs.DynamoDb
{
    public class CreateTable : ICreateTable
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = "ArtistsInfo";

        public CreateTable(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable()
        {
            try
            {
                CreateTempTable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void CreateTempTable()
        {
            Console.WriteLine("Creating Table. Please wait...");

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "ArtistName",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "ArtistName",
                        KeyType = "HASH"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };
            var responce = _dynamoDbClient.CreateTableAsync(request);
            WaitUntilTableReady(tableName);
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;
            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    
                }

            } while (status != "ACTIVE");
            {
                Console.WriteLine("The table was created successfully!");
            }
        }
    }

    
}
