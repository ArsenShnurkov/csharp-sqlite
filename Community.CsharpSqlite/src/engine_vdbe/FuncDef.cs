namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using i16 = System.Int16;
    using u16 = System.UInt16;
    using sqlite3_value = Mem;
    
    /*
     * FUNCTIONS
     *
     */
    public delegate void dxFunc( sqlite3_context ctx, int intValue, sqlite3_value[] value );
    public delegate void dxStep( sqlite3_context ctx, int intValue, sqlite3_value[] value );
    public delegate void dxFinal( sqlite3_context ctx );

    /*
    ** Each SQL function is defined by an instance of the following
    ** structure.  A pointer to this structure is stored in the sqlite.aFunc
    ** hash table.  When multiple functions have the same name, the hash table
    ** points to a linked list of these structures.
    */
    public class FuncDef
    {
        public i16 nArg;           /* Number of arguments.  -1 means unlimited */
        public u8 iPrefEnc;        /* Preferred text encoding (SQLITE_UTF8, 16LE, 16BE) */
        public u8 flags;           /* Some combination of SQLITE_FUNC_* */
        public object pUserData;   /* User data parameter */
        public FuncDef pNext;      /* Next function with same name */
        public dxFunc xFunc;//)(sqlite3_context*,int,sqlite3_value*); /* Regular function */
        public dxStep xStep;//)(sqlite3_context*,int,sqlite3_value*); /* Aggregate step */
        public dxFinal xFinalize;//)(sqlite3_context);                /* Aggregate finalizer */
        public string zName;       /* SQL name of the function. */
        public FuncDef pHash;      /* Next with a different name but the same hash */
        public FuncDestructor pDestructor;   /* Reference counted destructor function */

        public FuncDef()
        {
        }

        public FuncDef( i16 nArg, u8 iPrefEnc, u8 iflags, object pUserData, FuncDef pNext, dxFunc xFunc, dxStep xStep, dxFinal xFinalize, string zName, FuncDef pHash, FuncDestructor pDestructor )
        {
            this.nArg = nArg;
            this.iPrefEnc = iPrefEnc;
            this.flags = iflags;
            this.pUserData = pUserData;
            this.pNext = pNext;
            this.xFunc = xFunc;
            this.xStep = xStep;
            this.xFinalize = xFinalize;
            this.zName = zName;
            this.pHash = pHash;
            this.pDestructor = pDestructor;
        }
        public FuncDef( string zName, u8 iPrefEnc, i16 nArg, int iArg, u8 iflags, dxFunc xFunc )
        {
            this.nArg = nArg;
            this.iPrefEnc = iPrefEnc;
            this.flags = iflags;
            this.pUserData = iArg;
            this.pNext = null;
            this.xFunc = xFunc;
            this.xStep = null;
            this.xFinalize = null;
            this.zName = zName;
        }

        public FuncDef( string zName, u8 iPrefEnc, i16 nArg, int iArg, u8 iflags, dxStep xStep, dxFinal xFinal )
        {
            this.nArg = nArg;
            this.iPrefEnc = iPrefEnc;
            this.flags = iflags;
            this.pUserData = iArg;
            this.pNext = null;
            this.xFunc = null;
            this.xStep = xStep;
            this.xFinalize = xFinal;
            this.zName = zName;
        }

        public FuncDef( string zName, u8 iPrefEnc, i16 nArg, object arg, dxFunc xFunc, u8 flags )
        {
            this.nArg = nArg;
            this.iPrefEnc = iPrefEnc;
            this.flags = flags;
            this.pUserData = arg;
            this.pNext = null;
            this.xFunc = xFunc;
            this.xStep = null;
            this.xFinalize = null;
            this.zName = zName;
        }

        public FuncDef Copy()
        {
            FuncDef c = new FuncDef();
            c.nArg = nArg;
            c.iPrefEnc = iPrefEnc;
            c.flags = flags;
            c.pUserData = pUserData;
            c.pNext = pNext;
            c.xFunc = xFunc;
            c.xStep = xStep;
            c.xFinalize = xFinalize;
            c.zName = zName;
            c.pHash = pHash;
            c.pDestructor = pDestructor;
            return c;
        }
    }
}

