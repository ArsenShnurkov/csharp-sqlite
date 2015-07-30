namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    ** A NameContext defines a context in which to resolve table and column
    ** names.  The context consists of a list of tables (the pSrcList) field and
    ** a list of named expression (pEList).  The named expression list may
    ** be NULL.  The pSrc corresponds to the FROM clause of a SELECT or
    ** to the table being operated on by INSERT, UPDATE, or DELETE.  The
    ** pEList corresponds to the result set of a SELECT and is NULL for
    ** other statements.
    **
    ** NameContexts can be nested.  When resolving names, the inner-most
    ** context is searched first.  If no match is found, the next outer
    ** context is checked.  If there is still no match, the next context
    ** is checked.  This process continues until either a match is found
    ** or all contexts are check.  When a match is found, the nRef member of
    ** the context containing the match is incremented.
    **
    ** Each subquery gets a new NameContext.  The pNext field points to the
    ** NameContext in the parent query.  Thus the process of scanning the
    ** NameContext list corresponds to searching through successively outer
    ** subqueries looking for a match.
    */
    public class NameContext
    {
        public Parse pParse;       /* The parser */
        public SrcList pSrcList;   /* One or more tables used to resolve names */
        public ExprList pEList;    /* Optional list of named expressions */
        public int nRef;           /* Number of names resolved by this context */
        public int nErr;           /* Number of errors encountered while resolving names */
        public u8 allowAgg;        /* Aggregate functions allowed here */
        public u8 hasAgg;          /* True if aggregates are seen */
        public u8 isCheck;         /* True if resolving names in a CHECK constraint */
        public int nDepth;         /* Depth of subquery recursion. 1 for no recursion */
        public AggInfo pAggInfo;   /* Information about aggregates at this level */
        public NameContext pNext;  /* Next outer name context.  NULL for outermost */
    };
    
}

