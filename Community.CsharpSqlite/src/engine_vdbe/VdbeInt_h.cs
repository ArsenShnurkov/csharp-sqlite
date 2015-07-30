using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using FILE = System.IO.TextWriter;

using i64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;
using unsigned = System.UIntPtr;

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


namespace Community.CsharpSqlite
{
  using Op = VdbeOp;

  public partial class Globals
  {
    /*
    ** 2003 September 6
    **
    ** The author disclaims copyright to this source code.  In place of
    ** a legal notice, here is a blessing:
    **
    **    May you do good and not evil.
    **    May you find forgiveness for yourself and forgive others.
    **    May you share freely, never taking more than you give.
    **
    *************************************************************************
    ** This is the header file for information that is private to the
    ** VDBE.  This information used to all be at the top of the single
    ** source code file "vdbe.c".  When that file became too big (over
    ** 6000 lines long) it was split up into several smaller files and
    ** this header information was factored out.
    *************************************************************************
    **  Included in SQLite3 port to C#-SQLite;  2008 Noah B Hart
    **  C#-SQLite is an independent reimplementation of the SQLite software library
    **
    **  SQLITE_SOURCE_ID: 2011-06-23 19:49:22 4374b7e83ea0a3fbc3691f9c0c936272862f32f2
    **
    *************************************************************************
    */
    //#if !_VDBEINT_H_
    //#define _VDBEINT_H_




    //#define VdbeFrameMem(p) ((Mem )&((u8 )p)[ROUND8(sizeof(VdbeFrame))])
    /*
    ** A value for VdbeCursor.cacheValid that means the cache is always invalid.
    */
    const int CACHE_STALE = 0;


    /* One or more of the following flags are set to indicate the validOK
    ** representations of the value stored in the Mem struct.
    **
    ** If the MEM_Null flag is set, then the value is an SQL NULL value.
    ** No other flags may be set in this case.
    **
    ** If the MEM_Str flag is set then Mem.z points at a string representation.
    ** Usually this is encoded in the same unicode encoding as the main
    ** database (see below for exceptions). If the MEM_Term flag is also
    ** set, then the string is nul terminated. The MEM_Int and MEM_Real
    ** flags may coexist with the MEM_Str flag.
    */
    //#define MEM_Null      0x0001   /* Value is NULL */
    //#define MEM_Str       0x0002   /* Value is a string */
    //#define MEM_Int       0x0004   /* Value is an integer */
    //#define MEM_Real      0x0008   /* Value is a real number */
    //#define MEM_Blob      0x0010   /* Value is a BLOB */
    //#define MEM_RowSet    0x0020   /* Value is a RowSet object */
    //#define MEM_Frame     0x0040   /* Value is a VdbeFrame object */
    //#define MEM_Invalid   0x0080   /* Value is undefined */
    //#define MEM_TypeMask  0x00ff   /* Mask of type bits */
    const int MEM_Null = 0x0001;
    const int MEM_Str = 0x0002;
    const int MEM_Int = 0x0004;
    const int MEM_Real = 0x0008;
    const int MEM_Blob = 0x0010;
    const int MEM_RowSet = 0x0020;
    const int MEM_Frame = 0x0040;
    const int MEM_Invalid = 0x0080;
    const int MEM_TypeMask = 0x00ff;

