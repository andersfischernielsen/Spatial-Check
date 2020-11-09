using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace AudioHelpers
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BBF8E066-AAAA-49BE-9A4D-FD2A858EA27F")]
    interface ISpatialAudioClient
    {
        [PreserveSig]
        int GetMaxDynamicObjectCount(
            [Out][MarshalAs(UnmanagedType.U4)] out uint value);
    }

    public class SpatialAudioClient
    {

        private readonly ISpatialAudioClient spatialAudioClientInterface;
        public int MaxDynamicObjectCount
        {
            get
            {
                Marshal.ThrowExceptionForHR(spatialAudioClientInterface.GetMaxDynamicObjectCount(out uint dynamicObjectCount));
                return (int) dynamicObjectCount;
            }
        }

        internal SpatialAudioClient(ISpatialAudioClient spatialAudioClientInterface)
        {
            this.spatialAudioClientInterface = spatialAudioClientInterface;
        }
    }
}
