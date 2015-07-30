using System.Text;

namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u32 = System.UInt32;
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
    ** An SQL parser context.  A copy of this structure is passed through
    ** the parser and down into all the parser action routine in order to
    ** carry around information that is global to the entire parse.
    **
    ** The structure is divided into two parts.  When the parser and code
    ** generate call themselves recursively, the first part of the structure
    ** is constant but the second part is reset at the beginning and end of
    ** each recursion.
    **
    ** The nTableLock and aTableLock variables are only used if the shared-cache
    ** feature is enabled (if sqlite3Tsd()->useSharedData is true). They are
    ** used to store the set of table-locks required by the statement being
    ** compiled. Function sqlite3TableLock() is used to add entries to the
    ** list.
    */
    public class yColCache
    {
        public int iTable;
        /* Table cursor number */
        public int iColumn;
        /* Table column number */
        public u8 tempReg;
        /* iReg is a temp register that needs to be freed */
        public int iLevel;
        /* Nesting level */
        public int iReg;
        /* Reg with value of this column. 0 means none. */
        public int lru;
        /* Least recently used entry has the smallest value */
    }

    public class Parse
    {
        public sqlite3 db;
        /* The main database structure */
        public int rc;
        /* Return code from execution */
        public string zErrMsg;
        /* An error message */
        public Vdbe pVdbe;
        /* An engine for executing database bytecode */
        public u8 colNamesSet;
        /* TRUE after OP_ColumnName has been issued to pVdbe */
        public u8 nameClash;
        /* A permanent table name clashes with temp table name */
        public u8 checkSchema;
        /* Causes schema cookie check after an error */
        public u8 nested;
        /* Number of nested calls to the parser/code generator */
        public u8 parseError;
        /* True after a parsing error.  Ticket #1794 */
        public u8 nTempReg;
        /* Number of temporary registers in aTempReg[] */
        public u8 nTempInUse;
        /* Number of aTempReg[] currently checked out */
        public int[] aTempReg;
        /* Holding area for temporary registers */
        public int nRangeReg;
        /* Size of the temporary register block */
        public int iRangeReg;
        /* First register in temporary register block */
        public int nErr;
        /* Number of errors seen */
        public int nTab;
        /* Number of previously allocated VDBE cursors */
        public int nMem;
        /* Number of memory cells used so far */
        public int nSet;
        /* Number of sets used so far */
        public int ckBase;
        /* Base register of data during check constraints */
        public int iCacheLevel;
        /* ColCache valid when aColCache[].iLevel<=iCacheLevel */
        public int iCacheCnt;
        /* Counter used to generate aColCache[].lru values */
        public u8 nColCache;
        /* Number of entries in the column cache */
        public u8 iColCache;
        /* Next entry of the cache to replace */
        public yColCache[] aColCache;
/* One for each valid column cache entry */
        public yDbMask writeMask;
        /* Start a write transaction on these databases */
        public yDbMask cookieMask;
        /* Bitmask of schema verified databases */
        public u8 isMultiWrite;
        /* True if statement may affect/insert multiple rows */
        public u8 mayAbort;
        /* True if statement may throw an ABORT exception */
        public int cookieGoto;
        /* Address of OP_Goto to cookie verifier subroutine */
        public int[] cookieValue;
        /* Values of cookies to verify */
        #if !SQLITE_OMIT_SHARED_CACHE
        public int nTableLock;         /* Number of locks in aTableLock */
        public TableLock[] aTableLock; /* Required table locks for shared-cache mode */
        #endif
        public int regRowid;
        /* Register holding rowid of CREATE TABLE entry */
        public int regRoot;
        /* Register holding root page number for new objects */
        public AutoincInfo pAinc;
        /* Information about AUTOINCREMENT counters */
        public int nMaxArg;
        /* Max args passed to user function by sub-program */

        /* Information used while coding trigger programs. */
        public Parse pToplevel;
        /* Parse structure for main program (or NULL) */
        public Table pTriggerTab;
        /* Table triggers are being coded for */
        public u32 oldmask;
        /* Mask of old.* columns referenced */
        public u32 newmask;
        /* Mask of new.* columns referenced */
        public u8 eTriggerOp;
        /* TK_UPDATE, TK_INSERT or TK_DELETE */
        public u8 eOrconf;
        /* Default ON CONFLICT policy for trigger steps */
        public u8 disableTriggers;
        /* True to disable triggers */
        public double nQueryLoop;
        /* Estimated number of iterations of a query */

        /* Above is constant between recursions.  Below is reset before and after
        ** each recursion */

        public int nVar;
        /* Number of '?' variables seen in the SQL so far */
        public int nzVar;
        /* Number of available slots in azVar[] */
        public string[] azVar;
        /* Pointers to names of parameters */
        public Vdbe pReprepare;
        /* VM being reprepared (sqlite3Reprepare()) */
        public int nAlias;
        /* Number of aliased result set columns */
        public int nAliasAlloc;
        /* Number of allocated slots for aAlias[] */
        public int[] aAlias;
        /* Register used to hold aliased result */
        public u8 explain;
        /* True if the EXPLAIN flag is found on the query */
        public Token sNameToken;
        /* Token with unqualified schema object name */
        public Token sLastToken;
        /* The last token parsed */
        public StringBuilder zTail;
        /* All SQL text past the last semicolon parsed */
        public Table pNewTable;
        /* A table being constructed by CREATE TABLE */
        public Trigger pNewTrigger;
        /* Trigger under construct by a CREATE TRIGGER */
        public string zAuthContext;
        /* The 6th parameter to db.xAuth callbacks */
        #if !SQLITE_OMIT_VIRTUALTABLE
        public Token sArg;
        /* Complete text of a module argument */
        public u8 declareVtab;
        /* True if inside sqlite3_declare_vtab() */
        public int nVtabLock;
        /* Number of virtual tables to lock */
        public Table[] apVtabLock;
        /* Pointer to virtual tables needing locking */
        #endif
        public int nHeight;
        /* Expression tree height of current sub-select */
        public Table pZombieTab;
        /* List of Table objects to delete after code gen */
        public TriggerPrg pTriggerPrg;
        /* Linked list of coded triggers */
        #if !SQLITE_OMIT_EXPLAIN
        public int iSelectId;
        public int iNextSelectId;
        #endif

        // We need to create instances of the col cache
        public Parse()
        {
            aTempReg = new int[8];     /* Holding area for temporary registers */

            aColCache = new yColCache[SQLITE_N_COLCACHE];     /* One for each valid column cache entry */
            for (int i = 0; i < this.aColCache.Length; i++)
            {
                this.aColCache[i] = new yColCache();
            }

            cookieValue = new int[SQLITE_MAX_ATTACHED + 2];  /* Values of cookies to verify */

            sLastToken = new Token(); /* The last token parsed */

            #if !SQLITE_OMIT_VIRTUALTABLE
            sArg = new Token();
            #endif
        }

        public void ResetMembers() // Need to clear all the following variables during each recursion
        {
            nVar = 0;
            nzVar = 0;
            azVar = null;
            nAlias = 0;
            nAliasAlloc = 0;
            aAlias = null;
            explain = 0;
            sNameToken = new Token();
            sLastToken = new Token();
            zTail.Length = 0;
            pNewTable = null;
            pNewTrigger = null;
            zAuthContext = null;
            #if !SQLITE_OMIT_VIRTUALTABLE
            sArg = new Token();
            declareVtab = 0;
            nVtabLock = 0;
            apVtabLock = null;
            #endif
            nHeight = 0;
            pZombieTab = null;
            pTriggerPrg = null;
        }

        Parse[] SaveBuf = new Parse[10];
        //For Recursion Storage
        public void RestoreMembers()  // Need to clear all the following variables during each recursion
        {
            if (SaveBuf[nested] != null)
            {
                nVar = SaveBuf[nested].nVar;
                nzVar = SaveBuf[nested].nzVar;
                azVar = SaveBuf[nested].azVar;
                nAlias = SaveBuf[nested].nAlias;
                nAliasAlloc = SaveBuf[nested].nAliasAlloc;
                aAlias = SaveBuf[nested].aAlias;
                explain = SaveBuf[nested].explain;
                sNameToken = SaveBuf[nested].sNameToken;
                sLastToken = SaveBuf[nested].sLastToken;
                zTail = SaveBuf[nested].zTail;
                pNewTable = SaveBuf[nested].pNewTable;
                pNewTrigger = SaveBuf[nested].pNewTrigger;
                zAuthContext = SaveBuf[nested].zAuthContext;
                #if !SQLITE_OMIT_VIRTUALTABLE
                sArg = SaveBuf[nested].sArg;
                declareVtab = SaveBuf[nested].declareVtab;
                nVtabLock = SaveBuf[nested].nVtabLock;
                apVtabLock = SaveBuf[nested].apVtabLock;
                #endif
                nHeight = SaveBuf[nested].nHeight;
                pZombieTab = SaveBuf[nested].pZombieTab;
                pTriggerPrg = SaveBuf[nested].pTriggerPrg;
                SaveBuf[nested] = null;
            }
        }

        public void SaveMembers() // Need to clear all the following variables during each recursion
        {
            SaveBuf[nested] = new Parse();
            SaveBuf[nested].nVar = nVar;
            SaveBuf[nested].nzVar = nzVar;
            SaveBuf[nested].azVar = azVar;
            SaveBuf[nested].nAlias = nAlias;
            SaveBuf[nested].nAliasAlloc = nAliasAlloc;
            SaveBuf[nested].aAlias = aAlias;
            SaveBuf[nested].explain = explain;
            SaveBuf[nested].sNameToken = sNameToken;
            SaveBuf[nested].sLastToken = sLastToken;
            SaveBuf[nested].zTail = zTail;
            SaveBuf[nested].pNewTable = pNewTable;
            SaveBuf[nested].pNewTrigger = pNewTrigger;
            SaveBuf[nested].zAuthContext = zAuthContext;
            #if !SQLITE_OMIT_VIRTUALTABLE
            SaveBuf[nested].sArg = sArg;
            SaveBuf[nested].declareVtab = declareVtab;
            SaveBuf[nested].nVtabLock = nVtabLock;
            SaveBuf[nested].apVtabLock = apVtabLock;
            #endif
            SaveBuf[nested].nHeight = nHeight;
            SaveBuf[nested].pZombieTab = pZombieTab;
            SaveBuf[nested].pTriggerPrg = pTriggerPrg;
        }
    }
}

