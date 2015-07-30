namespace Community.CsharpSqlite
{
    using System;

    using u32 = System.UInt32;
    using Pgno = System.UInt32;

    /*
** Structure allocated for each backup operation.
*/
    public class sqlite3_backup
    {
        public sqlite3 pDestDb;
        /* Destination database handle */
        public Btree pDest;
        /* Destination b-tree file */
        public u32 iDestSchema;
        /* Original schema cookie in destination */
        public int bDestLocked;
        /* True once a write-transaction is open on pDest */

        public Pgno iNext;
        /* Page number of the next source page to copy */
        public sqlite3 pSrcDb;
        /* Source database handle */
        public Btree pSrc;
        /* Source b-tree file */

        public int rc;
        /* Backup process error code */

        /* These two variables are set by every call to backup_step(). They are
        ** read by calls to backup_remaining() and backup_pagecount().
        */
        public Pgno nRemaining;
        /* Number of pages left to copy */
        public Pgno nPagecount;
        /* Total number of pages to copy */

        public int isAttached;
        /* True once backup has been registered with pager */
        public sqlite3_backup pNext;
        /* Next backup associated with source pager */
    }
}
