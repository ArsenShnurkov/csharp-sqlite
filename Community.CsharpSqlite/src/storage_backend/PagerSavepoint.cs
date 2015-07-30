namespace Community.CsharpSqlite
{
    using System;

    using i64 = System.Int64;

    using Pgno = System.UInt32;
    
    /*
    ** An instance of the following structure is allocated for each active
    ** savepoint and statement transaction in the system. All such structures
    ** are stored in the Pager.aSavepoint[] array, which is allocated and
    ** resized using sqlite3Realloc().
    **
    ** When a savepoint is created, the PagerSavepoint.iHdrOffset field is
    ** set to 0. If a journal-header is written into the main journal while
    ** the savepoint is active, then iHdrOffset is set to the byte offset
    ** immediately following the last journal record written into the main
    ** journal before the journal-header. This is required during savepoint
    ** rollback (see pagerPlaybackSavepoint()).
    */
    //typedef struct PagerSavepoint PagerSavepoint;
    public class PagerSavepoint
    {
        public i64 iOffset;                 /* Starting offset in main journal */
        public i64 iHdrOffset;              /* See above */
        public Bitvec pInSavepoint;         /* Set of pages in this savepoint */
        public Pgno nOrig;                  /* Original number of pages in file */
        public Pgno iSubRec;                /* Index of first record in sub-journal */
        #if !SQLITE_OMIT_WAL
        public u32 aWalData[WAL_SAVEPOINT_NDATA];        /* WAL savepoint context */
        #else
        public object aWalData = null;      /* Used for C# convenience */
        #endif
        public static implicit operator bool( PagerSavepoint b )
        {
            return ( b != null );
        }
    }
}

