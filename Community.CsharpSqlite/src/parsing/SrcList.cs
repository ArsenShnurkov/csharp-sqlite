namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using i16 = System.Int16;
    using Bitmask = System.UInt64;
    
    /*
    ** The following structure describes the FROM clause of a SELECT statement.
    ** Each table or subquery in the FROM clause is a separate element of
    ** the SrcList.a[] array.
    **
    ** With the addition of multiple database support, the following structure
    ** can also be used to describe a particular table such as the table that
    ** is modified by an INSERT, DELETE, or UPDATE statement.  In standard SQL,
    ** such a table must be a simple name: ID.  But in SQLite, the table can
    ** now be identified by a database name, a dot, then the table name: ID.ID.
    **
    ** The jointype starts out showing the join type between the current table
    ** and the next table on the list.  The parser builds the list this way.
    ** But sqlite3SrcListShiftJoinType() later shifts the jointypes so that each
    ** jointype expresses the join between the table and the previous table.
    **
    ** In the colUsed field, the high-order bit (bit 63) is set if the table
    ** contains more than 63 columns and the 64-th or later column is used.
    */
    public class SrcList_item
    {
        public string zDatabase; /* Name of database holding this table */
        public string zName;     /* Name of the table */
        public string zAlias;    /* The "B" part of a "A AS B" phrase.  zName is the "A" */
        public Table pTab;       /* An SQL table corresponding to zName */
        public Select pSelect;   /* A SELECT statement used in place of a table name */
        public u8 isPopulated;   /* Temporary table associated with SELECT is populated */
        public u8 jointype;      /* Type of join between this able and the previous */
        public u8 notIndexed;    /* True if there is a NOT INDEXED clause */
        #if !SQLITE_OMIT_EXPLAIN
        public u8 iSelectId;     /* If pSelect!=0, the id of the sub-select in EQP */
        #endif
        public int iCursor;      /* The VDBE cursor number used to access this table */
        public Expr pOn;         /* The ON clause of a join */
        public IdList pUsing;    /* The USING clause of a join */
        public Bitmask colUsed;  /* Bit N (1<<N) set if column N of pTab is used */
        public string zIndex;    /* Identifier from "INDEXED BY <zIndex>" clause */
        public Index pIndex;     /* Index structure corresponding to zIndex, if any */
    }
    public class SrcList
    {
        public i16 nSrc;        /* Number of tables or subqueries in the FROM clause */
        public i16 nAlloc;      /* Number of entries allocated in a[] below */
        public SrcList_item[] a;/* One entry for each identifier on the list */
        public SrcList Copy()
        {
            if ( this == null )
                return null;
            else
            {
                SrcList cp = (SrcList)MemberwiseClone();
                if ( a != null )
                    a.CopyTo( cp.a, 0 );
                return cp;
            }
        }
    }
}

