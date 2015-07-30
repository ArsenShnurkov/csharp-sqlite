using System;

namespace Community.CsharpSqlite
{
    /*
    ** Before a virtual table xCreate() or xConnect() method is invoked, the
    ** sqlite3.pVtabCtx member variable is set to point to an instance of
    ** this struct allocated on the stack. It is used by the implementation of 
    ** the sqlite3_declare_vtab() and sqlite3_vtab_config() APIs, both of which
    ** are invoked only from within xCreate and xConnect methods.
    */
    public class VtabCtx
    {
        public Table pTab;
        public VTable pVTable;
    };
    
}

