namespace Community.CsharpSqlite
{
    using System;
    using u32 = System.UInt32;
    using BITVEC_TELEM = System.Byte;
    
    /*
** A bitmap is an instance of the following structure.
**
** This bitmap records the existence of zero or more bits
** with values between 1 and iSize, inclusive.
**
** There are three possible representations of the bitmap.
** If iSize<=BITVEC_NBIT, then Bitvec.u.aBitmap[] is a straight
** bitmap.  The least significant bit is bit 1.
**
** If iSize>BITVEC_NBIT and iDivisor==0 then Bitvec.u.aHash[] is
** a hash table that will hold up to BITVEC_MXHASH distinct values.
**
** Otherwise, the value i is redirected into one of BITVEC_NPTR
** sub-bitmaps pointed to by Bitvec.u.apSub[].  Each subbitmap
** handles up to iDivisor separate values of i.  apSub[0] holds
** values between 1 and iDivisor.  apSub[1] holds values between
** iDivisor+1 and 2*iDivisor.  apSub[N] holds values between
** N*iDivisor+1 and (N+1)*iDivisor.  Each subbitmap is normalized
** to hold deal with values between 1 and iDivisor.
*/
    public class _u
    {
        public BITVEC_TELEM[] aBitmap = new byte[BITVEC_NELEM];   /* Bitmap representation */
        public u32[] aHash = new u32[BITVEC_NINT];        /* Hash table representation */
        public Bitvec[] apSub = new Bitvec[BITVEC_NPTR];  /* Recursive representation */
    }
    public class Bitvec
    {
        public u32 iSize;     /* Maximum bit index.  Max iSize is 4,294,967,296. */
        public u32 nSet;      /* Number of bits that are set - only valid for aHash
  ** element.  Max is BITVEC_NINT.  For BITVEC_SZ of 512,
  ** this would be 125. */
        public u32 iDivisor;  /* Number of bits handled by each apSub[] entry. */
        /* Should >=0 for apSub element. */
        /* Max iDivisor is max(u32) / BITVEC_NPTR + 1.  */
        /* For a BITVEC_SZ of 512, this would be 34,359,739. */
        public _u u = new _u();

        public static implicit operator bool( Bitvec b )
        {
            return ( b != null );
        }
    };
}

