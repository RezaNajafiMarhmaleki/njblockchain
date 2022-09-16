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
    public class NodesController : ControllerBase
    {
        private readonly ILogger<NodesController> _logger;
        private readonly BlockChain _blockChain;
        public NodesController(ILogger<NodesController> logger, BlockChain blockChain)
        {
            _logger = logger;
            _blockChain = blockChain;
        }

        // GET: api/<NodesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _blockChain.Nodes;
        }         

        // POST api/<NodesController>
        [HttpPost]
        public void Post(string node_address)
        {
            _blockChain.RegisterNode(node_address);
        }

       
    }
}
