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
    ** An instance of the following structure is used to hold information
    ** about a cell.  The parseCellPtr() function fills in this structure
    ** based on information extract from the raw disk page.
    */
    //typedef struct CellInfo CellInfo;
    public struct CellInfo
    {
        public int iCell;
        /* Offset to start of cell content -- Needed for C# */
        public byte[] pCell;
        /* Pointer to the start of cell content */
        public i64 nKey;
        /* The key for INTKEY tables, or number of bytes in key */
        public u32 nData;
        /* Number of bytes of data */
        public u32 nPayload;
        /* Total amount of payload */
        public u16 nHeader;
        /* Size of the cell content header in bytes */
        public u16 nLocal;
        /* Amount of payload held locally */
        public u16 iOverflow;
        /* Offset to overflow page number.  Zero if no overflow */
        public u16 nSize;
        /* Size of the cell content on the main b-tree page */
        public bool Equals (CellInfo ci)
        {
            if (ci.iCell >= ci.pCell.Length || iCell >= this.pCell.Length)
                return false;
            if (ci.pCell [ci.iCell] != this.pCell [iCell])
                return false;
            if (ci.nKey != this.nKey || ci.nData != this.nData || ci.nPayload != this.nPayload)
                return false;
            if (ci.nHeader != this.nHeader || ci.nLocal != this.nLocal)
                return false;
            if (ci.iOverflow != this.iOverflow || ci.nSize != this.nSize)
                return false;
            return true;
        }
    }
}

