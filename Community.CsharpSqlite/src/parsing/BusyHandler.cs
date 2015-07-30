using System;

namespace Community.CsharpSqlite
{
    public delegate int dxBusy( object pBtShared, int iValue );
    
    /*
    ** An instance of the following structure is used to store the busy-handler
    ** callback for a given sqlite handle.
    **
    ** The sqlite.busyHandler member of the sqlite struct contains the busy
    ** callback for the database handle. Each pager opened via the sqlite
    ** handle is passed a pointer to sqlite.busyHandler. The busy-handler
    ** callback is currently invoked only from within pager.c.
    */
    //typedef struct BusyHandler BusyHandler;
    public class BusyHandler
    {
        public dxBusy xFunc;//)(void *,int);  /* The busy callback */
        public object pArg;                   /* First arg to busy callback */
        public int nBusy;                     /* Incremented with each busy call */
    };
}
