using System;
using System.Runtime.InteropServices;

namespace SpatialAudio
{
    public static class SpatialAudio
    {
        public static int GetMaxDynamicObjectCount()
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDevice device = null;
            try
            {
                deviceEnumerator = (IMMDeviceEnumerator) new MMDeviceEnumerator();
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out device);
                var IID_ISpatialAudioClient = typeof(ISpatialAudioClient).GUID;
                Marshal.ThrowExceptionForHR(device.Activate(ref IID_ISpatialAudioClient, 1, UIntPtr.Zero, out object o));
                var client = o as ISpatialAudioClient;
                Marshal.ThrowExceptionForHR(client.GetMaxDynamicObjectCount(out uint result));
                return (int) result;
            }
            finally
            {
                if (device != null) Marshal.ReleaseComObject(device);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }
    }

    [ComImport]
    [Guid("BBF8E066-AAAA-49BE-9A4D-FD2A858EA27F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface ISpatialAudioClient
    {
        int GetStaticObjectPosition(int type, out float x, out float y, out float z);
        
        int GetNativeStaticObjectTypeMask(IntPtr mask);
        
        int GetMaxDynamicObjectCount(out uint value);

        int GetSupportedAudioObjectFormatEnumerator(out object enumerator);
        
        int GetMaxFrameCount(object objectFormat, out uint frameCountPerBuffer);

        int IsAudioObjectFormatSupported(object objectFormat);

        int IsSpatialAudioStreamAvailable(Guid streamUuid, object auxiliaryInfo);

        int ActivateSpatialAudioStream(object activationParams, Guid riid, [Out] object stream);

    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    {
    }

    internal enum EDataFlow
    {
        eRender,
        eCapture,
        eAll,
        EDataFlow_enum_count
    }

    internal enum ERole
    {
        eConsole,
        eMultimedia,
        eCommunications,
        ERole_enum_count
    }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int NotImpl1();

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);

        // the rest is not implemented
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig]
        int Activate(ref Guid iid, int dwClsCtx, UIntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
    }
}
