using System;

namespace Community.CsharpSqlite
{
    /*
    ** An instance of the following structure keeps track of a mapping
    ** between VDBE cursor numbers and bits of the bitmasks in WhereTerm.
    **
    ** The VDBE cursor numbers are small integers contained in
    ** SrcList_item.iCursor and Expr.iTable fields.  For any given WHERE
    ** clause, the cursor numbers might not begin with 0 and they might
    ** contain gaps in the numbering sequence.  But we want to make maximum
    ** use of the bits in our bitmasks.  This structure provides a mapping
    ** from the sparse cursor numbers into consecutive integers beginning
    ** with 0.
    **
    ** If WhereMaskSet.ix[A]==B it means that The A-th bit of a Bitmask
    ** corresponds VDBE cursor number B.  The A-th bit of a bitmask is 1<<A.
    **
    ** For example, if the WHERE clause expression used these VDBE
    ** cursors:  4, 5, 8, 29, 57, 73.  Then the  WhereMaskSet structure
    ** would map those cursor numbers into bits 0 through 5.
    **
    ** Note that the mapping is not necessarily ordered.  In the example
    ** above, the mapping might go like this:  4.3, 5.1, 8.2, 29.0,
    ** 57.5, 73.4.  Or one of 719 other combinations might be used. It
    ** does not really matter.  What is important is that sparse cursor
    ** numbers all get mapped into bit numbers that begin with 0 and contain
    ** no gaps.
    */
    public class WhereMaskSet
    {
        public int n;                        /* Number of Debug.Assigned cursor values */
        public int[] ix = new int[BMS];       /* Cursor Debug.Assigned to each bit */

        public void CopyTo( WhereMaskSet wms )
        {
            wms.n = this.n;
            wms.ix = (int[])this.ix.Clone();
        }
    }
}

