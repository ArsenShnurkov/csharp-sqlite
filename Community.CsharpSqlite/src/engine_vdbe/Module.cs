using System;

namespace Community.CsharpSqlite
{
    /*
    ** Each SQLite module (virtual table definition) is defined by an
    ** instance of the following structure, stored in the sqlite3.aModule
    ** hash table.
    */
    public class Module
    {
        public sqlite3_module pModule;         /* Callback pointers */
        public string zName;                   /* Name passed to create_module() */
        public object pAux;                    /* pAux passed to create_module() */
        public smdxDestroy xDestroy;//)(void );/* Module destructor function */
    }
}

