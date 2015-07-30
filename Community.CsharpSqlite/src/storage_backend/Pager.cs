namespace Community.CsharpSqlite
{
    using System;
    
    using i16 = System.Int16;
    using i32 = System.Int32;
    using i64 = System.Int64;
    using u8 = System.Byte;
    using u16 = System.UInt16;
    using u32 = System.UInt32;
    using u64 = System.UInt64;

    using Pgno = System.UInt32;
    
    /*
    ** A open page cache is an instance of struct Pager. A description of
    ** some of the more important member variables follows:
    **
    ** eState
    **
    **   The current 'state' of the pager object. See the comment and state
    **   diagram above for a description of the pager state.
    **
    ** eLock
    **
    **   For a real on-disk database, the current lock held on the database file -
    **   NO_LOCK, SHARED_LOCK, RESERVED_LOCK or EXCLUSIVE_LOCK.
    **
    **   For a temporary or in-memory database (neither of which require any
    **   locks), this variable is always set to EXCLUSIVE_LOCK. Since such
    **   databases always have Pager.exclusiveMode==1, this tricks the pager
    **   logic into thinking that it already has all the locks it will ever
    **   need (and no reason to release them).
    **
    **   In some (obscure) circumstances, this variable may also be set to
    **   UNKNOWN_LOCK. See the comment above the #define of UNKNOWN_LOCK for
    **   details.
    **
    ** changeCountDone
    **
    **   This boolean variable is used to make sure that the change-counter 
    **   (the 4-byte header field at byte offset 24 of the database file) is 
    **   not updated more often than necessary. 
    **
    **   It is set to true when the change-counter field is updated, which 
    **   can only happen if an exclusive lock is held on the database file.
    **   It is cleared (set to false) whenever an exclusive lock is 
    **   relinquished on the database file. Each time a transaction is committed,
    **   The changeCountDone flag is inspected. If it is true, the work of
    **   updating the change-counter is omitted for the current transaction.
    **
    **   This mechanism means that when running in exclusive mode, a connection 
    **   need only update the change-counter once, for the first transaction
    **   committed.
    **
    ** setMaster
    **
    **   When PagerCommitPhaseOne() is called to commit a transaction, it may
    **   (or may not) specify a master-journal name to be written into the 
    **   journal file before it is synced to disk.
    **
    **   Whether or not a journal file contains a master-journal pointer affects 
    **   the way in which the journal file is finalized after the transaction is 
    **   committed or rolled back when running in "journal_mode=PERSIST" mode.
    **   If a journal file does not contain a master-journal pointer, it is
    **   finalized by overwriting the first journal header with zeroes. If
    **   it does contain a master-journal pointer the journal file is finalized 
    **   by truncating it to zero bytes, just as if the connection were 
    **   running in "journal_mode=truncate" mode.
    **
    **   Journal files that contain master journal pointers cannot be finalized
    **   simply by overwriting the first journal-header with zeroes, as the
    **   master journal pointer could interfere with hot-journal rollback of any
    **   subsequently interrupted transaction that reuses the journal file.
    **
    **   The flag is cleared as soon as the journal file is finalized (either
    **   by PagerCommitPhaseTwo or PagerRollback). If an IO error prevents the
    **   journal file from being successfully finalized, the setMaster flag
    **   is cleared anyway (and the pager will move to ERROR state).
    **
    ** doNotSpill, doNotSyncSpill
    **
    **   These two boolean variables control the behaviour of cache-spills
    **   (calls made by the pcache module to the pagerStress() routine to
    **   write cached data to the file-system in order to free up memory).
    **
    **   When doNotSpill is non-zero, writing to the database from pagerStress()
    **   is disabled altogether. This is done in a very obscure case that
    **   comes up during savepoint rollback that requires the pcache module
    **   to allocate a new page to prevent the journal file from being written
    **   while it is being traversed by code in pager_playback().
    ** 
    **   If doNotSyncSpill is non-zero, writing to the database from pagerStress()
    **   is permitted, but syncing the journal file is not. This flag is set
    **   by sqlite3PagerWrite() when the file-system sector-size is larger than
    **   the database page-size in order to prevent a journal sync from happening 
    **   in between the journalling of two pages on the same sector. 
    **
    ** subjInMemory
    **
    **   This is a boolean variable. If true, then any required sub-journal
    **   is opened as an in-memory journal file. If false, then in-memory
    **   sub-journals are only used for in-memory pager files.
    **
    **   This variable is updated by the upper layer each time a new 
    **   write-transaction is opened.
    **
    ** dbSize, dbOrigSize, dbFileSize
    **
    **   Variable dbSize is set to the number of pages in the database file.
    **   It is valid in PAGER_READER and higher states (all states except for
    **   OPEN and ERROR). 
    **
    **   dbSize is set based on the size of the database file, which may be 
    **   larger than the size of the database (the value stored at offset
    **   28 of the database header by the btree). If the size of the file
    **   is not an integer multiple of the page-size, the value stored in
    **   dbSize is rounded down (i.e. a 5KB file with 2K page-size has dbSize==2).
    **   Except, any file that is greater than 0 bytes in size is considered
    **   to have at least one page. (i.e. a 1KB file with 2K page-size leads
    **   to dbSize==1).
    **
    **   During a write-transaction, if pages with page-numbers greater than
    **   dbSize are modified in the cache, dbSize is updated accordingly.
    **   Similarly, if the database is truncated using PagerTruncateImage(), 
    **   dbSize is updated.
    **
    **   Variables dbOrigSize and dbFileSize are valid in states 
    **   PAGER_WRITER_LOCKED and higher. dbOrigSize is a copy of the dbSize
    **   variable at the start of the transaction. It is used during rollback,
    **   and to determine whether or not pages need to be journalled before
    **   being modified.
    **
    **   Throughout a write-transaction, dbFileSize contains the size of
    **   the file on disk in pages. It is set to a copy of dbSize when the
    **   write-transaction is first opened, and updated when VFS calls are made
    **   to write or truncate the database file on disk. 
    **
    **   The only reason the dbFileSize variable is required is to suppress 
    **   unnecessary calls to xTruncate() after committing a transaction. If, 
    **   when a transaction is committed, the dbFileSize variable indicates 
    **   that the database file is larger than the database image (Pager.dbSize), 
    **   pager_truncate() is called. The pager_truncate() call uses xFilesize()
    **   to measure the database file on disk, and then truncates it if required.
    **   dbFileSize is not used when rolling back a transaction. In this case
    **   pager_truncate() is called unconditionally (which means there may be
    **   a call to xFilesize() that is not strictly required). In either case,
    **   pager_truncate() may cause the file to become smaller or larger.
    **
    ** dbHintSize
    **
    **   The dbHintSize variable is used to limit the number of calls made to
    **   the VFS xFileControl(FCNTL_SIZE_HINT) method. 
    **
    **   dbHintSize is set to a copy of the dbSize variable when a
    **   write-transaction is opened (at the same time as dbFileSize and
    **   dbOrigSize). If the xFileControl(FCNTL_SIZE_HINT) method is called,
    **   dbHintSize is increased to the number of pages that correspond to the
    **   size-hint passed to the method call. See pager_write_pagelist() for 
    **   details.
    **
    ** errCode
    **
    **   The Pager.errCode variable is only ever used in PAGER_ERROR state. It
    **   is set to zero in all other states. In PAGER_ERROR state, Pager.errCode 
    **   is always set to SQLITE_FULL, SQLITE_IOERR or one of the SQLITE_IOERR_XXX 
    **   sub-codes.
    */
    public class Pager
    {
        public sqlite3_vfs pVfs;           /* OS functions to use for IO */
        public bool exclusiveMode;         /* Boolean. True if locking_mode==EXCLUSIVE */
        public u8 journalMode;             /* One of the PAGER_JOURNALMODE_* values */
        public u8 useJournal;              /* Use a rollback journal on this file */
        public u8 noReadlock;              /* Do not bother to obtain readlocks */
        public bool noSync;                /* Do not sync the journal if true */
        public bool fullSync;              /* Do extra syncs of the journal for robustness */
        public u8 ckptSyncFlags;           /* SYNC_NORMAL or SYNC_FULL for checkpoint */
        public u8 syncFlags;               /* SYNC_NORMAL or SYNC_FULL otherwise */
        public bool tempFile;              /* zFilename is a temporary file */
        public bool readOnly;              /* True for a read-only database */
        public bool alwaysRollback;        /* Disable DontRollback() for all pages */
        public u8 memDb;                   /* True to inhibit all file I/O */
        /**************************************************************************
        ** The following block contains those class members that change during
        ** routine opertion.  Class members not in this block are either fixed
        ** when the pager is first created or else only change when there is a
            ** significant mode change (such as changing the page_size, locking_mode,
                ** or the journal_mode).  From another view, these class members describe
            ** the "state" of the pager, while other class members describe the
                ** "configuration" of the pager.
                */
                public u8 eState;                  /* Pager state (OPEN, READER, WRITER_LOCKED..) */
        public u8 eLock;                   /* Current lock held on database file */
        public bool changeCountDone;       /* Set after incrementing the change-counter */
        public int setMaster;              /* True if a m-j name has been written to jrnl */
        public u8 doNotSpill;              /* Do not spill the cache when non-zero */
        public u8 doNotSyncSpill;          /* Do not do a spill that requires jrnl sync */
        public u8 subjInMemory;            /* True to use in-memory sub-journals */
        public Pgno dbSize;                /* Number of pages in the database */
        public Pgno dbOrigSize;            /* dbSize before the current transaction */
        public Pgno dbFileSize;            /* Number of pages in the database file */
        public Pgno dbHintSize;            /* Value passed to FCNTL_SIZE_HINT call */
        public int errCode;                /* One of several kinds of errors */
        public int nRec;                   /* Pages journalled since last j-header written */
        public u32 cksumInit;              /* Quasi-random value added to every checksum */
        public u32 nSubRec;                /* Number of records written to sub-journal */
        public Bitvec pInJournal;          /* One bit for each page in the database file */
        public sqlite3_file fd;            /* File descriptor for database */
        public sqlite3_file jfd;           /* File descriptor for main journal */
        public sqlite3_file sjfd;          /* File descriptor for sub-journal */
        public i64 journalOff;             /* Current write offset in the journal file */
        public i64 journalHdr;             /* Byte offset to previous journal header */
        public sqlite3_backup pBackup;     /* Pointer to list of ongoing backup processes */
        public PagerSavepoint[] aSavepoint;/* Array of active savepoints */
        public int nSavepoint;             /* Number of elements in aSavepoint[] */
        public u8[] dbFileVers = new u8[16];/* Changes whenever database file changes */
        /*
        ** End of the routinely-changing class members
        ***************************************************************************/

        public u16 nExtra;                 /* Add this many bytes to each in-memory page */
        public i16 nReserve;               /* Number of unused bytes at end of each page */
        public u32 vfsFlags;               /* Flags for sqlite3_vfs.xOpen() */
        public u32 sectorSize;             /* Assumed sector size during rollback */
        public int pageSize;               /* Number of bytes in a page */
        public Pgno mxPgno;                /* Maximum allowed size of the database */
        public i64 journalSizeLimit;       /* Size limit for persistent journal files */
        public string zFilename;           /* Name of the database file */
        public string zJournal;            /* Name of the journal file */
        public dxBusyHandler xBusyHandler; /* Function to call when busy */
        public object pBusyHandlerArg;     /* Context argument for xBusyHandler */
        #if SQLITE_TEST || DEBUG
        public int nHit, nMiss;              /* Cache hits and missing */
        public int nRead, nWrite;            /* Database pages read/written */
        #else
        public int nHit;
        #endif
        public dxReiniter xReiniter; //(DbPage*,int);/* Call this routine when reloading pages */
        #if SQLITE_HAS_CODEC
        //void *(*xCodec)(void*,void*,Pgno,int); 
        public dxCodec xCodec;                 /* Routine for en/decoding data */
        //void (*xCodecSizeChng)(void*,int,int); 
        public dxCodecSizeChng xCodecSizeChng; /* Notify of page size changes */
        //void (*xCodecFree)(void*);             
        public dxCodecFree xCodecFree;         /* Destructor for the codec */
        public codec_ctx pCodec;               /* First argument to xCodec... methods */
        #endif
        public byte[] pTmpSpace;               /* Pager.pageSize bytes of space for tmp use */
        public PCache pPCache;                 /* Pointer to page cache object */
        #if !SQLITE_OMIT_WAL
        public Wal pWal;                       /* Write-ahead log used by "journal_mode=wal" */
        public string zWal;                    /* File name for write-ahead log */
        #else
        public sqlite3_vfs pWal = null;             /* Having this dummy here makes C# easier */
        #endif
    }
}

