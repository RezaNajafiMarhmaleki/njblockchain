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
  public  class MineController: ControllerBase
    {
        private readonly BlockChain _blockChain;

        private readonly ILogger<MineController> _logger;
        public MineController(ILogger<MineController> logger,BlockChain blockChain)
        {
            _blockChain = blockChain;
            _logger = logger;
        }

        [HttpGet("{uid}")]
        public Block Get(Guid uid)
        {
            _logger.LogInformation("Mine: run the proof of work algorithm to get the next proof");

            return _blockChain.MineCurrentBlock(uid.ToString());
        }

        [HttpGet]
        public async Task< dynamic> Consensus()
        {
            _logger.LogInformation("Resolve confilicts");

            var result =await _blockChain.ResolveConflict();
            if (result)
                return new { Message = "Chain replaced!" };
            return new { Message = "Im the best!" }; 
        }


    }
}
