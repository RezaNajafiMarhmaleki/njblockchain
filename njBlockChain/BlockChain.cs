using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace njBlockChain
{
    class BlockChain
    {
        private Queue<Trx> current_trxs;
        private List<Block> chain;
        private const int VALID_PROOF_LEN= 4;
        private const string VALID_PROOF_STR = "0000";

        public BlockChain()
        { // defines a block chain on one machine
            chain = new List<Block>();
            current_trxs = new Queue<Trx>();
            this.new_block(100, "0");
        }

        public void new_block(int proof , string default_prevhash="" )
        {// create new block
            Block block = new Block
            {
                Index = chain.Count + 1,
                timestamp = DateTime.Now.ToUniversalTime(),
                trxes = current_trxs.ToArray(),
                proof = proof,
                previous_hash = string.IsNullOrWhiteSpace(default_prevhash) ?chain[chain.Count-1].previous_hash: default_prevhash,
            };

            chain.Add(block);

            current_trxs = new Queue<Trx>();
        }

        public long add_trx(Trx   trx)
        {
            //add a new trx to the mempool
            current_trxs.Enqueue(trx);
            return last_block.Index + 1;
        }

        private string hash(Block block)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // JsonConvert.SerializeObject(block);
                byte[] salt = Encoding.UTF8.GetBytes(block.ToString());
                byte[] hashed = sha256.ComputeHash(salt);

                return Convert.ToBase64String(hashed).Replace("-", "");
            }

        }

        public Block last_block
        {// get last block
            get
            {
                return chain[chain.Count - 1];
            }
        }

        private bool valid_proof(long last_proof,long proof)
        {//check if this proof is fine or not
            using(SHA256 sha = SHA256.Create())
            {
                string salt = $"{last_proof}{proof}";
                byte[] saltbyte = Encoding.UTF8.GetBytes(salt);
                byte[] hashed = sha.ComputeHash(saltbyte);

                string hashed64 = Convert.ToBase64String(hashed).Replace("-", "");

                return hashed64.Substring(hashed64.Length - VALID_PROOF_LEN+1, VALID_PROOF_LEN).Equals(VALID_PROOF_STR);
            }
        }
        private long proof_of_work(long last_proof)
        {
            //show that the work is done
            long proof = 0;
            while (valid_proof(last_proof, proof))
                proof++;
            return proof;
        }

    }
}
