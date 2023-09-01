using Microsoft.JSInterop;

namespace AutoHub
{
    public static class JSExtensions
    {
        public static ValueTask DisplayMessage(this IJSRuntime js, string message, string title) =>
            js.InvokeVoidAsync("displayMessage", message, title);

        public static ValueTask<bool> Confirmation(this IJSRuntime js, string message, string? title = null)
        {
            if (js == null) return new ValueTask<bool>();
            return js.InvokeAsync<bool>("confirmation", message, title);
        }
    }
}
