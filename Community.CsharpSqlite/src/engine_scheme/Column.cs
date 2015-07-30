namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    
    /*
    ** information about each column of an SQL table is held in an instance
    ** of this structure.
    */
    public class Column
    {
        public string zName;      /* Name of this column */
        public Expr pDflt;        /* Default value of this column */
        public string zDflt;      /* Original text of the default value */
        public string zType;      /* Data type for this column */
        public string zColl;      /* Collating sequence.  If NULL, use the default */
        public u8 notNull;        /* True if there is a NOT NULL constraint */
        public u8 isPrimKey;      /* True if this column is part of the PRIMARY KEY */
        public char affinity;     /* One of the SQLITE_AFF_... values */
        #if !SQLITE_OMIT_VIRTUALTABLE
        public   u8 isHidden;     /* True if this column is 'hidden' */
        #endif
        public Column Copy()
        {
            Column cp = (Column)MemberwiseClone();
            if ( cp.pDflt != null )
                cp.pDflt = pDflt.Copy();
            return cp;
        }
    }
}

