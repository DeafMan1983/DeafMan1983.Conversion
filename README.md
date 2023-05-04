# ConversionFunctions
For sbyte * and string in C# with [TerraFX.Interop.Xlib](https://github.com/terrafx/terrafx.interop.xlib).
For anything String[] and sbyte ** for deleagte unmanaged[Cdecl] functions for access external method from native library or whatever communication with libraries....

Example:
For `XOpenDisplay(sbyte * display_name)`
```
using static DeafMan1983.ConvFunctions;
...
Display *display = XOpenDisplay(SBytePointerWithString(string.Empty));
...
```
For `XFontStruct` and `XFontSet`
See in 04_Text and 05_Rearranging ( PS: I have ported from http://xopendisplay.hilltopia.ca/index.html )

Remember that multiple pointer like `XFontStruct **font_r` becomes `out XFontStruct *font`
Because Dotnet doesn't knon before `font` loads for mapping window and gc etc...
