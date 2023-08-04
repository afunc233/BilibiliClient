using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#if NET7_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace BilibiliClient.Utils;

/// <summary>
/// QQ 群里看到的俩方案 ， 未测试 不过应该管用
/// </summary>
public static class OpenUrlUtil
{
    public static void Open(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        url = url.Replace("&", "^&");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var psi = new ProcessStartInfo("cmd", $"/c start {url}")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            try
            {
                Process.Start("open", url);
            }
            catch (Exception)
            {
                // ignored
#if NET7_0_OR_GREATER
                ObjectiveC.NsString nsStringPath = new(url);
                ObjectiveC.Object nsUrl = new("NSURL");
                var urlPtr = nsUrl.GetFromMessage("URLWithString:", nsStringPath);
                ObjectiveC.Object nsWorkspace = new("NSWorkspace");
                ObjectiveC.Object sharedWorkspace = nsWorkspace.GetFromMessage("sharedWorkspace");
                sharedWorkspace.GetBoolFromMessage("openURL:", urlPtr);
#endif
            }
        }
    }
}
#if NET7_0_OR_GREATER

[SupportedOSPlatform("macos")]
internal static partial class ObjectiveC
{
    private const string ObjCRuntime = "/usr/lib/libobjc.A.dylib";

    [LibraryImport(ObjCRuntime, StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr sel_getUid(string name);

    [LibraryImport(ObjCRuntime, StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr objc_getClass(string name);

    [LibraryImport(ObjCRuntime)]
    private static partial void objc_msgSend(IntPtr receiver, Selector selector);

    [LibraryImport(ObjCRuntime)]
    private static partial void objc_msgSend(IntPtr receiver, Selector selector, byte value);

    [LibraryImport(ObjCRuntime)]
    private static partial void objc_msgSend(IntPtr receiver, Selector selector, IntPtr value);

    [LibraryImport(ObjCRuntime)]
    private static partial void objc_msgSend(IntPtr receiver, Selector selector, NsRect point);

    [LibraryImport(ObjCRuntime)]
    private static partial void objc_msgSend(IntPtr receiver, Selector selector, double value);

    [LibraryImport(ObjCRuntime, EntryPoint = "objc_msgSend")]
    private static partial IntPtr IntPtr_objc_msgSend(IntPtr receiver, Selector selector);

    [LibraryImport(ObjCRuntime, EntryPoint = "objc_msgSend")]
    private static partial IntPtr IntPtr_objc_msgSend(IntPtr receiver, Selector selector, IntPtr param);

    [LibraryImport(ObjCRuntime, EntryPoint = "objc_msgSend", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr IntPtr_objc_msgSend(IntPtr receiver, Selector selector, string param);

    [LibraryImport(ObjCRuntime, EntryPoint = "objc_msgSend")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool bool_objc_msgSend(IntPtr receiver, Selector selector, IntPtr param);

    public readonly struct Object
    {
        public readonly IntPtr objPtr;

        private Object(IntPtr pointer)
        {
            objPtr = pointer;
        }

        public Object(string name)
        {
            objPtr = objc_getClass(name);
        }

        public void SendMessage(Selector selector)
        {
            objc_msgSend(objPtr, selector);
        }

        public void SendMessage(Selector selector, byte value)
        {
            objc_msgSend(objPtr, selector, value);
        }

        public void SendMessage(Selector selector, Object obj)
        {
            objc_msgSend(objPtr, selector, obj.objPtr);
        }

        public void SendMessage(Selector selector, NsRect point)
        {
            objc_msgSend(objPtr, selector, point);
        }

        public void SendMessage(Selector selector, double value)
        {
            objc_msgSend(objPtr, selector, value);
        }

        public Object GetFromMessage(Selector selector)
        {
            return new Object(IntPtr_objc_msgSend(objPtr, selector));
        }

        public Object GetFromMessage(Selector selector, Object obj)
        {
            return new Object(IntPtr_objc_msgSend(objPtr, selector, obj.objPtr));
        }

        public Object GetFromMessage(Selector selector, NsString nsString)
        {
            return new Object(IntPtr_objc_msgSend(objPtr, selector, nsString.strPtr));
        }

        public Object GetFromMessage(Selector selector, string param)
        {
            return new Object(IntPtr_objc_msgSend(objPtr, selector, param));
        }

        public bool GetBoolFromMessage(Selector selector, Object obj)
        {
            return bool_objc_msgSend(objPtr, selector, obj.objPtr);
        }
    }

    public readonly struct Selector
    {
        public readonly IntPtr selPtr;

        private Selector(string name)
        {
            selPtr = sel_getUid(name);
        }

        public static implicit operator Selector(string value) => new(value);
    }

    public readonly struct NsString
    {
        public readonly IntPtr strPtr;

        public NsString(string aString)
        {
            IntPtr nsString = objc_getClass("NSString");
            strPtr = IntPtr_objc_msgSend(nsString, "stringWithUTF8String:", aString);
        }

        public static implicit operator IntPtr(NsString nsString) => nsString.strPtr;
    }

    public readonly struct NsPoint
    {
        public readonly double x;
        public readonly double y;

        public NsPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public readonly struct NsRect
    {
        public readonly NsPoint pos;
        public readonly NsPoint size;

        public NsRect(double x, double y, double width, double height)
        {
            pos = new NsPoint(x, y);
            size = new NsPoint(width, height);
        }
    }
}

#endif