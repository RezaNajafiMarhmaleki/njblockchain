using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        Dictionary<string, string> _uids;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ILogger<UsersController> logger)
        {
            _uids = new Dictionary<string, string>();           
            _logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<KeyValuePair< string,string>> Get()
        {
            return _uids;
        }

        // GET api/<UsersController>/5
        [HttpGet("{username}")]
        public string Get(string username)
        {
            if(_uids.ContainsKey(username))return _uids[username];
            _uids.Add(username, Guid.NewGuid().ToString());
            return _uids[username];
        }

      
    }
}
