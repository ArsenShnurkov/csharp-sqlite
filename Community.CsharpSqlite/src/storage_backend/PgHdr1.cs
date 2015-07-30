namespace Community.CsharpSqlite
{
    using System;

    using Pgno = System.UInt32;
    
    /*
    ** Each cache entry is represented by an instance of the following 
    ** structure. A buffer of PgHdr1.pCache.szPage bytes is allocated 
    ** directly before this structure in memory (see the PGHDR1_TO_PAGE() 
    ** macro below).
    */
    public class PgHdr1
    {
        public Pgno iKey;                   /* Key value (page number) */
        public PgHdr1 pNext;                /* Next in hash table chain */
        public PCache1 pCache;              /* Cache that currently owns this page */
        public PgHdr1 pLruNext;             /* Next in LRU list of unpinned pages */
        public PgHdr1 pLruPrev;             /* Previous in LRU list of unpinned pages */

        // For C#
        public PgHdr pPgHdr = new PgHdr();   /* Pointer to Actual Page Header */

        public void Clear()
        {
            this.iKey = 0;
            this.pNext = null;
            this.pCache = null;
            this.pPgHdr.Clear();
        }

    }
}

