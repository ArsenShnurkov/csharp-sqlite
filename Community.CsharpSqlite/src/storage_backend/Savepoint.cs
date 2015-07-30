namespace Community.CsharpSqlite
{
    using System;
    
    using i64 = System.Int64;
    
    /*
    ** All current savepoints are stored in a linked list starting at
    ** sqlite3.pSavepoint. The first element in the list is the most recently
    ** opened savepoint. Savepoints are added to the list by the vdbe
    ** OP_Savepoint instruction.
    */
    //struct Savepoint {
    //  string zName;                        /* Savepoint name (nul-terminated) */
    //  i64 nDeferredCons;                  /* Number of deferred fk violations */
    //  Savepoint *pNext;                   /* Parent savepoint (if any) */
    //};
    public class Savepoint
    {
        public string zName;              /* Savepoint name (nul-terminated) */
        public i64 nDeferredCons;         /* Number of deferred fk violations */
        public Savepoint pNext;           /* Parent savepoint (if any) */
    }
}
