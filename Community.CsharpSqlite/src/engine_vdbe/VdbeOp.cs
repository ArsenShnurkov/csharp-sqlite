namespace Community.CsharpSqlite
{
    using System;
    using u8 = System.Byte;
    using i64 = System.Int64;

    /*
    ** A single instruction of the virtual machine has an opcode
    ** and as many as three operands.  The instruction is recorded
    ** as an instance of the following structure:
    */
    public class union_p4
    {             /* fourth parameter */
        public int i;                /* Integer value if p4type==P4_INT32 */
        public object p;             /* Generic pointer */
        //public string z;           /* Pointer to data for string (char array) types */
        public string z;             // In C# string is unicode, so use byte[] instead
        public i64 pI64;             /* Used when p4type is P4_INT64 */
        public double pReal;         /* Used when p4type is P4_REAL */
        public FuncDef pFunc;        /* Used when p4type is P4_FUNCDEF */
        public VdbeFunc pVdbeFunc;   /* Used when p4type is P4_VDBEFUNC */
        public CollSeq pColl;        /* Used when p4type is P4_COLLSEQ */
        public Mem pMem;             /* Used when p4type is P4_MEM */
        public VTable pVtab;         /* Used when p4type is P4_VTAB */
        public KeyInfo pKeyInfo;     /* Used when p4type is P4_KEYINFO */
        public int[] ai;             /* Used when p4type is P4_INTARRAY */
        public SubProgram pProgram;  /* Used when p4type is P4_SUBPROGRAM */
        public dxDel pFuncDel;       /* Used when p4type is P4_FUNCDEL */
    } ;
    public class VdbeOp
    {
        public u8 opcode;           /* What operation to perform */
        public int p4type;          /* One of the P4_xxx constants for p4 */
        public u8 opflags;          /* Mask of the OPFLG_* flags in opcodes.h */
        public u8 p5;               /* Fifth parameter is an unsigned character */
        #if DEBUG_CLASS_VDBEOP || DEBUG_CLASS_ALL
        public int _p1;              /* First operand */
        public int p1
        {
        get { return _p1; }
        set { _p1 = value; }
        }

        public int _p2;              /* Second parameter (often the jump destination) */
        public int p2
        {
        get { return _p2; }
        set { _p2 = value; }
        }

        public int _p3;              /* The third parameter */
        public int p3
        {
        get { return _p3; }
        set { _p3 = value; }
        }
        #else
        public int p1;              /* First operand */
        public int p2;              /* Second parameter (often the jump destination) */
        public int p3;              /* The third parameter */
        #endif
        public union_p4 p4 = new union_p4();
        #if SQLITE_DEBUG || DEBUG
        public string zComment;     /* Comment to improve readability */
        #endif
        #if VDBE_PROFILE
        public int cnt;             /* Number of times this instruction was executed */
        public u64 cycles;         /* Total time spend executing this instruction */
        #endif
    }
}

