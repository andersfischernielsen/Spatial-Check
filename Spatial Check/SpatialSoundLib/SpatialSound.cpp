#pragma once 

#include "pch.h"
#include "SpatialSound.h"

using namespace Windows::Media::Devices;
using Microsoft::WRL::ComPtr;

/*
 *	Adapted from https://github.com/microsoft/Xbox-ATG-Samples/tree/master/UWPSamples/Audio/SimpleSpatialPlaySoundUWP
*/

SpatialSound::SpatialSound() :
	m_SpatialAudioClient(nullptr) { }

SpatialSound::~SpatialSound() { }

HRESULT InitializeAudioDeviceAsync()
{
	ComPtr<IActivateAudioInterfaceAsyncOperation> asyncOp;
	HRESULT hr = S_OK;

	m_DeviceIdString = MediaDevice::GetDefaultAudioRenderId(Windows::Media::Devices::AudioDeviceRole::Default);
	hr = ActivateAudioInterfaceAsync(m_DeviceIdString->Data(), __uuidof(ISpatialAudioClient), nullptr, this, &asyncOp);
	if (FAILED(hr)) return E_FAIL;
	
	return hr;
}

HRESULT ActivateCompleted(IActivateAudioInterfaceAsyncOperation* operation) {

	HRESULT hr = S_OK;
	HRESULT hrActivateResult = S_OK;
	ComPtr<IUnknown> audioInterface = nullptr;
	Microsoft::WRL::ComPtr<ISpatialAudioClient>	m_SpatialAudioClient;

	hr = operation->GetActivateResult(&hrActivateResult, &audioInterface);
	if (SUCCEEDED(hr) && SUCCEEDED(hrActivateResult))
	{
		audioInterface.Get()->QueryInterface(IID_PPV_ARGS(&m_SpatialAudioClient));
		if (nullptr == m_SpatialAudioClient)
		{
			return E_FAIL;
		}

		hr = m_SpatialAudioClient->GetMaxDynamicObjectCount(&m_MaxDynamicObjects);
	}

	return S_OK;
}

HRESULT SpatialSound::OnAvailableDynamicObjectCountChange(ISpatialAudioObjectRenderStreamBase* sender, LONGLONG hnsComplianceDeadlineTime, UINT32 availableDynamicObjectCountChange)
{
	sender;
	hnsComplianceDeadlineTime;

	m_MaxDynamicObjects = availableDynamicObjectCountChange;
	return S_OK;
}


UINT32 InitializeAll() {
	Microsoft::WRL::ComPtr<SpatialSound> m_Client = Microsoft::WRL::Make<SpatialSound>();
	m_Client->InitializeAudioDeviceAsync();
	return m_Client.GetMaxDynamicObjects();
}