namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;

    /*
    ** Lookaside malloc is a set of fixed-size buffers that can be used
    ** to satisfy small transient memory allocation requests for objects
    ** associated with a particular database connection.  The use of
    ** lookaside malloc provides a significant performance enhancement
    ** (approx 10%) by avoiding numerous malloc/free requests while parsing
    ** SQL statements.
    **
    ** The Lookaside structure holds configuration information about the
    ** lookaside malloc subsystem.  Each available memory allocation in
    ** the lookaside subsystem is stored on a linked list of LookasideSlot
    ** objects.
    **
    ** Lookaside allocations are only allowed for objects that are associated
    ** with a particular database connection.  Hence, schema information cannot
    ** be stored in lookaside because in shared cache mode the schema information
    ** is shared by multiple database connections.  Therefore, while parsing
    ** schema information, the Lookaside.bEnabled flag is cleared so that
    ** lookaside allocations are not used to construct the schema objects.
    */
    public class Lookaside
    {
        public int sz;               /* Size of each buffer in bytes */
        public u8 bEnabled;          /* False to disable new lookaside allocations */
        public bool bMalloced;       /* True if pStart obtained from sqlite3_malloc() */
        public int nOut;             /* Number of buffers currently checked out */
        public int mxOut;            /* Highwater mark for nOut */
        public int[] anStat = new int[3];        /* 0: hits.  1: size misses.  2: full misses */
        public LookasideSlot pFree;  /* List of available buffers */
        public int pStart;           /* First byte of available memory space */
        public int pEnd;             /* First byte past end of available space */
    };
    public class LookasideSlot
    {
        public LookasideSlot pNext;    /* Next buffer in the list of free buffers */
    };
}

