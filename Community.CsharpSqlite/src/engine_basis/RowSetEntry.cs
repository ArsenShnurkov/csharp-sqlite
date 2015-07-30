namespace Community.CsharpSqlite
{
    using System;
    using i64 = System.Int64;

    /*
    ** Each entry in a RowSet is an instance of the following object.
    */
    public class RowSetEntry
    {
        public i64 v;                /* ROWID value for this entry */
        public RowSetEntry pRight;   /* Right subtree (larger entries) or list */
        public RowSetEntry pLeft;    /* Left subtree (smaller entries) */
    }
}

