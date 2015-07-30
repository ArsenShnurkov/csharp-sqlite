namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;

    /*
    ** Each SQL index is represented in memory by an
    ** instance of the following structure.
    **
    ** The columns of the table that are to be indexed are described
    ** by the aiColumn[] field of this structure.  For example, suppose
    ** we have the following table and index:
    **
    **     CREATE TABLE Ex1(c1 int, c2 int, c3 text);
    **     CREATE INDEX Ex2 ON Ex1(c3,c1);
    **
    ** In the Table structure describing Ex1, nCol==3 because there are
    ** three columns in the table.  In the Index structure describing
    ** Ex2, nColumn==2 since 2 of the 3 columns of Ex1 are indexed.
    ** The value of aiColumn is {2, 0}.  aiColumn[0]==2 because the
    ** first column to be indexed (c3) has an index of 2 in Ex1.aCol[].
    ** The second column to be indexed (c1) has an index of 0 in
    ** Ex1.aCol[], hence Ex2.aiColumn[1]==0.
    **
    ** The Index.onError field determines whether or not the indexed columns
    ** must be unique and what to do if they are not.  When Index.onError=OE_None,
    ** it means this is not a unique index.  Otherwise it is a unique index
    ** and the value of Index.onError indicate the which conflict resolution
    ** algorithm to employ whenever an attempt is made to insert a non-unique
    ** element.
    */
    public class Index
    {
        public string zName;      /* Name of this index */
        public int nColumn;       /* Number of columns in the table used by this index */
        public int[] aiColumn;    /* Which columns are used by this index.  1st is 0 */
        public int[] aiRowEst;    /* Result of ANALYZE: Est. rows selected by each column */
        public Table pTable;      /* The SQL table being indexed */
        public int tnum;          /* Page containing root of this index in database file */
        public u8 onError;        /* OE_Abort, OE_Ignore, OE_Replace, or OE_None */
        public u8 autoIndex;      /* True if is automatically created (ex: by UNIQUE) */
        public u8 bUnordered;     /* Use this index for == or IN queries only */
        public string zColAff;    /* String defining the affinity of each column */
        public Index pNext;       /* The next index associated with the same table */
        public Schema pSchema;    /* Schema containing this index */
        public u8[] aSortOrder;   /* Array of size Index.nColumn. True==DESC, False==ASC */
        public string[] azColl;   /* Array of collation sequence names for index */
        public IndexSample[] aSample;    /* Array of SQLITE_INDEX_SAMPLES samples */

        public Index Copy()
        {
            if ( this == null )
                return null;
            else
            {
                Index cp = (Index)MemberwiseClone();
                return cp;
            }
        }
    }
}

