namespace Community.CsharpSqlite
{
    using System;

    /*
    ** Each token coming out of the lexer is an instance of
    ** this structure.  Tokens are also used as part of an expression.
    **
    ** Note if Token.z==0 then Token.dyn and Token.n are undefined and
    ** may contain random values.  Do not make any assumptions about Token.dyn
    ** and Token.n when Token.z==0.
    */
    public class Token
    {
        #if DEBUG_CLASS_TOKEN || DEBUG_CLASS_ALL
        public string _z; /* Text of the token.  Not NULL-terminated! */
        public bool dyn;//  : 1;      /* True for malloced memory, false for static */
        public Int32 _n;//  : 31;     /* Number of characters in this token */

        public string z
        {
        get { return _z; }
        set { _z = value; }
        }

        public Int32 n
        {
        get { return _n; }
        set { _n = value; }
        }
        #else
        public string z; /* Text of the token.  Not NULL-terminated! */
        public Int32 n;  /* Number of characters in this token */
        #endif
        public Token()
        {
            this.z = null;
            this.n = 0;
        }
        public Token( string z, Int32 n )
        {
            this.z = z;
            this.n = n;
        }
        public Token Copy()
        {
            if ( this == null )
                return null;
            else
            {
                Token cp = (Token)MemberwiseClone();
                if ( z == null || z.Length == 0 )
                    cp.n = 0;
                else
                    if ( n > z.Length )
                        cp.n = z.Length;
                return cp;
            }
        }
    }
}

