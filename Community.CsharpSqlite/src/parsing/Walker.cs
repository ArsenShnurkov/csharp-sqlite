namespace Community.CsharpSqlite
{
    using System;
    
    /*
    ** Context pointer passed down through the tree-walk.
    */
    public class Walker
    {
        public dxExprCallback xExprCallback; //)(Walker*, Expr);     /* Callback for expressions */
        public dxSelectCallback xSelectCallback; //)(Walker*,Select);  /* Callback for SELECTs */
        public Parse pParse;                            /* Parser context.  */
        public struct uw
        {                              /* Extra data for callback */
            public NameContext pNC;                       /* Naming context */
            public int i;                                 /* Integer value */
        }
        public uw u;
    }
}

