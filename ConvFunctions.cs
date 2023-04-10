namespace DeafMan1983;

using System.Runtime.InteropServices;
using System.Text;
public unsafe static class ConvFunctions
{
    //
    //  Conversion functions for sbyte * and string hacks!
    //
    public static int StringOfSize(string str_value)
    {
        if (str_value == null)
            {
                return 0;
            }
            return (str_value.Length * 4) + 1;
    }

    public static sbyte *StringFromCharPtr(string str_value, sbyte *buf_value, int size_value)
    {
        if (str_value == null)
            {
                return (sbyte*)0;
            }
            fixed (char* strPtr = str_value)
            {
                Encoding.UTF8.GetBytes(strPtr, str_value.Length + 1, (byte *)buf_value, size_value);
            }
            return buf_value;
    }

    public static sbyte *StringFromHeap(string str_value)
    {
        if (str_value == null)
        {
            return (sbyte*)0;
        }

        int strOfSize = StringOfSize(str_value);
        sbyte* buf_value = (sbyte*)Marshal.AllocHGlobal(strOfSize);
        fixed (char* strPtr = str_value)
        {
            Encoding.UTF8.GetBytes(strPtr, str_value.Length + 1, (byte *)buf_value, strOfSize);
        }
        return buf_value;
    }

    public static string CharPtrToString(sbyte *charptr)
    {
        if (charptr == null)
        {
            return string.Empty;
        }

        sbyte *ptr = charptr;
        while (*ptr != 0)
        {
            ptr++;
        }

        return Encoding.UTF8.GetString((byte *)charptr, (int)(ptr - (sbyte *)charptr));
    }
}