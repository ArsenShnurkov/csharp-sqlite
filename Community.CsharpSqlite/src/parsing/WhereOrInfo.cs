namespace Community.CsharpSqlite
{
    using System;
    using Bitmask = System.UInt64;
    
    /*
    ** A WhereTerm with eOperator==WO_OR has its u.pOrInfo pointer set to
    ** a dynamically allocated instance of the following structure.
    */
    public class WhereOrInfo
    {
        public WhereClause wc = new WhereClause();/* Decomposition into subterms */
        public Bitmask indexable;                 /* Bitmask of all indexable tables in the clause */
    };
}