    /* Whenever Mem contains a valid string or blob representation, one of
    ** the following flags must be set to determine the memory management
    ** policy for Mem.z.  The MEM_Term flag tells us whether or not the
    ** string is \000 or \u0000 terminated
    //    */
    //#define MEM_Term      0x0200   /* String rep is nul terminated */
    //#define MEM_Dyn       0x0400   /* Need to call sqliteFree() on Mem.z */
    //#define MEM_Static    0x0800   /* Mem.z points to a static string */
    //#define MEM_Ephem     0x1000   /* Mem.z points to an ephemeral string */
    //#define MEM_Agg       0x2000   /* Mem.z points to an agg function context */
    //#define MEM_Zero      0x4000   /* Mem.i contains count of 0s appended to blob */
    //#if SQLITE_OMIT_INCRBLOB
    //  #undef MEM_Zero
    //  #define MEM_Zero 0x0000
    //#endif
    const int MEM_Term = 0x0200;
    const int MEM_Dyn = 0x0400;
    const int MEM_Static = 0x0800;
    const int MEM_Ephem = 0x1000;
    const int MEM_Agg = 0x2000;
#if !SQLITE_OMIT_INCRBLOB
const int MEM_Zero = 0x4000;  
#else
    const int MEM_Zero = 0x0000;
#endif

    /*
** Clear any existing type flags from a Mem and replace them with f
*/
    //#define MemSetTypeFlag(p, f) \
    //   ((p)->flags = ((p)->flags&~(MEM_TypeMask|MEM_Zero))|f)
    static void MemSetTypeFlag( Mem p, int f )
    {
      p.flags = (u16)( p.flags & ~( MEM_TypeMask | MEM_Zero ) | f );
    }// TODO -- Convert back to inline for speed

    /*
    ** Return true if a memory cell is not marked as invalid.  This macro
    ** is for use inside Debug.Assert() statements only.
    */
#if SQLITE_DEBUG
    //#define memIsValid(M)  ((M)->flags & MEM_Invalid)==0
    static bool memIsValid( Mem M )
    {
      return ( ( M ).flags & MEM_Invalid ) == 0;
    }
#else
static bool memIsValid( Mem M ) { return true; }
#endif





    /*
    ** The following are allowed values for Vdbe.magic
    */
    //#define VDBE_MAGIC_INIT     0x26bceaa5    /* Building a VDBE program */
    //#define VDBE_MAGIC_RUN      0xbdf20da3    /* VDBE is ready to execute */
    //#define VDBE_MAGIC_HALT     0x519c2973    /* VDBE has completed execution */
    //#define VDBE_MAGIC_DEAD     0xb606c3c8    /* The VDBE has been deallocated */
    const u32 VDBE_MAGIC_INIT = 0x26bceaa5;   /* Building a VDBE program */
    const u32 VDBE_MAGIC_RUN = 0xbdf20da3;   /* VDBE is ready to execute */
    const u32 VDBE_MAGIC_HALT = 0x519c2973;   /* VDBE has completed execution */
    const u32 VDBE_MAGIC_DEAD = 0xb606c3c8;   /* The VDBE has been deallocated */
    /*
    ** Function prototypes
    */
    //void sqlite3VdbeFreeCursor(Vdbe *, VdbeCursor);
    //void sqliteVdbePopStack(Vdbe*,int);
    //int sqlite3VdbeCursorMoveto(VdbeCursor);
    //#if (SQLITE_DEBUG) || defined(VDBE_PROFILE)
    //void sqlite3VdbePrintOp(FILE*, int, Op);
    //#endif
    //u32 sqlite3VdbeSerialTypeLen(u32);
    //u32 sqlite3VdbeSerialType(Mem*, int);
    //u32sqlite3VdbeSerialPut(unsigned char*, int, Mem*, int);
    //u32 sqlite3VdbeSerialGet(const unsigned char*, u32, Mem);
    //void sqlite3VdbeDeleteAuxData(VdbeFunc*, int);

