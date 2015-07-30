namespace Community.CsharpSqlite
{
    using System;
    using i64 = System.Int64;

    /*
** An instance of the following structure holds the context of a
** sum() or avg() aggregate computation.
*/
    //typedef struct SumCtx SumCtx;
    public class SumCtx
    {
        public double rSum;      /* Floating point sum */
        public i64 iSum;         /* Integer sum */
        public i64 cnt;          /* Number of elements summed */
        public int overflow;     /* True if integer overflow seen */
        public bool approx;      /* True if non-integer value was input to the sum */
        public Mem _M;
        public Mem Context
        {
            get
            {
                return _M;
            }
            set
            {
                _M = value;
                if ( _M == null || _M.z == null )
                    iSum = 0;
                else
                    iSum = Convert.ToInt64( _M.z );
            }
        }
    }
}

