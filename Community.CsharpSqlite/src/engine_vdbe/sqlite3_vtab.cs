namespace Community.CsharpSqlite
{
    using System;
    
    /*
    ** CAPI3REF: Virtual Table Instance Object
    ** KEYWORDS: sqlite3_vtab
    **
    ** Every [virtual table module] implementation uses a subclass
    ** of this object to describe a particular instance
    ** of the [virtual table].  Each subclass will
    ** be tailored to the specific needs of the module implementation.
    ** The purpose of this superclass is to define certain fields that are
    ** common to all module implementations.
    **
    ** ^Virtual tables methods can set an error message by assigning a
    ** string obtained from [sqlite3_mprintf()] to zErrMsg.  The method should
    ** take care that any prior string is freed by a call to [sqlite3_free()]
    ** prior to assigning a new string to zErrMsg.  ^After the error message
    ** is delivered up to the client application, the string will be automatically
    ** freed by sqlite3_free() and the zErrMsg field will be zeroed.
    */
    //struct sqlite3_vtab {
    //  const sqlite3_module *pModule;  /* The module for this virtual table */
    //  int nRef;                       /* NO LONGER USED */
    //  string zErrMsg;                  /* Error message from sqlite3_mprintf() */
    //  /* Virtual table implementations will typically add additional fields */
    //};
    public class sqlite3_vtab
    {
        public sqlite3_module pModule;       /* The module for this virtual table */
        public int nRef;                     /* Used internally */
        public string zErrMsg;               /* Error message from sqlite3_mprintf() */
        /* Virtual table implementations will typically add additional fields */
    }
}

