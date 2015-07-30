namespace Community.CsharpSqlite
{
    using System;


    public class AuxData
    {
        public object pAux;
        /* Aux data for the i-th argument */
        //(void );      /* Destructor for the aux data */
    };

    /* A VdbeFunc is just a FuncDef (defined in sqliteInt.h) that contains
** additional information about auxiliary information bound to arguments
** of the function.  This is used to implement the sqlite3_get_auxdata()
** and sqlite3_set_auxdata() APIs.  The "auxdata" is some auxiliary data
** that can be associated with a constant argument to a function.  This
** allows functions such as "regexp" to compile their constant regular
** expression argument once and reused the compiled code for multiple
** invocations.
*/
    public class VdbeFunc : FuncDef
    {
        public FuncDef pFunc;
        /* The definition of the function */
        public int nAux;
        /* Number of entries allocated for apAux[] */
        public AuxData[] apAux = new AuxData[2];
        /* One slot for each function argument */
    }
}
