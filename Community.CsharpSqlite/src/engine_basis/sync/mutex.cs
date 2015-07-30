namespace Community.CsharpSqlite
{
    using System;
    using DWORD = System.Int32;
    
    public class sqlite3_mutex
    {
    }
    
#if FALSE
    /*
    ** If this is a no-op implementation, implement everything as macros.
    */


    public partial class Globals
    {
        static sqlite3_mutex mutex = null;
        
        /*
    ** The sqlite3_mutex_alloc() routine allocates a new
    ** mutex and returns a pointer to it.  If it returns NULL
    ** that means that a mutex could not be allocated.  SQLite
    ** will unwind its stack and return an error.  The argument
    ** to sqlite3_mutex_alloc() is one of these integer constants:
    **
    ** <ul>
    ** <li>  SQLITE_MUTEX_FAST
    ** <li>  SQLITE_MUTEX_RECURSIVE
    ** <li>  SQLITE_MUTEX_STATIC_MASTER
    ** <li>  SQLITE_MUTEX_STATIC_MEM
    ** <li>  SQLITE_MUTEX_STATIC_MEM2
    ** <li>  SQLITE_MUTEX_STATIC_PRNG
    ** <li>  SQLITE_MUTEX_STATIC_LRU
    ** <li>  SQLITE_MUTEX_STATIC_LRU2
    ** </ul>
    **
    ** The first two constants cause sqlite3_mutex_alloc() to create
    ** a new mutex.  The new mutex is recursive when SQLITE_MUTEX_RECURSIVE
    ** is used but not necessarily so when SQLITE_MUTEX_FAST is used.
    ** The mutex implementation does not need to make a distinction
    ** between SQLITE_MUTEX_RECURSIVE and SQLITE_MUTEX_FAST if it does
    ** not want to.  But SQLite will only request a recursive mutex in
    ** cases where it really needs one.  If a faster non-recursive mutex
    ** implementation is available on the host platform, the mutex subsystem
    ** might return such a mutex in response to SQLITE_MUTEX_FAST.
    **
    ** The other allowed parameters to sqlite3_mutex_alloc() each return
    ** a pointer to a static preexisting mutex.  Six static mutexes are
    ** used by the current version of SQLite.  Future versions of SQLite
    ** may add additional static mutexes.  Static mutexes are for internal
    ** use by SQLite only.  Applications that use SQLite mutexes should
    ** use only the dynamic mutexes returned by SQLITE_MUTEX_FAST or
    ** SQLITE_MUTEX_RECURSIVE.
    **
    ** Note that if one of the dynamic mutex parameters (SQLITE_MUTEX_FAST
    ** or SQLITE_MUTEX_RECURSIVE) is used then sqlite3_mutex_alloc()
    ** returns a different mutex on every call.  But for the static
    ** mutex types, the same mutex is returned on every call that has
    ** the same type number.
    */
        static sqlite3_mutex sqlite3_mutex_alloc(int iType)
        {
            return new sqlite3_mutex();
        }
//#define sqlite3_mutex_alloc(X)    ((sqlite3_mutex*)8)
        static void sqlite3_mutex_free(sqlite3_mutex m)
        {
        }
        //#define sqlite3_mutex_free(X)
        static void sqlite3_mutex_enter(sqlite3_mutex m)
        {
        }
        //#define sqlite3_mutex_enter(X)
        static int sqlite3_mutex_try(int iType)
        {
            return SQLITE_OK;
        }
        //#define sqlite3_mutex_try(X)      SQLITE_OK
        static void sqlite3_mutex_leave(sqlite3_mutex m)
        {
        }
        //#define sqlite3_mutex_leave(X)
        static bool sqlite3_mutex_held(sqlite3_mutex m)
        {
            return true;
        }
        static bool sqlite3_mutex_notheld(sqlite3_mutex m)
        {
            return true;
        }
        static int sqlite3MutexInit()
        {
            return SQLITE_OK;
        }
        static void sqlite3MutexEnd()
        {
        }
    }
#endif
}

