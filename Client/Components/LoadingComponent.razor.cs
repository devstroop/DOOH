using Microsoft.AspNetCore.Components;

namespace DOOH.Client.Components
{
    public partial class LoadingComponent
    {
        [Parameter]
        public int Speed { get; set; } = 1;
        [Parameter]
        public string Height { get; set; } = "300px";
        [Parameter]
        public string Width { get; set; } = "300px";
        [Parameter]
        public bool Visible { get; set; } = true;
        [Parameter]
        public string Style { get; set; }
        private string _style => $"{Style}; width: {Width}; height: {Height};";
    }
}
