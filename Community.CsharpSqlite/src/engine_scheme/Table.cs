namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    
    /*
    ** Each SQL table is represented in memory by an instance of the
    ** following structure.
    **
    ** Table.zName is the name of the table.  The case of the original
    ** CREATE TABLE statement is stored, but case is not significant for
    ** comparisons.
    **
    ** Table.nCol is the number of columns in this table.  Table.aCol is a
    ** pointer to an array of Column structures, one for each column.
    **
    ** If the table has an INTEGER PRIMARY KEY, then Table.iPKey is the index of
    ** the column that is that key.   Otherwise Table.iPKey is negative.  Note
    ** that the datatype of the PRIMARY KEY must be INTEGER for this field to
    ** be set.  An INTEGER PRIMARY KEY is used as the rowid for each row of
    ** the table.  If a table has no INTEGER PRIMARY KEY, then a random rowid
    ** is generated for each row of the table.  TF_HasPrimaryKey is set if
    ** the table has any PRIMARY KEY, INTEGER or otherwise.
    **
    ** Table.tnum is the page number for the root BTree page of the table in the
    ** database file.  If Table.iDb is the index of the database table backend
    ** in sqlite.aDb[].  0 is for the main database and 1 is for the file that
    ** holds temporary tables and indices.  If TF_Ephemeral is set
    ** then the table is stored in a file that is automatically deleted
    ** when the VDBE cursor to the table is closed.  In this case Table.tnum
    ** refers VDBE cursor number that holds the table open, not to the root
    ** page number.  Transient tables are used to hold the results of a
    ** sub-query that appears instead of a real table name in the FROM clause
    ** of a SELECT statement.
    */
    public class Table
    {
        public string zName;      /* Name of the table or view */
        public int iPKey;         /* If not negative, use aCol[iPKey] as the primary key */
        public int nCol;          /* Number of columns in this table */
        public Column[] aCol;     /* Information about each column */
        public Index pIndex;      /* List of SQL indexes on this table. */
        public int tnum;          /* Root BTree node for this table (see note above) */
        public u32 nRowEst;       /* Estimated rows in table - from sqlite_stat1 table */
        public Select pSelect;    /* NULL for tables.  Points to definition if a view. */
        public u16 nRef;          /* Number of pointers to this Table */
        public u8 tabFlags;       /* Mask of TF_* values */
        public u8 keyConf;        /* What to do in case of uniqueness conflict on iPKey */
        public FKey pFKey;        /* Linked list of all foreign keys in this table */
        public string zColAff;    /* String defining the affinity of each column */
        #if !SQLITE_OMIT_CHECK
        public Expr pCheck;       /* The AND of all CHECK constraints */
        #endif
        #if !SQLITE_OMIT_ALTERTABLE
        public int addColOffset;  /* Offset in CREATE TABLE stmt to add a new column */
        #endif
        #if !SQLITE_OMIT_VIRTUALTABLE
        public VTable pVTable;      /* List of VTable objects. */
        public int nModuleArg;      /* Number of arguments to the module */
        public string[] azModuleArg;/* Text of all module args. [0] is module name */
        #endif
        public Trigger pTrigger;  /* List of SQL triggers on this table */
        public Schema pSchema;    /* Schema that contains this table */
        public Table pNextZombie;  /* Next on the Parse.pZombieTab list */

        public Table Copy()
        {
            if ( this == null )
                return null;
            else
            {
                Table cp = (Table)MemberwiseClone();
                if ( pIndex != null )
                    cp.pIndex = pIndex.Copy();
                if ( pSelect != null )
                    cp.pSelect = pSelect.Copy();
                if ( pTrigger != null )
                    cp.pTrigger = pTrigger.Copy();
                if ( pFKey != null )
                    cp.pFKey = pFKey.Copy();
                #if !SQLITE_OMIT_CHECK
                // Don't Clone Checks, only copy reference via Memberwise Clone above --
                //if ( pCheck != null ) cp.pCheck = pCheck.Copy();
                #endif
                // Don't Clone Schema, only copy reference via Memberwise Clone above --
                // if ( pSchema != null ) cp.pSchema=pSchema.Copy();
                // Don't Clone pNextZombie, only copy reference via Memberwise Clone above --
                // if ( pNextZombie != null ) cp.pNextZombie=pNextZombie.Copy();
                return cp;
            }
        }
    }
}

