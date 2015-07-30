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

    using Pgno = System.UInt32;
    using DbPage = PgHdr;

    /*
    ** As each page of the file is loaded into memory, an instance of the following
    ** structure is appended and initialized to zero.  This structure stores
    ** information about the page that is decoded from the raw file page.
    **
    ** The pParent field points back to the parent page.  This allows us to
    ** walk up the BTree from any leaf to the root.  Care must be taken to
    ** unref() the parent page pointer when this page is no longer referenced.
    ** The pageDestructor() routine handles that chore.
    **
    ** Access to all fields of this structure is controlled by the mutex
    ** stored in MemPage.pBt.mutex.
    */
    public struct _OvflCell
    {
        /* Cells that will not fit on aData[] */
        public u8[] pCell;
        /* Pointers to the body of the overflow cell */
        public u16 idx;
        /* Insert this cell before idx-th non-overflow cell */
        public _OvflCell Copy ()
        {
            _OvflCell cp = new _OvflCell ();
            if (pCell != null) {
                cp.pCell = sqlite3Malloc (pCell.Length);
                Buffer.BlockCopy (pCell, 0, cp.pCell, 0, pCell.Length);
            }
            cp.idx = idx;
            return cp;
        }
    };

    public class MemPage
    {
        public u8 isInit;
        /* True if previously initialized. MUST BE FIRST! */
        public u8 nOverflow;
        /* Number of overflow cell bodies in aCell[] */
        public u8 intKey;
        /* True if u8key flag is set */
        public u8 leaf;
        /* 1 if leaf flag is set */
        public u8 hasData;
        /* True if this page stores data */
        public u8 hdrOffset;
        /* 100 for page 1.  0 otherwise */
        public u8 childPtrSize;
        /* 0 if leaf==1.  4 if leaf==0 */
        public u16 maxLocal;
        /* Copy of BtShared.maxLocal or BtShared.maxLeaf */
        public u16 minLocal;
        /* Copy of BtShared.minLocal or BtShared.minLeaf */
        public u16 cellOffset;
        /* Index in aData of first cell pou16er */
        public u16 nFree;
        /* Number of free bytes on the page */
        public u16 nCell;
        /* Number of cells on this page, local and ovfl */
        public u16 maskPage;
        /* Mask for page offset */
        public _OvflCell[] aOvfl = new _OvflCell[5];
        public BtShared pBt;
        /* Pointer to BtShared that this page is part of */
        public byte[] aData;
        /* Pointer to disk image of the page data */
        public DbPage pDbPage;
        /* Pager page handle */
        public Pgno pgno;
        /* Page number for this page */

        //public byte[] aData
        //{
        //  get
        //  {
        //    Debug.Assert( pgno != 1 || pDbPage.pData == _aData );
        //    return _aData;
        //  }
        //  set
        //  {
        //    _aData = value;
        //    Debug.Assert( pgno != 1 || pDbPage.pData == _aData );
        //  }
        //}

        public MemPage Copy ()
        {
            MemPage cp = (MemPage)MemberwiseClone ();
            if (aOvfl != null) {
                cp.aOvfl = new _OvflCell[aOvfl.Length];
                for (int i = 0; i < aOvfl.Length; i++)
                    cp.aOvfl [i] = aOvfl [i].Copy ();
            }
            if (aData != null) {
                cp.aData = sqlite3Malloc (aData.Length);
                Buffer.BlockCopy (aData, 0, cp.aData, 0, aData.Length);
            }
            return cp;
        }
    }
}

