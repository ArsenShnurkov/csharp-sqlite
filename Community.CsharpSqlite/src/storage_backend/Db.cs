namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    ** Each database file to be accessed by the system is an instance
    ** of the following structure.  There are normally two of these structures
    ** in the sqlite.aDb[] array.  aDb[0] is the main database file and
    ** aDb[1] is the database file used to hold temporary tables.  Additional
    ** databases may be attached.
    */
    public class Db
    {
        public string zName;                  /*  Name of this database  */
        public Btree pBt;                     /*  The B Tree structure for this database file  */
        public u8 inTrans;                    /*  0: not writable.  1: Transaction.  2: Checkpoint  */
        public u8 safety_level;               /*  How aggressive at syncing data to disk  */
        public Schema pSchema;                /* Pointer to database schema (possibly shared)  */
    }
}

