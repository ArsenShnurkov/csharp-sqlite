using System.IO;

namespace Community.CsharpSqlite
{
    using System;
    using DWORD = System.Int32;
    
    //typedef struct sqlite3_file sqlite3_file;
    public partial class sqlite3_file
    {
        public sqlite3_vfs pVfs;
        /* The VFS used to open this file */
        #if SQLITE_WINRT
        public IRandomAccessStream fs;














        #else
        public FileStream fs;
        /* Filestream access to this file*/
        #endif
        // public HANDLE h;            /* Handle for accessing the file */
        public int locktype;
        /* Type of lock currently held on this file */
        public int sharedLockByte;
        /* Randomly chosen byte used as a shared lock */
        public DWORD lastErrno;
        /* The Windows errno from the last I/O error */
        public DWORD sectorSize;
        /* Sector size of the device file is on */
        #if !SQLITE_OMIT_WAL
        public winShm pShm;            /* Instance of shared memory on this file */














        #else
        public object pShm;
        /* DUMMY Instance of shared memory on this file */
        #endif
        public string zPath;
        /* Full pathname of this file */
        public int szChunk;
        /* Chunk size configured by FCNTL_CHUNK_SIZE */
        #if SQLITE_OS_WINCE
        Wstring zDeleteOnClose;  /* Name of file to delete when closing */
        HANDLE hMutex;          /* Mutex used to control access to shared lock */
        HANDLE hShared;         /* Shared memory segment used for locking */
        winceLock local;        /* Locks obtained by this instance of sqlite3_file */
        winceLock *shared;      /* Global shared lock memory for the file  */
        #endif
        public void Clear()
        {
            pMethods = null;
            fs = null;
            locktype = 0;
            sharedLockByte = 0;
            lastErrno = 0;
            sectorSize = 0;
        }
    };
    
}

