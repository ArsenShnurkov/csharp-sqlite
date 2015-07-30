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
    
    using Pgno = System.UInt32;

    /*
    ** An instance of this object represents a single database file.
    **
    ** A single database file can be in use as the same time by two
    ** or more database connections.  When two or more connections are
    ** sharing the same database file, each connection has it own
    ** private Btree object for the file and each of those Btrees points
    ** to this one BtShared object.  BtShared.nRef is the number of
    ** connections currently sharing this database file.
    **
    ** Fields in this structure are accessed under the BtShared.mutex
    ** mutex, except for nRef and pNext which are accessed under the
    ** global SQLITE_MUTEX_STATIC_MASTER mutex.  The pPager field
    ** may not be modified once it is initially set as long as nRef>0.
    ** The pSchema field may be set once under BtShared.mutex and
    ** thereafter is unchanged as long as nRef>0.
    **
    ** isPending:
    **
    **   If a BtShared client fails to obtain a write-lock on a database
    **   table (because there exists one or more read-locks on the table),
    **   the shared-cache enters 'pending-lock' state and isPending is
    **   set to true.
    **
    **   The shared-cache leaves the 'pending lock' state when either of
    **   the following occur:
    **
    **     1) The current writer (BtShared.pWriter) concludes its transaction, OR
    **     2) The number of locks held by other connections drops to zero.
    **
    **   while in the 'pending-lock' state, no connection may start a new
    **   transaction.
    **
    **   This feature is included to help prevent writer-starvation.
    */
    public class BtShared
    {
        public Pager pPager;
        /* The page cache */
        public sqlite3 db;
        /* Database connection currently using this Btree */
        public BtCursor pCursor;
        /* A list of all open cursors */
        public MemPage pPage1;
        /* First page of the database */
        public bool readOnly;
        /* True if the underlying file is readonly */
        public bool pageSizeFixed;
        /* True if the page size can no longer be changed */
        public bool secureDelete;
        /* True if secure_delete is enabled */
        public bool initiallyEmpty;
        /* Database is empty at start of transaction */
        public u8 openFlags;
        /* Flags to sqlite3BtreeOpen() */
        #if !SQLITE_OMIT_AUTOVACUUM
        public bool autoVacuum;
        /* True if auto-vacuum is enabled */
        public bool incrVacuum;
        /* True if incr-vacuum is enabled */
        #endif
        public u8 inTransaction;
        /* Transaction state */
        public bool doNotUseWAL;
        /* If true, do not open write-ahead-log file */
        public u16 maxLocal;
        /* Maximum local payload in non-LEAFDATA tables */
        public u16 minLocal;
        /* Minimum local payload in non-LEAFDATA tables */
        public u16 maxLeaf;
        /* Maximum local payload in a LEAFDATA table */
        public u16 minLeaf;
        /* Minimum local payload in a LEAFDATA table */
        public u32 pageSize;
        /* Total number of bytes on a page */
        public u32 usableSize;
        /* Number of usable bytes on each page */
        public int nTransaction;
        /* Number of open transactions (read + write) */
        public Pgno nPage;
        /* Number of pages in the database */
        public Schema pSchema;
        /* Pointer to space allocated by sqlite3BtreeSchema() */
        public dxFreeSchema xFreeSchema;
        /* Destructor for BtShared.pSchema */
        public sqlite3_mutex mutex;
        /* Non-recursive mutex required to access this object */
        public Bitvec pHasContent;
        /* Set of pages moved to free-list this transaction */
        #if !SQLITE_OMIT_SHARED_CACHE
        public int nRef;                /* Number of references to this structure */
        public BtShared pNext;          /* Next on a list of sharable BtShared structs */
        public BtLock pLock;            /* List of locks held on this shared-btree struct */
        public Btree pWriter;           /* Btree with currently open write transaction */
        public u8 isExclusive;          /* True if pWriter has an EXCLUSIVE lock on the db */
        public u8 isPending;            /* If waiting for read-locks to clear */
        #endif
        public byte[] pTmpSpace;
        /* BtShared.pageSize bytes of space for tmp use */
    }
}
