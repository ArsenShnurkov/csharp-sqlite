#define SQLITE_MAX_EXPR_DEPTH

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using Bitmask = System.UInt64;
using i16 = System.Int16;
using i64 = System.Int64;
using sqlite3_int64 = System.Int64;

using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;
using unsigned = System.UInt64;

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
  using sqlite3_value = Mem;

  public partial class Globals
  {
    /*
    ** 2001 September 15
    **
    ** The author disclaims copyright to this source code.  In place of
    ** a legal notice, here is a blessing:
    **
    **    May you do good and not evil.
    **    May you find forgiveness for yourself and forgive others.
    **    May you share freely, never taking more than you give.
    **
    *************************************************************************
    ** Internal interface definitions for SQLite.
    **
    *************************************************************************
    **  Included in SQLite3 port to C#-SQLite;  2008 Noah B Hart
    **  C#-SQLite is an independent reimplementation of the SQLite software library
    **
    **  SQLITE_SOURCE_ID: 2011-06-23 19:49:22 4374b7e83ea0a3fbc3691f9c0c936272862f32f2
    **
    *************************************************************************
    */
    //#if !_SQLITEINT_H_
    //#define _SQLITEINT_H_

    /*
    ** These #defines should enable >2GB file support on POSIX if the
    ** underlying operating system supports it.  If the OS lacks
    ** large file support, or if the OS is windows, these should be no-ops.
    **
    ** Ticket #2739:  The _LARGEFILE_SOURCE macro must appear before any
    ** system #includes.  Hence, this block of code must be the very first
    ** code in all source files.
    **
    ** Large file support can be disabled using the -DSQLITE_DISABLE_LFS switch
    ** on the compiler command line.  This is necessary if you are compiling
    ** on a recent machine (ex: Red Hat 7.2) but you want your code to work
    ** on an older machine (ex: Red Hat 6.0).  If you compile on Red Hat 7.2
    ** without this option, LFS is enable.  But LFS does not exist in the kernel
    ** in Red Hat 6.0, so the code won't work.  Hence, for maximum binary
    ** portability you should omit LFS.
    **
    ** Similar is true for Mac OS X.  LFS is only supported on Mac OS X 9 and later.
    */
    //#if !SQLITE_DISABLE_LFS
    //# define _LARGE_FILE       1
    //# ifndef _FILE_OFFSET_BITS
    //#   define _FILE_OFFSET_BITS 64
    //# endif
    //# define _LARGEFILE_SOURCE 1
    //#endif

    /*
    ** Include the configuration header output by 'configure' if we're using the
    ** autoconf-based build
    */
#if _HAVE_SQLITE_CONFIG_H
//#include "config.h"
#endif
    //#include "sqliteLimit.h"

    /* Disable nuisance warnings on Borland compilers */
    //#if (__BORLANDC__)
    //#pragma warn -rch /* unreachable code */
    //#pragma warn -ccc /* Condition is always true or false */
    //#pragma warn -aus /* Assigned value is never used */
    //#pragma warn -csu /* Comparing signed and unsigned */
    //#pragma warn -spa /* Suspicious pointer arithmetic */
    //#endif

    /* Needed for various definitions... */
    //#if !_GNU_SOURCE
    //#define _GNU_SOURCE
    //#endif
    /*
    ** Include standard header files as necessary
    */
#if HAVE_STDINT_H
//#include <stdint.h>
#endif
#if HAVE_INTTYPES_H
//#include <inttypes.h>
#endif

    /*
** The number of samples of an index that SQLite takes in order to 
** construct a histogram of the table content when running ANALYZE
** and with SQLITE_ENABLE_STAT2
*/
    //#define SQLITE_INDEX_SAMPLES 10
    public const int SQLITE_INDEX_SAMPLES = 10;

    /*
    ** The following macros are used to cast pointers to integers and
    ** integers to pointers.  The way you do this varies from one compiler
    ** to the next, so we have developed the following set of #if statements
    ** to generate appropriate macros for a wide range of compilers.
    **
    ** The correct "ANSI" way to do this is to use the intptr_t type. 
    ** Unfortunately, that typedef is not available on all compilers, or
    ** if it is available, it requires an #include of specific headers
    ** that vary from one machine to the next.
    **
    ** Ticket #3860:  The llvm-gcc-4.2 compiler from Apple chokes on
    ** the ((void)&((char)0)[X]) construct.  But MSVC chokes on ((void)(X)).
    ** So we have to define the macros in different ways depending on the
    ** compiler.
    */
    //#if (__PTRDIFF_TYPE__)  /* This case should work for GCC */
    //# define SQLITE_INT_TO_PTR(X)  ((void)(__PTRDIFF_TYPE__)(X))
    //# define SQLITE_PTR_TO_INT(X)  ((int)(__PTRDIFF_TYPE__)(X))
    //#elif !defined(__GNUC__)       /* Works for compilers other than LLVM */
    //# define SQLITE_INT_TO_PTR(X)  ((void)&((char)0)[X])
    //# define SQLITE_PTR_TO_INT(X)  ((int)(((char)X)-(char)0))
    //#elif defined(HAVE_STDINT_H)   /* Use this case if we have ANSI headers */
    //# define SQLITE_INT_TO_PTR(X)  ((void)(intptr_t)(X))
    //# define SQLITE_PTR_TO_INT(X)  ((int)(intptr_t)(X))
    //#else                          /* Generates a warning - but it always works */
    //# define SQLITE_INT_TO_PTR(X)  ((void)(X))
    //# define SQLITE_PTR_TO_INT(X)  ((int)(X))
    //#endif

    /*
    ** The SQLITE_THREADSAFE macro must be defined as 0, 1, or 2.
    ** 0 means mutexes are permanently disable and the library is never
    ** threadsafe.  1 means the library is serialized which is the highest
    ** level of threadsafety.  2 means the libary is multithreaded - multiple
    ** threads can use SQLite as long as no two threads try to use the same
    ** database connection at the same time.
    **
    ** Older versions of SQLite used an optional THREADSAFE macro.
    ** We support that for legacy.
    */

#if !SQLITE_THREADSAFE
    //# define SQLITE_THREADSAFE 2
    const int SQLITE_THREADSAFE = 2;
#else
    const int SQLITE_THREADSAFE = 2; /* IMP: R-07272-22309 */
#endif

    /*
** The SQLITE_DEFAULT_MEMSTATUS macro must be defined as either 0 or 1.
** It determines whether or not the features related to
** SQLITE_CONFIG_MEMSTATUS are available by default or not. This value can
** be overridden at runtime using the sqlite3_config() API.
*/
#if !(SQLITE_DEFAULT_MEMSTATUS)
    //# define SQLITE_DEFAULT_MEMSTATUS 1
    const int SQLITE_DEFAULT_MEMSTATUS = 0;
#else
const int SQLITE_DEFAULT_MEMSTATUS = 1;
#endif

    /*
** Exactly one of the following macros must be defined in order to
** specify which memory allocation subsystem to use.
**
**     SQLITE_SYSTEM_MALLOC          // Use normal system malloc()
**     SQLITE_MEMDEBUG               // Debugging version of system malloc()
**
** (Historical note:  There used to be several other options, but we've
** pared it down to just these two.)
**
** If none of the above are defined, then set SQLITE_SYSTEM_MALLOC as
** the default.
*/
    //#if (SQLITE_SYSTEM_MALLOC)+defined(SQLITE_MEMDEBUG)+\
    //# error "At most one of the following compile-time configuration options\
    // is allows: SQLITE_SYSTEM_MALLOC, SQLITE_MEMDEBUG"
    //#endif
    //#if (SQLITE_SYSTEM_MALLOC)+defined(SQLITE_MEMDEBUG)+\
    //# define SQLITE_SYSTEM_MALLOC 1
    //#endif

    /*
    ** If SQLITE_MALLOC_SOFT_LIMIT is not zero, then try to keep the
    ** sizes of memory allocations below this value where possible.
    */
#if !(SQLITE_MALLOC_SOFT_LIMIT)
    const int SQLITE_MALLOC_SOFT_LIMIT = 1024;
#endif

    /*
** We need to define _XOPEN_SOURCE as follows in order to enable
** recursive mutexes on most Unix systems.  But Mac OS X is different.
** The _XOPEN_SOURCE define causes problems for Mac OS X we are told,
** so it is omitted there.  See ticket #2673.
**
** Later we learn that _XOPEN_SOURCE is poorly or incorrectly
** implemented on some systems.  So we avoid defining it at all
** if it is already defined or if it is unneeded because we are
** not doing a threadsafe build.  Ticket #2681.
**
** See also ticket #2741.
*/
#if !_XOPEN_SOURCE && !__DARWIN__ && !__APPLE__ && SQLITE_THREADSAFE
    const int _XOPEN_SOURCE = 500;//#define _XOPEN_SOURCE 500  /* Needed to enable pthread recursive mutexes */
#endif

    /*
** The TCL headers are only needed when compiling the TCL bindings.
*/
#if SQLITE_TCL || TCLSH
    //# include <tcl.h>
#endif

    /*
** Many people are failing to set -DNDEBUG=1 when compiling SQLite.
** Setting NDEBUG makes the code smaller and run faster.  So the following
** lines are added to automatically set NDEBUG unless the -DSQLITE_DEBUG=1
** option is set.  Thus NDEBUG becomes an opt-in rather than an opt-out
** feature.
*/
#if !NDEBUG && !SQLITE_DEBUG
const int NDEBUG = 1;//# define NDEBUG 1
#endif

    /*
** The testcase() macro is used to aid in coverage testing.  When
** doing coverage testing, the condition inside the argument to
** testcase() must be evaluated both true and false in order to
** get full branch coverage.  The testcase() macro is inserted
** to help ensure adequate test coverage in places where simple
** condition/decision coverage is inadequate.  For example, testcase()
** can be used to make sure boundary values are tested.  For
** bitmask tests, testcase() can be used to make sure each bit
** is significant and used at least once.  On switch statements
** where multiple cases go to the same block of code, testcase()
** can insure that all cases are evaluated.
**
*/
#if SQLITE_COVERAGE_TEST
void sqlite3Coverage(int);
//# define testcase(X)  if( X ){ sqlite3Coverage(__LINE__); }
#else
    //# define testcase(X)
    static void testcase<T>( T X )
    {
    }
#endif

    /*
** The TESTONLY macro is used to enclose variable declarations or
** other bits of code that are needed to support the arguments
** within testcase() and Debug.Assert() macros.
*/
#if !NDEBUG || SQLITE_COVERAGE_TEST
    //# define TESTONLY(X)  X
    // -- Need workaround for C#, since inline macros don't exist
#else
//# define TESTONLY(X)
#endif

    /*
** Sometimes we need a small amount of code such as a variable initialization
** to setup for a later Debug.Assert() statement.  We do not want this code to
** appear when Debug.Assert() is disabled.  The following macro is therefore
** used to contain that setup code.  The "VVA" acronym stands for
** "Verification, Validation, and Accreditation".  In other words, the
** code within VVA_ONLY() will only run during verification processes.
*/
#if !NDEBUG
    //# define VVA_ONLY(X)  X
#else
//# define VVA_ONLY(X)
#endif

    /*
** The ALWAYS and NEVER macros surround boolean expressions which
** are intended to always be true or false, respectively.  Such
** expressions could be omitted from the code completely.  But they
** are included in a few cases in order to enhance the resilience
** of SQLite to unexpected behavior - to make the code "self-healing"
** or "ductile" rather than being "brittle" and crashing at the first
** hint of unplanned behavior.
**
** In other words, ALWAYS and NEVER are added for defensive code.
**
** When doing coverage testing ALWAYS and NEVER are hard-coded to
** be true and false so that the unreachable code then specify will
** not be counted as untested code.
*/
#if SQLITE_COVERAGE_TEST
//# define ALWAYS(X)      (1)
//# define NEVER(X)       (0)
#elif !NDEBUG
    //# define ALWAYS(X)      ((X)?1:(Debug.Assert(0),0))
    static bool ALWAYS( bool X )
    {
      if ( X != true )
        Debug.Assert( false );
      return true;
    }
    static int ALWAYS( int X )
    {
      if ( X == 0 )
        Debug.Assert( false );
      return 1;
    }
    static bool ALWAYS<T>( T X )
    {
      if ( X == null )
        Debug.Assert( false );
      return true;
    }

    //# define NEVER(X)       ((X)?(Debug.Assert(0),1):0)
    static bool NEVER( bool X )
    {
      if ( X == true )
        Debug.Assert( false );
      return false;
    }
    static byte NEVER( byte X )
    {
      if ( X != 0 )
        Debug.Assert( false );
      return 0;
    }
    static int NEVER( int X )
    {
      if ( X != 0 )
        Debug.Assert( false );
      return 0;
    }
    static bool NEVER<T>( T X )
    {
      if ( X != null )
        Debug.Assert( false );
      return false;
    }
#else
//# define ALWAYS(X)      (X)
static bool ALWAYS(bool X) { return X; }
static byte ALWAYS(byte X) { return X; }
static int ALWAYS(int X) { return X; }
static bool ALWAYS<T>( T X ) { return true; }

//# define NEVER(X)       (X)
static bool NEVER(bool X) { return X; }
static byte NEVER(byte X) { return X; }
static int NEVER(int X) { return X; }
static bool NEVER<T>(T X) { return false; }
#endif

    /*
** Return true (non-zero) if the input is a integer that is too large
** to fit in 32-bits.  This macro is used inside of various testcase()
** macros to verify that we have tested SQLite for large-file support.
*/
    static bool IS_BIG_INT( i64 X )
    {
      return ( ( ( X ) & ~(i64)0xffffffff ) != 0 );
    }//#define IS_BIG_INT(X)  (((X)&~(i64)0xffffffff)!=0)

    /*
    ** The macro unlikely() is a hint that surrounds a boolean
    ** expression that is usually false.  Macro likely() surrounds
    ** a boolean expression that is usually true.  GCC is able to
    ** use these hints to generate better code, sometimes.
    */
#if (__GNUC__) && FALSE
//# define likely(X)    __builtin_expect((X),1)
//# define unlikely(X)  __builtin_expect((X),0)
#else
    //# define likely(X)    !!(X)
    static bool likely( bool X )
    {
      return !!X;
    }
    //# define unlikely(X)  !!(X)
    static bool unlikely( bool X )
    {
      return !!X;
    }
#endif

    //#include "sqlite3.h"
    //#include "hash.h"
    //#include "parse.h"
    //#include <stdio.h>
    //#include <stdlib.h>
    //#include <string.h>
    //#include <assert.h>
    //#include <stddef.h>

    /*
    ** If compiling for a processor that lacks floating point support,
    ** substitute integer for floating-point
    */
#if SQLITE_OMIT_FLOATING_POINT
//# define double sqlite_int64
//# define float sqlite_int64
//# define LONGDOUBLE_TYPE sqlite_int64
//#if !SQLITE_BIG_DBL
//#   define SQLITE_BIG_DBL (((sqlite3_int64)1)<<50)
//# endif
//# define SQLITE_OMIT_DATETIME_FUNCS 1
//# define SQLITE_OMIT_TRACE 1
//# undef SQLITE_MIXED_ENDIAN_64BIT_FLOAT
//# undef SQLITE_HAVE_ISNAN
#endif
#if !SQLITE_BIG_DBL
    const double SQLITE_BIG_DBL = ( ( (sqlite3_int64)1 ) << 60 );//# define SQLITE_BIG_DBL (1e99)
#endif

    /*
** OMIT_TEMPDB is set to 1 if SQLITE_OMIT_TEMPDB is defined, or 0
** afterward. Having this macro allows us to cause the C compiler
** to omit code used by TEMP tables without messy #if !statements.
*/
#if SQLITE_OMIT_TEMPDB
//#define OMIT_TEMPDB 1
#else
    static int OMIT_TEMPDB = 0;
#endif


    /*
** The "file format" number is an integer that is incremented whenever
** the VDBE-level file format changes.  The following macros define the
** the default file format for new databases and the maximum file format
** that the library can read.
*/
    static public int SQLITE_MAX_FILE_FORMAT = 4;//#define SQLITE_MAX_FILE_FORMAT 4
    //#if !SQLITE_DEFAULT_FILE_FORMAT
    static int SQLITE_DEFAULT_FILE_FORMAT = 1;//# define SQLITE_DEFAULT_FILE_FORMAT 1
    //#endif

    /*
    ** Determine whether triggers are recursive by default.  This can be
    ** changed at run-time using a pragma.
    */
#if !SQLITE_DEFAULT_RECURSIVE_TRIGGERS
    //# define SQLITE_DEFAULT_RECURSIVE_TRIGGERS 0
    static public bool SQLITE_DEFAULT_RECURSIVE_TRIGGERS = false;
#else
static public bool SQLITE_DEFAULT_RECURSIVE_TRIGGERS = true;
#endif


    /*
** Provide a default value for SQLITE_TEMP_STORE in case it is not specified
** on the command-line
*/
    //#if !SQLITE_TEMP_STORE
    static int SQLITE_TEMP_STORE = 1;//#define SQLITE_TEMP_STORE 1
    //#endif

    /*
** GCC does not define the offsetof() macro so we'll have to do it
** ourselves.
*/
#if !offsetof
    //#define offsetof(STRUCTURE,FIELD) ((int)((char)&((STRUCTURE)0)->FIELD))
#endif

    /*
** Check to see if this machine uses EBCDIC.  (Yes, believe it or
** not, there are still machines out there that use EBCDIC.)
*/
#if FALSE //'A' == '\301'
//# define SQLITE_EBCDIC 1
#else
    const int SQLITE_ASCII = 1;//#define SQLITE_ASCII 1
#endif

    /*
** Integers of known sizes.  These typedefs might change for architectures
** where the sizes very.  Preprocessor macros are available so that the
** types can be conveniently redefined at compile-type.  Like this:
**
**         cc '-Du32PTR_TYPE=long long int' ...
*/
    //#if !u32_TYPE
    //# ifdef HAVE_u32_T
    //#  define u32_TYPE u32_t
    //# else
    //#  define u32_TYPE unsigned int
    //# endif
    //#endif
    //#if !u3216_TYPE
    //# ifdef HAVE_u3216_T
    //#  define u3216_TYPE u3216_t
    //# else
    //#  define u3216_TYPE unsigned short int
    //# endif
    //#endif
    //#if !INT16_TYPE
    //# ifdef HAVE_INT16_T
    //#  define INT16_TYPE int16_t
    //# else
    //#  define INT16_TYPE short int
    //# endif
    //#endif
    //#if !u328_TYPE
    //# ifdef HAVE_u328_T
    //#  define u328_TYPE u328_t
    //# else
    //#  define u328_TYPE unsigned char
    //# endif
    //#endif
    //#if !INT8_TYPE
    //# ifdef HAVE_INT8_T
    //#  define INT8_TYPE int8_t
    //# else
    //#  define INT8_TYPE signed char
    //# endif
    //#endif
    //#if !LONGDOUBLE_TYPE
    //# define LONGDOUBLE_TYPE long double
    //#endif
    //typedef sqlite_int64 i64;          /* 8-byte signed integer */
    //typedef sqlite_u3264 u64;         /* 8-byte unsigned integer */
    //typedef u32_TYPE u32;           /* 4-byte unsigned integer */
    //typedef u3216_TYPE u16;           /* 2-byte unsigned integer */
    //typedef INT16_TYPE i16;            /* 2-byte signed integer */
    //typedef u328_TYPE u8;             /* 1-byte unsigned integer */
    //typedef INT8_TYPE i8;              /* 1-byte signed integer */

    /*
    ** SQLITE_MAX_U32 is a u64 constant that is the maximum u64 value
    ** that can be stored in a u32 without loss of data.  The value
    ** is 0x00000000ffffffff.  But because of quirks of some compilers, we
    ** have to specify the value in the less intuitive manner shown:
    */
    //#define SQLITE_MAX_U32  ((((u64)1)<<32)-1)
    const u32 SQLITE_MAX_U32 = (u32)( ( ( (u64)1 ) << 32 ) - 1 );


    /*
    ** Macros to determine whether the machine is big or little endian,
    ** evaluated at runtime.
    */
#if SQLITE_AMALGAMATION
//const int sqlite3one = 1;
#else
    const bool sqlite3one = true;
#endif
#if i386 || __i386__ || _M_IX86
const int ;//#define SQLITE_BIGENDIAN    0
const int ;//#define SQLITE_LITTLEENDIAN 1
const int ;//#define SQLITE_UTF16NATIVE  SQLITE_UTF16LE
#else
    static u8 SQLITE_BIGENDIAN = 0;//#define SQLITE_BIGENDIAN    (*(char )(&sqlite3one)==0)
    static u8 SQLITE_LITTLEENDIAN = 1;//#define SQLITE_LITTLEENDIAN (*(char )(&sqlite3one)==1)
    static u8 SQLITE_UTF16NATIVE = ( SQLITE_BIGENDIAN != 0 ? SQLITE_UTF16BE : SQLITE_UTF16LE );//#define SQLITE_UTF16NATIVE (SQLITE_BIGENDIAN?SQLITE_UTF16BE:SQLITE_UTF16LE)
#endif

    /*
** Constants for the largest and smallest possible 64-bit signed integers.
** These macros are designed to work correctly on both 32-bit and 64-bit
** compilers.
*/
    //#define LARGEST_INT64  (0xffffffff|(((i64)0x7fffffff)<<32))
    //#define SMALLEST_INT64 (((i64)-1) - LARGEST_INT64)
    const i64 LARGEST_INT64 = i64.MaxValue;//( 0xffffffff | ( ( (i64)0x7fffffff ) << 32 ) );
    const i64 SMALLEST_INT64 = i64.MinValue;//( ( ( i64 ) - 1 ) - LARGEST_INT64 );

    /*
    ** Round up a number to the next larger multiple of 8.  This is used
    ** to force 8-byte alignment on 64-bit architectures.
    */
    //#define ROUND8(x)     (((x)+7)&~7)
    static int ROUND8( int x )
    {
      return ( x + 7 ) & ~7;
    }

    /*
    ** Round down to the nearest multiple of 8
    */
    //#define ROUNDDOWN8(x) ((x)&~7)
    static int ROUNDDOWN8( int x )
    {
      return x & ~7;
    }

    /*
    ** Assert that the pointer X is aligned to an 8-byte boundary.  This
    ** macro is used only within Debug.Assert() to verify that the code gets
    ** all alignment restrictions correct.
    **
    ** Except, if SQLITE_4_BYTE_ALIGNED_MALLOC is defined, then the
    ** underlying malloc() implemention might return us 4-byte aligned
    ** pointers.  In that case, only verify 4-byte alignment.
    */
    //#if SQLITE_4_BYTE_ALIGNED_MALLOC
    //# define EIGHT_BYTE_ALIGNMENT(X)   ((((char)(X) - (char)0)&3)==0)
    //#else
    //# define EIGHT_BYTE_ALIGNMENT(X)   ((((char)(X) - (char)0)&7)==0)
    //#endif


    /*
    ** Name of the master database table.  The master database table
    ** is a special table that holds the names and attributes of all
    ** user tables and indices.
    */
    const string MASTER_NAME = "sqlite_master";//#define MASTER_NAME       "sqlite_master"
    const string TEMP_MASTER_NAME = "sqlite_temp_master";//#define TEMP_MASTER_NAME  "sqlite_temp_master"

    /*
    ** The root-page of the master database table.
    */
    const int MASTER_ROOT = 1;//#define MASTER_ROOT       1

    /*
    ** The name of the schema table.
    */
    static string SCHEMA_TABLE( int x ) //#define SCHEMA_TABLE(x)  ((!OMIT_TEMPDB)&&(x==1)?TEMP_MASTER_NAME:MASTER_NAME)
    {
      return ( ( OMIT_TEMPDB == 0 ) && ( x == 1 ) ? TEMP_MASTER_NAME : MASTER_NAME );
    }

    /*
    ** A convenience macro that returns the number of elements in
    ** an array.
    */
    //#define ArraySize(X)    ((int)(sizeof(X)/sizeof(X[0])))
    static int ArraySize<T>( T[] x )
    {
      return x.Length;
    }

    /*
    ** The following value as a destructor means to use sqlite3DbFree().
    ** This is an internal extension to SQLITE_STATIC and SQLITE_TRANSIENT.
    */
    //#define SQLITE_DYNAMIC   ((sqlite3_destructor_type)sqlite3DbFree)
    static dxDel SQLITE_DYNAMIC;

    /*
    ** When SQLITE_OMIT_WSD is defined, it means that the target platform does
    ** not support Writable Static Data (WSD) such as global and static variables.
    ** All variables must either be on the stack or dynamically allocated from
    ** the heap.  When WSD is unsupported, the variable declarations scattered
    ** throughout the SQLite code must become constants instead.  The SQLITE_WSD
    ** macro is used for this purpose.  And instead of referencing the variable
    ** directly, we use its constant as a key to lookup the run-time allocated
    ** buffer that holds real variable.  The constant is also the initializer
    ** for the run-time allocated buffer.
    **
    ** In the usual case where WSD is supported, the SQLITE_WSD and GLOBAL
    ** macros become no-ops and have zero performance impact.
    */
#if SQLITE_OMIT_WSD
//#define SQLITE_WSD const
//#define GLOBAL(t,v) (*(t)sqlite3_wsd_find((void)&(v), sizeof(v)))
//#define sqlite3GlobalConfig GLOBAL(struct Sqlite3Config, sqlite3Config)
int sqlite3_wsd_init(int N, int J);
void *sqlite3_wsd_find(void *K, int L);
#else
    //#define SQLITE_WSD
    //#define GLOBAL(t,v) v
    //#define sqlite3GlobalConfig sqlite3Config
    static Sqlite3Config sqlite3GlobalConfig;
#endif

    /*
** The following macros are used to suppress compiler warnings and to
** make it clear to human readers when a function parameter is deliberately
** left unused within the body of a function. This usually happens when
** a function is called via a function pointer. For example the
** implementation of an SQL aggregate step callback may not use the
** parameter indicating the number of arguments passed to the aggregate,
** if it knows that this is enforced elsewhere.
**
** When a function parameter is not used at all within the body of a function,
** it is generally named "NotUsed" or "NotUsed2" to make things even clearer.
** However, these macros may also be used to suppress warnings related to
** parameters that may or may not be used depending on compilation options.
** For example those parameters only used in Debug.Assert() statements. In these
** cases the parameters are named as per the usual conventions.
*/
    //#define UNUSED_PARAMETER(x) (void)(x)
    static void UNUSED_PARAMETER<T>( T x )
    {
    }

    //#define UNUSED_PARAMETER2(x,y) UNUSED_PARAMETER(x),UNUSED_PARAMETER(y)
    static void UNUSED_PARAMETER2<T1, T2>( T1 x, T2 y )
    {
      UNUSED_PARAMETER( x );
      UNUSED_PARAMETER( y );
    }

    /*
    ** Forward references to structures
    */
    //typedef struct AggInfo AggInfo;
    //typedef struct AuthContext AuthContext;
    //typedef struct AutoincInfo AutoincInfo;
    //typedef struct Bitvec Bitvec;
    //typedef struct CollSeq CollSeq;
    //typedef struct Column Column;
    //typedef struct Db Db;
    //typedef struct Schema Schema;
    //typedef struct Expr Expr;
    //typedef struct ExprList ExprList;
    //typedef struct ExprSpan ExprSpan;
    //typedef struct FKey FKey;
    //typedef struct FuncDestructor FuncDestructor;
    //typedef struct FuncDef FuncDef;
    //typedef struct IdList IdList;
    //typedef struct Index Index;
    //typedef struct IndexSample IndexSample;
    //typedef struct KeyClass KeyClass;
    //typedef struct KeyInfo KeyInfo;
    //typedef struct Lookaside Lookaside;
    //typedef struct LookasideSlot LookasideSlot;
    //typedef struct Module Module;
    //typedef struct NameContext NameContext;
    //typedef struct Parse Parse;
    //typedef struct RowSet RowSet;
    //typedef struct Savepoint Savepoint;
    //typedef struct Select Select;
    //typedef struct SrcList SrcList;
    //typedef struct StrAccum StrAccum;
    //typedef struct Table Table;
    //typedef struct TableLock TableLock;
    //typedef struct Token Token;
    //typedef struct Trigger Trigger;
    //typedef struct TriggerPrg TriggerPrg;
    //typedef struct TriggerStep TriggerStep;
    //typedef struct UnpackedRecord UnpackedRecord;
    //typedef struct VTable VTable;
    //typedef struct VtabCtx VtabCtx;
    //typedef struct Walker Walker;
    //typedef struct WherePlan WherePlan;
    //typedef struct WhereInfo WhereInfo;
    //typedef struct WhereLevel WhereLevel;

    /*
    ** Defer sourcing vdbe.h and btree.h until after the "u8" and
    ** "BusyHandler" typedefs. vdbe.h also requires a few of the opaque
    ** pointer types (i.e. FuncDef) defined above.
    */
    //#include "btree.h"
    //#include "vdbe.h"
    //#include "pager.h"
    //#include "pcache_g.h"

    //#include "os.h"
    //#include "mutex.h"



    /*
    ** These macros can be used to test, set, or clear bits in the
    ** Db.pSchema->flags field.
    */
    //#define DbHasProperty(D,I,P)     (((D)->aDb[I].pSchema->flags&(P))==(P))
    static bool DbHasProperty( sqlite3 D, int I, ushort P )
    {
      return ( D.aDb[I].pSchema.flags & P ) == P;
    }
    //#define DbHasAnyProperty(D,I,P)  (((D)->aDb[I].pSchema->flags&(P))!=0)
    //#define DbSetProperty(D,I,P)     (D)->aDb[I].pSchema->flags|=(P)
    static void DbSetProperty( sqlite3 D, int I, ushort P )
    {
      D.aDb[I].pSchema.flags = (u16)( D.aDb[I].pSchema.flags | P );
    }
    //#define DbClearProperty(D,I,P)   (D)->aDb[I].pSchema->flags&=~(P)
    static void DbClearProperty( sqlite3 D, int I, ushort P )
    {
      D.aDb[I].pSchema.flags = (u16)( D.aDb[I].pSchema.flags & ~P );
    }
    /*
    ** Allowed values for the DB.pSchema->flags field.
    **
    ** The DB_SchemaLoaded flag is set after the database schema has been
    ** read into internal hash tables.
    **
    ** DB_UnresetViews means that one or more views have column names that
    ** have been filled out.  If the schema changes, these column names might
    ** changes and so the view will need to be reset.
    */
    //#define DB_SchemaLoaded    0x0001  /* The schema has been loaded */
    //#define DB_UnresetViews    0x0002  /* Some views have defined column names */
    //#define DB_Empty           0x0004  /* The file is empty (length 0 bytes) */
    const u16 DB_SchemaLoaded = 0x0001;
    const u16 DB_UnresetViews = 0x0002;
    const u16 DB_Empty = 0x0004;

    /*
    ** The number of different kinds of things that can be limited
    ** using the sqlite3_limit() interface.
    */
    //#define SQLITE_N_LIMIT (SQLITE_LIMIT_TRIGGER_DEPTH+1)
    const int SQLITE_N_LIMIT = SQLITE_LIMIT_TRIGGER_DEPTH + 1;




    /*
    ** A macro to discover the encoding of a database.
    */
    //#define ENC(db) ((db)->aDb[0].pSchema->enc)
    static u8 ENC( sqlite3 db )
    {
      return db.aDb[0].pSchema.enc;
    }

    /*
    ** Possible values for the sqlite3.flags.
    */
    //#define SQLITE_VdbeTrace      0x00000100  /* True to trace VDBE execution */
    //#define SQLITE_InternChanges  0x00000200  /* Uncommitted Hash table changes */
    //#define SQLITE_FullColNames   0x00000400  /* Show full column names on SELECT */
    //#define SQLITE_ShortColNames  0x00000800  /* Show short columns names */
    //#define SQLITE_CountRows      0x00001000  /* Count rows changed by INSERT, */
    //                                          /*   DELETE, or UPDATE and return */
    //                                          /*   the count using a callback. */
    //#define SQLITE_NullCallback   0x00002000  /* Invoke the callback once if the */
    //                                          /*   result set is empty */
    //#define SQLITE_SqlTrace       0x00004000  /* Debug print SQL as it executes */
    //#define SQLITE_VdbeListing    0x00008000  /* Debug listings of VDBE programs */
    //#define SQLITE_WriteSchema    0x00010000  /* OK to update SQLITE_MASTER */
    //#define SQLITE_NoReadlock     0x00020000  /* Readlocks are omitted when 
    //                                          ** accessing read-only databases */
    //#define SQLITE_IgnoreChecks   0x00040000  /* Do not enforce check constraints */
    //#define SQLITE_ReadUncommitted 0x0080000  /* For shared-cache mode */
    //#define SQLITE_LegacyFileFmt  0x00100000  /* Create new databases in format 1 */
    //#define SQLITE_FullFSync      0x00200000  /* Use full fsync on the backend */
    //#define SQLITE_CkptFullFSync  0x00400000  /* Use full fsync for checkpoint */
    //#define SQLITE_RecoveryMode   0x00800000  /* Ignore schema errors */
    //#define SQLITE_ReverseOrder   0x01000000  /* Reverse unordered SELECTs */
    //#define SQLITE_RecTriggers    0x02000000  /* Enable recursive triggers */
    //#define SQLITE_ForeignKeys    0x04000000  /* Enforce foreign key constraints  */
    //#define SQLITE_AutoIndex      0x08000000  /* Enable automatic indexes */
    //#define SQLITE_PreferBuiltin  0x10000000  /* Preference to built-in funcs */
    //#define SQLITE_LoadExtension  0x20000000  /* Enable load_extension */
    //define SQLITE_EnableTrigger  0x40000000  /* True to enable triggers */
    const int SQLITE_VdbeTrace = 0x00000100;
    const int SQLITE_InternChanges = 0x00000200;
    const int SQLITE_FullColNames = 0x00000400;
    const int SQLITE_ShortColNames = 0x00000800;
    const int SQLITE_CountRows = 0x00001000;
    const int SQLITE_NullCallback = 0x00002000;
    const int SQLITE_SqlTrace = 0x00004000;
    const int SQLITE_VdbeListing = 0x00008000;
    const int SQLITE_WriteSchema = 0x00010000;
    const int SQLITE_NoReadlock = 0x00020000;
    const int SQLITE_IgnoreChecks = 0x00040000;
    const int SQLITE_ReadUncommitted = 0x0080000;
    const int SQLITE_LegacyFileFmt = 0x00100000;
    const int SQLITE_FullFSync = 0x00200000;
    const int SQLITE_CkptFullFSync = 0x00400000;
    const int SQLITE_RecoveryMode = 0x00800000;
    const int SQLITE_ReverseOrder = 0x01000000;
    const int SQLITE_RecTriggers = 0x02000000;
    const int SQLITE_ForeignKeys = 0x04000000;
    const int SQLITE_AutoIndex = 0x08000000;
    const int SQLITE_PreferBuiltin = 0x10000000;
    const int SQLITE_LoadExtension = 0x20000000;
    const int SQLITE_EnableTrigger = 0x40000000;

    /*
    ** Bits of the sqlite3.flags field that are used by the
    ** sqlite3_test_control(SQLITE_TESTCTRL_OPTIMIZATIONS,...) interface.
    ** These must be the low-order bits of the flags field.
    */
    //#define SQLITE_QueryFlattener 0x01        /* Disable query flattening */
    //#define SQLITE_ColumnCache    0x02        /* Disable the column cache */
    //#define SQLITE_IndexSort      0x04        /* Disable indexes for sorting */
    //#define SQLITE_IndexSearch    0x08        /* Disable indexes for searching */
    //#define SQLITE_IndexCover     0x10        /* Disable index covering table */
    //#define SQLITE_GroupByOrder   0x20        /* Disable GROUPBY cover of ORDERBY */
    //#define SQLITE_FactorOutConst 0x40        /* Disable factoring out constants */
    //#define SQLITE_IdxRealAsInt   0x80        /* Store REAL as INT in indices */
    //#define SQLITE_OptMask        0xff        /* Mask of all disablable opts */
    const int SQLITE_QueryFlattener = 0x01;
    const int SQLITE_ColumnCache = 0x02;
    const int SQLITE_IndexSort = 0x04;
    const int SQLITE_IndexSearch = 0x08;
    const int SQLITE_IndexCover = 0x10;
    const int SQLITE_GroupByOrder = 0x20;
    const int SQLITE_FactorOutConst = 0x40;
    const int SQLITE_IdxRealAsInt = 0x80;
    const int SQLITE_OptMask = 0xff;

    /*
    ** Possible values for the sqlite.magic field.
    ** The numbers are obtained at random and have no special meaning, other
    ** than being distinct from one another.
    */
    const int SQLITE_MAGIC_OPEN = 0x1029a697;   //#define SQLITE_MAGIC_OPEN     0xa029a697  /* Database is open */
    const int SQLITE_MAGIC_CLOSED = 0x2f3c2d33; //#define SQLITE_MAGIC_CLOSED   0x9f3c2d33  /* Database is closed */
    const int SQLITE_MAGIC_SICK = 0x3b771290;   //#define SQLITE_MAGIC_SICK     0x4b771290  /* Error and awaiting close */
    const int SQLITE_MAGIC_BUSY = 0x403b7906;   //#define SQLITE_MAGIC_BUSY     0xf03b7906  /* Database currently in use */
    const int SQLITE_MAGIC_ERROR = 0x55357930;  //#define SQLITE_MAGIC_ERROR    0xb5357930  /* An SQLITE_MISUSE error occurred */




    /*
    ** Possible values for FuncDef.flags
    */
    //#define SQLITE_FUNC_LIKE     0x01  /* Candidate for the LIKE optimization */
    //#define SQLITE_FUNC_CASE     0x02  /* Case-sensitive LIKE-type function */
    //#define SQLITE_FUNC_EPHEM    0x04  /* Ephemeral.  Delete with VDBE */
    //#define SQLITE_FUNC_NEEDCOLL 0x08 /* sqlite3GetFuncCollSeq() might be called */
    //#define SQLITE_FUNC_PRIVATE  0x10 /* Allowed for internal use only */
    //#define SQLITE_FUNC_COUNT    0x20 /* Built-in count() aggregate */
    //#define SQLITE_FUNC_COALESCE 0x40 /* Built-in coalesce() or ifnull() function */
    const int SQLITE_FUNC_LIKE = 0x01;    /* Candidate for the LIKE optimization */
    const int SQLITE_FUNC_CASE = 0x02;    /* Case-sensitive LIKE-type function */
    const int SQLITE_FUNC_EPHEM = 0x04;   /* Ephermeral.  Delete with VDBE */
    const int SQLITE_FUNC_NEEDCOLL = 0x08;/* sqlite3GetFuncCollSeq() might be called */
    const int SQLITE_FUNC_PRIVATE = 0x10; /* Allowed for internal use only */
    const int SQLITE_FUNC_COUNT = 0x20;   /* Built-in count() aggregate */
    const int SQLITE_FUNC_COALESCE = 0x40;/* Built-in coalesce() or ifnull() function */

    /*
    ** The following three macros, FUNCTION(), LIKEFUNC() and AGGREGATE() are
    ** used to create the initializers for the FuncDef structures.
    **
    **   FUNCTION(zName, nArg, iArg, bNC, xFunc)
    **     Used to create a scalar function definition of a function zName
    **     implemented by C function xFunc that accepts nArg arguments. The
    **     value passed as iArg is cast to a (void) and made available
    **     as the user-data (sqlite3_user_data()) for the function. If
    **     argument bNC is true, then the SQLITE_FUNC_NEEDCOLL flag is set.
    **
    **   AGGREGATE(zName, nArg, iArg, bNC, xStep, xFinal)
    **     Used to create an aggregate function definition implemented by
    **     the C functions xStep and xFinal. The first four parameters
    **     are interpreted in the same way as the first 4 parameters to
    **     FUNCTION().
    **
    **   LIKEFUNC(zName, nArg, pArg, flags)
    **     Used to create a scalar function definition of a function zName
    **     that accepts nArg arguments and is implemented by a call to C
    **     function likeFunc. Argument pArg is cast to a (void ) and made
    **     available as the function user-data (sqlite3_user_data()). The
    **     FuncDef.flags variable is set to the value passed as the flags
    **     parameter.
    */
    //#define FUNCTION(zName, nArg, iArg, bNC, xFunc) \
    //  {nArg, SQLITE_UTF8, bNC*SQLITE_FUNC_NEEDCOLL, \
    //SQLITE_INT_TO_PTR(iArg), 0, xFunc, 0, 0, #zName, 0, 0}

    static FuncDef FUNCTION( string zName, i16 nArg, int iArg, u8 bNC, dxFunc xFunc )
    {
      return new FuncDef( zName, SQLITE_UTF8, nArg, iArg, (u8)( bNC * SQLITE_FUNC_NEEDCOLL ), xFunc );
    }

    //#define STR_FUNCTION(zName, nArg, pArg, bNC, xFunc) \
    //  {nArg, SQLITE_UTF8, bNC*SQLITE_FUNC_NEEDCOLL, \
    //pArg, 0, xFunc, 0, 0, #zName, 0, 0}

    //#define LIKEFUNC(zName, nArg, arg, flags) \
    //  {nArg, SQLITE_UTF8, flags, (void )arg, 0, likeFunc, 0, 0, #zName, 0, 0}
    static FuncDef LIKEFUNC( string zName, i16 nArg, object arg, u8 flags )
    {
      return new FuncDef( zName, SQLITE_UTF8, nArg, arg, likeFunc, flags );
    }

    //#define AGGREGATE(zName, nArg, arg, nc, xStep, xFinal) \
    //  {nArg, SQLITE_UTF8, nc*SQLITE_FUNC_NEEDCOLL, \
    //SQLITE_INT_TO_PTR(arg), 0, 0, xStep,xFinal,#zName,0,0}

    static FuncDef AGGREGATE( string zName, i16 nArg, int arg, u8 nc, dxStep xStep, dxFinal xFinal )
    {
      return new FuncDef( zName, SQLITE_UTF8, nArg, arg, (u8)( nc * SQLITE_FUNC_NEEDCOLL ), xStep, xFinal );
    }


    /*
    ** The following are used as the second parameter to sqlite3Savepoint(),
    ** and as the P1 argument to the OP_Savepoint instruction.
    */
    const int SAVEPOINT_BEGIN = 0;   //#define SAVEPOINT_BEGIN      0
    const int SAVEPOINT_RELEASE = 1;   //#define SAVEPOINT_RELEASE    1
    const int SAVEPOINT_ROLLBACK = 2;    //#define SAVEPOINT_ROLLBACK   2




    /*
    ** Allowed values of CollSeq.type:
    */
    const int SQLITE_COLL_BINARY = 1;//#define SQLITE_COLL_BINARY  1  /* The default memcmp() collating sequence */
    const int SQLITE_COLL_NOCASE = 2;//#define SQLITE_COLL_NOCASE  2  /* The built-in NOCASE collating sequence */
    const int SQLITE_COLL_REVERSE = 3;//#define SQLITE_COLL_REVERSE 3  /* The built-in REVERSE collating sequence */
    const int SQLITE_COLL_USER = 0;//#define SQLITE_COLL_USER    0  /* Any other user-defined collating sequence */

    /*
    ** A sort order can be either ASC or DESC.
    */
    const int SQLITE_SO_ASC = 0;//#define SQLITE_SO_ASC       0  /* Sort in ascending order */
    const int SQLITE_SO_DESC = 1;//#define SQLITE_SO_DESC     1  /* Sort in ascending order */

    /*
    ** Column affinity types.
    **
    ** These used to have mnemonic name like 'i' for SQLITE_AFF_INTEGER and
    ** 't' for SQLITE_AFF_TEXT.  But we can save a little space and improve
    ** the speed a little by numbering the values consecutively.
    **
    ** But rather than start with 0 or 1, we begin with 'a'.  That way,
    ** when multiple affinity types are concatenated into a string and
    ** used as the P4 operand, they will be more readable.
    **
    ** Note also that the numeric types are grouped together so that testing
    ** for a numeric type is a single comparison.
    */
    const char SQLITE_AFF_TEXT = 'a';//#define SQLITE_AFF_TEXT     'a'
    const char SQLITE_AFF_NONE = 'b';//#define SQLITE_AFF_NONE     'b'
    const char SQLITE_AFF_NUMERIC = 'c';//#define SQLITE_AFF_NUMERIC  'c'
    const char SQLITE_AFF_INTEGER = 'd';//#define SQLITE_AFF_INTEGER  'd'
    const char SQLITE_AFF_REAL = 'e';//#define SQLITE_AFF_REAL     'e'

    //#define sqlite3IsNumericAffinity(X)  ((X)>=SQLITE_AFF_NUMERIC)

    /*
    ** The SQLITE_AFF_MASK values masks off the significant bits of an
    ** affinity value.
    */
    const int SQLITE_AFF_MASK = 0x67;//#define SQLITE_AFF_MASK     0x67

    /*
    ** Additional bit values that can be ORed with an affinity without
    ** changing the affinity.
    */
    const int SQLITE_JUMPIFNULL = 0x08; //#define SQLITE_JUMPIFNULL   0x08  /* jumps if either operand is NULL */
    const int SQLITE_STOREP2 = 0x10;    //#define SQLITE_STOREP2      0x10  /* Store result in reg[P2] rather than jump */
    const int SQLITE_NULLEQ = 0x80;     //#define SQLITE_NULLEQ       0x80  /* NULL=NULL */



    /*
    ** Allowed values for Tabe.tabFlags.
    */
    //#define TF_Readonly        0x01    /* Read-only system table */
    //#define TF_Ephemeral       0x02    /* An ephemeral table */
    //#define TF_HasPrimaryKey   0x04    /* Table has a primary key */
    //#define TF_Autoincrement   0x08    /* Integer primary key is autoincrement */
    //#define TF_Virtual         0x10    /* Is a virtual table */
    //#define TF_NeedMetadata    0x20    /* aCol[].zType and aCol[].pColl missing */
    /*
    ** Allowed values for Tabe.tabFlags.
    */
    const int TF_Readonly = 0x01;   /* Read-only system table */
    const int TF_Ephemeral = 0x02;   /* An ephemeral table */
    const int TF_HasPrimaryKey = 0x04;   /* Table has a primary key */
    const int TF_Autoincrement = 0x08;   /* Integer primary key is autoincrement */
    const int TF_Virtual = 0x10;   /* Is a virtual table */
    const int TF_NeedMetadata = 0x20;   /* aCol[].zType and aCol[].pColl missing */

    /*
    ** Test to see whether or not a table is a virtual table.  This is
    ** done as a macro so that it will be optimized out when virtual
    ** table support is omitted from the build.
    */
#if !SQLITE_OMIT_VIRTUALTABLE
//#  define IsVirtual(X)      (((X)->tabFlags & TF_Virtual)!=0)
    static bool IsVirtual( Table X )
    {
      return ( X.tabFlags & TF_Virtual ) != 0;
    }
    //#  define IsHiddenColumn(X) ((X)->isHidden)
static bool IsHiddenColumn( Column X )
{
  return X.isHidden != 0;
}
#else
    //#  define IsVirtual(X)      0
    static bool IsVirtual( Table T )
    {
      return false;
    }
    //#  define IsHiddenColumn(X) 0
    static bool IsHiddenColumn( Column C )
    {
      return false;
    }
#endif


    /*
    ** SQLite supports many different ways to resolve a constraint
    ** error.  ROLLBACK processing means that a constraint violation
    ** causes the operation in process to fail and for the current transaction
    ** to be rolled back.  ABORT processing means the operation in process
    ** fails and any prior changes from that one operation are backed out,
    ** but the transaction is not rolled back.  FAIL processing means that
    ** the operation in progress stops and returns an error code.  But prior
    ** changes due to the same operation are not backed out and no rollback
    ** occurs.  IGNORE means that the particular row that caused the constraint
    ** error is not inserted or updated.  Processing continues and no error
    ** is returned.  REPLACE means that preexisting database rows that caused
    ** a UNIQUE constraint violation are removed so that the new insert or
    ** update can proceed.  Processing continues and no error is reported.
    **
    ** RESTRICT, SETNULL, and CASCADE actions apply only to foreign keys.
    ** RESTRICT is the same as ABORT for IMMEDIATE foreign keys and the
    ** same as ROLLBACK for DEFERRED keys.  SETNULL means that the foreign
    ** key is set to NULL.  CASCADE means that a DELETE or UPDATE of the
    ** referenced table row is propagated into the row that holds the
    ** foreign key.
    **
    ** The following symbolic values are used to record which type
    ** of action to take.
    */
    const int OE_None = 0;//#define OE_None     0   /* There is no constraint to check */
    const int OE_Rollback = 1;//#define OE_Rollback 1   /* Fail the operation and rollback the transaction */
    const int OE_Abort = 2;//#define OE_Abort    2   /* Back out changes but do no rollback transaction */
    const int OE_Fail = 3;//#define OE_Fail     3   /* Stop the operation but leave all prior changes */
    const int OE_Ignore = 4;//#define OE_Ignore   4   /* Ignore the error. Do not do the INSERT or UPDATE */
    const int OE_Replace = 5;//#define OE_Replace  5   /* Delete existing record, then do INSERT or UPDATE */

    const int OE_Restrict = 6;//#define OE_Restrict 6   /* OE_Abort for IMMEDIATE, OE_Rollback for DEFERRED */
    const int OE_SetNull = 7;//#define OE_SetNull  7   /* Set the foreign key value to NULL */
    const int OE_SetDflt = 8;//#define OE_SetDflt  8   /* Set the foreign key value to its default */
    const int OE_Cascade = 9;//#define OE_Cascade  9   /* Cascade the changes */

    const int OE_Default = 99;//#define OE_Default  99  /* Do whatever the default action is */






    /*
    ** The datatype ynVar is a signed integer, either 16-bit or 32-bit.
    ** Usually it is 16-bits.  But if SQLITE_MAX_VARIABLE_NUMBER is greater
    ** than 32767 we have to make it 32-bit.  16-bit is preferred because
    ** it uses less memory in the Expr object, which is a big memory user
    ** in systems with lots of prepared statements.  And few applications
    ** need more than about 10 or 20 variables.  But some extreme users want
    ** to have prepared statements with over 32767 variables, and for them
    ** the option is available (at compile-time).
    */
    //#if SQLITE_MAX_VARIABLE_NUMBER<=32767
    //typedef i16 ynVar;
    //#else
    //typedef int ynVar;
    //#endif


    /*
    ** The following are the meanings of bits in the Expr.flags field.
    */
    //#define EP_FromJoin   0x0001  /* Originated in ON or USING clause of a join */
    //#define EP_Agg        0x0002  /* Contains one or more aggregate functions */
    //#define EP_Resolved   0x0004  /* IDs have been resolved to COLUMNs */
    //#define EP_Error      0x0008  /* Expression contains one or more errors */
    //#define EP_Distinct   0x0010  /* Aggregate function with DISTINCT keyword */
    //#define EP_VarSelect  0x0020  /* pSelect is correlated, not constant */
    //#define EP_DblQuoted  0x0040  /* token.z was originally in "..." */
    //#define EP_InfixFunc  0x0080  /* True for an infix function: LIKE, GLOB, etc */
    //#define EP_ExpCollate 0x0100  /* Collating sequence specified explicitly */
    //#define EP_FixedDest  0x0200  /* Result needed in a specific register */
    //#define EP_IntValue   0x0400  /* Integer value contained in u.iValue */
    //#define EP_xIsSelect  0x0800  /* x.pSelect is valid (otherwise x.pList is) */

    //#define EP_Reduced    0x1000  /* Expr struct is EXPR_REDUCEDSIZE bytes only */
    //#define EP_TokenOnly  0x2000  /* Expr struct is EXPR_TOKENONLYSIZE bytes only */
    //#define EP_Static     0x4000  /* Held in memory not obtained from malloc() */

    const ushort EP_FromJoin = 0x0001;
    const ushort EP_Agg = 0x0002;
    const ushort EP_Resolved = 0x0004;
    const ushort EP_Error = 0x0008;
    const ushort EP_Distinct = 0x0010;
    const ushort EP_VarSelect = 0x0020;
    const ushort EP_DblQuoted = 0x0040;
    const ushort EP_InfixFunc = 0x0080;
    const ushort EP_ExpCollate = 0x0100;
    const ushort EP_FixedDest = 0x0200;
    const ushort EP_IntValue = 0x0400;
    const ushort EP_xIsSelect = 0x0800;

    const ushort EP_Reduced = 0x1000;
    const ushort EP_TokenOnly = 0x2000;
    const ushort EP_Static = 0x4000;

    /*
    ** The following are the meanings of bits in the Expr.flags2 field.
    */
    //#define EP2_MallocedToken  0x0001  /* Need to sqlite3DbFree() Expr.zToken */
    //#define EP2_Irreducible    0x0002  /* Cannot EXPRDUP_REDUCE this Expr */
    const u8 EP2_MallocedToken = 0x0001;
    const u8 EP2_Irreducible = 0x0002;

    /*
    ** The pseudo-routine sqlite3ExprSetIrreducible sets the EP2_Irreducible
    ** flag on an expression structure.  This flag is used for VV&A only.  The
    ** routine is implemented as a macro that only works when in debugging mode,
    ** so as not to burden production code.
    */
#if SQLITE_DEBUG
    //# define ExprSetIrreducible(X)  (X)->flags2 |= EP2_Irreducible
    static void ExprSetIrreducible( Expr X )
    {
      X.flags2 |= EP2_Irreducible;
    }
#else
//# define ExprSetIrreducible(X)
static void ExprSetIrreducible( Expr X ) { }
#endif

    /*
** These macros can be used to test, set, or clear bits in the
** Expr.flags field.
*/
    //#define ExprHasProperty(E,P)     (((E)->flags&(P))==(P))
    static bool ExprHasProperty( Expr E, int P )
    {
      return ( E.flags & P ) == P;
    }
    //#define ExprHasAnyProperty(E,P)  (((E)->flags&(P))!=0)
    static bool ExprHasAnyProperty( Expr E, int P )
    {
      return ( E.flags & P ) != 0;
    }
    //#define ExprSetProperty(E,P)     (E)->flags|=(P)
    static void ExprSetProperty( Expr E, int P )
    {
      E.flags = (ushort)( E.flags | P );
    }
    //#define ExprClearProperty(E,P)   (E)->flags&=~(P)
    static void ExprClearProperty( Expr E, int P )
    {
      E.flags = (ushort)( E.flags & ~P );
    }

    /*
    ** Macros to determine the number of bytes required by a normal Expr
    ** struct, an Expr struct with the EP_Reduced flag set in Expr.flags
    ** and an Expr struct with the EP_TokenOnly flag set.
    */
    //#define EXPR_FULLSIZE           sizeof(Expr)           /* Full size */
    //#define EXPR_REDUCEDSIZE        offsetof(Expr,iTable)  /* Common features */
    //#define EXPR_TOKENONLYSIZE      offsetof(Expr,pLeft)   /* Fewer features */

    // We don't use these in C#, but define them anyway,
    const int EXPR_FULLSIZE = 48;
    const int EXPR_REDUCEDSIZE = 24;
    const int EXPR_TOKENONLYSIZE = 8;

    /*
    ** Flags passed to the sqlite3ExprDup() function. See the header comment
    ** above sqlite3ExprDup() for details.
    */
    //#define EXPRDUP_REDUCE         0x0001  /* Used reduced-size Expr nodes */
    const int EXPRDUP_REDUCE = 0x0001;




    /*
    ** The bitmask datatype defined below is used for various optimizations.
    **
    ** Changing this from a 64-bit to a 32-bit type limits the number of
    ** tables in a join to 32 instead of 64.  But it also reduces the size
    ** of the library by 738 bytes on ix86.
    */
    //typedef u64 Bitmask;

    /*
    ** The number of bits in a Bitmask.  "BMS" means "BitMask Size".
    */
    //#define BMS  ((int)(sizeof(Bitmask)*8))
    const int BMS = ( (int)( sizeof( Bitmask ) * 8 ) );



    /*
    ** Permitted values of the SrcList.a.jointype field
    */
    const int JT_INNER = 0x0001;   //#define JT_INNER     0x0001    /* Any kind of inner or cross join */
    const int JT_CROSS = 0x0002;   //#define JT_CROSS     0x0002    /* Explicit use of the CROSS keyword */
    const int JT_NATURAL = 0x0004; //#define JT_NATURAL   0x0004    /* True for a "natural" join */
    const int JT_LEFT = 0x0008;    //#define JT_LEFT      0x0008    /* Left outer join */
    const int JT_RIGHT = 0x0010;   //#define JT_RIGHT     0x0010    /* Right outer join */
    const int JT_OUTER = 0x0020;   //#define JT_OUTER     0x0020    /* The "OUTER" keyword is present */
    const int JT_ERROR = 0x0040;   //#define JT_ERROR     0x0040    /* unknown or unsupported join type */



    /*
    ** For each nested loop in a WHERE clause implementation, the WhereInfo
    ** structure contains a single instance of this structure.  This structure
    ** is intended to be private the the where.c module and should not be
    ** access or modified by other modules.
    **
    ** The pIdxInfo field is used to help pick the best index on a
    ** virtual table.  The pIdxInfo pointer contains indexing
    ** information for the i-th table in the FROM clause before reordering.
    ** All the pIdxInfo pointers are freed by whereInfoFree() in where.c.
    ** All other information in the i-th WhereLevel object for the i-th table
    ** after FROM clause ordering.
    */
    public class InLoop
    {
      public int iCur;              /* The VDBE cursor used by this IN operator */
      public int addrInTop;         /* Top of the IN loop */
    }
    public class WhereLevel
    {
      public WherePlan plan = new WherePlan();       /* query plan for this element of the FROM clause */
      public int iLeftJoin;        /* Memory cell used to implement LEFT OUTER JOIN */
      public int iTabCur;          /* The VDBE cursor used to access the table */
      public int iIdxCur;          /* The VDBE cursor used to access pIdx */
      public int addrBrk;          /* Jump here to break out of the loop */
      public int addrNxt;          /* Jump here to start the next IN combination */
      public int addrCont;         /* Jump here to continue with the next loop cycle */
      public int addrFirst;        /* First instruction of interior of the loop */
      public u8 iFrom;             /* Which entry in the FROM clause */
      public u8 op, p5;            /* Opcode and P5 of the opcode that ends the loop */
      public int p1, p2;           /* Operands of the opcode used to ends the loop */
      public class _u
      {
        public class __in               /* Information that depends on plan.wsFlags */
        {
          public int nIn;              /* Number of entries in aInLoop[] */
          public InLoop[] aInLoop;           /* Information about each nested IN operator */
        }
        public __in _in = new __in();                 /* Used when plan.wsFlags&WHERE_IN_ABLE */
      }
      public _u u = new _u();


      /* The following field is really not part of the current level.  But
      ** we need a place to cache virtual table index information for each
      ** virtual table in the FROM clause and the WhereLevel structure is
      ** a convenient place since there is one WhereLevel for each FROM clause
      ** element.
      */
      public sqlite3_index_info pIdxInfo;  /* Index info for n-th source table */
    };

    /*
    ** Flags appropriate for the wctrlFlags parameter of sqlite3WhereBegin()
    ** and the WhereInfo.wctrlFlags member.
    */
    //#define WHERE_ORDERBY_NORMAL   0x0000 /* No-op */
    //#define WHERE_ORDERBY_MIN      0x0001 /* ORDER BY processing for min() func */
    //#define WHERE_ORDERBY_MAX      0x0002 /* ORDER BY processing for max() func */
    //#define WHERE_ONEPASS_DESIRED  0x0004 /* Want to do one-pass UPDATE/DELETE */
    //#define WHERE_DUPLICATES_OK    0x0008 /* Ok to return a row more than once */
    //#define WHERE_OMIT_OPEN        0x0010  /* Table cursors are already open */
    //#define WHERE_OMIT_CLOSE       0x0020  /* Omit close of table & index cursors */
    //#define WHERE_FORCE_TABLE      0x0040 /* Do not use an index-only search */
    //#define WHERE_ONETABLE_ONLY    0x0080 /* Only code the 1st table in pTabList */
    const int WHERE_ORDERBY_NORMAL = 0x0000;
    const int WHERE_ORDERBY_MIN = 0x0001;
    const int WHERE_ORDERBY_MAX = 0x0002;
    const int WHERE_ONEPASS_DESIRED = 0x0004;
    const int WHERE_DUPLICATES_OK = 0x0008;
    const int WHERE_OMIT_OPEN = 0x0010;
    const int WHERE_OMIT_CLOSE = 0x0020;
    const int WHERE_FORCE_TABLE = 0x0040;
    const int WHERE_ONETABLE_ONLY = 0x0080;

    /*
    ** The WHERE clause processing routine has two halves.  The
    ** first part does the start of the WHERE loop and the second
    ** half does the tail of the WHERE loop.  An instance of
    ** this structure is returned by the first half and passed
    ** into the second half to give some continuity.
    */
    public class WhereInfo
    {
      public Parse pParse;            /* Parsing and code generating context */
      public u16 wctrlFlags;          /* Flags originally passed to sqlite3WhereBegin() */
      public u8 okOnePass;            /* Ok to use one-pass algorithm for UPDATE or DELETE */
      public u8 untestedTerms;        /* Not all WHERE terms resolved by outer loop */
      public SrcList pTabList;        /* List of tables in the join */
      public int iTop;                /* The very beginning of the WHERE loop */
      public int iContinue;           /* Jump here to continue with next record */
      public int iBreak;              /* Jump here to break out of the loop */
      public int nLevel;              /* Number of nested loop */
      public WhereClause pWC;         /* Decomposition of the WHERE clause */
      public double savedNQueryLoop;  /* pParse->nQueryLoop outside the WHERE loop */
      public double nRowOut;          /* Estimated number of output rows */
      public WhereLevel[] a = new WhereLevel[] { new WhereLevel() };     /* Information about each nest loop in the WHERE */
    };



    /*
    ** Allowed values for Select.selFlags.  The "SF" prefix stands for
    ** "Select Flag".
    */
    //#define SF_Distinct        0x0001  /* Output should be DISTINCT */
    //#define SF_Resolved        0x0002  /* Identifiers have been resolved */
    //#define SF_Aggregate       0x0004  /* Contains aggregate functions */
    //#define SF_UsesEphemeral   0x0008  /* Uses the OpenEphemeral opcode */
    //#define SF_Expanded        0x0010  /* sqlite3SelectExpand() called on this */
    //#define SF_HasTypeInfo     0x0020  /* FROM subqueries have Table metadata */
    const int SF_Distinct = 0x0001;  /* Output should be DISTINCT */
    const int SF_Resolved = 0x0002;  /* Identifiers have been resolved */
    const int SF_Aggregate = 0x0004;  /* Contains aggregate functions */
    const int SF_UsesEphemeral = 0x0008;  /* Uses the OpenEphemeral opcode */
    const int SF_Expanded = 0x0010;  /* sqlite3SelectExpand() called on this */
    const int SF_HasTypeInfo = 0x0020;  /* FROM subqueries have Table metadata */


    /*
    ** The results of a select can be distributed in several ways.  The
    ** "SRT" prefix means "SELECT Result Type".
    */
    const int SRT_Union = 1;//#define SRT_Union        1  /* Store result as keys in an index */
    const int SRT_Except = 2;//#define SRT_Except      2  /* Remove result from a UNION index */
    const int SRT_Exists = 3;//#define SRT_Exists      3  /* Store 1 if the result is not empty */
    const int SRT_Discard = 4;//#define SRT_Discard    4  /* Do not save the results anywhere */

    /* The ORDER BY clause is ignored for all of the above */
    //#define IgnorableOrderby(X) ((X->eDest)<=SRT_Discard)

    const int SRT_Output = 5;//#define SRT_Output      5  /* Output each row of result */
    const int SRT_Mem = 6;//#define SRT_Mem            6  /* Store result in a memory cell */
    const int SRT_Set = 7;//#define SRT_Set            7  /* Store results as keys in an index */
    const int SRT_Table = 8;//#define SRT_Table        8  /* Store result as data with an automatic rowid */
    const int SRT_EphemTab = 9;//#define SRT_EphemTab  9  /* Create transient tab and store like SRT_Table /
    const int SRT_Coroutine = 10;//#define SRT_Coroutine   10  /* Generate a single row of result */

    /*
    ** A structure used to customize the behavior of sqlite3Select(). See
    ** comments above sqlite3Select() for details.
    */
    //typedef struct SelectDest SelectDest;
    public class SelectDest
    {
      public u8 eDest;        /* How to dispose of the results */
      public char affinity;    /* Affinity used when eDest==SRT_Set */
      public int iParm;        /* A parameter used by the eDest disposal method */
      public int iMem;         /* Base register where results are written */
      public int nMem;         /* Number of registers allocated */
      public SelectDest()
      {
        this.eDest = 0;
        this.affinity = '\0';
        this.iParm = 0;
        this.iMem = 0;
        this.nMem = 0;
      }
      public SelectDest( u8 eDest, char affinity, int iParm )
      {
        this.eDest = eDest;
        this.affinity = affinity;
        this.iParm = iParm;
        this.iMem = 0;
        this.nMem = 0;
      }
      public SelectDest( u8 eDest, char affinity, int iParm, int iMem, int nMem )
      {
        this.eDest = eDest;
        this.affinity = affinity;
        this.iParm = iParm;
        this.iMem = iMem;
        this.nMem = nMem;
      }
    };

    /*
    ** Size of the column cache
    */
#if !SQLITE_N_COLCACHE
    //# define SQLITE_N_COLCACHE 10
    const int SQLITE_N_COLCACHE = 10;
#endif


    /*
    ** The yDbMask datatype for the bitmask of all attached databases.
    */
    //#if SQLITE_MAX_ATTACHED>30
    //  typedef sqlite3_uint64 yDbMask;
    //#else
    //  typedef unsigned int yDbMask;
    //#endif


#if SQLITE_OMIT_VIRTUALTABLE
//#define IN_DECLARE_VTAB 0
    static bool IN_DECLARE_VTAB( Parse pParse )
    {
      return false;
    }
#else
    //#define IN_DECLARE_VTAB (pParse.declareVtab)
    static bool IN_DECLARE_VTAB( Parse pParse )
    {
      return pParse.declareVtab != 0;
    }
#endif

    /*
** An instance of the following structure can be declared on a stack and used
** to save the Parse.zAuthContext value so that it can be restored later.
*/
    public class AuthContext
    {
      public string zAuthContext;   /* Put saved Parse.zAuthContext here */
      public Parse pParse;              /* The Parse structure */
    };

    /*
    ** Bitfield flags for P5 value in OP_Insert and OP_Delete
    */
    //#define OPFLAG_NCHANGE       0x01    /* Set to update db->nChange */
    //#define OPFLAG_LASTROWID     0x02    /* Set to update db->lastRowid */
    //#define OPFLAG_ISUPDATE      0x04    /* This OP_Insert is an sql UPDATE */
    //#define OPFLAG_APPEND        0x08    /* This is likely to be an append */
    //#define OPFLAG_USESEEKRESULT 0x10    /* Try to avoid a seek in BtreeInsert() */
    //#define OPFLAG_CLEARCACHE    0x20    /* Clear pseudo-table cache in OP_Column */
    const byte OPFLAG_NCHANGE = 0x01;
    const byte OPFLAG_LASTROWID = 0x02;
    const byte OPFLAG_ISUPDATE = 0x04;
    const byte OPFLAG_APPEND = 0x08;
    const byte OPFLAG_USESEEKRESULT = 0x10;
    const byte OPFLAG_CLEARCACHE = 0x20;


    /*
    ** A trigger is either a BEFORE or an AFTER trigger.  The following constants
    ** determine which.
    **
    ** If there are multiple triggers, you might of some BEFORE and some AFTER.
    ** In that cases, the constants below can be ORed together.
    */
    const u8 TRIGGER_BEFORE = 1;//#define TRIGGER_BEFORE  1
    const u8 TRIGGER_AFTER = 2;//#define TRIGGER_AFTER   2


    /*
    ** The following structure contains information used by the sqliteFix...
    ** routines as they walk the parse tree to make database references
    ** explicit.
    */
    //typedef struct DbFixer DbFixer;
    public class DbFixer
    {
      public Parse pParse;       /* The parsing context.  Error messages written here */
      public string zDb;         /* Make sure all objects are contained in this database */
      public string zType;       /* Type of the container - used for error messages */
      public Token pName;        /* Name of the container - used for error messages */
    };


    /*
    ** A pointer to this structure is used to communicate information
    ** from sqlite3Init and OP_ParseSchema into the sqlite3InitCallback.
    */
    public class InitData
    {
      public sqlite3 db;        /* The database being initialized */
      public int iDb;            /* 0 for main database.  1 for TEMP, 2.. for ATTACHed */
      public string pzErrMsg;    /* Error message stored here */
      public int rc;             /* Result code stored here */
    }

    /*
    ** Structure containing global configuration data for the SQLite library.
    **
    ** This structure also contains some state information.
    */
    public class Sqlite3Config
    {
      public bool bMemstat;                    /* True to enable memory status */
      public bool bCoreMutex;                  /* True to enable core mutexing */
      public bool bFullMutex;                  /* True to enable full mutexing */
      public bool bOpenUri;                    /* True to interpret filenames as URIs */
      public int mxStrlen;                     /* Maximum string length */
      public int szLookaside;                  /* Default lookaside buffer size */
      public int nLookaside;                   /* Default lookaside buffer count */
      public sqlite3_mem_methods m;            /* Low-level memory allocation interface */
      public sqlite3_mutex_methods mutex;      /* Low-level mutex interface */
      public sqlite3_pcache_methods pcache;    /* Low-level page-cache interface */
      public byte[] pHeap;                     /* Heap storage space */
      public int nHeap;                        /* Size of pHeap[] */
      public int mnReq, mxReq;                 /* Min and max heap requests sizes */
      public byte[][] pScratch2;               /* Scratch memory */
      public byte[][] pScratch;                  /* Scratch memory */
      public int szScratch;                    /* Size of each scratch buffer */
      public int nScratch;                     /* Number of scratch buffers */
      public MemPage pPage;                    /* Page cache memory */
      public int szPage;                       /* Size of each page in pPage[] */
      public int nPage;                        /* Number of pages in pPage[] */
      public int mxParserStack;                /* maximum depth of the parser stack */
      public bool sharedCacheEnabled;          /* true if shared-cache mode enabled */
      /* The above might be initialized to non-zero.  The following need to always
      ** initially be zero, however. */
      public int isInit;                       /* True after initialization has finished */
      public int inProgress;                   /* True while initialization in progress */
      public int isMutexInit;                  /* True after mutexes are initialized */
      public int isMallocInit;                 /* True after malloc is initialized */
      public int isPCacheInit;                 /* True after malloc is initialized */
      public sqlite3_mutex pInitMutex;         /* Mutex used by sqlite3_initialize() */
      public int nRefInitMutex;                /* Number of users of pInitMutex */
      public dxLog xLog; //void (*xLog)(void*,int,const char); /* Function for logging */
      public object pLogArg;                   /* First argument to xLog() */
      public bool bLocaltimeFault;             /* True to fail localtime() calls */

      public Sqlite3Config(
        int bMemstat
        , int bCoreMutex
        , bool bFullMutex
        , bool bOpenUri
        , int mxStrlen
        , int szLookaside
        , int nLookaside
      , sqlite3_mem_methods m
      , sqlite3_mutex_methods mutex
      , sqlite3_pcache_methods pcache
      , byte[] pHeap
      , int nHeap
      , int mnReq
      , int mxReq
      , byte[][] pScratch
      , int szScratch
      , int nScratch
      , MemPage pPage
      , int szPage
      , int nPage
      , int mxParserStack
      , bool sharedCacheEnabled
      , int isInit
      , int inProgress
      , int isMutexInit
      , int isMallocInit
      , int isPCacheInit
      , sqlite3_mutex pInitMutex
      , int nRefInitMutex
      , dxLog xLog
      , object pLogArg
      , bool bLocaltimeFault
      )
      {
        this.bMemstat = bMemstat != 0;
        this.bCoreMutex = bCoreMutex != 0;
        this.bOpenUri = bOpenUri;
        this.bFullMutex = bFullMutex;
        this.mxStrlen = mxStrlen;
        this.szLookaside = szLookaside;
        this.nLookaside = nLookaside;
        this.m = m;
        this.mutex = mutex;
        this.pcache = pcache;
        this.pHeap = pHeap;
        this.nHeap = nHeap;
        this.mnReq = mnReq;
        this.mxReq = mxReq;
        this.pScratch = pScratch;
        this.szScratch = szScratch;
        this.nScratch = nScratch;
        this.pPage = pPage;
        this.szPage = szPage;
        this.nPage = nPage;
        this.mxParserStack = mxParserStack;
        this.sharedCacheEnabled = sharedCacheEnabled;
        this.isInit = isInit;
        this.inProgress = inProgress;
        this.isMutexInit = isMutexInit;
        this.isMallocInit = isMallocInit;
        this.isPCacheInit = isPCacheInit;
        this.pInitMutex = pInitMutex;
        this.nRefInitMutex = nRefInitMutex;
        this.xLog = xLog;
        this.pLogArg = pLogArg;
        this.bLocaltimeFault = bLocaltimeFault;
      }
    };



    /* Forward declarations */
    //int sqlite3WalkExpr(Walker*, Expr);
    //int sqlite3WalkExprList(Walker*, ExprList);
    //int sqlite3WalkSelect(Walker*, Select);
    //int sqlite3WalkSelectExpr(Walker*, Select);
    //int sqlite3WalkSelectFrom(Walker*, Select);

    /*
    ** Return code from the parse-tree walking primitives and their
    ** callbacks.
    */
    //#define WRC_Continue    0   /* Continue down into children */
    //#define WRC_Prune       1   /* Omit children but continue walking siblings */
    //#define WRC_Abort       2   /* Abandon the tree walk */
    const int WRC_Continue = 0;
    const int WRC_Prune = 1;
    const int WRC_Abort = 2;


    /*
    ** Assuming zIn points to the first byte of a UTF-8 character,
    ** advance zIn to point to the first byte of the next UTF-8 character.
    */
    //#define SQLITE_SKIP_UTF8(zIn) {                        \
    //  if( (*(zIn++))>=0xc0 ){                              \
    //    while( (*zIn & 0xc0)==0x80 ){ zIn++; }             \
    //  }                                                    \
    //}
    static void SQLITE_SKIP_UTF8( string zIn, ref int iz )
    {
      iz++;
      if ( iz < zIn.Length && zIn[iz - 1] >= 0xC0 )
      {
        while ( iz < zIn.Length && ( zIn[iz] & 0xC0 ) == 0x80 )
        {
          iz++;
        }
      }
    }
    static void SQLITE_SKIP_UTF8(
    byte[] zIn, ref int iz )
    {
      iz++;
      if ( iz < zIn.Length && zIn[iz - 1] >= 0xC0 )
      {
        while ( iz < zIn.Length && ( zIn[iz] & 0xC0 ) == 0x80 )
        {
          iz++;
        }
      }
    }

    /*
    ** The SQLITE_*_BKPT macros are substitutes for the error codes with
    ** the same name but without the _BKPT suffix.  These macros invoke
    ** routines that report the line-number on which the error originated
    ** using sqlite3_log().  The routines also provide a convenient place
    ** to set a debugger breakpoint.
    */
    //int sqlite3CorruptError(int);
    //int sqlite3MisuseError(int);
    //int sqlite3CantopenError(int);
#if DEBUG
  
    //#define SQLITE_CORRUPT_BKPT sqlite3CorruptError(__LINE__)
    static int SQLITE_CORRUPT_BKPT()
    {
      return sqlite3CorruptError( 0 );
    }

    //#define SQLITE_MISUSE_BKPT sqlite3MisuseError(__LINE__)
    static int SQLITE_MISUSE_BKPT()
    {
      return sqlite3MisuseError( 0 );
    }

    //#define SQLITE_CANTOPEN_BKPT sqlite3CantopenError(__LINE__)
    static int SQLITE_CANTOPEN_BKPT()
    {
      return sqlite3CantopenError( 0 );
    }
#else
static int SQLITE_CORRUPT_BKPT() {return SQLITE_CORRUPT;}
static int SQLITE_MISUSE_BKPT() {return SQLITE_MISUSE;}
static int SQLITE_CANTOPEN_BKPT() {return SQLITE_CANTOPEN;}
#endif

    /*
** FTS4 is really an extension for FTS3.  It is enabled using the
** SQLITE_ENABLE_FTS3 macro.  But to avoid confusion we also all
** the SQLITE_ENABLE_FTS4 macro to serve as an alisse for SQLITE_ENABLE_FTS3.
*/
    //#if (SQLITE_ENABLE_FTS4) && !defined(SQLITE_ENABLE_FTS3)
    //# define SQLITE_ENABLE_FTS3
    //#endif

    /*
    ** The ctype.h header is needed for non-ASCII systems.  It is also
    ** needed by FTS3 when FTS3 is included in the amalgamation.
    */
    //#if !defined(SQLITE_ASCII) || \
    //    (defined(SQLITE_ENABLE_FTS3) && defined(SQLITE_AMALGAMATION))
    //# include <ctype.h>
    //#endif


    /*
    ** The following macros mimic the standard library functions toupper(),
    ** isspace(), isalnum(), isdigit() and isxdigit(), respectively. The
    ** sqlite versions only work for ASCII characters, regardless of locale.
    */
#if SQLITE_ASCII
    //# define sqlite3Toupper(x)  ((x)&~(sqlite3CtypeMap[(unsigned char)(x)]&0x20))

    //# define sqlite3Isspace(x)   (sqlite3CtypeMap[(unsigned char)(x)]&0x01)
    static bool sqlite3Isspace( byte x )
    {
      return ( sqlite3CtypeMap[(byte)( x )] & 0x01 ) != 0;
    }
    static bool sqlite3Isspace( char x )
    {
      return x < 256 && ( sqlite3CtypeMap[(byte)( x )] & 0x01 ) != 0;
    }

    //# define sqlite3Isalnum(x)   (sqlite3CtypeMap[(unsigned char)(x)]&0x06)
    static bool sqlite3Isalnum( byte x )
    {
      return ( sqlite3CtypeMap[(byte)( x )] & 0x06 ) != 0;
    }
    static bool sqlite3Isalnum( char x )
    {
      return x < 256 && ( sqlite3CtypeMap[(byte)( x )] & 0x06 ) != 0;
    }

    //# define sqlite3Isalpha(x)   (sqlite3CtypeMap[(unsigned char)(x)]&0x02)

    //# define sqlite3Isdigit(x)   (sqlite3CtypeMap[(unsigned char)(x)]&0x04)
    static bool sqlite3Isdigit( byte x )
    {
      return ( sqlite3CtypeMap[( (byte)x )] & 0x04 ) != 0;
    }
    static bool sqlite3Isdigit( char x )
    {
      return x < 256 && ( sqlite3CtypeMap[( (byte)x )] & 0x04 ) != 0;
    }

    //# define sqlite3Isxdigit(x)  (sqlite3CtypeMap[(unsigned char)(x)]&0x08)
    static bool sqlite3Isxdigit( byte x )
    {
      return ( sqlite3CtypeMap[( (byte)x )] & 0x08 ) != 0;
    }
    static bool sqlite3Isxdigit( char x )
    {
      return x < 256 && ( sqlite3CtypeMap[( (byte)x )] & 0x08 ) != 0;
    }

    //# define sqlite3Tolower(x)   (sqlite3UpperToLower[(unsigned char)(x)])
#else
//# define sqlite3Toupper(x)   toupper((unsigned char)(x))
//# define sqlite3Isspace(x)   isspace((unsigned char)(x))
//# define sqlite3Isalnum(x)   isalnum((unsigned char)(x))
//# define sqlite3Isalpha(x)   isalpha((unsigned char)(x))
//# define sqlite3Isdigit(x)   isdigit((unsigned char)(x))
//# define sqlite3Isxdigit(x)  isxdigit((unsigned char)(x))
//# define sqlite3Tolower(x)   tolower((unsigned char)(x))
#endif

    /*
** Internal function prototypes
*/
    //int sqlite3StrICmp(string , string );
    //int sqlite3Strlen30(const char);
    //#define sqlite3StrNICmp sqlite3_strnicmp

    //int sqlite3MallocInit(void);
    //void sqlite3MallocEnd(void);
    //void *sqlite3Malloc(int);
    //void *sqlite3MallocZero(int);
    //void *sqlite3DbMallocZero(sqlite3*, int);
    //void *sqlite3DbMallocRaw(sqlite3*, int);
    //char *sqlite3DbStrDup(sqlite3*,const char);
    //char *sqlite3DbStrNDup(sqlite3*,const char*, int);
    //void *sqlite3Realloc(void*, int);
    //void *sqlite3DbReallocOrFree(sqlite3 *, object  *, int);
    //void *sqlite3DbRealloc(sqlite3 *, object  *, int);
    //void sqlite3DbFree(sqlite3*, void);
    //int sqlite3MallocSize(void);
    //int sqlite3DbMallocSize(sqlite3*, void);
    //void *sqlite3ScratchMalloc(int);
    //void //sqlite3ScratchFree(void);
    //void *sqlite3PageMalloc(int);
    //void sqlite3PageFree(void);
    //void sqlite3MemSetDefault(void);
    //void sqlite3BenignMallocHooks(void ()(void), object  ()(void));
    //int sqlite3HeapNearlyFull(void);

    /*
    ** On systems with ample stack space and that support alloca(), make
    ** use of alloca() to obtain space for large automatic objects.  By default,
    ** obtain space from malloc().
    **
    ** The alloca() routine never returns NULL.  This will cause code paths
    ** that deal with sqlite3StackAlloc() failures to be unreachable.
    */
#if SQLITE_USE_ALLOCA
//# define sqlite3StackAllocRaw(D,N)   alloca(N)
//# define sqlite3StackAllocZero(D,N)  memset(alloca(N), 0, N)
//# define sqlite3StackFree(D,P)
#else
#if FALSE
//# define sqlite3StackAllocRaw(D,N)   sqlite3DbMallocRaw(D,N)
static void sqlite3StackAllocRaw( sqlite3 D, int N ) { sqlite3DbMallocRaw( D, N ); }
//# define sqlite3StackAllocZero(D,N)  sqlite3DbMallocZero(D,N)
static void sqlite3StackAllocZero( sqlite3 D, int N ) { sqlite3DbMallocZero( D, N ); }
//# define sqlite3StackFree(D,P)       sqlite3DbFree(D,P)
static void sqlite3StackFree( sqlite3 D, object P ) {sqlite3DbFree( D, P ); }
#endif
#endif

#if SQLITE_ENABLE_MEMSYS3
const sqlite3_mem_methods *sqlite3MemGetMemsys3(void);
#endif
#if SQLITE_ENABLE_MEMSYS5
const sqlite3_mem_methods *sqlite3MemGetMemsys5(void);
#endif

#if !SQLITE_MUTEX_OMIT
    //  sqlite3_mutex_methods const *sqlite3DefaultMutex(void);
    //  sqlite3_mutex_methods const *sqlite3NoopMutex(void);
    //  sqlite3_mutex *sqlite3MutexAlloc(int);
    //  int sqlite3MutexInit(void);
    //  int sqlite3MutexEnd(void);
#endif

    //int sqlite3StatusValue(int);
    //void sqlite3StatusAdd(int, int);
    //void sqlite3StatusSet(int, int);

    //#if !SQLITE_OMIT_FLOATING_POINT
    //  int sqlite3IsNaN(double);
    //#else
    //# define sqlite3IsNaN(X)  0
    //#endif

    //void sqlite3VXPrintf(StrAccum*, int, const char*, va_list);
#if!SQLITE_OMIT_TRACE
    //void sqlite3XPrintf(StrAccum*, const char*, ...);
#endif
    //char *sqlite3MPrintf(sqlite3*,const char*, ...);
    //char *sqlite3VMPrintf(sqlite3*,const char*, va_list);
    //char *sqlite3MAppendf(sqlite3*,char*,const char*,...);
#if SQLITE_TEST || SQLITE_DEBUG
    //  void sqlite3DebugPrintf(const char*, ...);
#endif
#if SQLITE_TEST
    //  void *sqlite3TestTextToPtr(const char);
#endif
    //void sqlite3SetString(char **, sqlite3*, const char*, ...);
    //void sqlite3ErrorMsg(Parse*, const char*, ...);
    //int sqlite3Dequote(char);
    //int sqlite3KeywordCode(const unsigned char*, int);
    //int sqlite3RunParser(Parse*, const char*, char *);
    //void sqlite3FinishCoding(Parse);
    //int sqlite3GetTempReg(Parse);
    //void sqlite3ReleaseTempReg(Parse*,int);
    //int sqlite3GetTempRange(Parse*,int);
    //void sqlite3ReleaseTempRange(Parse*,int,int);
    //Expr *sqlite3ExprAlloc(sqlite3*,int,const Token*,int);
    //Expr *sqlite3Expr(sqlite3*,int,const char);
    //void sqlite3ExprAttachSubtrees(sqlite3*,Expr*,Expr*,Expr);
    //Expr *sqlite3PExpr(Parse*, int, Expr*, Expr*, const Token);
    //Expr *sqlite3ExprAnd(sqlite3*,Expr*, Expr);
    //Expr *sqlite3ExprFunction(Parse*,ExprList*, Token);
    //void sqlite3ExprAssignVarNumber(Parse*, Expr);
    //void sqlite3ExprDelete(sqlite3*, Expr);
    //ExprList *sqlite3ExprListAppend(Parse*,ExprList*,Expr);
    //void sqlite3ExprListSetName(Parse*,ExprList*,Token*,int);
    //void sqlite3ExprListSetSpan(Parse*,ExprList*,ExprSpan);
    //void sqlite3ExprListDelete(sqlite3*, ExprList);
    //int sqlite3Init(sqlite3*, char*);
    //int sqlite3InitCallback(void*, int, char**, char*);
    //void sqlite3Pragma(Parse*,Token*,Token*,Token*,int);
    //void sqlite3ResetInternalSchema(sqlite3*, int);
    //void sqlite3BeginParse(Parse*,int);
    //void sqlite3CommitInternalChanges(sqlite3);
    //Table *sqlite3ResultSetOfSelect(Parse*,Select);
    //void sqlite3OpenMasterTable(Parse *, int);
    //void sqlite3StartTable(Parse*,Token*,Token*,int,int,int,int);
    //void sqlite3AddColumn(Parse*,Token);
    //void sqlite3AddNotNull(Parse*, int);
    //void sqlite3AddPrimaryKey(Parse*, ExprList*, int, int, int);
    //void sqlite3AddCheckConstraint(Parse*, Expr);
    //void sqlite3AddColumnType(Parse*,Token);
    //void sqlite3AddDefaultValue(Parse*,ExprSpan);
    //void sqlite3AddCollateType(Parse*, Token);
    //void sqlite3EndTable(Parse*,Token*,Token*,Select);
    //int sqlite3ParseUri(const char*,const char*,unsigned int*,
    //                sqlite3_vfs**,char**,char *);

    //Bitvec *sqlite3BitvecCreate(u32);
    //int sqlite3BitvecTest(Bitvec*, u32);
    //int sqlite3BitvecSet(Bitvec*, u32);
    //void sqlite3BitvecClear(Bitvec*, u32, void);
    //void sqlite3BitvecDestroy(Bitvec);
    //u32 sqlite3BitvecSize(Bitvec);
    //int sqlite3BitvecBuiltinTest(int,int);

    //RowSet *sqlite3RowSetInit(sqlite3*, void*, unsigned int);
    //void sqlite3RowSetClear(RowSet);
    //void sqlite3RowSetInsert(RowSet*, i64);
    //int sqlite3RowSetTest(RowSet*, u8 iBatch, i64);
    //int sqlite3RowSetNext(RowSet*, i64);

    //void sqlite3CreateView(Parse*,Token*,Token*,Token*,Select*,int,int);

#if !SQLITE_OMIT_VIEW || !SQLITE_OMIT_VIRTUALTABLE
    //int sqlite3ViewGetColumnNames(Parse*,Table);
#else
    //# define sqlite3ViewGetColumnNames(A,B) 0
    static int sqlite3ViewGetColumnNames( Parse A, Table B )
    {
      return 0;
    }
#endif

    //void sqlite3DropTable(Parse*, SrcList*, int, int);
    //void sqlite3DeleteTable(sqlite3*, Table);
    //#if !SQLITE_OMIT_AUTOINCREMENT
    //  void sqlite3AutoincrementBegin(Parse *pParse);
    //  void sqlite3AutoincrementEnd(Parse *pParse);
    //#else
    //# define sqlite3AutoincrementBegin(X)
    //# define sqlite3AutoincrementEnd(X)
    //#endif
    //void sqlite3Insert(Parse*, SrcList*, ExprList*, Select*, IdList*, int);
    //void *sqlite3ArrayAllocate(sqlite3*,void*,int,int,int*,int*,int);
    //IdList *sqlite3IdListAppend(sqlite3*, IdList*, Token);
    //int sqlite3IdListIndex(IdList*,const char);
    //SrcList *sqlite3SrcListEnlarge(sqlite3*, SrcList*, int, int);
    //SrcList *sqlite3SrcListAppend(sqlite3*, SrcList*, Token*, Token);
    //SrcList *sqlite3SrcListAppendFromTerm(Parse*, SrcList*, Token*, Token*,
    //                                      Token*, Select*, Expr*, IdList);
    //void sqlite3SrcListIndexedBy(Parse *, SrcList *, Token );
    //int sqlite3IndexedByLookup(Parse *, struct SrcList_item );
    //void sqlite3SrcListShiftJoinType(SrcList);
    //void sqlite3SrcListAssignCursors(Parse*, SrcList);
    //void sqlite3IdListDelete(sqlite3*, IdList);
    //void sqlite3SrcListDelete(sqlite3*, SrcList);
    //Index *sqlite3CreateIndex(Parse*,Token*,Token*,SrcList*,ExprList*,int,Token*,
    //                        Token*, int, int);
    //void sqlite3DropIndex(Parse*, SrcList*, int);
    //int sqlite3Select(Parse*, Select*, SelectDest);
    //Select *sqlite3SelectNew(Parse*,ExprList*,SrcList*,Expr*,ExprList*,
    //                         Expr*,ExprList*,int,Expr*,Expr);
    //void sqlite3SelectDelete(sqlite3*, Select);
    //Table *sqlite3SrcListLookup(Parse*, SrcList);
    //int sqlite3IsReadOnly(Parse*, Table*, int);
    //void sqlite3OpenTable(Parse*, int iCur, int iDb, Table*, int);
#if (SQLITE_ENABLE_UPDATE_DELETE_LIMIT) && !(SQLITE_OMIT_SUBQUERY)
//Expr *sqlite3LimitWhere(Parse *, SrcList *, Expr *, ExprList *, Expr *, Expr *, char );
#endif
    //void sqlite3DeleteFrom(Parse*, SrcList*, Expr);
    //void sqlite3Update(Parse*, SrcList*, ExprList*, Expr*, int);
    //WhereInfo *sqlite3WhereBegin(Parse*, SrcList*, Expr*, ExprList**, u16);
    //void sqlite3WhereEnd(WhereInfo);
    //int sqlite3ExprCodeGetColumn(Parse*, Table*, int, int, int);
    //void sqlite3ExprCodeGetColumnOfTable(Vdbe*, Table*, int, int, int);
    //void sqlite3ExprCodeMove(Parse*, int, int, int);
    //void sqlite3ExprCodeCopy(Parse*, int, int, int);
    //void sqlite3ExprCacheStore(Parse*, int, int, int);
    //void sqlite3ExprCachePush(Parse);
    //void sqlite3ExprCachePop(Parse*, int);
    //void sqlite3ExprCacheRemove(Parse*, int, int);
    //void sqlite3ExprCacheClear(Parse);
    //void sqlite3ExprCacheAffinityChange(Parse*, int, int);
    //int sqlite3ExprCode(Parse*, Expr*, int);
    //int sqlite3ExprCodeTemp(Parse*, Expr*, int);
    //int sqlite3ExprCodeTarget(Parse*, Expr*, int);
    //int sqlite3ExprCodeAndCache(Parse*, Expr*, int);
    //void sqlite3ExprCodeConstants(Parse*, Expr);
    //int sqlite3ExprCodeExprList(Parse*, ExprList*, int, int);
    //void sqlite3ExprIfTrue(Parse*, Expr*, int, int);
    //void sqlite3ExprIfFalse(Parse*, Expr*, int, int);
    //Table *sqlite3FindTable(sqlite3*,const char*, const char);
    //Table *sqlite3LocateTable(Parse*,int isView,const char*, const char);
    //Index *sqlite3FindIndex(sqlite3*,const char*, const char);
    //void sqlite3UnlinkAndDeleteTable(sqlite3*,int,const char);
    //void sqlite3UnlinkAndDeleteIndex(sqlite3*,int,const char);
    //void sqlite3Vacuum(Parse);
    //int sqlite3RunVacuum(char**, sqlite3);
    //char *sqlite3NameFromToken(sqlite3*, Token);
    //int sqlite3ExprCompare(Expr*, Expr);
    //int sqlite3ExprListCompare(ExprList*, ExprList);
    //void sqlite3ExprAnalyzeAggregates(NameContext*, Expr);
    //void sqlite3ExprAnalyzeAggList(NameContext*,ExprList);
    //Vdbe *sqlite3GetVdbe(Parse);
    //void sqlite3PrngSaveState(void);
    //void sqlite3PrngRestoreState(void);
    //void sqlite3PrngResetState(void);
    //void sqlite3RollbackAll(sqlite3);
    //void sqlite3CodeVerifySchema(Parse*, int);
    //void sqlite3CodeVerifyNamedSchema(Parse*, string zDb);
    //void sqlite3BeginTransaction(Parse*, int);
    //void sqlite3CommitTransaction(Parse);
    //void sqlite3RollbackTransaction(Parse);
    //void sqlite3Savepoint(Parse*, int, Token);
    //void sqlite3CloseSavepoints(sqlite3 );
    //int sqlite3ExprIsConstant(Expr);
    //int sqlite3ExprIsConstantNotJoin(Expr);
    //int sqlite3ExprIsConstantOrFunction(Expr);
    //int sqlite3ExprIsInteger(Expr*, int);
    //int sqlite3ExprCanBeNull(const Expr);
    //void sqlite3ExprCodeIsNullJump(Vdbe*, const Expr*, int, int);
    //int sqlite3ExprNeedsNoAffinityChange(const Expr*, char);
    //int sqlite3IsRowid(const char);
    //void sqlite3GenerateRowDelete(Parse*, Table*, int, int, int, Trigger *, int);
    //void sqlite3GenerateRowIndexDelete(Parse*, Table*, int, int);
    //int sqlite3GenerateIndexKey(Parse*, Index*, int, int, int);
    //void sqlite3GenerateConstraintChecks(Parse*,Table*,int,int,
    //                                     int*,int,int,int,int,int);
    //void sqlite3CompleteInsertion(Parse*, Table*, int, int, int*, int, int, int);
    //int sqlite3OpenTableAndIndices(Parse*, Table*, int, int);
    //void sqlite3BeginWriteOperation(Parse*, int, int);
    //void sqlite3MultiWrite(Parse);
    //void sqlite3MayAbort(Parse );
    //void sqlite3HaltConstraint(Parse*, int, char*, int);
    //Expr *sqlite3ExprDup(sqlite3*,Expr*,int);
    //ExprList *sqlite3ExprListDup(sqlite3*,ExprList*,int);
    //SrcList *sqlite3SrcListDup(sqlite3*,SrcList*,int);
    //IdList *sqlite3IdListDup(sqlite3*,IdList);
    //Select *sqlite3SelectDup(sqlite3*,Select*,int);
    //void sqlite3FuncDefInsert(FuncDefHash*, FuncDef);
    //FuncDef *sqlite3FindFunction(sqlite3*,const char*,int,int,u8,int);
    //void sqlite3RegisterBuiltinFunctions(sqlite3);
    //void sqlite3RegisterDateTimeFunctions(void);
    //void sqlite3RegisterGlobalFunctions(void);
    //int sqlite3SafetyCheckOk(sqlite3);
    //int sqlite3SafetyCheckSickOrOk(sqlite3);
    //void sqlite3ChangeCookie(Parse*, int);
#if !(SQLITE_OMIT_VIEW) && !(SQLITE_OMIT_TRIGGER)
    //void sqlite3MaterializeView(Parse*, Table*, Expr*, int);
#endif

#if !SQLITE_OMIT_TRIGGER
    //void sqlite3BeginTrigger(Parse*, Token*,Token*,int,int,IdList*,SrcList*,
    //                         Expr*,int, int);
    //void sqlite3FinishTrigger(Parse*, TriggerStep*, Token);
    //void sqlite3DropTrigger(Parse*, SrcList*, int);
    //Trigger *sqlite3TriggersExist(Parse *, Table*, int, ExprList*, int *pMask);
    //Trigger *sqlite3TriggerList(Parse *, Table );
    //  void sqlite3CodeRowTrigger(Parse*, Trigger *, int, ExprList*, int, Table *,
    //                        int, int, int);
    //void sqliteViewTriggers(Parse*, Table*, Expr*, int, ExprList);
    //void sqlite3DeleteTriggerStep(sqlite3*, TriggerStep);
    //TriggerStep *sqlite3TriggerSelectStep(sqlite3*,Select);
    //TriggerStep *sqlite3TriggerInsertStep(sqlite3*,Token*, IdList*,
    //                                      ExprList*,Select*,u8);
    //TriggerStep *sqlite3TriggerUpdateStep(sqlite3*,Token*,ExprList*, Expr*, u8);
    //TriggerStep *sqlite3TriggerDeleteStep(sqlite3*,Token*, Expr);
    //void sqlite3DeleteTrigger(sqlite3*, Trigger);
    //void sqlite3UnlinkAndDeleteTrigger(sqlite3*,int,const char);
    //  u32  sqlite3TriggerColmask(Parse*,Trigger*,ExprList*,int,int,Table*,int);
    //# define sqlite3ParseToplevel(p) ((p)->pToplevel ? (p)->pToplevel : (p))
    static Parse sqlite3ParseToplevel( Parse p )
    {
      return p.pToplevel != null ? p.pToplevel : p;
    }
#else
    static void sqlite3BeginTrigger( Parse A, Token B, Token C, int D, int E, IdList F, SrcList G, Expr H, int I, int J )
    {
    }
    static void sqlite3FinishTrigger( Parse P, TriggerStep TS, Token T )
    {
    }
    static TriggerStep sqlite3TriggerSelectStep( sqlite3 A, Select B )
    {
      return null;
    }
    static TriggerStep sqlite3TriggerInsertStep( sqlite3 A, Token B, IdList C, ExprList D, Select E, u8 F )
    {
      return null;
    }
    static TriggerStep sqlite3TriggerInsertStep( sqlite3 A, Token B, IdList C, int D, Select E, u8 F )
    {
      return null;
    }
    static TriggerStep sqlite3TriggerInsertStep( sqlite3 A, Token B, IdList C, ExprList D, int E, u8 F )
    {
      return null;
    }
    static TriggerStep sqlite3TriggerUpdateStep( sqlite3 A, Token B, ExprList C, Expr D, u8 E )
    {
      return null;
    }
    static TriggerStep sqlite3TriggerDeleteStep( sqlite3 A, Token B, Expr C )
    {
      return null;
    }
    static u32 sqlite3TriggerColmask( Parse A, Trigger B, ExprList C, int D, int E, Table F, int G )
    {
      return 0;
    }

    //# define sqlite3TriggersExist(B,C,D,E,F) 0
    static Trigger sqlite3TriggersExist( Parse B, Table C, int D, ExprList E, ref int F )
    {
      return null;
    }

    //# define sqlite3DeleteTrigger(A,B)
    static void sqlite3DeleteTrigger( sqlite3 A, ref Trigger B )
    {
    }
    static void sqlite3DeleteTriggerStep( sqlite3 A, ref TriggerStep B )
    {
    }

    //# define sqlite3DropTriggerPtr(A,B)
    static void sqlite3DropTriggerPtr( Parse A, Trigger B )
    {
    }
    static void sqlite3DropTrigger( Parse A, SrcList B, int C )
    {
    }

    //# define sqlite3UnlinkAndDeleteTrigger(A,B,C)
    static void sqlite3UnlinkAndDeleteTrigger( sqlite3 A, int B, string C )
    {
    }

    //# define sqlite3CodeRowTrigger(A,B,C,D,E,F,G,H,I)
    static void sqlite3CodeRowTrigger( Parse A, Trigger B, int C, ExprList D, int E, Table F, int G, int H, int I )
    {
    }

    //# define sqlite3CodeRowTriggerDirect(A,B,C,D,E,F)
    static Trigger sqlite3TriggerList( Parse pParse, Table pTab )
    {
      return null;
    } //# define sqlite3TriggerList(X, Y) 0

    //# define sqlite3ParseToplevel(p) p
    static Parse sqlite3ParseToplevel( Parse p )
    {
      return p;
    }

    //# define sqlite3TriggerOldmask(A,B,C,D,E,F) 0
    static u32 sqlite3TriggerOldmask( Parse A, Trigger B, int C, ExprList D, Table E, int F )
    {
      return 0;
    }
#endif

    //int sqlite3JoinType(Parse*, Token*, Token*, Token);
    //void sqlite3CreateForeignKey(Parse*, ExprList*, Token*, ExprList*, int);
    //void sqlite3DeferForeignKey(Parse*, int);
#if !SQLITE_OMIT_AUTHORIZATION
void sqlite3AuthRead(Parse*,Expr*,Schema*,SrcList);
int sqlite3AuthCheck(Parse*,int, const char*, const char*, const char);
void sqlite3AuthContextPush(Parse*, AuthContext*, const char);
void sqlite3AuthContextPop(AuthContext);
int sqlite3AuthReadCol(Parse*, string , string , int);
#else
    //# define sqlite3AuthRead(a,b,c,d)
    static void sqlite3AuthRead( Parse a, Expr b, Schema c, SrcList d )
    {
    }

    //# define sqlite3AuthCheck(a,b,c,d,e)    SQLITE_OK
    static int sqlite3AuthCheck( Parse a, int b, string c, byte[] d, byte[] e )
    {
      return SQLITE_OK;
    }

    //# define sqlite3AuthContextPush(a,b,c)
    static void sqlite3AuthContextPush( Parse a, AuthContext b, string c )
    {
    }

    //# define sqlite3AuthContextPop(a)  ((void)(a))
    static Parse sqlite3AuthContextPop( Parse a )
    {
      return a;
    }
#endif
    //void sqlite3Attach(Parse*, Expr*, Expr*, Expr);
    //void sqlite3Detach(Parse*, Expr);
    //int sqlite3FixInit(DbFixer*, Parse*, int, const char*, const Token);
    //int sqlite3FixSrcList(DbFixer*, SrcList);
    //int sqlite3FixSelect(DbFixer*, Select);
    //int sqlite3FixExpr(DbFixer*, Expr);
    //int sqlite3FixExprList(DbFixer*, ExprList);
    //int sqlite3FixTriggerStep(DbFixer*, TriggerStep);
    //sqlite3AtoF(string z, double*, int, u8)
    //int sqlite3GetInt32(string , int);
    //int sqlite3Atoi(string );
    //int sqlite3Utf16ByteLen(const void pData, int nChar);
    //int sqlite3Utf8CharLen(const char pData, int nByte);
    //u32 sqlite3Utf8Read(const u8*, const u8*);

    /*
    ** Routines to read and write variable-length integers.  These used to
    ** be defined locally, but now we use the varint routines in the util.c
    ** file.  Code should use the MACRO forms below, as the Varint32 versions
    ** are coded to assume the single byte case is already handled (which
    ** the MACRO form does).
    */
    //int sqlite3PutVarint(unsigned char*, u64);
    //int putVarint32(unsigned char*, u32);
    //u8 sqlite3GetVarint(const unsigned char *, u64 );
    //u8 sqlite3GetVarint32(const unsigned char *, u32 );
    //int sqlite3VarintLen(u64 v);

    /*
    ** The header of a record consists of a sequence variable-length integers.
    ** These integers are almost always small and are encoded as a single byte.
    ** The following macros take advantage this fact to provide a fast encode
    ** and decode of the integers in a record header.  It is faster for the common
    ** case where the integer is a single byte.  It is a little slower when the
    ** integer is two or more bytes.  But overall it is faster.
    **
    ** The following expressions are equivalent:
    **
    **     x = sqlite3GetVarint32( A, B );
    **     x = putVarint32( A, B );
    **
    **     x = getVarint32( A, B );
    **     x = putVarint32( A, B );
    **
    */
    //#define getVarint32(A,B)  (u8)((*(A)<(u8)0x80) ? ((B) = (u32)*(A)),1 : sqlite3GetVarint32((A), (u32 )&(B)))
    //#define putVarint32(A,B)  (u8)(((u32)(B)<(u32)0x80) ? (*(A) = (unsigned char)(B)),1 : sqlite3PutVarint32((A), (B)))
    //#define getVarint    sqlite3GetVarint
    //#define putVarint    sqlite3PutVarint


    //string sqlite3IndexAffinityStr(Vdbe *, Index );
    //void sqlite3TableAffinityStr(Vdbe *, Table );
    //char sqlite3CompareAffinity(Expr pExpr, char aff2);
    //int sqlite3IndexAffinityOk(Expr pExpr, char idx_affinity);
    //char sqlite3ExprAffinity(Expr pExpr);
    //int sqlite3Atoi64(const char*, i64*, int, u8);
    //void sqlite3Error(sqlite3*, int, const char*,...);
    //void *sqlite3HexToBlob(sqlite3*, string z, int n);
    //u8 sqlite3HexToInt(int h);
    //int sqlite3TwoPartName(Parse *, Token *, Token *, Token *);
    //string sqlite3ErrStr(int);
    //int sqlite3ReadSchema(Parse pParse);
    //CollSeq *sqlite3FindCollSeq(sqlite3*,u8 enc, const char*,int);
    //CollSeq *sqlite3LocateCollSeq(Parse *pParse, const char*zName);
    //CollSeq *sqlite3ExprCollSeq(Parse pParse, Expr pExpr);
    //Expr *sqlite3ExprSetColl(Expr*, CollSeq);
    //Expr *sqlite3ExprSetCollByToken(Parse *pParse, Expr*, Token);
    //int sqlite3CheckCollSeq(Parse *, CollSeq );
    //int sqlite3CheckObjectName(Parse *, string );
    //void sqlite3VdbeSetChanges(sqlite3 *, int);
    //int sqlite3AddInt64(i64*,i64);
    //int sqlite3SubInt64(i64*,i64);
    //int sqlite3MulInt64(i64*,i64);
    //int sqlite3AbsInt32(int);
    #if SQLITE_ENABLE_8_3_NAMES
    //void sqlite3FileSuffix3(const char*, char);
    #else
    //# define sqlite3FileSuffix3(X,Y)
    static void sqlite3FileSuffix3(string X, string Y){}
    #endif
    //u8 sqlite3GetBoolean(string z);

    //const void *sqlite3ValueText(sqlite3_value*, u8);
    //int sqlite3ValueBytes(sqlite3_value*, u8);
    //void sqlite3ValueSetStr(sqlite3_value*, int, const void *,u8,
    //                      //  void()(void));
    //void sqlite3ValueFree(sqlite3_value);
    //sqlite3_value *sqlite3ValueNew(sqlite3 );
    //char *sqlite3Utf16to8(sqlite3 *, const void*, int, u8);
    //#if SQLITE_ENABLE_STAT2
    //char *sqlite3Utf8to16(sqlite3 *, u8, char *, int, int );
    //#endif
    //int sqlite3ValueFromExpr(sqlite3 *, Expr *, u8, u8, sqlite3_value *);
    //void sqlite3ValueApplyAffinity(sqlite3_value *, u8, u8);
    //#if !SQLITE_AMALGAMATION
    //extern const unsigned char sqlite3OpcodeProperty[];
    //extern const unsigned char sqlite3UpperToLower[];
    //extern const unsigned char sqlite3CtypeMap[];
    //extern const Token sqlite3IntTokens[];
    //extern SQLITE_WSD struct Sqlite3Config sqlite3Config;
    //extern SQLITE_WSD FuncDefHash sqlite3GlobalFunctions;
    //#if !SQLITE_OMIT_WSD
    //extern int sqlite3PendingByte;
    //#endif
    //#endif
    //void sqlite3RootPageMoved(sqlite3*, int, int, int);
    //void sqlite3Reindex(Parse*, Token*, Token);
    //void sqlite3AlterFunctions(void);
    //void sqlite3AlterRenameTable(Parse*, SrcList*, Token);
    //int sqlite3GetToken(const unsigned char *, int );
    //void sqlite3NestedParse(Parse*, const char*, ...);
    //void sqlite3ExpirePreparedStatements(sqlite3);
    //int sqlite3CodeSubselect(Parse *, Expr *, int, int);
    //void sqlite3SelectPrep(Parse*, Select*, NameContext);
    //int sqlite3ResolveExprNames(NameContext*, Expr);
    //void sqlite3ResolveSelectNames(Parse*, Select*, NameContext);
    //int sqlite3ResolveOrderGroupBy(Parse*, Select*, ExprList*, const char);
    //void sqlite3ColumnDefault(Vdbe *, Table *, int, int);
    //void sqlite3AlterFinishAddColumn(Parse *, Token );
    //void sqlite3AlterBeginAddColumn(Parse *, SrcList );
    //CollSeq *sqlite3GetCollSeq(sqlite3*, u8, CollSeq *, const char);
    //char sqlite3AffinityType(const char);
    //void sqlite3Analyze(Parse*, Token*, Token);
    //int sqlite3InvokeBusyHandler(BusyHandler);
    //int sqlite3FindDb(sqlite3*, Token);
    //int sqlite3FindDbName(sqlite3 *, string );
    //int sqlite3AnalysisLoad(sqlite3*,int iDB);
    //void sqlite3DeleteIndexSamples(sqlite3*,Index);
    //void sqlite3DefaultRowEst(Index);
    //void sqlite3RegisterLikeFunctions(sqlite3*, int);
    //int sqlite3IsLikeFunction(sqlite3*,Expr*,int*,char);
    //void sqlite3MinimumFileFormat(Parse*, int, int);
    //void sqlite3SchemaClear(void );
    //Schema *sqlite3SchemaGet(sqlite3 *, Btree );
    //int sqlite3SchemaToIndex(sqlite3 db, Schema );
    //KeyInfo *sqlite3IndexKeyinfo(Parse *, Index );
    //int sqlite3CreateFunc(sqlite3 *, string , int, int, object  *, 
    //  void ()(sqlite3_context*,int,sqlite3_value *),
    //  void ()(sqlite3_context*,int,sqlite3_value *), object  ()(sqlite3_context),
    //  FuncDestructor *pDestructor
    //);
    //int sqlite3ApiExit(sqlite3 db, int);
    //int sqlite3OpenTempDatabase(Parse );

    //void sqlite3StrAccumAppend(StrAccum*,const char*,int);
    //char *sqlite3StrAccumFinish(StrAccum);
    //void sqlite3StrAccumReset(StrAccum);
    //void sqlite3SelectDestInit(SelectDest*,int,int);
    //Expr *sqlite3CreateColumnExpr(sqlite3 *, SrcList *, int, int);

    //void sqlite3BackupRestart(sqlite3_backup );
    //void sqlite3BackupUpdate(sqlite3_backup *, Pgno, const u8 );

    /*
    ** The interface to the LEMON-generated parser
    */
    //void *sqlite3ParserAlloc(void*()(size_t));
    //void sqlite3ParserFree(void*, void()(void));
    //void sqlite3Parser(void*, int, Token, Parse);
#if YYTRACKMAXSTACKDEPTH
int sqlite3ParserStackPeak(void);
#endif

    //void sqlite3AutoLoadExtensions(sqlite3);
#if !SQLITE_OMIT_LOAD_EXTENSION
    //void sqlite3CloseExtensions(sqlite3);
#else
//# define sqlite3CloseExtensions(X)
#endif

#if !SQLITE_OMIT_SHARED_CACHE
//void sqlite3TableLock(Parse *, int, int, u8, string );
#else
    //#define sqlite3TableLock(v,w,x,y,z)
    static void sqlite3TableLock( Parse p, int p1, int p2, u8 p3, byte[] p4 )
    {
    }
    static void sqlite3TableLock( Parse p, int p1, int p2, u8 p3, string p4 )
    {
    }
#endif

#if SQLITE_TEST
    ///int sqlite3Utf8To8(unsigned char);
#endif

#if SQLITE_OMIT_VIRTUALTABLE
    //#  define sqlite3VtabClear(D, Y)
    static void sqlite3VtabClear( sqlite3 db, Table Y )
    {
    }

    //#  define sqlite3VtabSync(X,Y) SQLITE_OK
    static int sqlite3VtabSync( sqlite3 X, ref string Y )
    {
      return SQLITE_OK;
    }

    //#  define sqlite3VtabRollback(X)
    static void sqlite3VtabRollback( sqlite3 X )
    {
    }

    //#  define sqlite3VtabCommit(X)
    static void sqlite3VtabCommit( sqlite3 X )
    {
    }

    //#  define sqlite3VtabLock(X) 
    static void sqlite3VtabLock( VTable X )
    {
    }

    //#  define sqlite3VtabUnlock(X)
    static void sqlite3VtabUnlock( VTable X )
    {
    }

    //#  define sqlite3VtabUnlockList(X)
    static void sqlite3VtabUnlockList( sqlite3 X )
    {
    }
    //#  define sqlite3VtabSavepoint(X, Y, Z) SQLITE_OK
    static int sqlite3VtabSavepoint( sqlite3 X, int Y, int Z )
    {
      return SQLITE_OK;
    }
    //#  define sqlite3VtabInSync(db) ((db)->nVTrans>0 && (db)->aVTrans==0)
    static bool sqlite3VtabInSync( sqlite3 db )
    {
      return false;
    }

    //#  define sqlite3VtabArgExtend(P, T)
    static void sqlite3VtabArgExtend( Parse P, Token T )
    {
    }

    //#  define sqlite3VtabArgInit(P)
    static void sqlite3VtabArgInit( Parse P )
    {
    }

    //#  define sqlite3VtabBeginParse(P, T, T1, T2);
    static void sqlite3VtabBeginParse( Parse P, Token T, Token T1, Token T2 )
    {
    }

    //#  define sqlite3VtabFinishParse(P, T)
    static void sqlite3VtabFinishParse<T>( Parse P, T t )
    {
    }

    static VTable sqlite3GetVTable( sqlite3 db, Table T )
    {
      return null;
    }
#else
//void sqlite3VtabClear(sqlite3 db, Table);
//int sqlite3VtabSync(sqlite3 db, int rc);
//int sqlite3VtabRollback(sqlite3 db);
//int sqlite3VtabCommit(sqlite3 db);
//void sqlite3VtabLock(VTable );
//void sqlite3VtabUnlock(VTable );
//void sqlite3VtabUnlockList(sqlite3);
//int sqlite3VtabSavepoint(sqlite3 *, int, int);
//#  define sqlite3VtabInSync(db) ((db)->nVTrans>0 && (db)->aVTrans==0)
    static bool sqlite3VtabInSync( sqlite3 db )
    {
      return ( db.nVTrans > 0 && db.aVTrans == null );
    }
#endif
    //void sqlite3VtabMakeWritable(Parse*,Table);
    //void sqlite3VtabBeginParse(Parse*, Token*, Token*, Token);
    //void sqlite3VtabFinishParse(Parse*, Token);
    //void sqlite3VtabArgInit(Parse);
    //void sqlite3VtabArgExtend(Parse*, Token);
    //int sqlite3VtabCallCreate(sqlite3*, int, string , char *);
    //int sqlite3VtabCallConnect(Parse*, Table);
    //int sqlite3VtabCallDestroy(sqlite3*, int, string );
    //int sqlite3VtabBegin(sqlite3 *, VTable );
    //FuncDef *sqlite3VtabOverloadFunction(sqlite3 *,FuncDef*, int nArg, Expr);
    //void sqlite3InvalidFunction(sqlite3_context*,int,sqlite3_value*);
    //int sqlite3VdbeParameterIndex(Vdbe*, const char*, int);
    //int sqlite3TransferBindings(sqlite3_stmt *, sqlite3_stmt );
    //int sqlite3Reprepare(Vdbe);
    //void sqlite3ExprListCheckLength(Parse*, ExprList*, const char);
    //CollSeq *sqlite3BinaryCompareCollSeq(Parse *, Expr *, Expr );
    //int sqlite3TempInMemory(const sqlite3);
    //VTable *sqlite3GetVTable(sqlite3*, Table);
    //string sqlite3JournalModename(int);
    //int sqlite3Checkpoint(sqlite3*, int, int, int*, int);
    //int sqlite3WalDefaultHook(void*,sqlite3*,const char*,int);

    /* Declarations for functions in fkey.c. All of these are replaced by
    ** no-op macros if OMIT_FOREIGN_KEY is defined. In this case no foreign
    ** key functionality is available. If OMIT_TRIGGER is defined but
    ** OMIT_FOREIGN_KEY is not, only some of the functions are no-oped. In
    ** this case foreign keys are parsed, but no other functionality is 
    ** provided (enforcement of FK constraints requires the triggers sub-system).
    */
#if !(SQLITE_OMIT_FOREIGN_KEY) && !(SQLITE_OMIT_TRIGGER)
    //void sqlite3FkCheck(Parse*, Table*, int, int);
    //void sqlite3FkDropTable(Parse*, SrcList *, Table);
    //void sqlite3FkActions(Parse*, Table*, ExprList*, int);
    //int sqlite3FkRequired(Parse*, Table*, int*, int);
    //u32 sqlite3FkOldmask(Parse*, Table);
    //FKey *sqlite3FkReferences(vtable );
#else
//#define sqlite3FkActions(a,b,c,d)
static void sqlite3FkActions( Parse a, Table b, ExprList c, int d ) { }

//#define sqlite3FkCheck(a,b,c,d)
static void sqlite3FkCheck( Parse a, Table b, int c, int d ) { }

//#define sqlite3FkDropTable(a,b,c)
static void sqlite3FkDropTable( Parse a, SrcList b, Table c ) { }

//#define sqlite3FkOldmask(a,b)      0
static u32 sqlite3FkOldmask( Parse a, Table b ) { return 0; }

//#define sqlite3FkRequired(a,b,c,d) 0
static int sqlite3FkRequired( Parse a, Table b, int[] c, int d ) { return 0; }
#endif
#if !SQLITE_OMIT_FOREIGN_KEY
    //void sqlite3FkDelete(sqlite3 *, Table);
#else
//#define sqlite3FkDelete(a, b)
static void sqlite3FkDelete(sqlite3 a, Table b) {}                 
#endif

    /*
** Available fault injectors.  Should be numbered beginning with 0.
*/
    const int SQLITE_FAULTINJECTOR_MALLOC = 0;//#define SQLITE_FAULTINJECTOR_MALLOC     0
    const int SQLITE_FAULTINJECTOR_COUNT = 1;//#define SQLITE_FAULTINJECTOR_COUNT      1

    /*
    ** The interface to the code in fault.c used for identifying "benign"
    ** malloc failures. This is only present if SQLITE_OMIT_BUILTIN_TEST
    ** is not defined.
    */
#if !SQLITE_OMIT_BUILTIN_TEST
    //void sqlite3BeginBenignMalloc(void);
    //void sqlite3EndBenignMalloc(void);
#else
//#define sqlite3BeginBenignMalloc()
//#define sqlite3EndBenignMalloc()
#endif

    const int IN_INDEX_ROWID = 1;//#define IN_INDEX_ROWID           1
    const int IN_INDEX_EPH = 2;//#define IN_INDEX_EPH             2
    const int IN_INDEX_INDEX = 3;//#define IN_INDEX_INDEX           3
    //int sqlite3FindInIndex(Parse *, Expr *, int);

#if SQLITE_ENABLE_ATOMIC_WRITE
//  int sqlite3JournalOpen(sqlite3_vfs *, string , sqlite3_file *, int, int);
//  int sqlite3JournalSize(sqlite3_vfs );
//  int sqlite3JournalCreate(sqlite3_file );
#else
    //#define sqlite3JournalSize(pVfs) ((pVfs)->szOsFile)
    static int sqlite3JournalSize( sqlite3_vfs pVfs )
    {
      return pVfs.szOsFile;
    }
#endif

    //void sqlite3MemJournalOpen(sqlite3_file );
    //int sqlite3MemJournalSize(void);
    //int sqlite3IsMemJournal(sqlite3_file );

#if SQLITE_MAX_EXPR_DEPTH//>0
    //  void sqlite3ExprSetHeight(Parse pParse, Expr p);
    //  int sqlite3SelectExprHeight(Select );
    //int sqlite3ExprCheckHeight(Parse*, int);
#else
//#define sqlite3ExprSetHeight(x,y)
//#define sqlite3SelectExprHeight(x) 0
//#define sqlite3ExprCheckHeight(x,y)
#endif

    //u32 sqlite3Get4byte(const u8);
    //void sqlite3sqlite3Put4byte(u8*, u32);

#if SQLITE_ENABLE_UNLOCK_NOTIFY
void sqlite3ConnectionBlocked(sqlite3 *, sqlite3 );
void sqlite3ConnectionUnlocked(sqlite3 db);
void sqlite3ConnectionClosed(sqlite3 db);
#else
    static void sqlite3ConnectionBlocked( sqlite3 x, sqlite3 y )
    {
    } //#define sqlite3ConnectionBlocked(x,y)
    static void sqlite3ConnectionUnlocked( sqlite3 x )
    {
    }                   //#define sqlite3ConnectionUnlocked(x)
    static void sqlite3ConnectionClosed( sqlite3 x )
    {
    }                     //#define sqlite3ConnectionClosed(x)
#endif

#if SQLITE_DEBUG
    //  void sqlite3ParserTrace(FILE*, char );
#endif

    /*
** If the SQLITE_ENABLE IOTRACE exists then the global variable
** sqlite3IoTrace is a pointer to a printf-like routine used to
** print I/O tracing messages.
*/
#if SQLITE_ENABLE_IOTRACE
static bool SQLite3IoTrace = false;
//#define IOTRACE(A)  if( sqlite3IoTrace ){ sqlite3IoTrace A; }
static void IOTRACE( string X, params object[] ap ) { if ( SQLite3IoTrace ) { printf( X, ap ); } }

//  void sqlite3VdbeIOTraceSql(Vdbe);
//SQLITE_EXTERN void (*sqlite3IoTrace)(const char*,...);
#else
    //#define IOTRACE(A)
    static void IOTRACE( string F, params object[] ap )
    {
    }
    //#define sqlite3VdbeIOTraceSql(X)
    static void sqlite3VdbeIOTraceSql( Vdbe X )
    {
    }
#endif

    /*
** These routines are available for the mem2.c debugging memory allocator
** only.  They are used to verify that different "types" of memory
** allocations are properly tracked by the system.
**
** sqlite3MemdebugSetType() sets the "type" of an allocation to one of
** the MEMTYPE_* macros defined below.  The type must be a bitmask with
** a single bit set.
**
** sqlite3MemdebugHasType() returns true if any of the bits in its second
** argument match the type set by the previous sqlite3MemdebugSetType().
** sqlite3MemdebugHasType() is intended for use inside Debug.Assert() statements.
**
** sqlite3MemdebugNoType() returns true if none of the bits in its second
** argument match the type set by the previous sqlite3MemdebugSetType().
**
** Perhaps the most important point is the difference between MEMTYPE_HEAP
** and MEMTYPE_LOOKASIDE.  If an allocation is MEMTYPE_LOOKASIDE, that means
** it might have been allocated by lookaside, except the allocation was
** too large or lookaside was already full.  It is important to verify
** that allocations that might have been satisfied by lookaside are not
** passed back to non-lookaside free() routines.  Asserts such as the
** example above are placed on the non-lookaside free() routines to verify
** this constraint. 
**
** All of this is no-op for a production build.  It only comes into
** play when the SQLITE_MEMDEBUG compile-time option is used.
*/
#if SQLITE_MEMDEBUG
//  void sqlite3MemdebugSetType(void*,u8);
//  int sqlite3MemdebugHasType(void*,u8);
//  int sqlite3MemdebugNoType(void*,u8);
#else
    //# define sqlite3MemdebugSetType(X,Y)  /* no-op */
    static void sqlite3MemdebugSetType<T>( T X, int Y )
    {
    }
    //# define sqlite3MemdebugHasType(X,Y)  1
    static bool sqlite3MemdebugHasType<T>( T X, int Y )
    {
      return true;
    }
    //# define sqlite3MemdebugNoType(X,Y)   1
    static bool sqlite3MemdebugNoType<T>( T X, int Y )
    {
      return true;
    }
#endif
    //#define MEMTYPE_HEAP       0x01  /* General heap allocations */
    //#define MEMTYPE_LOOKASIDE  0x02  /* Might have been lookaside memory */
    //#define MEMTYPE_SCRATCH    0x04  /* Scratch allocations */
    //#define MEMTYPE_PCACHE     0x08  /* Page cache allocations */
    //#define MEMTYPE_DB         0x10  /* Uses sqlite3DbMalloc, not sqlite_malloc */
    public const int MEMTYPE_HEAP = 0x01;
    public const int MEMTYPE_LOOKASIDE = 0x02;
    public const int MEMTYPE_SCRATCH = 0x04;
    public const int MEMTYPE_PCACHE = 0x08;
    public const int MEMTYPE_DB = 0x10;

    //#endif //* _SQLITEINT_H_ */

  }
}
