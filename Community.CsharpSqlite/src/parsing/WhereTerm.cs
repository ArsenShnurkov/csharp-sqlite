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
    ** The query generator uses an array of instances of this structure to
    ** help it analyze the subexpressions of the WHERE clause.  Each WHERE
    ** clause subexpression is separated from the others by AND operators,
    ** usually, or sometimes subexpressions separated by OR.
    **
    ** All WhereTerms are collected into a single WhereClause structure.
    ** The following identity holds:
    **
    **        WhereTerm.pWC.a[WhereTerm.idx] == WhereTerm
    **
    ** When a term is of the form:
    **
    **              X <op> <expr>
    **
    ** where X is a column name and <op> is one of certain operators,
    ** then WhereTerm.leftCursor and WhereTerm.u.leftColumn record the
    ** cursor number and column number for X.  WhereTerm.eOperator records
    ** the <op> using a bitmask encoding defined by WO_xxx below.  The
    ** use of a bitmask encoding for the operator allows us to search
    ** quickly for terms that match any of several different operators.
    **
    ** A WhereTerm might also be two or more subterms connected by OR:
    **
    **         (t1.X <op> <expr>) OR (t1.Y <op> <expr>) OR ....
    **
    ** In this second case, wtFlag as the TERM_ORINFO set and eOperator==WO_OR
    ** and the WhereTerm.u.pOrInfo field points to auxiliary information that
    ** is collected about the
    **
    ** If a term in the WHERE clause does not match either of the two previous
    ** categories, then eOperator==0.  The WhereTerm.pExpr field is still set
    ** to the original subexpression content and wtFlags is set up appropriately
    ** but no other fields in the WhereTerm object are meaningful.
    **
    ** When eOperator!=0, prereqRight and prereqAll record sets of cursor numbers,
    ** but they do so indirectly.  A single WhereMaskSet structure translates
    ** cursor number into bits and the translated bit is stored in the prereq
    ** fields.  The translation is used in order to maximize the number of
    ** bits that will fit in a Bitmask.  The VDBE cursor numbers might be
    ** spread out over the non-negative integers.  For example, the cursor
    ** numbers might be 3, 8, 9, 10, 20, 23, 41, and 45.  The WhereMaskSet
    ** translates these sparse cursor numbers into consecutive integers
    ** beginning with 0 in order to make the best possible use of the available
    ** bits in the Bitmask.  So, in the example above, the cursor numbers
    ** would be mapped into integers 0 through 7.
    **
    ** The number of terms in a join is limited by the number of bits
    ** in prereqRight and prereqAll.  The default is 64 bits, hence SQLite
    ** is only able to process joins with 64 or fewer tables.
    */
    //typedef struct WhereTerm WhereTerm;
    public class WhereTerm
    {
        public Expr pExpr;
        /* Pointer to the subexpression that is this term */
        public int iParent;
        /* Disable pWC.a[iParent] when this term disabled */
        public int leftCursor;
        /* Cursor number of X in "X <op> <expr>" */
        public class _u
        {
            public int leftColumn;
            /* Column number of X in "X <op> <expr>" */
            public WhereOrInfo pOrInfo;
            /* Extra information if eOperator==WO_OR */
            public WhereAndInfo pAndInfo;
            /* Extra information if eOperator==WO_AND */
        }

        public _u u = new _u();
        public u16 eOperator;
        /* A WO_xx value describing <op> */
        public u8 wtFlags;
        /* TERM_xxx bit flags.  See below */
        public u8 nChild;
        /* Number of children that must disable us */
        public WhereClause pWC;
        /* The clause this term is part of */
        public Bitmask prereqRight;
        /* Bitmask of tables used by pExpr.pRight */
        public Bitmask prereqAll;
        /* Bitmask of tables referenced by pExpr */
    };

}