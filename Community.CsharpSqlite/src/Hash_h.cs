using u8 = System.Byte;
using u32 = System.UInt32;

namespace Community.CsharpSqlite
{
  public partial class Globals
  {

    /*
    ** Access routines.  To delete, insert a NULL pointer.
    */
    //void sqlite3HashInit(Hash);
    //void *sqlite3HashInsert(Hash*, string pKey, int nKey, object  *pData);
    //void *sqlite3HashFind(const Hash*, string pKey, int nKey);
    //void sqlite3HashClear(Hash);

    /*
    ** Macros for looping over all elements of a hash table.  The idiom is
    ** like this:
    **
    **   Hash h;
    **   HashElem p;
    **   ...
    **   for(p=sqliteHashFirst(&h); p; p=sqliteHashNext(p)){
    **     SomeStructure pData = sqliteHashData(p);
    **     // do something with pData
    **   }
    */
    //#define sqliteHashFirst(H)  ((H).first)
    static HashElem sqliteHashFirst( Hash H )
    {
      return H.first;
    }
    //#define sqliteHashNext(E)   ((E).next)
    static HashElem sqliteHashNext( HashElem E )
    {
      return E.next;
    }
    //#define sqliteHashData(E)   ((E).data)
    static object sqliteHashData( HashElem E )
    {
      return E.data;
    }
    /* #define sqliteHashKey(E)    ((E)->pKey) // NOT USED */
    /* #define sqliteHashKeysize(E) ((E)->nKey)  // NOT USED */

    /*
    ** Number of entries in a hash table
    */
    /* #define sqliteHashCount(H)  ((H)->count) // NOT USED */

    //#endif // * _SQLITE_HASH_H_ */
  }
}
