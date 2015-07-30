namespace Community.CsharpSqlite
{
    using System;

    using Bitmask = System.UInt64;
    using i16 = System.Int16;
    using i64 = System.Int64;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using u64 = System.UInt64;

    using sqlite3_int64 = System.Int64;
    using Pgno = System.UInt32;

    /*
    ** A list of expressions.  Each expression may optionally have a
    ** name.  An expr/name combination can be used in several ways, such
    ** as the list of "expr AS ID" fields following a "SELECT" or in the
    ** list of "ID = expr" items in an UPDATE.  A list of expressions can
    ** also be used as the argument to a function, in which case the a.zName
    ** field is not used.
    */
    public class ExprList_item
    {
        public Expr pExpr;          /* The list of expressions */
        public string zName;        /* Token associated with this expression */
        public string zSpan;        /*  Original text of the expression */
        public u8 sortOrder;        /* 1 for DESC or 0 for ASC */
        public u8 done;             /* A flag to indicate when processing is finished */
        public u16 iCol;            /* For ORDER BY, column number in result set */
        public u16 iAlias;          /* Index into Parse.aAlias[] for zName */
    }
    public class ExprList
    {
        public int nExpr;             /* Number of expressions on the list */
        public int nAlloc;            /* Number of entries allocated below */
        public int iECursor;          /* VDBE VdbeCursor associated with this ExprList */
        public ExprList_item[] a;     /* One entry for each expression */

        public ExprList Copy()
        {
            if ( this == null )
                return null;
            else
            {
                ExprList cp = (ExprList)MemberwiseClone();
                a.CopyTo( cp.a, 0 );
                return cp;
            }
        }

    };
}

