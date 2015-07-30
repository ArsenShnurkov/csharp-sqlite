using System;

namespace Community.CsharpSqlite
{
    public class codec_ctx
    {//typedef struct {
        public int mode_rekey;
        public byte[] buffer;
        public Btree pBt;
        public cipher_ctx read_ctx;
        public cipher_ctx write_ctx;

        public codec_ctx Copy()
        {
            codec_ctx c = new codec_ctx();
            c.mode_rekey = mode_rekey;
            c.buffer = sqlite3MemMalloc( buffer.Length );
            c.pBt = pBt;
            if ( read_ctx != null )
                c.read_ctx = read_ctx.Copy();
            if ( write_ctx != null )
                c.write_ctx = write_ctx.Copy();
            return c;
        }
    }
}

