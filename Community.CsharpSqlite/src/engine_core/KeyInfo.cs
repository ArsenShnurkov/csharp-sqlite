namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    
    /*
    ** An instance of the following structure is passed as the first
    ** argument to sqlite3VdbeKeyCompare and is used to control the
    ** comparison of the two index keys.
    */
    public class KeyInfo
    {
        public sqlite3 db;         /* The database connection */
        public u8 enc;             /* Text encoding - one of the SQLITE_UTF* values */
        public u16 nField;         /* Number of entries in aColl[] */
        public u8[] aSortOrder;    /* Sort order for each column.  May be NULL */
        public CollSeq[] aColl = new CollSeq[1];  /* Collating sequence for each term of the key */
        public KeyInfo Copy()
        {
            return (KeyInfo)MemberwiseClone();
        }
    };
    
}

