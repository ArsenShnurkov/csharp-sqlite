namespace Community.CsharpSqlite
{
    using System;

    using sqlite3_pcache = PCache1;

    /*
    ** A complete page cache is an instance of this structure.
    */
    public class PCache
    {
        public PgHdr pDirty, pDirtyTail;           /* List of dirty pages in LRU order */
        public PgHdr pSynced;                      /* Last synced page in dirty page list */
        public int _nRef;                           /* Number of referenced pages */
        public int nMax;                           /* Configured cache size */
        public int szPage;                         /* Size of every page in this cache */
        public int szExtra;                        /* Size of extra space for each page */
        public bool bPurgeable;                    /* True if pages are on backing store */
        public dxStress xStress; //int (*xStress)(void*,PgHdr*);       /* Call to try make a page clean */
        public object pStress;                     /* Argument to xStress */
        public sqlite3_pcache pCache;              /* Pluggable cache module */
        public PgHdr pPage1;                       /* Reference to page 1 */

        public int nRef                            /* Number of referenced pages */
        {
            get
            {
                return _nRef;
            }
            set
            {
                _nRef = value;
            }
        }

        public void Clear()
        {
            pDirty = null;
            pDirtyTail = null;
            pSynced = null;
            nRef = 0;
        }
    }
}

