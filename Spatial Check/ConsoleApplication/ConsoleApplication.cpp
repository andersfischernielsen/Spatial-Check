#include <iostream>
#include <SpatialAudioClient.h>
#include <wrl.h>
#include <mmdeviceapi.h>
#include "wintoastlib.h"

using namespace WinToastLib;

class CustomHandler : public IWinToastHandler {
public:
    void toastActivated() const {
        exit(0);
    }

    void toastActivated(int actionIndex) const { exit(16 + actionIndex); }

    void toastDismissed(WinToastDismissalReason state) const { }

    void toastFailed() const {
        std::wcout << L"Error showing current toast" << std::endl;
        exit(5);
    }
};


void showToast(UINT32 dynamicObjectCount, bool userCheckInitiated) {
    if (!WinToast::isCompatible()) {
        std::wcerr << L"Error, your system in not supported!" << std::endl;
        return;
    }

    if (dynamicObjectCount > 0 && !userCheckInitiated) return; 
    WinToastTemplate templ(WinToastTemplate::Text02);
    std::wstring appName = L"Spatial Check";
    std::wstring appUserModelID = L"Spatial Check";
    std::wstring notifyText;
    std::wstring detailsText;
    std::wstring attribute = L" ";
    INT64 expiration = 5;
    
    WinToast::instance()->setAppName(appName);
    WinToast::instance()->setAppUserModelId(appUserModelID);
    if (!WinToast::instance()->initialize()) {
        std::wcerr << L"Error, your system in not compatible!" << std::endl;
        return;
    }


    if (dynamicObjectCount > 0 && userCheckInitiated) {
        notifyText = L"Spatial sound is enabled";
        detailsText = L"Open Sound settings to disable";
    }
    if (dynamicObjectCount <= 0) {
        notifyText = L"Spatial sound is disabled";
        detailsText = L"Open Sound settings to re-enable";
    }
    
    templ.setExpiration(expiration);
    templ.setTextField(notifyText, WinToastTemplate::FirstLine);
    templ.setTextField(detailsText, WinToastTemplate::SecondLine);
    templ.setAttributionText(attribute);

    if (WinToast::instance()->showToast(templ, new CustomHandler(), nullptr) < 0) {
        std::wcerr << L"Could not launch your toast notification!";
        return;
    }

    Sleep(expiration ? (DWORD)expiration + 1000 : 10000);
}

int main(int argc, char* argv[], char* envp[])
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
    
    if (argc > 1) { showToast(count, true); }
    else showToast(count, false);
}
