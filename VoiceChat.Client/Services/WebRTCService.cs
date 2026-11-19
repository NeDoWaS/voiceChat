using Microsoft.JSInterop;

public class WebRTCService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public WebRTCService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task StartMicrophoneAsync()
    {
        _module ??= await _js.InvokeAsync<IJSObjectReference>(
            "import", "./js/webrtc.js");

        await _module.InvokeAsync<bool>("startMicrophone");
    }

    public async Task StopMicrophoneAsync()
    {
        if (_module != null)
            await _module.InvokeVoidAsync("stopMicrophone");
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
            await _module.DisposeAsync();
    }
}