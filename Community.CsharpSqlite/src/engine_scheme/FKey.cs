namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;

    /*
** Each foreign key constraint is an instance of the following structure.
**
** A foreign key is associated with two tables.  The "from" table is
** the table that contains the REFERENCES clause that creates the foreign
** key.  The "to" table is the table that is named in the REFERENCES clause.
** Consider this example:
**
**     CREATE TABLE ex1(
**       a INTEGER PRIMARY KEY,
**       b INTEGER CONSTRAINT fk1 REFERENCES ex2(x)
**     );
**
** For foreign key "fk1", the from-table is "ex1" and the to-table is "ex2".
**
** Each REFERENCES clause generates an instance of the following structure
** which is attached to the from-table.  The to-table need not exist when
** the from-table is created.  The existence of the to-table is not checked.
*/
    public class FKey
    {
        public Table pFrom;
        /* Table containing the REFERENCES clause (aka: Child) */
        public FKey pNextFrom;
        /* Next foreign key in pFrom */
        public string zTo;
        /* Name of table that the key points to (aka: Parent) */
        public FKey pNextTo;
        /* Next foreign key on table named zTo */
        public FKey pPrevTo;
        /* Previous foreign key on table named zTo */
        public int nCol;
        /* Number of columns in this key */
        /* EV: R-30323-21917 */
        public u8 isDeferred;
        /* True if constraint checking is deferred till COMMIT */
        public u8[] aAction = new u8[2];
        /* ON DELETE and ON UPDATE actions, respectively */
        public Trigger[] apTrigger = new Trigger[2];
/* Triggers for aAction[] actions */

        public class sColMap
        {
            /* Mapping of columns in pFrom to columns in zTo */
            public int iFrom;
            /* Index of column in pFrom */
            public string zCol;
            /* Name of column in zTo.  If 0 use PRIMARY KEY */
        };

        public sColMap[] aCol;
        /* One entry for each of nCol column s */

        public FKey Copy()
        {
            if (this == null)
                return null;
            else
            {
                FKey cp = (FKey)MemberwiseClone();
                return cp;
            }
        }

    }
}

