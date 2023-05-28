namespace DeafMan1983;

using System.Runtime.InteropServices;
using System.Text;

public unsafe static class ConvFunctions
{
    //
    //  Conversion functions for sbyte *, sbyte **, byte *, byte ** void *, string and string[]
    //
    //  Changvs and fixes:
    //  Fixed: 
    //      Since NullReferenceExeepction cause some can't new instance
    //      some functions
    //
    //  Replaced: for sbyte * and sbyte **
    //      StringSize to LengthSize
    //      StringwithSBytePointer to StringFromSBytePointer
    //      StringArrayWithSByteDoublePointers to StringArrayFromByteDoublePointers
    //      SBytePointerWithString to SBytePointerFromString
    //      SByteDoublePointerWithStringArrays to SByteDoublePointersFromStringArray
    //  
    //  Added;
    //      byte * and byte ** for StringFromBytePointer, StringArrayFromByteDoublePointers,
    //      BytePointerFromString, ByteDoublePointersFromStringArray
    //      void * for Alloc, Free and Delete
    //      
    //  Updated to 4.0.0


    /*
        Length Size means sizeof() or strlen();
    */
    public static int LengthSize(string str)
    {
        if (str == null)
        {
            return 0;
        }
        return (str.Length * 4) + 1;
    }

    /*
        BYTE * and BYTE **
    */
    public static string StringFromBytePointer(byte *s, bool freePtr = false)
    {
        if (s == null)
        {
            return string.Empty;
        }

        byte* ptr = (byte*) s;
        while (*ptr != 0)
        {
            ptr++;
        }

        string result = System.Text.Encoding.UTF8.GetString(
            s, (int) (ptr - (byte*) s)
        );

        if (freePtr)
        {
            NativeMemory.Free(s);
        }
        return result;
    }

    public static byte* BytePointerFromString(string str)
    {
        if (str == null)
        {
            return (byte*) 0;
        }

        int bufferSize = LengthSize(str);
        byte *buffer = (byte *)NativeMemory.Alloc((nuint)bufferSize);
        fixed (char* strPtr = str)
        {
            Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
        }
        return buffer;
    }

    class UnmanagedFromBytePoublePointers : IDisposable
    {
        private byte **__values;
        private int __length;
        private bool disposed = false;

        public UnmanagedFromBytePoublePointers(string[] arrays)
        {
            __values = (byte **)NativeMemory.Alloc((nuint)arrays.Length, (nuint)sizeof(sbyte *));
            __length = arrays.Length;

            for (int i = 0; i < arrays.Length; i++)
            {
                int len = Encoding.UTF8.GetByteCount(arrays[i]) + 1;
                __values[i] = (byte *)NativeMemory.Alloc((nuint)len);
                Encoding.UTF8.GetBytes(arrays[i].AsSpan(), new Span<byte>(__values[i], len));
                __values[i][len - 1] = 0;
            }
        }

        ~UnmanagedFromBytePoublePointers()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
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

        public byte **Valves => __values;
        public int Length => __length;
        public byte *this[int index]
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

    public static byte **ByteDoublePointersFromStringArray(string[] strs)
    {
        UnmanagedFromBytePoublePointers um = new(strs);
        return um.Valves;
    }

    public static string[] StringArrayFromByteDoublePointers(byte **bytes, int lengthSize)
    {
        if (lengthSize < 0)
        {
            throw new IndexOutOfRangeException();
        }

        string[] arrays = new string[lengthSize];
        for (int i = 0; i < arrays.Length; i++)
        {
            arrays[i] = StringFromBytePointer(bytes[i]);
        }

        return arrays;
    }


    /*
        SBYTE * and SBYTE **
    */
    public static string StringFromSBytePointer(sbyte *s, bool freePtr = false)
    {
        if (s == null)
        {
            return string.Empty;
        }

        byte* ptr = (byte*) s;
        while (*ptr != 0)
        {
            ptr++;
        }

        string result = System.Text.Encoding.UTF8.GetString(
            (byte *)s, (int) (ptr - (byte*) s)
        );

        if (freePtr)
        {
            NativeMemory.Free(s);
        }
        return result;
    }

    public static unsafe sbyte* SBytePointerFromString(string str)
    {
        if (str == null)
        {
            return (sbyte*) 0;
        }

        int bufferSize = LengthSize(str);
        sbyte *buffer = (sbyte *)NativeMemory.Alloc((nuint)bufferSize);
        fixed (char* strPtr = str)
        {
            Encoding.UTF8.GetBytes(strPtr, str.Length + 1, (byte *)buffer, bufferSize);
        }
        return buffer;
    }

    class UnmanagedFromSBytePoublePointers : IDisposable
    {
        private sbyte **__values;
        private int __length;
        private bool disposed = false;

        public UnmanagedFromSBytePoublePointers(string[] arrays)
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

        ~UnmanagedFromSBytePoublePointers()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
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

    public static sbyte **SByteDoublePointersFromStringArray(string[] args)
    {
        UnmanagedFromSBytePoublePointers um = new(args);
        return um.Valves;
    }

    public static string[] StringArrayFromSByteDoublePointers(sbyte **sArrays, int array_length)
    {
        if (array_length < 0)
        {
            throw new IndexOutOfRangeException();
        }

        string[] arrays = new string[array_length];
        for (int i = 0; i < array_length; i++)
        {
            arrays[i] = StringFromSBytePointer(sArrays[i]);
        }

        return arrays;
    }

    /*
        Next version possible with void * and void ** for Structures
        Like GLFW GLFW_Monitors** as void **
        Still working....
    */
    class UnmanagedStructurePointer : IDisposable
    {
        private void *_value;
        private bool disposed = false;

        public UnmanagedStructurePointer(void *structure)
        {
            if (_value != null)
            {
                _value = NativeMemory.Alloc((nuint)structure);
            }
        }

        ~UnmanagedStructurePointer()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                if (disposing)
                {
                    NativeMemory.Free(_value);
                }

                NativeMemory.Free(_value);
            }
        }

        public void *this[void *structure]
        {
            get
            {
                if (structure == null)
                {
                    NativeMemory.Free(structure);
                }
                else
                {
                    _value = NativeMemory.Alloc((nuint)structure);
                }
                return _value;
            }
        }

        public void *Value
        {
            get
            {
                return _value;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    // Like malloc(), alloc() or other alloc-functions in C/C++
    // Never use Marshal.AllocGlobal()!!!
    public static void *Alloc(void* structure)
    {
        UnmanagedStructurePointer um = new(structure);
        return um.Value;
    }

    // Like free() in C/C++
    public static void Free(void *structure)
    {
        UnmanagedStructurePointer um = new(structure);
        if (um.Value != null)
        {
            um.Dispose();
        }
    }

    // Like delete() in C/C++
    public static void Delete(void *structure)
    {
        if (Free != null)
        {
            Free(structure);
        }
    }
}