namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u32 = System.UInt32;

    /*
    * Each trigger present in the database schema is stored as an instance of
    * struct Trigger.
    *
    * Pointers to instances of struct Trigger are stored in two ways.
    * 1. In the "trigHash" hash table (part of the sqlite3* that represents the
    *    database). This allows Trigger structures to be retrieved by name.
    * 2. All triggers associated with a single table form a linked list, using the
    *    pNext member of struct Trigger. A pointer to the first element of the
    *    linked list is stored as the "pTrigger" member of the associated
    *    struct Table.
    *
    * The "step_list" member points to the first element of a linked list
    * containing the SQL statements specified as the trigger program.
    */
    public class Trigger
    {
        public string zName;
        /* The name of the trigger                        */
        public string table;
        /* The table or view to which the trigger applies */
        public u8 op;
        /* One of TK_DELETE, TK_UPDATE, TK_INSERT         */
        public u8 tr_tm;
        /* One of TRIGGER_BEFORE, TRIGGER_AFTER */
        public Expr pWhen;
        /* The WHEN clause of the expression (may be NULL) */
        public IdList pColumns;
        /* If this is an UPDATE OF <column-list> trigger,
the <column-list> is stored here */
        public Schema pSchema;
        /* Schema containing the trigger */
        public Schema pTabSchema;
        /* Schema containing the table */
        public TriggerStep step_list;
        /* Link list of trigger program steps             */
        public Trigger pNext;
        /* Next trigger associated with the table */

        public Trigger Copy()
        {
            if (this == null)
                return null;
            else
            {
                Trigger cp = (Trigger)MemberwiseClone();
                if (pWhen != null)
                    cp.pWhen = pWhen.Copy();
                if (pColumns != null)
                    cp.pColumns = pColumns.Copy();
                if (pSchema != null)
                    cp.pSchema = pSchema.Copy();
                if (pTabSchema != null)
                    cp.pTabSchema = pTabSchema.Copy();
                if (step_list != null)
                    cp.step_list = step_list.Copy();
                if (pNext != null)
                    cp.pNext = pNext.Copy();
                return cp;
            }
        }
    }

       
}

