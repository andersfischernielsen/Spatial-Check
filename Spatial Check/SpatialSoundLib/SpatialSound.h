
#include "SpatialAudioClient.h"
#include <mmdeviceapi.h>

#pragma once

#ifdef SPATIALSOUND_EXPORTS
#define SPATIALSOUND_API __declspec(dllexport)
#else
#define SPATIALSOUND_API __declspec(dllimport)
#endif

/*
 *	Adapted from https://github.com/microsoft/Xbox-ATG-Samples/tree/master/UWPSamples/Audio/SimpleSpatialPlaySoundUWP
*/

class SpatialSound :
	public Microsoft::WRL::RuntimeClass< Microsoft::WRL::RuntimeClassFlags< Microsoft::WRL::ClassicCom >, Microsoft::WRL::FtmBase, IActivateAudioInterfaceCompletionHandler >
{
public:
	SpatialSound();

	UINT32 GetMaxDynamicObjects() { return m_MaxDynamicObjects; }
	HRESULT InitializeAudioDeviceAsync();

	UINT32 m_MaxDynamicObjects;

	STDMETHOD(ActivateCompleted)(IActivateAudioInterfaceAsyncOperation* operation);

	STDMETHOD(OnAvailableDynamicObjectCountChange)(ISpatialAudioObjectRenderStreamBase* sender, LONGLONG hnsComplianceDeadlineTime, UINT32 availableDynamicObjectCountChange);
	
	Microsoft::WRL::ComPtr<ISpatialAudioClient> m_SpatialAudioClient;

private: 
	~SpatialSound();

private:
	UINT32 m_MaxDynamicObjects;
	Platform::String^ m_DeviceIdString;

};