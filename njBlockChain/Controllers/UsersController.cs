using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using njBlockChain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace njBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly BlockChain _blockChain;
        public UsersController(ILogger<UsersController> logger, BlockChain blockChain)
        {
            _logger = logger;
            _blockChain = blockChain;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<KeyValuePair< string,string>> Get()
        {
            return  _blockChain.Users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{username}")]
        public string Get(string username)
        {
          string id=   _blockChain.RegisterUser(username);
           
            return  id;
        }

      
    }
}
