using System;

namespace Community.CsharpSqlite
{
    /*
    ** A WhereTerm with eOperator==WO_AND has its u.pAndInfo pointer set to
    ** a dynamically allocated instance of the following structure.
    */
    public class WhereAndInfo
    {
        public WhereClause wc = new WhereClause();          /* The subexpression broken out */
    };
}

