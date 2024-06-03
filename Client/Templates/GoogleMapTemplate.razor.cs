using Microsoft.AspNetCore.Components;
using Radzen;

namespace DOOH.Client.Templates
{
    public partial class GoogleMapTemplate
    {
        [Parameter]
        public IEnumerable<Tuple<string, GoogleMapPosition>> Markers { get; set; } = new List<Tuple<string, GoogleMapPosition>>();

        [Parameter]
        public GoogleMapPosition Center { get; set; }

        [Parameter]
        public int Zoom { get; set; } = 15;

    }
}
