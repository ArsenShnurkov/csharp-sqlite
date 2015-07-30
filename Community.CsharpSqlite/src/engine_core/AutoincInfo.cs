namespace Community.CsharpSqlite
{
    using System;
    
    /*
    ** During code generation of statements that do inserts into AUTOINCREMENT
    ** tables, the following information is attached to the Table.u.autoInc.p
    ** pointer of each autoincrement table to record some side information that
    ** the code generator needs.  We have to keep per-table autoincrement
    ** information in case inserts are down within triggers.  Triggers do not
    ** normally coordinate their activities, but we do need to coordinate the
    ** loading and saving of autoincrement information.
    */
    public class AutoincInfo
    {
        public AutoincInfo pNext;    /* Next info block in a list of them all */
        public Table pTab;           /* Table this info block refers to */
        public int iDb;              /* Index in sqlite3.aDb[] of database holding pTab */
        public int regCtr;           /* Memory register holding the rowid counter */
    }
}
