using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace njBlockChain.Models
{
    public class BlockChain
    {
        private Queue<Trx> current_trxs;
        private List<Block> chain;
        private const int VALID_PROOF_LEN = 4;
        private const int DEFAULT_PROOF = 100;
        private const string VALID_PROOF_STR = "0000";

        public BlockChain()
        { // defines a block chain on one machine
            chain = new List<Block>();
            current_trxs = new Queue<Trx>();
            this.CreateBlock(DEFAULT_PROOF, "0");
        }

        public List<Block> Chain { get { return chain; } }
        public Queue<Trx> Trxs { get { return current_trxs; } }
        private Block CurrentBlock(long proof, string default_prevhash = "")
        {
            return  new Block
            {
                Index = chain.Count + 1,
                timestamp = DateTime.Now.Ticks,
                trxes = current_trxs.ToArray(),
                proof = proof,
                previous_hash = string.IsNullOrWhiteSpace(default_prevhash) ? this.Hash(chain[chain.Count - 1]) : default_prevhash,
            };

        }
        public Block CreateBlock(long proof, string default_prevhash = "")
        {// create new block
            Block block = CurrentBlock(proof, default_prevhash);
             AddBlock(block);
            return block;
        }
        private void AddBlock(Block block)
        {// add  block to chain
           
            chain.Add(block);
            current_trxs = new Queue<Trx>();           
        }

        public long Append_Trx(Trx trx)
        {
            //add a new trx to the mempool
            current_trxs.Enqueue(trx);
            return LastBlock.Index + 1;
        }

        private string Hash(Block block)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // JsonConvert.SerializeObject(block);
                byte[] salt = Encoding.UTF8.GetBytes(block.ToString());
                byte[] hashed = sha256.ComputeHash(salt);

                return Convert.ToBase64String(hashed);
            }

        }

        public Block LastBlock
        {// get last block
            get
            {
                return chain[chain.Count - 1];
            }
        }

        private bool valid_proof(long last_proof, Block block)
        {//check if this proof is fine or not
            using (SHA256 sha = SHA256.Create())
            {
                string hashed64 = Hash(block); 

                return hashed64.Substring(hashed64.Length - (VALID_PROOF_LEN + 1), VALID_PROOF_LEN).Equals(VALID_PROOF_STR);
            }
        }
        private Block proof_of_work(long lastProof)
        {
            //show that the work is done
          
            Block currBlock = CurrentBlock(0);
            while (!valid_proof(lastProof, currBlock))
                currBlock.proof++;       

            return currBlock;
        }

        public Block MineCurrentBlock(string Recipient)
        {
            Block lastBlock = this.LastBlock;
            this.Append_Trx(new Trx { sender = "", recipient = Recipient, amount = 12.5M });
            var validBlock = this.proof_of_work(lastBlock.proof);
            this.AddBlock(validBlock);
            return validBlock;
        }

    }
}
