using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace njBlockChain.Models
{
    public class BlockChain
    {
        private Queue<Trx> current_trxs;
        private List<Block> chain;
        private const int VALID_PROOF_LEN = 4;
        private const int DEFAULT_PROOF = 100;
        private const string VALID_PROOF_STR = "0000";
        private const string GET_CHAIN_ENDPOINT = "Chain";
        private Dictionary<string, string> users;
        private HashSet<string> nodes;

        public BlockChain()
        { // defines a block chain on one machine
            chain = new List<Block>();
            current_trxs = new Queue<Trx>();
            users = new Dictionary<string, string>();
            nodes = new HashSet<string>();
           
            this.RegisterUser("reza najaf");
            this.RegisterUser("njnj");
            this.RegisterNode("http://localhost:54872");
            this.CreateBlock(DEFAULT_PROOF, "0");
        }

        public List<Block> Chain { get { return chain; } }
        public Queue<Trx> Trxs { get { return current_trxs; } }
        public Dictionary<string, string> Users { get { return users; } }
        public HashSet<string> Nodes { get { return nodes; } }

        public string RegisterUser(string username)
        {
            if (!users.ContainsKey(username))
                users.Add(username, Guid.NewGuid().ToString());
            return users[username];
        }
        public void RegisterNode(string nodeAddress)
        {
            Uri url = new Uri(nodeAddress);
            nodes.Add(url.ToString());
           
        }
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
        public async Task<bool> ResolveConflict()
        {
            //check all nodes and select best chain

            var Neighbors = this.nodes;
            List<Block> bestChain = null;
            int max_len = this.chain.Count;

            foreach (var neighbor in Neighbors)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(neighbor + GET_CHAIN_ENDPOINT))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var neighborchain = JsonConvert.DeserializeObject<List<Block>>(result);
                        if (neighborchain == null) continue;
                        if (neighborchain.Count > max_len && this.valid_chain(neighborchain))
                        {
                            max_len = neighborchain.Count;
                            bestChain = neighborchain;
                        }
                    }
                }
            }

            if (bestChain != null)
            {
                this.chain = bestChain;
                return true;
            }
            return false;
        }
        private bool valid_chain(List<Block> chain)
        {
            //check the valid chain

            if (chain == null) return false;
            if (chain.Count == 0) return false;

            Block lastBlock = chain[0];
            int chainlen = chain.Count;
            int currentindex = 1;
            while(currentindex< chainlen)
            {
                if (!chain[currentindex].previous_hash.Equals(this.Hash(lastBlock)))
                    return false;
                if (!this.valid_proof(chain[currentindex]))
                    return false;

                lastBlock = chain[currentindex++];
            }
            return true;
        }

        private bool valid_proof(Block block)
        {
            //check if this proof is fine or not          
            string hashed64 = Hash(block);
            return hashed64.Substring(hashed64.Length - (VALID_PROOF_LEN + 1), VALID_PROOF_LEN).Equals(VALID_PROOF_STR);
        }
        private Block proof_of_work()
        {
            //show that the work is done
            
            //can running parallel 
            Block currBlock = CurrentBlock(0);
            while (!valid_proof(currBlock))
                currBlock.proof++;       

            return currBlock;
        }

        public Block MineCurrentBlock(string Recipient)
        {
            Block lastBlock = this.LastBlock;
            this.Append_Trx(new Trx { sender = "", recipient = Recipient, amount = 12.5M });
            var validBlock = this.proof_of_work();
            this.AddBlock(validBlock);
            return validBlock;
        }

    }
}
