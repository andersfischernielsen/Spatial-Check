// ConsoleApplication.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <SpatialAudioClient.h>
#include <wrl.h>
#include <mmdeviceapi.h>

int main()
{
    HRESULT hr;
    Microsoft::WRL::ComPtr<IMMDeviceEnumerator> deviceEnum;
    Microsoft::WRL::ComPtr<IMMDevice> defaultDevice;
    hr = CoInitialize(NULL);
    hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&deviceEnum);
    hr = deviceEnum->GetDefaultAudioEndpoint(EDataFlow::eRender, eMultimedia, &defaultDevice);
    Microsoft::WRL::ComPtr<ISpatialAudioClient> spatialAudioClient;
    hr = defaultDevice->Activate(__uuidof(ISpatialAudioClient), CLSCTX_INPROC_SERVER, nullptr, (void**)&spatialAudioClient);
    UINT32 count;
    spatialAudioClient->GetMaxDynamicObjectCount(&count);
    std::cout << count << " \n";
}
