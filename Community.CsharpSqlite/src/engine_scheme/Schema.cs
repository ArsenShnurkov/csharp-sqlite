namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    
    /*
    ** An instance of the following structure stores a database schema.
    **
    ** Most Schema objects are associated with a Btree.  The exception is
    ** the Schema for the TEMP databaes (sqlite3.aDb[1]) which is free-standing.
    ** In shared cache mode, a single Schema object can be shared by multiple
    ** Btrees that refer to the same underlying BtShared object.
    ** 
    ** Schema objects are automatically deallocated when the last Btree that
    ** references them is destroyed.   The TEMP Schema is manually freed by
    ** sqlite3_close().
    *
    ** A thread must be holding a mutex on the corresponding Btree in order
    ** to access Schema content.  This implies that the thread must also be
    ** holding a mutex on the sqlite3 connection pointer that owns the Btree.
    ** For a TEMP Schema, only the connection mutex is required.
    */
    public class Schema
    {
        public int schema_cookie;         /* Database schema version number for this file */
        public u32 iGeneration;           /* Generation counter.  Incremented with each change */
        public Hash tblHash = new Hash(); /* All tables indexed by name */
        public Hash idxHash = new Hash(); /* All (named) indices indexed by name */
        public Hash trigHash = new Hash();/* All triggers indexed by name */
        public Hash fkeyHash = new Hash();/* All foreign keys by referenced table name */
        public Table pSeqTab;             /* The sqlite_sequence table used by AUTOINCREMENT */
        public u8 file_format;            /* Schema format version for this file */
        public u8 enc;                    /* Text encoding used by this database */
        public u16 flags;                 /* Flags associated with this schema */
        public int cache_size;            /* Number of pages to use in the cache */
        public Schema Copy()
        {
            if ( this == null )
                return null;
            else
            {
                Schema cp = (Schema)MemberwiseClone();
                return cp;
            }
        }

        public void Clear()
        {
            if ( this != null )
            {
                schema_cookie = 0;
                tblHash = new Hash();
                idxHash = new Hash();
                trigHash = new Hash();
                fkeyHash = new Hash();
                pSeqTab = null;
            }
        }
    }
}
