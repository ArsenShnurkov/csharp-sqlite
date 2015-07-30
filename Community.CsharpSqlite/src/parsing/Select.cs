namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u16 = System.UInt16;

    /*
    ** An instance of the following structure contains all information
    ** needed to generate code for a single SELECT statement.
    **
    ** nLimit is set to -1 if there is no LIMIT clause.  nOffset is set to 0.
    ** If there is a LIMIT clause, the parser sets nLimit to the value of the
    ** limit and nOffset to the value of the offset (or 0 if there is not
    ** offset).  But later on, nLimit and nOffset become the memory locations
    ** in the VDBE that record the limit and offset counters.
    **
    ** addrOpenEphm[] entries contain the address of OP_OpenEphemeral opcodes.
    ** These addresses must be stored so that we can go back and fill in
    ** the P4_KEYINFO and P2 parameters later.  Neither the KeyInfo nor
    ** the number of columns in P2 can be computed at the same time
    ** as the OP_OpenEphm instruction is coded because not
    ** enough information about the compound query is known at that point.
    ** The KeyInfo for addrOpenTran[0] and [1] contains collating sequences
    ** for the result set.  The KeyInfo for addrOpenTran[2] contains collating
    ** sequences for the ORDER BY clause.
    */
    public class Select
    {
        public ExprList pEList;      /* The fields of the result */
        public u8 op;                /* One of: TK_UNION TK_ALL TK_INTERSECT TK_EXCEPT */
        public char affinity;        /* MakeRecord with this affinity for SRT_Set */
        public u16 selFlags;         /* Various SF_* values */
        public SrcList pSrc;         /* The FROM clause */
        public Expr pWhere;          /* The WHERE clause */
        public ExprList pGroupBy;    /* The GROUP BY clause */
        public Expr pHaving;         /* The HAVING clause */
        public ExprList pOrderBy;    /* The ORDER BY clause */
        public Select pPrior;        /* Prior select in a compound select statement */
        public Select pNext;         /* Next select to the left in a compound */
        public Select pRightmost;    /* Right-most select in a compound select statement */
        public Expr pLimit;          /* LIMIT expression. NULL means not used. */
        public Expr pOffset;         /* OFFSET expression. NULL means not used. */
        public int iLimit;
        public int iOffset;          /* Memory registers holding LIMIT & OFFSET counters */
        public int[] addrOpenEphm = new int[3];   /* OP_OpenEphem opcodes related to this select */
        public double nSelectRow;    /* Estimated number of result rows */

        public Select Copy()
        {
            if ( this == null )
                return null;
            else
            {
                Select cp = (Select)MemberwiseClone();
                if ( pEList != null )
                    cp.pEList = pEList.Copy();
                if ( pSrc != null )
                    cp.pSrc = pSrc.Copy();
                if ( pWhere != null )
                    cp.pWhere = pWhere.Copy();
                if ( pGroupBy != null )
                    cp.pGroupBy = pGroupBy.Copy();
                if ( pHaving != null )
                    cp.pHaving = pHaving.Copy();
                if ( pOrderBy != null )
                    cp.pOrderBy = pOrderBy.Copy();
                if ( pPrior != null )
                    cp.pPrior = pPrior.Copy();
                if ( pNext != null )
                    cp.pNext = pNext.Copy();
                if ( pRightmost != null )
                    cp.pRightmost = pRightmost.Copy();
                if ( pLimit != null )
                    cp.pLimit = pLimit.Copy();
                if ( pOffset != null )
                    cp.pOffset = pOffset.Copy();
                return cp;
            }
        }
    };
    }

