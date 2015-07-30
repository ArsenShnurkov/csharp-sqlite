namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    ** Each sample stored in the sqlite_stat2 table is represented in memory 
    ** using a structure of this type.
    */
    public class IndexSample
    {
        public struct _u
        { //union {
            public string z;        /* Value if eType is SQLITE_TEXT */
            public byte[] zBLOB;    /* Value if eType is SQLITE_BLOB */
            public double r;        /* Value if eType is SQLITE_FLOAT or SQLITE_INTEGER */
        }
        public _u u;
        public u8 eType;         /* SQLITE_NULL, SQLITE_INTEGER ... etc. */
        public u8 nByte;         /* Size in byte of text or blob. */
    }
}

