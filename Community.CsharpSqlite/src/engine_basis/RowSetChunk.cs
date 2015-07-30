using System;

namespace Community.CsharpSqlite
{
    /*
    ** Index entries are allocated in large chunks (instances of the
    ** following structure) to reduce memory allocation overhead.  The
    ** chunks are kept on a linked list so that they can be deallocated
    ** when the RowSet is destroyed.
    */
    public class RowSetChunk
    {
        public RowSetChunk pNextChunk;             /* Next chunk on list of them all */
        public RowSetEntry[] aEntry = new RowSetEntry[ROWSET_ENTRY_PER_CHUNK]; /* Allocated entries */
    };
}

