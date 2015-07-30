namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    * An instance of struct TriggerStep is used to store a single SQL statement
    * that is a part of a trigger-program.
    *
    * Instances of struct TriggerStep are stored in a singly linked list (linked
    * using the "pNext" member) referenced by the "step_list" member of the
    * associated struct Trigger instance. The first element of the linked list is
    * the first step of the trigger-program.
    *
    * The "op" member indicates whether this is a "DELETE", "INSERT", "UPDATE" or
    * "SELECT" statement. The meanings of the other members is determined by the
    * value of "op" as follows:
    *
    * (op == TK_INSERT)
    * orconf    -> stores the ON CONFLICT algorithm
    * pSelect   -> If this is an INSERT INTO ... SELECT ... statement, then
    *              this stores a pointer to the SELECT statement. Otherwise NULL.
    * target    -> A token holding the quoted name of the table to insert into.
    * pExprList -> If this is an INSERT INTO ... VALUES ... statement, then
    *              this stores values to be inserted. Otherwise NULL.
    * pIdList   -> If this is an INSERT INTO ... (<column-names>) VALUES ...
    *              statement, then this stores the column-names to be
    *              inserted into.
    *
    * (op == TK_DELETE)
    * target    -> A token holding the quoted name of the table to delete from.
    * pWhere    -> The WHERE clause of the DELETE statement if one is specified.
    *              Otherwise NULL.
    *
    * (op == TK_UPDATE)
    * target    -> A token holding the quoted name of the table to update rows of.
    * pWhere    -> The WHERE clause of the UPDATE statement if one is specified.
    *              Otherwise NULL.
    * pExprList -> A list of the columns to update and the expressions to update
    *              them to. See sqlite3Update() documentation of "pChanges"
    *              argument.
    *
    */
    public class TriggerStep
    {
        public u8 op;               /* One of TK_DELETE, TK_UPDATE, TK_INSERT, TK_SELECT */
        public u8 orconf;           /* OE_Rollback etc. */
        public Trigger pTrig;       /* The trigger that this step is a part of */
        public Select pSelect;      /* SELECT statment or RHS of INSERT INTO .. SELECT ... */
        public Token target;        /* Target table for DELETE, UPDATE, INSERT */
        public Expr pWhere;         /* The WHERE clause for DELETE or UPDATE steps */
        public ExprList pExprList;  /* SET clause for UPDATE.  VALUES clause for INSERT */
        public IdList pIdList;      /* Column names for INSERT */
        public TriggerStep pNext;   /* Next in the link-list */
        public TriggerStep pLast;   /* Last element in link-list. Valid for 1st elem only */

        public TriggerStep()
        {
            target = new Token();
        }
        public TriggerStep Copy()
        {
            if ( this == null )
                return null;
            else
            {
                TriggerStep cp = (TriggerStep)MemberwiseClone();
                return cp;
            }
        }
    }
}

