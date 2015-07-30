namespace Community.CsharpSqlite
{
    using System;
    using u32 = System.UInt32;

    /*
    ** A WherePlan object holds information that describes a lookup
    ** strategy.
    **
    ** This object is intended to be opaque outside of the where.c module.
    ** It is included here only so that that compiler will know how big it
    ** is.  None of the fields in this object should be used outside of
    ** the where.c module.
    **
    ** Within the union, pIdx is only used when wsFlags&WHERE_INDEXED is true.
    ** pTerm is only used when wsFlags&WHERE_MULTI_OR is true.  And pVtabIdx
    ** is only used when wsFlags&WHERE_VIRTUALTABLE is true.  It is never the
    ** case that more than one of these conditions is true.
    */
    public class WherePlan
    {
        public u32 wsFlags;
        /* WHERE_* flags that describe the strategy */
        public u32 nEq;
        /* Number of == constraints */
        public double nRow;
        /* Estimated number of rows (for EQP) */
        public class _u
        {
            public Index pIdx;
            /* Index when WHERE_INDEXED is true */
            public WhereTerm pTerm;
            /* WHERE clause term for OR-search */
            public sqlite3_index_info pVtabIdx;
            /* Virtual table index to use */
        }

        public _u u = new _u();

        public void Clear()
        {
            wsFlags = 0;
            nEq = 0;
            nRow = 0;
            u.pIdx = null;
            u.pTerm = null;
            u.pVtabIdx = null;
        }
    }
}

