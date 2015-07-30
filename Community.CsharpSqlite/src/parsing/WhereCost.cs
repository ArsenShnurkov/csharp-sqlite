namespace Community.CsharpSqlite
{
    using System;
    using Bitmask = System.UInt64;

    /*
    ** A WhereCost object records a lookup strategy and the estimated
    ** cost of pursuing that strategy.
    */
    public class WhereCost
    {
        public WherePlan plan = new WherePlan();/* The lookup strategy */
        public double rCost;                    /* Overall cost of pursuing this search strategy */
        public Bitmask used;                    /* Bitmask of cursors used by this plan */

        public void Clear()
        {
            plan.Clear();
            rCost = 0;
            used = 0;
        }
    };
}

