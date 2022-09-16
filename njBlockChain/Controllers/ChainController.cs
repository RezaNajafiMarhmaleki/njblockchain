using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using njBlockChain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace njBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChainController : ControllerBase
    {
        private readonly BlockChain _blockChain;

        private readonly ILogger<ChainController> _logger;
        public ChainController(ILogger<ChainController> logger, BlockChain blockChain)
        {
            _blockChain = blockChain;
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<Block> Get()
        {
            _logger.LogInformation("get mempool");

            return _blockChain.Chain;
        }
    }
}
