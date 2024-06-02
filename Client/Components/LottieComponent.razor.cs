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
        public string Style { get; set; }
        [Parameter]
        public string Class { get; set; }

    }
}
