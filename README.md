# ConversionFunctions

New 2.0.0 ConversionFunction

Changes & fixes:
* Replaced:
         StringFromCharPtr to SBytePointerWithStringForAdvanced
         StringFromHeap to SBytePointerWithString
         CharPtrToString to StringwithSBytePointer
*  Added: new functions for sbyte ** and string[]
         SByteDoublePointersWithStringArray ( Fixed and add NativeMemory, Thanks Jan Kotas!!! )
         StringArrayWithSByteDoublePointers
* Updated to 2.0.0

How does it work?
If you use `sbyte*` for `string` like example in X11's `XOpenDisplay()` or `SDL_SetWindowTitle()`

For X11 from TerraFX.Interop.Xlib
```
....
Display *display = XOpenDisplay(SBytePointerWithString(string.Empty));
....
```
Or for SDL3 from DeafMan1983.Interop.SDL3
```
....
SDL_SetWindows(window, SBytePointerWithString("Hello SDL3 Window"));
....
```

For `string` for `sbyte *` like example X11's `XDisplayString()` or SDL3's `SDL_GetWindowTitle()`
```
....
string d_str = StringWithSBytePointer(XDisplayString(display));
....
```
or SDL3
```
....
string title_str = StringWithSBytePointer(SDL_GetWindowTitle(window));
....
```

And new more functions:

For `string[]` for `sbyte **` like you use `delegate *unmanaged[Cdecl]<sbyte **, int, void>` or `delegate *unmanaged[Cdecl]<sbyte **, int, int>` and It works for Game, Application they can load external native library but I have tested with `Console.WriteLine()` It is successfully. But I never did other like Winfows.Forms or Xlib, SDL3 I will see how does it work for external native libraries.
```
....
string[] args = StringArrayWithSByteDoublePointers(sArrays, array_length);
....
```
Warning It is important for `[UnmanagedCallerOnly]` and it uses `sbyte **` and `int` for `array_length`
If you pass `string[] args` from Program.cs then you need add `SByteDoublePointersWithStringArray`

Example:
```
sbyte **sArrays = SByteDoublePointersWithStringArray(args);
pfnMainFunc = (`delegate *unmanaged[Cdecl]<sbyte **, int, int`)dlsym(dlopen(SBytePointerWithString("libApp.so"), 0x00002), SBytePointerWithString("LA_MainFunc"));
....
```
And I have updated already in Nuget.org
