 

namespace WakalaPlus.Models
{
    public class DeviceDetails
    {
        public string deviceId { get; set; }
        public string user { get; set; }
        public DateTime lastUpdate { get; set; }
        #region Singleton pattern
        private static DeviceDetails _instance;

        public static DeviceDetails Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeviceDetails();
                }
                return _instance;
            }
        }

        private DeviceDetails() { }

        #endregion
    }



}
