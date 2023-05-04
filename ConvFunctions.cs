namespace DeafMan1983;

using System.Runtime.InteropServices;
using System.Text;
public unsafe static class ConvFunctions
{
        //
    //  Conversion functions for sbyte * and string hacks!
    //
    //  Changvs and fixes:
    //  -----------------------------------------------------
    //  Replaced:
    //      StringFromCharPtr to SBytePointerWithStringForAdvanced
    //      StringFromHeap to SBytePointerWithString
    //      CharPtrToString to StringwithSBytePointer
    //  Added:
    //  new functions for sbyte ** and string[]
    //      SByteDoublePointersWithStringArray
    //      StringArrayWithSByteDoublePointers
    // 
    //      

    public static int StringOfSize(string str_value)
    {
        if (str_value == null)
            {
                return 0;
            }
            return (str_value.Length * 4) + 1;
    }

    public static sbyte *SBytePointerWithStringForAdvanced(string str_value, sbyte *buf_value, int size_value)
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

    public static sbyte *SBytePointerWithString(string str_value)
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

    public static string StringwithSBytePointer(sbyte *charptr)
    {
        if (charptr == null)
        {
            return string.Empty;
        }

        byte *ptr = (byte*)charptr;
        while (*ptr != 0)
        {
            ptr++;
        }

        return Encoding.UTF8.GetString((byte *)charptr, (int)(ptr - (byte *)charptr));
    }

    public static sbyte **SByteDoublePointersWithStringArray(string[] arrays)
    {
        sbyte **array_ptrs = null;
        for (int i = 0; i < arrays.Length; i++)
        {
            array_ptrs[i] = SBytePointerWithString(arrays[i]);
        }

        return array_ptrs;
    }

    public static string[] StringArrayWithSByteDoublePointers(sbyte **array_ptrs)
    {
        Span<string> span_arrays = new Span<string>();
        for (int i = 0; i < span_arrays.Length; i++)
        {
            string[] arrays = span_arrays.ToArray();;
            arrays[i] = StringwithSBytePointer(array_ptrs[i]);
        }

        return span_arrays.ToArray();;
    }
}
