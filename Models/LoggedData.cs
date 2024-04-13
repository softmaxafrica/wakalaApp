using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Models
{
    public  class LoggedData
    {
        public string agentCode { get; set; }
        public string password { get; set; }


        #region Singleton pattern
        private static LoggedData _loggedUser;

        public static LoggedData LoggedUser
        {
            get
            {
                if (_loggedUser == null)
                {
                    _loggedUser = new LoggedData();
                }
                return _loggedUser;
            }
        }

        private LoggedData() { }

        #endregion
    }

}

