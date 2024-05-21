using System.Collections.Generic;

namespace DOOH.Adboard.Services
{
    public class AdService
    {
        private readonly object _lock = new object();
        private List<Server.Models.DOOHDB.Advertisement> _advertisements = new List<Server.Models.DOOHDB.Advertisement>();

        private readonly DOOHDBService _doohdbService;

        public AdService(DOOHDBService doohdbService)
        {
            _doohdbService = doohdbService;
        }

        public IReadOnlyList<Server.Models.DOOHDB.Advertisement> Advertisements
        {
            get
            {
                lock (_lock)
                {
                    return _advertisements.AsReadOnly();
                }
            }
        }

        public void UpdateAdvertisements(IEnumerable<Server.Models.DOOHDB.Advertisement> advertisements)
        {
            lock (_lock)
            {
                _advertisements.Clear();
                _advertisements.AddRange(advertisements);
            }
        }

        public async Task Sync()
        {
            var advertisements = await _doohdbService.GetAdvertisements(expand: "Attachment");
            UpdateAdvertisements(advertisements.Value);
        }
    }
}
