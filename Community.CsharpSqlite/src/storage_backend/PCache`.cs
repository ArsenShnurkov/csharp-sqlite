namespace Community.CsharpSqlite
{
    using System;

    using Pgno = System.UInt32;

    /* Each page cache is an instance of the following object.  Every
    ** open database file (including each in-memory database and each
    ** temporary or transient database) has a single page cache which
    ** is an instance of this object.
    **
    ** Pointers to structures of this type are cast and returned as 
    ** opaque sqlite3_pcache* handles.
    */
    public class PCache1
    {
        /* Cache configuration parameters. Page size (szPage) and the purgeable
      ** flag (bPurgeable) are set when the cache is created. nMax may be 
      ** modified at any time by a call to the pcache1CacheSize() method.
      ** The PGroup mutex must be held when accessing nMax.
      */
        public PGroup pGroup;              /* PGroup this cache belongs to */
        public int szPage;                 /* Size of allocated pages in bytes */
        public bool bPurgeable;            /* True if cache is purgeable */
        public int nMin;                   /* Minimum number of pages reserved */
        public int nMax;                   /* Configured "cache_size" value */
        public int n90pct;                 /* nMax*9/10 */

        /* Hash table of all pages. The following variables may only be accessed
        ** when the accessor is holding the PGroup mutex.
        */
        public int nRecyclable;             /* Number of pages in the LRU list */
        public int nPage;                   /* Total number of pages in apHash */
        public int nHash;                   /* Number of slots in apHash[] */
        public PgHdr1[] apHash;             /* Hash table for fast lookup by key */

        public Pgno iMaxKey;                /* Largest key seen since xTruncate() */

        public void Clear()
        {
            nRecyclable = 0;
            nPage = 0;
            nHash = 0;
            apHash = null;
            iMaxKey = 0;
        }
    }
}

