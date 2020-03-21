using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamoDb.Libs.DynamoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDb_ARTieChatbot.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class DynamoDbController : ControllerBase
    {
        private readonly ICreateTable _dynamoDbClass;
        private readonly IPutItem _putItem;
        public DynamoDbController(ICreateTable dynamoDbClass, IPutItem putItem)
        {
            _dynamoDbClass = dynamoDbClass;
            _putItem = putItem;
        }

        [HttpGet]
        [Route("createTable")]
        public IActionResult CreateDynamoDbTable()
        {
            _dynamoDbClass.CreateDynamoDbTable();
            return Ok();
        }

        [Route("putItems")]
        public IActionResult PutItem([FromQuery] int id, string replyDateTime)
        {
            _putItem.AddNewEntry(id, replyDateTime);
            return Ok();
        }
    }
}