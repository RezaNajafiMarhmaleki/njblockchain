 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace njBlockChain
{
    class Block
    {
        public long Index { set; get; }

        public DateTime timestamp { set; get; }

        public Trx [] trxes { set; get; }

        public long proof { set; get; }

        public string previous_hash { set; get; }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}{4}" ,this.Index,
                this.timestamp,
                string.Join(';', this.trxes.Select(x=>x.ToString())),
                this.proof,this.previous_hash);
        }
    }
}
