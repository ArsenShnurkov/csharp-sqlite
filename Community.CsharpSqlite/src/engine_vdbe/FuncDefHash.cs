namespace Community.CsharpSqlite
{
    using System;
    /*
    ** A hash table for function definitions.
    **
    ** Hash each FuncDef structure into one of the FuncDefHash.a[] slots.
    ** Collisions are on the FuncDef.pHash chain.
    */
    public class FuncDefHash
    {
        public FuncDef[] a = new FuncDef[23];       /* Hash table for functions */
    }
}

