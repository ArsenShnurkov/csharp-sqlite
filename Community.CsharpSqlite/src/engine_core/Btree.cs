namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /* A Btree handle
    **
    ** A database connection contains a pointer to an instance of
    ** this object for every database file that it has open.  This structure
    ** is opaque to the database connection.  The database connection cannot
    ** see the internals of this structure and only deals with pointers to
    ** this structure.
    **
    ** For some database files, the same underlying database cache might be
    ** shared between multiple connections.  In that case, each connection
    ** has it own instance of this object.  But each instance of this object
    ** points to the same BtShared object.  The database cache and the
    ** schema associated with the database file are all contained within
    ** the BtShared object.
    **
    ** All fields in this structure are accessed under sqlite3.mutex.
    ** The pBt pointer itself may not be changed while there exists cursors
    ** in the referenced BtShared that point back to this Btree since those
    ** cursors have to go through this Btree to find their BtShared and
    ** they often do so without holding sqlite3.mutex.
    */
    public class Btree
    {
        public sqlite3 db;
        /* The database connection holding this Btree */
        public BtShared pBt;
        /* Sharable content of this Btree */
        public u8 inTrans;
        /* TRANS_NONE, TRANS_READ or TRANS_WRITE */
        public bool sharable;
        /* True if we can share pBt with another db */
        public bool locked;
        /* True if db currently has pBt locked */
        public int wantToLock;
        /* Number of nested calls to sqlite3BtreeEnter() */
        public int nBackup;
        /* Number of backup operations reading this btree */
        public Btree pNext;
        /* List of other sharable Btrees from the same db */
        public Btree pPrev;
        /* Back pointer of the same list */
        #if !SQLITE_OMIT_SHARED_CACHE
        BtLock lock;              /* Object used to lock page 1 */
        #endif
    }
}
