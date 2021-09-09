using Microsoft.WindowsAzure.MobileServices;

namespace Repositories
{
    public static class MobileServiceConnection
    {
        static readonly MobileServiceClient mobileService = new MobileServiceClient("https://cricket-simulator.azure-mobile.net/", "CnmjHcDVJRNYFHZTujAHlnlsQqkrne93");

        public static MobileServiceClient MobileService
        {
            get
            {
                return mobileService;
            }
        }
    }
}
