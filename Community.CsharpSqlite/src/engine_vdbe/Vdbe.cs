namespace Community.CsharpSqlite
{
    using System;
    using Op = VdbeOp;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using i64 = System.Int64;
    
    using FILE = System.IO.TextWriter;
    
    #if !SQLITE_MAX_VARIABLE_NUMBER
    using ynVar = System.Int16;
    #else
    using ynVar = System.Int32; 
    #endif
    /*
** The yDbMask datatype for the bitmask of all attached databases.
*/
    #if SQLITE_MAX_ATTACHED//>30
    //  typedef sqlite3_uint64 yDbMask;
    using yDbMask = System.Int64; 
    #else
    //  typedef unsigned int yDbMask;
    using yDbMask = System.Int32;
    #endif
    
    
    /*
    ** An instance of the virtual machine.  This structure contains the complete
    ** state of the virtual machine.
    **
    ** The "sqlite3_stmt" structure pointer that is returned by sqlite3_prepare()
    ** is really a pointer to an instance of this structure.
    **
    ** The Vdbe.inVtabMethod variable is set to non-zero for the duration of
    ** any virtual table method invocations made by the vdbe program. It is
    ** set to 2 for xDestroy method calls and 1 for all other methods. This
    ** variable is used for two purposes: to allow xDestroy methods to execute
    ** "DROP TABLE" statements and to prevent some nasty side effects of
    ** malloc failure when SQLite is invoked recursively by a virtual table
    ** method function.
    */
    public class Vdbe
    {
        public sqlite3 db;             /* The database connection that owns this statement */
        public Op[] aOp;               /* Space to hold the virtual machine's program */
        public Mem[] aMem;             /* The memory locations */
        public Mem[] apArg;            /* Arguments to currently executing user function */
        public Mem[] aColName;         /* Column names to return */
        public Mem[] pResultSet;       /* Pointer to an array of results */
        public int nMem;               /* Number of memory locations currently allocated */
        public int nOp;                /* Number of instructions in the program */
        public int nOpAlloc;           /* Number of slots allocated for aOp[] */
        public int nLabel;             /* Number of labels used */
        public int nLabelAlloc;        /* Number of slots allocated in aLabel[] */
        public int[] aLabel;           /* Space to hold the labels */
        public u16 nResColumn;         /* Number of columns in one row of the result set */
        public u16 nCursor;            /* Number of slots in apCsr[] */
        public u32 magic;              /* Magic number for sanity checking */
        public string zErrMsg;         /* Error message written here */
        public Vdbe pPrev;             /* Linked list of VDBEs with the same Vdbe.db */
        public Vdbe pNext;             /* Linked list of VDBEs with the same Vdbe.db */
        public VdbeCursor[] apCsr;     /* One element of this array for each open cursor */
        public Mem[] aVar;             /* Values for the OP_Variable opcode. */
        public string[] azVar;         /* Name of variables */
        public ynVar nVar;             /* Number of entries in aVar[] */
        public ynVar nzVar;            /* Number of entries in azVar[] */
        public u32 cacheCtr;           /* VdbeCursor row cache generation counter */
        public int pc;                 /* The program counter */
        public int rc;                 /* Value to return */
        public u8 errorAction;         /* Recovery action to do in case of an error */
        public int explain;            /* True if EXPLAIN present on SQL command */
        public bool changeCntOn;       /* True to update the change-counter */
        public bool expired;           /* True if the VM needs to be recompiled */
        public u8 runOnlyOnce;         /* Automatically expire on reset */
        public int minWriteFileFormat; /* Minimum file format for writable database files */
        public int inVtabMethod;       /* See comments above */
        public bool usesStmtJournal;   /* True if uses a statement journal */
        public bool readOnly;          /* True for read-only statements */
        public int nChange;            /* Number of db changes made since last reset */
        public bool isPrepareV2;       /* True if prepared with prepare_v2() */
        public yDbMask btreeMask;          /* Bitmask of db.aDb[] entries referenced */
        public yDbMask lockMask;       /* Subset of btreeMask that requires a lock */

        public int iStatement;         /* Statement number (or 0 if has not opened stmt) */
        public int[] aCounter = new int[3]; /* Counters used by sqlite3_stmt_status() */
        #if !SQLITE_OMIT_TRACE
        public i64 startTime;          /* Time when query started - used for profiling */
        #endif
        public i64 nFkConstraint;      /* Number of imm. FK constraints this VM */
        public i64 nStmtDefCons;       /* Number of def. constraints when stmt started */
        public string zSql = "";       /* Text of the SQL statement that generated this */
        public object pFree;           /* Free this when deleting the vdbe */
        #if SQLITE_DEBUG
        public FILE trace;             /* Write an execution trace here, if not NULL */
        #endif
        public VdbeFrame pFrame;       /* Parent frame */
        public VdbeFrame pDelFrame;    /* List of frame objects to free on VM reset */
        public int nFrame;             /* Number of frames in pFrame list */
        public u32 expmask;            /* Binding to these vars invalidates VM */
        public SubProgram pProgram;    /* Linked list of all sub-programs used by VM */

        public Vdbe Copy()
        {
            Vdbe cp = (Vdbe)MemberwiseClone();
            return cp;
        }
        public void CopyTo( Vdbe ct )
        {
            ct.db = db;
            ct.pPrev = pPrev;
            ct.pNext = pNext;
            ct.nOp = nOp;
            ct.nOpAlloc = nOpAlloc;
            ct.aOp = aOp;
            ct.nLabel = nLabel;
            ct.nLabelAlloc = nLabelAlloc;
            ct.aLabel = aLabel;
            ct.apArg = apArg;
            ct.aColName = aColName;
            ct.nCursor = nCursor;
            ct.apCsr = apCsr;
            ct.aVar = aVar;
            ct.azVar = azVar;
            ct.nVar = nVar;
            ct.nzVar = nzVar;
            ct.magic = magic;
            ct.nMem = nMem;
            ct.aMem = aMem;
            ct.cacheCtr = cacheCtr;
            ct.pc = pc;
            ct.rc = rc;
            ct.errorAction = errorAction;
            ct.nResColumn = nResColumn;
            ct.zErrMsg = zErrMsg;
            ct.pResultSet = pResultSet;
            ct.explain = explain;
            ct.changeCntOn = changeCntOn;
            ct.expired = expired;
            ct.minWriteFileFormat = minWriteFileFormat;
            ct.inVtabMethod = inVtabMethod;
            ct.usesStmtJournal = usesStmtJournal;
            ct.readOnly = readOnly;
            ct.nChange = nChange;
            ct.isPrepareV2 = isPrepareV2;
            #if !SQLITE_OMIT_TRACE
            ct.startTime = startTime;
            #endif
            ct.btreeMask = btreeMask;
            ct.lockMask = lockMask;
            aCounter.CopyTo( ct.aCounter, 0 );
            ct.zSql = zSql;
            ct.pFree = pFree;
            #if SQLITE_DEBUG
            ct.trace = trace;
            #endif
            ct.nFkConstraint = nFkConstraint;
            ct.nStmtDefCons = nStmtDefCons;
            ct.iStatement = iStatement;
            ct.pFrame = pFrame;
            ct.nFrame = nFrame;
            ct.expmask = expmask;
            ct.pProgram = pProgram;
            #if SQLITE_SSE
            ct.fetchId=fetchId;
            ct.lru=lru;
            #endif
            #if SQLITE_ENABLE_MEMORY_MANAGEMENT
            ct.pLruPrev=pLruPrev;
            ct.pLruNext=pLruNext;
            #endif
        }
    }
}

