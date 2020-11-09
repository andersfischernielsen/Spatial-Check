using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi.Interfaces;

/*
 * 
 * Adapted/simplified from NAudio's MMDevice.
 * 
 */

namespace AudioHelpers
{
    public class MMDevice : IDisposable
    {
        public SpatialAudioClient SpatialAudioClient => GetAudioClient();

        #region Variables
        private readonly IMMDevice deviceInterface;
        #endregion

        private static Guid IID_ISpatialAudioClient = new Guid("BBF8E066-AAAA-49BE-9A4D-FD2A858EA27F");

        private SpatialAudioClient GetAudioClient()
        {
            Marshal.ThrowExceptionForHR(deviceInterface.Activate(ref IID_ISpatialAudioClient, ClsCtx.ALL, IntPtr.Zero, out var result));
            return new SpatialAudioClient(result as ISpatialAudioClient);
        }

        internal MMDevice(IMMDevice realDevice)
        {
            deviceInterface = realDevice;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
