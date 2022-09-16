using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using njBlockChain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace njBlockChain
{  [ApiController]
    [Route("[controller]")]
    class MineController: ControllerBase
    {
        private readonly BlockChain _blockChain;

        private readonly ILogger<MineController> _logger;
        public MineController(ILogger<MineController> logger,BlockChain blockChain)
        {
            _blockChain = blockChain;
            _logger = logger;
        }
 

        [HttpGet]
        public IEnumerable<Block> Get()
        {
            _logger.LogInformation("add a new trxs");

            return _blockChain.Chain;
        }

        
        [HttpGet ("{id}")]
        public Block Get(Guid id)
        {
            _logger.LogInformation("Mine: run the proof of work algorithm to get the next proof");

            return _blockChain.MineCurrentBlock(id.ToString());
        }

       
    }
}
