namespace Community.CsharpSqlite
{
    using System;
    /*
    ** An instance of this structure can hold a simple list of identifiers,
    ** such as the list "a,b,c" in the following statements:
    **
    **      INSERT INTO t(a,b,c) VALUES ...;
    **      CREATE INDEX idx ON t(a,b,c);
    **      CREATE TRIGGER trig BEFORE UPDATE ON t(a,b,c) ...;
    **
    ** The IdList.a.idx field is used when the IdList represents the list of
    ** column names after a table name in an INSERT statement.  In the statement
    **
    **     INSERT INTO t(a,b,c) ...
    **
    ** If "a" is the k-th column of table "t", then IdList.a[0].idx==k.
    */
    public class IdList_item
    {
        public string zName;      /* Name of the identifier */
        public int idx;          /* Index in some Table.aCol[] of a column named zName */
    }
    public class IdList
    {
        public IdList_item[] a;
        public int nId;         /* Number of identifiers on the list */
        public int nAlloc;      /* Number of entries allocated for a[] below */

        public IdList Copy()
        {
            if ( this == null )
                return null;
            else
            {
                IdList cp = (IdList)MemberwiseClone();
                a.CopyTo( cp.a, 0 );
                return cp;
            }
        }
    }
}
