using Microsoft.AspNetCore.Components;

namespace DOOH.Client.Components
{
    public partial class LottieComponent
    {
        [Parameter]
        public required string Src { get; set; }
        [Parameter]
        public int Speed { get; set; } = 1;
        [Parameter]
        public string Height { get; set; } = "auto";
        [Parameter]
        public string Width { get; set; } = "auto";

        public string Style => $"width: {Width}; height: {Height};";
    }
}
