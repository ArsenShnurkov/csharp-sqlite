namespace Community.CsharpSqlite
{
    using System;

    #if !SQLITE_MAX_VARIABLE_NUMBER
    using ynVar = System.Int16;
    #else
    using ynVar = System.Int32; 
    #endif

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
    ** Each node of an expression in the parse tree is an instance
    ** of this structure.
    **
    ** Expr.op is the opcode.  The integer parser token codes are reused
    ** as opcodes here.  For example, the parser defines TK_GE to be an integer
    ** code representing the ">=" operator.  This same integer code is reused
    ** to represent the greater-than-or-equal-to operator in the expression
    ** tree.
    **
    ** If the expression is an SQL literal (TK_INTEGER, TK_FLOAT, TK_BLOB,
    ** or TK_STRING), then Expr.token contains the text of the SQL literal. If
    ** the expression is a variable (TK_VARIABLE), then Expr.token contains the
    ** variable name. Finally, if the expression is an SQL function (TK_FUNCTION),
    ** then Expr.token contains the name of the function.
    **
    ** Expr.pRight and Expr.pLeft are the left and right subexpressions of a
    ** binary operator. Either or both may be NULL.
    **
    ** Expr.x.pList is a list of arguments if the expression is an SQL function,
    ** a CASE expression or an IN expression of the form "<lhs> IN (<y>, <z>...)".
    ** Expr.x.pSelect is used if the expression is a sub-select or an expression of
    ** the form "<lhs> IN (SELECT ...)". If the EP_xIsSelect bit is set in the
    ** Expr.flags mask, then Expr.x.pSelect is valid. Otherwise, Expr.x.pList is
    ** valid.
    **
    ** An expression of the form ID or ID.ID refers to a column in a table.
    ** For such expressions, Expr.op is set to TK_COLUMN and Expr.iTable is
    ** the integer cursor number of a VDBE cursor pointing to that table and
    ** Expr.iColumn is the column number for the specific column.  If the
    ** expression is used as a result in an aggregate SELECT, then the
    ** value is also stored in the Expr.iAgg column in the aggregate so that
    ** it can be accessed after all aggregates are computed.
    **
    ** If the expression is an unbound variable marker (a question mark
    ** character '?' in the original SQL) then the Expr.iTable holds the index
    ** number for that variable.
    **
    ** If the expression is a subquery then Expr.iColumn holds an integer
    ** register number containing the result of the subquery.  If the
    ** subquery gives a constant result, then iTable is -1.  If the subquery
    ** gives a different answer at different times during statement processing
    ** then iTable is the address of a subroutine that computes the subquery.
    **
    ** If the Expr is of type OP_Column, and the table it is selecting from
    ** is a disk table or the "old.*" pseudo-table, then pTab points to the
    ** corresponding table definition.
    **
    ** ALLOCATION NOTES:
    **
    ** Expr objects can use a lot of memory space in database schema.  To
    ** help reduce memory requirements, sometimes an Expr object will be
    ** truncated.  And to reduce the number of memory allocations, sometimes
    ** two or more Expr objects will be stored in a single memory allocation,
    ** together with Expr.zToken strings.
    **
    ** If the EP_Reduced and EP_TokenOnly flags are set when
    ** an Expr object is truncated.  When EP_Reduced is set, then all
    ** the child Expr objects in the Expr.pLeft and Expr.pRight subtrees
    ** are contained within the same memory allocation.  Note, however, that
    ** the subtrees in Expr.x.pList or Expr.x.pSelect are always separately
    ** allocated, regardless of whether or not EP_Reduced is set.
    */
    public class Expr
    {
        #if DEBUG_CLASS_EXPR || DEBUG_CLASS_ALL
        public u8 _op;                      /* Operation performed by this node */
        public u8 op
        {
        get { return _op; }
        set { _op = value; }
        }
        
#else
        public u8 op;
        /* Operation performed by this node */
        #endif
        public char affinity;
        /* The affinity of the column or 0 if not a column */
        #if DEBUG_CLASS_EXPR || DEBUG_CLASS_ALL
        public u16 _flags;                            /* Various flags.  EP_* See below */
        public u16 flags
        {
        get { return _flags; }
        set { _flags = value; }
        }
        public struct _u
        {
        public string _zToken;         /* Token value. Zero terminated and dequoted */
        public string zToken
        {
        get { return _zToken; }
        set { _zToken = value; }
        }
        public int iValue;            /* Non-negative integer value if EP_IntValue */
        }

        
#else
        public struct _u
        {
            public string zToken;
            /* Token value. Zero terminated and dequoted */
            public int iValue;
            /* Non-negative integer value if EP_IntValue */
        }

        public u16 flags;
        /* Various flags.  EP_* See below */
        #endif
        public _u u;

        /* If the EP_TokenOnly flag is set in the Expr.flags mask, then no
      ** space is allocated for the fields below this point. An attempt to
      ** access them will result in a segfault or malfunction.
      *********************************************************************/

        public Expr pLeft;
        /* Left subnode */
        public Expr pRight;
        /* Right subnode */
        public struct _x
        {
            public ExprList pList;
            /* Function arguments or in "<expr> IN (<expr-list)" */
            public Select pSelect;
            /* Used for sub-selects and "<expr> IN (<select>)" */
        }

        public _x x;
        public CollSeq pColl;
        /* The collation type of the column or 0 */

        /* If the EP_Reduced flag is set in the Expr.flags mask, then no
        ** space is allocated for the fields below this point. An attempt to
            ** access them will result in a segfault or malfunction.
            *********************************************************************/

        public int iTable;
        /* TK_COLUMN: cursor number of table holding column
  ** TK_REGISTER: register number
  ** TK_TRIGGER: 1 -> new, 0 -> old */
        public ynVar iColumn;
        /* TK_COLUMN: column index.  -1 for rowid.
  ** TK_VARIABLE: variable number (always >= 1). */
        public i16 iAgg;
        /* Which entry in pAggInfo->aCol[] or ->aFunc[] */
        public i16 iRightJoinTable;
        /* If EP_FromJoin, the right table of the join */
        public u8 flags2;
        /* Second set of flags.  EP2_... */
        public u8 op2;
        /* If a TK_REGISTER, the original value of Expr.op */
        public AggInfo pAggInfo;
        /* Used by TK_AGG_COLUMN and TK_AGG_FUNCTION */
        public Table pTab;
        /* Table for TK_COLUMN expressions. */
        #if SQLITE_MAX_EXPR_DEPTH //>0
        public int nHeight;           /* Height of the tree headed by this node */
        public Table pZombieTab;      /* List of Table objects to delete after code gen */
        #endif

        #if DEBUG_CLASS
        public int op
        {
        get { return _op; }
        set { _op = value; }
        }
        #endif
        public void CopyFrom(Expr cf)
        {
            op = cf.op;
            affinity = cf.affinity;
            flags = cf.flags;
            u = cf.u;
            pColl = cf.pColl == null ? null : cf.pColl.Copy();
            iTable = cf.iTable;
            iColumn = cf.iColumn;
            pAggInfo = cf.pAggInfo == null ? null : cf.pAggInfo.Copy();
            iAgg = cf.iAgg;
            iRightJoinTable = cf.iRightJoinTable;
            flags2 = cf.flags2;
            pTab = cf.pTab == null ? null : cf.pTab;
            #if SQLITE_TEST || SQLITE_MAX_EXPR_DEPTH //SQLITE_MAX_EXPR_DEPTH>0
            nHeight = cf.nHeight;
            pZombieTab = cf.pZombieTab;
            #endif
            pLeft = cf.pLeft == null ? null : cf.pLeft.Copy();
            pRight = cf.pRight == null ? null : cf.pRight.Copy();
            x.pList = cf.x.pList == null ? null : cf.x.pList.Copy();
            x.pSelect = cf.x.pSelect == null ? null : cf.x.pSelect.Copy();
        }

        public Expr Copy()
        {
            if (this == null)
                return null;
            else
                return Copy(flags);
        }

        public Expr Copy(int flag)
        {
            Expr cp = new Expr();
            cp.op = op;
            cp.affinity = affinity;
            cp.flags = flags;
            cp.u = u;
            if ((flag & EP_TokenOnly) != 0)
                return cp;
            if (pLeft != null)
                cp.pLeft = pLeft.Copy();
            if (pRight != null)
                cp.pRight = pRight.Copy();
            cp.x = x;
            cp.pColl = pColl;
            if ((flag & EP_Reduced) != 0)
                return cp;
            cp.iTable = iTable;
            cp.iColumn = iColumn;
            cp.iAgg = iAgg;
            cp.iRightJoinTable = iRightJoinTable;
            cp.flags2 = flags2;
            cp.op2 = op2;
            cp.pAggInfo = pAggInfo;
            cp.pTab = pTab;
            #if SQLITE_MAX_EXPR_DEPTH //>0
            cp.nHeight = nHeight;
            cp.pZombieTab = pZombieTab;
            #endif
            return cp;
        }
    };
}

