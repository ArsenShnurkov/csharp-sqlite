namespace Community.CsharpSqlite
{
    using System;

    using u8 = System.Byte;
    
    /*
    ** A RowSet in an instance of the following structure.
    **
    ** A typedef of this structure if found in sqliteInt.h.
    */
    public class RowSet
    {
        public RowSetChunk pChunk;            /* List of all chunk allocations */
        public sqlite3 db;                    /* The database connection */
        public RowSetEntry pEntry;            /* /* List of entries using pRight */
        public RowSetEntry pLast;             /* Last entry on the pEntry list */
        public RowSetEntry[] pFresh;          /* Source of new entry objects */
        public RowSetEntry pTree;             /* Binary tree of entries */
        public int nFresh;                    /* Number of objects on pFresh */
        public bool isSorted;                 /* True if pEntry is sorted */
        public u8 iBatch;                     /* Current insert batch */

        public RowSet( sqlite3 db, int N )
        {
            this.pChunk = null;
            this.db = db;
            this.pEntry = null;
            this.pLast = null;
            this.pFresh = new RowSetEntry[N];
            this.pTree = null;
            this.nFresh = N;
            this.isSorted = true;
            this.iBatch = 0;
        }
    }
}
