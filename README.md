# ConversionFunctions
For sbyte * and string in C# with [TerraFX.Interop.Xlib](https://github.com/terrafx/terrafx.interop.xlib).

Example:
For `XOpenDisplay(sbyte * display_name)`
```
using static DeafMan1983.ConvFunctions;
...
Display *display = XOpenDisplay(StringFromHeap(string.Empty));
...
```
For `XFontStruct` and `XFontSet`
See in 04_Text and 05_Rearranging ( PS: I have ported from http://xopendisplay.hilltopia.ca/index.html )

Remember that multiple pointer like `XFontStruct **font_r` becomes `out XFontStruct *font`
Because Dotnet doesn't knon before `font` loads for mapping window and gc etc...
