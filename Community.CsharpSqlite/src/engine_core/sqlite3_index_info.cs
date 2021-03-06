﻿using System;

namespace Community.CsharpSqlite
{
    /*
    ** CAPI3REF: Virtual Table Indexing Information
    ** KEYWORDS: sqlite3_index_info
    **
    ** The sqlite3_index_info structure and its substructures is used as part
    ** of the [virtual table] interface to
    ** pass information into and receive the reply from the [xBestIndex]
    ** method of a [virtual table module].  The fields under **Inputs** are the
    ** inputs to xBestIndex and are read-only.  xBestIndex inserts its
    ** results into the **Outputs** fields.
    **
    ** ^(The aConstraint[] array records WHERE clause constraints of the form:
    **
    ** <blockquote>column OP expr</blockquote>
    **
    ** where OP is =, &lt;, &lt;=, &gt;, or &gt;=.)^  ^(The particular operator is
    ** stored in aConstraint[].op using one of the
    ** [SQLITE_INDEX_CONSTRAINT_EQ | SQLITE_INDEX_CONSTRAINT_ values].)^
    ** ^(The index of the column is stored in
    ** aConstraint[].iColumn.)^  ^(aConstraint[].usable is TRUE if the
    ** expr on the right-hand side can be evaluated (and thus the constraint
    ** is usable) and false if it cannot.)^
    **
    ** ^The optimizer automatically inverts terms of the form "expr OP column"
    ** and makes other simplifications to the WHERE clause in an attempt to
    ** get as many WHERE clause terms into the form shown above as possible.
    ** ^The aConstraint[] array only reports WHERE clause terms that are
    ** relevant to the particular virtual table being queried.
    **
    ** ^Information about the ORDER BY clause is stored in aOrderBy[].
    ** ^Each term of aOrderBy records a column of the ORDER BY clause.
    **
    ** The [xBestIndex] method must fill aConstraintUsage[] with information
    ** about what parameters to pass to xFilter.  ^If argvIndex>0 then
    ** the right-hand side of the corresponding aConstraint[] is evaluated
    ** and becomes the argvIndex-th entry in argv.  ^(If aConstraintUsage[].omit
    ** is true, then the constraint is assumed to be fully handled by the
    ** virtual table and is not checked again by SQLite.)^
    **
    ** ^The idxNum and idxPtr values are recorded and passed into the
    ** [xFilter] method.
    ** ^[sqlite3_free()] is used to free idxPtr if and only if
    ** needToFreeIdxPtr is true.
    **
    ** ^The orderByConsumed means that output from [xFilter]/[xNext] will occur in
    ** the correct order to satisfy the ORDER BY clause so that no separate
    ** sorting step is required.
    **
    ** ^The estimatedCost value is an estimate of the cost of doing the
    ** particular lookup.  A full scan of a table with N entries should have
    ** a cost of N.  A binary search of a table of N entries should have a
    ** cost of approximately log(N).
    */
    //struct sqlite3_index_info {
    //  /* Inputs */
    //  int nConstraint;           /* Number of entries in aConstraint */
    //  struct sqlite3_index_constraint {
    //     int iColumn;              /* Column on left-hand side of constraint */
    //     unsigned char op;         /* Constraint operator */
    //     unsigned char usable;     /* True if this constraint is usable */
    //     int iTermOffset;          /* Used internally - xBestIndex should ignore */
    //  } *aConstraint;            /* Table of WHERE clause constraints */
    //  int nOrderBy;              /* Number of terms in the ORDER BY clause */
    //  struct sqlite3_index_orderby {
    //     int iColumn;              /* Column number */
    //     unsigned char desc;       /* True for DESC.  False for ASC. */
    //  } *aOrderBy;               /* The ORDER BY clause */
    //  /* Outputs */
    //  struct sqlite3_index_constraint_usage {
    //    int argvIndex;           /* if >0, constraint is part of argv to xFilter */
    //    unsigned char omit;      /* Do not code a test for this constraint */
    //  } *aConstraintUsage;
    //  int idxNum;                /* Number used to identify the index */
    //  char *idxStr;              /* String, possibly obtained from sqlite3_malloc */
    //  int needToFreeIdxStr;      /* Free idxStr using sqlite3_free() if true */
    //  int orderByConsumed;       /* True if output is already ordered */
    //  double estimatedCost;      /* Estimated cost of using this index */
    //};
    public class sqlite3_index_constraint
    {
        public int iColumn;              /* Column on left-hand side of constraint */
        public int op;                   /* Constraint operator */
        public bool usable;              /* True if this constraint is usable */
        public int iTermOffset;          /* Used internally - xBestIndex should ignore */
    }
    public class sqlite3_index_orderby
    {
        public int iColumn;              /* Column number */
        public bool desc;                /* True for DESC.  False for ASC. */
    }
    public class sqlite3_index_constraint_usage
    {
        public int argvIndex;   /* if >0, constraint is part of argv to xFilter */
        public bool omit;       /* Do not code a test for this constraint */
    }

    public class sqlite3_index_info
    {
        /* Inputs */
        public int nConstraint;             /* Number of entries in aConstraint */
        public sqlite3_index_constraint[] aConstraint;            /* Table of WHERE clause constraints */
        public int nOrderBy;                /* Number of terms in the ORDER BY clause */
        public sqlite3_index_orderby[] aOrderBy;/* The ORDER BY clause */

        /* Outputs */

        public sqlite3_index_constraint_usage[] aConstraintUsage;
        public int idxNum;                /* Number used to identify the index */
        public string idxStr;             /* String, possibly obtained from sqlite3Malloc */
        public int needToFreeIdxStr;      /* Free idxStr using sqlite3DbFree(db,) if true */
        public bool orderByConsumed;       /* True if output is already ordered */
        public double estimatedCost;      /* Estimated cost of using this index */
    }
}
