namespace DeafMan1983;

using System.Runtime.InteropServices;
using System.Text;

public unsafe static class ConvFunctions
{
    //
    //  Conversion functions for sbyte * and string hacks!
    //
    //  Changvs and fixes:
    //  Fixed:
    //      SByteDoublePointersWithStringArray, thanks Jester @QubitTooLate
    //      
    //  -----------------------------------------------------
    //  Replaced:
    //      StringFromCharPtr to SBytePointerWithStringForAdvanced
    //      StringFromHeap to SBytePointerWithString
    //      CharPtrToString to StringwithSBytePointer
    //  Added: new functions for sbyte ** and string[]
    //      SByteDoublePointersWithStringArray ( Fixed and add NativeMemory, Thanks Jan Kotas!!! )
    //      StringArrayWithSByteDoublePointers
    //  Update: to 3.0.0


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

    /*
        Woah it works fine, thanks Jester @QubitTooLate!!!
    */
    internal class UnmanagedUTF8StringArray : IDisposable
    {
        private sbyte **__values;
        private int __length;
        private bool disposed = false;

        public UnmanagedUTF8StringArray(string[] arrays)
        {
            __values = (sbyte **)NativeMemory.Alloc((nuint)arrays.Length, (nuint)sizeof(sbyte *));
            __length = arrays.Length;

            for (int i = 0; i < arrays.Length; i++)
            {
                int len = Encoding.UTF8.GetByteCount(arrays[i]) + 1;
                __values[i] = (sbyte *)NativeMemory.Alloc((nuint)len);
                Encoding.UTF8.GetBytes(arrays[i].AsSpan(), new Span<byte>(__values[i], len));
                __values[i][len - 1] = 0;
            }
        }

        ~UnmanagedUTF8StringArray()
        {
            Dispose(false);
        }

         private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                if(disposing)
                {
                    if (__values != null)
                    {
                        for (int i = 0; i < __length; i++)
                        {
                            if (__values[i] != null)
                            {
                                NativeMemory.Free(__values[i]);
                                __values[i] = null;
                            }
                        }
                    }
                }

                NativeMemory.Free(__values);
                __values = null;

                disposed = true;

            }
        }

        public sbyte **Valves => __values;
        public int Length => __length;
        public sbyte *this[int index]
        {
            get
            {
                if (index < 0 || index >= __length)
                {
                    throw new IndexOutOfRangeException();
                }
                return __values[index];
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public static sbyte **SByteDoublePointersWithStringArray(string[] args)
    {
        var um = new UnmanagedUTF8StringArray(args);
        return um.Valves;
    }

    public static string[] StringArrayWithSByteDoublePointers(sbyte **sArrays, int array_length)
    {
        if (array_length < 0)
        {
            throw new IndexOutOfRangeException();
        }

        string[] arrays = new string[array_length];
        for (int i = 0; i < array_length; i++)
        {
            arrays[i] = StringwithSBytePointer(sArrays[i]);
        }

        return arrays;
    }
}