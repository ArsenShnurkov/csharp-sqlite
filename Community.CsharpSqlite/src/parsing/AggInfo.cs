namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    ** An instance of this structure contains information needed to generate
    ** code for a SELECT that contains aggregate functions.
    **
    ** If Expr.op==TK_AGG_COLUMN or TK_AGG_FUNCTION then Expr.pAggInfo is a
    ** pointer to this structure.  The Expr.iColumn field is the index in
    ** AggInfo.aCol[] or AggInfo.aFunc[] of information needed to generate
    ** code for that node.
    **
    ** AggInfo.pGroupBy and AggInfo.aFunc.pExpr point to fields within the
    ** original Select structure that describes the SELECT statement.  These
    ** fields do not need to be freed when deallocating the AggInfo structure.
    */
    public class AggInfo_col
    {
        /* For each column used in source tables */
        public Table pTab;
        /* Source table */
        public int iTable;
        /* VdbeCursor number of the source table */
        public int iColumn;
        /* Column number within the source table */
        public int iSorterColumn;
        /* Column number in the sorting index */
        public int iMem;
        /* Memory location that acts as accumulator */
        public Expr pExpr;
        /* The original expression */
    };

    public class AggInfo_func
    {
        /* For each aggregate function */
        public Expr pExpr;
        /* Expression encoding the function */
        public FuncDef pFunc;
        /* The aggregate function implementation */
        public int iMem;
        /* Memory location that acts as accumulator */
        public int iDistinct;
        /* Ephemeral table used to enforce DISTINCT */
    }

    public class AggInfo
    {
        public u8 directMode;
        /* Direct rendering mode means take data directly
** from source tables rather than from accumulators */
        public u8 useSortingIdx;
        /* In direct mode, reference the sorting index rather
** than the source table */
        public int sortingIdx;
        /* VdbeCursor number of the sorting index */
        public ExprList pGroupBy;
        /* The group by clause */
        public int nSortingColumn;
        /* Number of columns in the sorting index */
        public AggInfo_col[] aCol;
        public int nColumn;
        /* Number of used entries in aCol[] */
        public int nColumnAlloc;
        /* Number of slots allocated for aCol[] */
        public int nAccumulator;
        /* Number of columns that show through to the output.
** Additional columns are used only as parameters to
** aggregate functions */
        public AggInfo_func[] aFunc;
        public int nFunc;
        /* Number of entries in aFunc[] */
        public int nFuncAlloc;
        /* Number of slots allocated for aFunc[] */

        public AggInfo Copy()
        {
            if (this == null)
                return null;
            else
            {
                AggInfo cp = (AggInfo)MemberwiseClone();
                if (pGroupBy != null)
                    cp.pGroupBy = pGroupBy.Copy();
                return cp;
            }
        }
    }
}

