namespace Community.CsharpSqlite
{
    using System;
    
    using i16 = System.Int16;
    using i32 = System.Int32;
    using i64 = System.Int64;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using u64 = System.UInt64;
    
    /*
    ** An instance of the following structure holds information about a
    ** single index record that has already been parsed out into individual
    ** values.
    **
    ** A record is an object that contains one or more fields of data.
    ** Records are used to store the content of a table row and to store
    ** the key of an index.  A blob encoding of a record is created by
    ** the OP_MakeRecord opcode of the VDBE and is disassembled by the
    ** OP_Column opcode.
    **
    ** This structure holds a record that has already been disassembled
    ** into its constituent fields.
    */
    public class UnpackedRecord
    {
        public KeyInfo pKeyInfo;   /* Collation and sort-order information */
        public u16 nField;         /* Number of entries in apMem[] */
        public u16 flags;          /* Boolean settings.  UNPACKED_... below */
        public i64 rowid;          /* Used by UNPACKED_PREFIX_SEARCH */
        public Mem[] aMem;         /* Values */
       
        /*
        ** Allowed values of UnpackedRecord.flags
        */   
        /// <summary>
        /// Memory is from sqlite3Malloc()
        /// </summary>
        const int UNPACKED_NEED_FREE = 0x0001;  
        /// <summary>
        /// apMem[]s should all be destroyed
        /// </summary>
        const int UNPACKED_NEED_DESTROY = 0x0002;  
        
        /// <summary>
        /// Ignore trailing rowid on key1
        /// </summary>
        const int UNPACKED_IGNORE_ROWID = 0x0004;  
        
        /// <summary>
        /// Make this key an epsilon larger
        /// </summary>
        const int UNPACKED_INCRKEY = 0x0008;  
        
        /// <summary>
        /// A prefix match is considered OK
        /// </summary>
        const int UNPACKED_PREFIX_MATCH = 0x0010; 
        
        /// <summary>
        /// A prefix match is considered OK
        /// </summary>
        const int UNPACKED_PREFIX_SEARCH = 0x0020; 
    };
}

