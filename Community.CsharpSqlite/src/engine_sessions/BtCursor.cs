namespace Community.CsharpSqlite
{
    using System;

    using i16 = System.Int16;
    using i32 = System.Int32;
    using i64 = System.Int64;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using u64 = System.UInt64;
    using sqlite3_int64 = System.Int64;

    using Pgno = System.UInt32;

    /*
    ** A cursor is a pointer to a particular entry within a particular
    ** b-tree within a database file.
    **
    ** The entry is identified by its MemPage and the index in
    ** MemPage.aCell[] of the entry.
    **
    ** A single database file can shared by two more database connections,
    ** but cursors cannot be shared.  Each cursor is associated with a
    ** particular database connection identified BtCursor.pBtree.db.
    **
    ** Fields in this structure are accessed under the BtShared.mutex
    ** found at self.pBt.mutex.
    */
    public class BtCursor
    {
        public Btree pBtree;
        /* The Btree to which this cursor belongs */
        public BtShared pBt;
        /* The BtShared this cursor points to */
        public BtCursor pNext;
        public BtCursor pPrev;
        /* Forms a linked list of all cursors */
        public KeyInfo pKeyInfo;
        /* Argument passed to comparison function */
        public Pgno pgnoRoot;
        /* The root page of this tree */
        public sqlite3_int64 cachedRowid;
        /* Next rowid cache.  0 means not valid */
        public CellInfo info = new CellInfo ();
        /* A parse of the cell we are pointing at */
        public byte[] pKey;
        /* Saved key that was cursor's last known position */
        public i64 nKey;
        /* Size of pKey, or last integer key */
        public int skipNext;
        /* Prev() is noop if negative. Next() is noop if positive */
        public u8 wrFlag;
        /* True if writable */
        public u8 atLast;
        /* VdbeCursor pointing to the last entry */
        public bool validNKey;
        /* True if info.nKey is valid */
        public int eState;
        /* One of the CURSOR_XXX constants (see below) */
        #if !SQLITE_OMIT_INCRBLOB
        public Pgno[] aOverflow;         /* Cache of overflow page locations */
        public bool isIncrblobHandle;   /* True if this cursor is an incr. io handle */
        #endif
        public i16 iPage;
        /* Index of current page in apPage */
        public u16[] aiIdx = new u16[BTCURSOR_MAX_DEPTH];
        /* Current index in apPage[i] */
        public MemPage[] apPage = new MemPage[BTCURSOR_MAX_DEPTH];
        /* Pages from root to current page */

        public void Clear ()
        {
            pNext = null;
            pPrev = null;
            pKeyInfo = null;
            pgnoRoot = 0;
            cachedRowid = 0;
            info = new CellInfo ();
            wrFlag = 0;
            atLast = 0;
            validNKey = false;
            eState = 0;
            pKey = null;
            nKey = 0;
            skipNext = 0;
            #if !SQLITE_OMIT_INCRBLOB
            isIncrblobHandle=false;
            aOverflow= null;
            #endif
            iPage = 0;
        }

        public BtCursor Copy ()
        {
            BtCursor cp = (BtCursor)MemberwiseClone ();
            return cp;
        }
    }
}

