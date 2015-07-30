using System;

namespace Community.CsharpSqlite
{
    /*
    ** The "context" argument for a installable function.  A pointer to an
    ** instance of this structure is the first argument to the routines used
    ** implement the SQL functions.
    **
    ** There is a typedef for this structure in sqlite.h.  So all routines,
    ** even the public interface to SQLite, can use a pointer to this structure.
    ** But this file is the only place where the internal details of this
    ** structure are known.
    **
    ** This structure is defined inside of vdbeInt.h because it uses substructures
    ** (Mem) which are only defined there.
    */
    public class sqlite3_context
    {
        public FuncDef pFunc;        /* Pointer to function information.  MUST BE FIRST */
        public VdbeFunc pVdbeFunc;   /* Auxilary data, if created. */
        public Mem s = new Mem();         /* The return value is stored here */
        public Mem pMem;             /* Memory cell used to store aggregate context */
        public int isError;          /* Error code returned by the function. */
        public CollSeq pColl;        /* Collating sequence */

    }
}

