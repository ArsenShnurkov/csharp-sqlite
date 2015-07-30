namespace Community.CsharpSqlite
{
    using System;
    using System.Text;

    using i64 = System.Int64;

    /*
    ** An objected used to accumulate the text of a string where we
    ** do not necessarily know how big the string will be in the end.
    */
    public class StrAccum
    {
        public sqlite3 db;          /* Optional database for lookaside.  Can be NULL */
        //public StringBuilder zBase; /* A base allocation.  Not from malloc. */
        public StringBuilder zText; /* The string collected so far */
        //public int nChar;           /* Length of the string so far */
        //public int nAlloc;          /* Amount of space allocated in zText */
        public int mxAlloc;         /* Maximum allowed string length */
        // Cannot happen under C#
        //public u8 mallocFailed;   /* Becomes true if any memory allocation fails */
        //public u8 useMalloc;        /* 0: none,  1: sqlite3DbMalloc,  2: sqlite3_malloc */
        //public u8 tooBig;           /* Becomes true if string size exceeds limits */
        public Mem Context;

        public StrAccum( int n )
        {
            db = null;
            //zBase = new StringBuilder( n );
            zText = new StringBuilder( n );
            //nChar = 0;
            //nAlloc = n;
            mxAlloc = 0;
            //useMalloc = 0;
            //tooBig = 0;
            Context = null;
        }

        public i64 nChar
        {
            get
            {
                return zText.Length;
            }
        }

        public bool tooBig
        {
            get
            {
                return mxAlloc > 0 && zText.Length > mxAlloc;
            }
        }
    }
}

