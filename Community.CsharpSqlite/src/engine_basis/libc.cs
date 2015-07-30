using System;
using System.IO;

namespace Community.CsharpSqlite
{
    public partial class Globals
    {
        static int atoi(byte[] inStr)
        {
            return atoi(Encoding.UTF8.GetString(inStr, 0, inStr.Length));
        }

        static int atoi(string inStr)
        {
            int i;
            for (i = 0; i < inStr.Length; i++)
            {
                if (!sqlite3Isdigit(inStr[i]) && inStr[i] != '-')
                    break;
            }
            int result = 0;
            #if WINDOWS_MOBILE 
            try { result = Int32.Parse(inStr.Substring(0, i)); }
            catch { }
            return result;
            #else
            return (Int32.TryParse(inStr.Substring(0, i), out result) ? result : 0);
            #endif
        }

        static void fprintf(TextWriter tw, string zFormat, params object[] ap)
        {
            tw.Write(sqlite3_mprintf(zFormat, ap));
        }

        static void printf(string zFormat, params object[] ap)
        {
            #if !SQLITE_WINRT
            Console.Out.Write(sqlite3_mprintf(zFormat, ap));
            #endif
        }


        //Byte Buffer Testing
        static int memcmp(byte[] bA, byte[] bB, int Limit)
        {
            if (bA.Length < Limit)
                return (bA.Length < bB.Length) ? -1 : +1;
            if (bB.Length < Limit)
                return +1;
            for (int i = 0; i < Limit; i++)
            {
                if (bA[i] != bB[i])
                    return (bA[i] < bB[i]) ? -1 : 1;
            }
            return 0;
        }

        //Byte Buffer  & String Testing
        static int memcmp(string A, byte[] bB, int Limit)
        {
            if (A.Length < Limit)
                return (A.Length < bB.Length) ? -1 : +1;
            if (bB.Length < Limit)
                return +1;
            char[] cA = A.ToCharArray();
            for (int i = 0; i < Limit; i++)
            {
                if (cA[i] != bB[i])
                    return (cA[i] < bB[i]) ? -1 : 1;
            }
            return 0;
        }

        //byte with Offset & String Testing
        static int memcmp(byte[] a, int Offset, byte[] b, int Limit)
        {
            if (a.Length < Offset + Limit)
                return (a.Length - Offset < b.Length) ? -1 : +1;
            if (b.Length < Limit)
                return +1;
            for (int i = 0; i < Limit; i++)
            {
                if (a[i + Offset] != b[i])
                    return (a[i + Offset] < b[i]) ? -1 : 1;
            }
            return 0;
        }

        //byte with Offset & String Testing
        static int memcmp(byte[] a, int Aoffset, byte[] b, int Boffset, int Limit)
        {
            if (a.Length < Aoffset + Limit)
                return (a.Length - Aoffset < b.Length - Boffset) ? -1 : +1;
            if (b.Length < Boffset + Limit)
                return +1;
            for (int i = 0; i < Limit; i++)
            {
                if (a[i + Aoffset] != b[i + Boffset])
                    return (a[i + Aoffset] < b[i + Boffset]) ? -1 : 1;
            }
            return 0;
        }

        static int memcmp(byte[] a, int Offset, string b, int Limit)
        {
            if (a.Length < Offset + Limit)
                return (a.Length - Offset < b.Length) ? -1 : +1;
            if (b.Length < Limit)
                return +1;
            for (int i = 0; i < Limit; i++)
            {
                if (a[i + Offset] != b[i])
                    return (a[i + Offset] < b[i]) ? -1 : 1;
            }
            return 0;
        }
        //String Testing
        static int memcmp(string A, string B, int Limit)
        {
            if (A.Length < Limit)
                return (A.Length < B.Length) ? -1 : +1;
            if (B.Length < Limit)
                return +1;
            int rc;
            if ((rc = String.Compare(A, 0, B, 0, Limit, StringComparison.Ordinal)) == 0)
                return 0;
            return rc < 0 ? -1 : +1;
        }
    }
}

