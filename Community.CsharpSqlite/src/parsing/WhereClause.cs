namespace Community.CsharpSqlite
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Bitmask = System.UInt64;
    using FILE = System.IO.TextWriter;
    using i16 = System.Int16;
    using i32 = System.Int32;
    using i64 = System.Int64;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using u64 = System.UInt64;

    using Pgno = System.UInt32;

    #if !SQLITE_MAX_VARIABLE_NUMBER
    using ynVar = System.Int16;
    #else
    using ynVar = System.Int32; 
    #endif

    /*
** The yDbMask datatype for the bitmask of all attached databases.
*/
    #if SQLITE_MAX_ATTACHED//>30
    //  typedef sqlite3_uint64 yDbMask;
    using yDbMask = System.Int64; 
    #else
    //  typedef unsigned int yDbMask;
    using yDbMask = System.Int32;
    #endif
        
    /*
    ** An instance of the following structure holds all information about a
    ** WHERE clause.  Mostly this is a container for one or more WhereTerms.
    */
    public class WhereClause
    {
        public Parse pParse;                              /* The parser context */
        public WhereMaskSet pMaskSet;                     /* Mapping of table cursor numbers to bitmasks */
        public Bitmask vmask;                             /* Bitmask identifying virtual table cursors */
        public u8 op;                                     /* Split operator.  TK_AND or TK_OR */
        public int nTerm;                                 /* Number of terms */
        public int nSlot;                                 /* Number of entries in a[] */
        public WhereTerm[] a;                             /* Each a[] describes a term of the WHERE cluase */
        #if (SQLITE_SMALL_STACK)
        public WhereTerm[] aStatic = new WhereTerm[1];    /* Initial static space for a[] */
        #else
        public WhereTerm[] aStatic = new WhereTerm[8];    /* Initial static space for a[] */
        #endif

        public void CopyTo( WhereClause wc )
        {
            wc.pParse = this.pParse;
            wc.pMaskSet = new WhereMaskSet();
            this.pMaskSet.CopyTo( wc.pMaskSet );
            wc.op = this.op;
            wc.nTerm = this.nTerm;
            wc.nSlot = this.nSlot;
            wc.a = (WhereTerm[])this.a.Clone();
            wc.aStatic = (WhereTerm[])this.aStatic.Clone();
        }
    };
}