    //int sqlite2BtreeKeyCompare(BtCursor *, const void *, int, int, int );
    //int sqlite3VdbeIdxKeyCompare(VdbeCursor*,UnpackedRecord*,int);
    //int sqlite3VdbeIdxRowid(sqlite3 *, i64 );
    //int sqlite3MemCompare(const Mem*, const Mem*, const CollSeq);
    //int sqlite3VdbeExec(Vdbe);
    //int sqlite3VdbeList(Vdbe);
    //int sqlite3VdbeHalt(Vdbe);
    //int sqlite3VdbeChangeEncoding(Mem *, int);
    //int sqlite3VdbeMemTooBig(Mem);
    //int sqlite3VdbeMemCopy(Mem*, const Mem);
    //void sqlite3VdbeMemShallowCopy(Mem*, const Mem*, int);
    //void sqlite3VdbeMemMove(Mem*, Mem);
    //int sqlite3VdbeMemNulTerminate(Mem);
    //int sqlite3VdbeMemSetStr(Mem*, const char*, int, u8, void()(void));
    //void sqlite3VdbeMemSetInt64(Mem*, i64);
#if SQLITE_OMIT_FLOATING_POINT
//# define sqlite3VdbeMemSetDouble sqlite3VdbeMemSetInt64
#else
    //void sqlite3VdbeMemSetDouble(Mem*, double);
#endif
    //void sqlite3VdbeMemSetNull(Mem);
    //void sqlite3VdbeMemSetZeroBlob(Mem*,int);
    //void sqlite3VdbeMemSetRowSet(Mem);
    //int sqlite3VdbeMemMakeWriteable(Mem);
    //int sqlite3VdbeMemStringify(Mem*, int);
    //i64 sqlite3VdbeIntValue(Mem);
    //int sqlite3VdbeMemIntegerify(Mem);
    //double sqlite3VdbeRealValue(Mem);
    //void sqlite3VdbeIntegerAffinity(Mem);
    //int sqlite3VdbeMemRealify(Mem);
    //int sqlite3VdbeMemNumerify(Mem);
    //int sqlite3VdbeMemFromBtree(BtCursor*,int,int,int,Mem);
    //void sqlite3VdbeMemRelease(Mem p);
    //void sqlite3VdbeMemReleaseExternal(Mem p);
    //int sqlite3VdbeMemFinalize(Mem*, FuncDef);
    //string sqlite3OpcodeName(int);
    //int sqlite3VdbeMemGrow(Mem pMem, int n, int preserve);
    //int sqlite3VdbeCloseStatement(Vdbe *, int);
    //void sqlite3VdbeFrameDelete(VdbeFrame);
    //int sqlite3VdbeFrameRestore(VdbeFrame );
    //void sqlite3VdbeMemStoreType(Mem *pMem);  

#if !(SQLITE_OMIT_SHARED_CACHE) && SQLITE_THREADSAFE//>0
  //void sqlite3VdbeEnter(Vdbe);
  //void sqlite3VdbeLeave(Vdbe);
#else
//# define sqlite3VdbeEnter(X)
    static void sqlite3VdbeEnter( Vdbe p )
    {
    }
//# define sqlite3VdbeLeave(X)
    static void sqlite3VdbeLeave( Vdbe p )
    {
    }
#endif

#if SQLITE_DEBUG
    //void sqlite3VdbeMemPrepareToChange(Vdbe*,Mem);
#endif

#if !SQLITE_OMIT_FOREIGN_KEY
    //int sqlite3VdbeCheckFk(Vdbe *, int);
#else
//# define sqlite3VdbeCheckFk(p,i) 0
static int sqlite3VdbeCheckFk( Vdbe p, int i ) { return 0; }
#endif

    //int sqlite3VdbeMemTranslate(Mem*, u8);
    //#if SQLITE_DEBUG
    //  void sqlite3VdbePrintSql(Vdbe);
    //  void sqlite3VdbeMemPrettyPrint(Mem pMem, string zBuf);
    //#endif
    //int sqlite3VdbeMemHandleBom(Mem pMem);

#if !SQLITE_OMIT_INCRBLOB
//  int sqlite3VdbeMemExpandBlob(Mem );
#else
    //  #define sqlite3VdbeMemExpandBlob(x) SQLITE_OK
    static int sqlite3VdbeMemExpandBlob( Mem x )
    {
      return SQLITE_OK;
    }
#endif

    //#endif //* !_VDBEINT_H_) */
  }
}
