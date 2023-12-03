# ConversionFunctions - Legacy version = 4.0.0

# // UPDATE: Notification: Next version will to be a closed sources for `Format<XXXXXXXXX>()` and more new functions with `sbyte*`, `byte*` and `void*.
Added new class SharedLibsfor Invoking shared libraries for Native Aot only in net7.0 and net8.0
Added new class-extension for `sbyte` and `byte` SbyteExtensions with implements like `string.Format()` and more.

Please be patient for next version 5.0.0 `DeafMan1983.Conversions`.
If you want to get a closed/paid source of `DeafMan1983.Conversions` - You would like to wait patient for new release on my website....

Conversion functions for sbyte * and string hacks!

Conversion functions for sbyte *, sbyte **, byte *, byte ** void *, string and string[]

Changvs and fixes:<br />
Fixed: <br />
- Since NullReferenceExeepction cause some can't new instance
- some functions

Replaced:<br />
- StringSize to LengthSize
- StringwithSBytePointer to StringFromSBytePointer
- StringArrayWithSByteDoublePointers to StringArrayFromByteDoublePointers
- SBytePointerWithString to SBytePointerFromString
- SByteDoublePointerWithStringArrays to SByteDoublePointersFromStringArray
 
Added;<br />
- byte * and byte ** for StringFromBytePointer, StringArrayFromByteDoublePointers,
- BytePointerFromString, ByteDoublePointersFromStringArray
- void * for Alloc, Free and Delete
<br />
pdated to 4.0.0

How does it work?
If you use `sbyte*` for `string` like example in X11's `XOpenDisplay()` or `SDL_SetWindowTitle()`

For X11 from TerraFX.Interop.Xlib
```cs
....
Display *display = XOpenDisplay(SBytePointerFromString(string.Empty));
....
```
Or for SDL3 from DeafMan1983.Interop.SDL3
```cs
....
SDL_SetWindows(window, SBytePointerWithString("Hello SDL3 Window"));
....
```

For `string` for `sbyte *` like example X11's `XDisplayString()` or SDL3's `SDL_GetWindowTitle()`
```cs
....
string d_str = StringFromSBytePointer(XDisplayString(display));
....
```
or SDL3
```cs
....
string title_str = StringFromSBytePointer(SDL_GetWindowTitle(window));
....
```

And new more functions:

For `string[]` for `sbyte **` like you use `delegate *unmanaged[Cdecl]<sbyte **, int, void>` or `delegate *unmanaged[Cdecl]<sbyte **, int, int>` and It works for Game, Application they can load external native library but I have tested with `Console.WriteLine()` It is successfully. But I never did other like Winfows.Forms or Xlib, SDL3 I will see how does it work for external native libraries.
```cs
....
string[] args = StringArrayFromSByteDoublePointers(sArrays, array_length);
....
```
Warning It is important for `[UnmanagedCallerOnly]` and it uses `sbyte **` and `int` for `array_length`
If you pass `string[] args` from Program.cs then you need add `SByteDoublePointersFromStringArray`

Example:
```cs
sbyte **sArrays = SByteDoublePointersFromStringArray(args);
pfnMainFunc(sArrays, args.length);
....
```
New feature for Alloc, Free and Delete like `C/C++`
```cs
SDL_Renderer *renderer = SDL_CreateRenderer(window, null, SDL_RENDER_ACCELERATOR);
Alloc(renderer);
....
SDL_DestroyRenderer(display);
Delete(renderer);
```

And I have updated already in Nuget.org
