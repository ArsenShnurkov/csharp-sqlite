namespace Community.CsharpSqlite
{
    using System;
    using u32 = System.UInt32;
    
    /*
** At least one instance of the following structure is created for each 
** trigger that may be fired while parsing an INSERT, UPDATE or DELETE
** statement. All such objects are stored in the linked list headed at
** Parse.pTriggerPrg and deleted once statement compilation has been
** completed.
**
** A Vdbe sub-program that implements the body and WHEN clause of trigger
** TriggerPrg.pTrigger, assuming a default ON CONFLICT clause of
** TriggerPrg.orconf, is stored in the TriggerPrg.pProgram variable.
** The Parse.pTriggerPrg list never contains two entries with the same
** values for both pTrigger and orconf.
**
** The TriggerPrg.aColmask[0] variable is set to a mask of old.* columns
** accessed (or set to 0 for triggers fired as a result of INSERT 
** statements). Similarly, the TriggerPrg.aColmask[1] variable is set to
** a mask of new.* columns used by the program.
*/
    public class TriggerPrg
    {
        public Trigger pTrigger;      /* Trigger this program was coded from */
        public int orconf;            /* Default ON CONFLICT policy */
        public SubProgram pProgram;   /* Program implementing pTrigger/orconf */
        public u32[] aColmask = new u32[2];        /* Masks of old.*, new.* columns accessed */
        public TriggerPrg pNext;      /* Next entry in Parse.pTriggerPrg list */
    }
}

