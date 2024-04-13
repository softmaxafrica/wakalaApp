using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Models
{
    public  class LoggedCustomer
    {
        public string CustomerId { get; set; }
        #region Singleton pattern
        private static LoggedCustomer _loggedCust;
        public static LoggedCustomer loggedCust
        {
            get
            {
                if (_loggedCust == null)
                {
                    _loggedCust = new LoggedCustomer();
                }
                return _loggedCust;
            }
        }
        private LoggedCustomer() { }
        #endregion
    }

}

