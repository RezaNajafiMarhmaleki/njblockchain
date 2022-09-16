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
    public class TrxsController : ControllerBase
    {
        private readonly ILogger<TrxsController> _logger;
        private readonly BlockChain _blockChain;

        public TrxsController(ILogger<TrxsController> logger, BlockChain blockChain)
        {
            _blockChain = blockChain;
            _logger = logger;
        }

        // GET: api/<TrxsController>
        [HttpGet]
        public IEnumerable<Trx> Get()
        {
            _logger.LogInformation("return current trxs");
            return _blockChain.Trxs;
        }

        // POST api/<TrxsController>
        [HttpPost]
        public void Post([FromBody] Trx trx)
        {
            _logger.LogInformation("add a new trxs");

            long block_index = _blockChain.Append_Trx(trx);

                //return new Result { Message = $"trx added to Block index{block_index}" };
        }


        // DELETE api/<TrxsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
