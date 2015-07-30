
namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using i64 = System.Int64;

    /*
    ** Internally, the vdbe manipulates nearly all SQL values as Mem
    ** structures. Each Mem struct may cache multiple representations (string,
    ** integer etc.) of the same value.
    */
    public class Mem
    {
        public sqlite3 db;            /* The associated database connection */
        public string z;              /* String value */
        public double r;              /* Real value */
        public struct union_ip
        {
            #if DEBUG_CLASS_MEM || DEBUG_CLASS_ALL
            public i64 _i;              /* First operand */
            public i64 i
            {
            get { return _i; }
            set { _i = value; }
            }
            #else
            public i64 i;               /* Integer value used when MEM_Int is set in flags */
            #endif
            public int nZero;           /* Used when bit MEM_Zero is set in flags */
            public FuncDef pDef;        /* Used only when flags==MEM_Agg */
            public RowSet pRowSet;      /* Used only when flags==MEM_RowSet */
            public VdbeFrame pFrame;    /* Used when flags==MEM_Frame */
        };
        public union_ip u;
        public byte[] zBLOB;          /* BLOB value */
        public int n;                 /* Number of characters in string value, excluding '\0' */
        #if DEBUG_CLASS_MEM || DEBUG_CLASS_ALL
        public u16 _flags;              /* First operand */
        public u16 flags
        {
        get { return _flags; }
        set { _flags = value; }
        }
        #else
        public u16 flags;             /* Some combination of MEM_Null, MEM_Str, MEM_Dyn, etc. */
        #endif
        public u8 type;               /* One of SQLITE_NULL, SQLITE_TEXT, SQLITE_INTEGER, etc */
        public u8 enc;                /* SQLITE_UTF8, SQLITE_UTF16BE, SQLITE_UTF16LE */
        #if SQLITE_DEBUG
        public Mem pScopyFrom;        /* This Mem is a shallow copy of pScopyFrom */
        public object pFiller;        /* So that sizeof(Mem) is a multiple of 8 */
        #endif
        public dxDel xDel;            /* If not null, call this function to delete Mem.z */
        // Not used under c#
        //public string zMalloc;      /* Dynamic buffer allocated by sqlite3Malloc() */
        public Mem _Mem;              /* Used when C# overload Z as MEM space */
        public SumCtx _SumCtx;        /* Used when C# overload Z as Sum context */
        public SubProgram[] _SubProgram;/* Used when C# overload Z as SubProgram*/
        public StrAccum _StrAccum;    /* Used when C# overload Z as STR context */
        public object _MD5Context;    /* Used when C# overload Z as MD5 context */

        public Mem()
        {
        }

        public Mem( sqlite3 db, string z, double r, int i, int n, u16 flags, u8 type, u8 enc
            #if SQLITE_DEBUG
            , Mem pScopyFrom, object pFiller  /* pScopyFrom, pFiller */
            #endif
        )
        {
            this.db = db;
            this.z = z;
            this.r = r;
            this.u.i = i;
            this.n = n;
            this.flags = flags;
            #if SQLITE_DEBUG
            this.pScopyFrom = pScopyFrom;
            this.pFiller = pFiller;
            #endif
            this.type = type;
            this.enc = enc;
        }

        public void CopyTo( ref Mem ct )
        {
            if ( ct == null )
                ct = new Mem();
            ct.u = u;
            ct.r = r;
            ct.db = db;
            ct.z = z;
            if ( zBLOB == null )
                ct.zBLOB = null;
            else
            {
                ct.zBLOB = sqlite3Malloc( zBLOB.Length );
                Buffer.BlockCopy( zBLOB, 0, ct.zBLOB, 0, zBLOB.Length );
            }
            ct.n = n;
            ct.flags = flags;
            ct.type = type;
            ct.enc = enc;
            ct.xDel = xDel;
        }

    };
    }

