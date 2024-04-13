using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mobilewakala.Models;
using mobilewakala.Services;

namespace mobilewakala.ViewModels
{
   public class VOpenTicket
    {
         LiveLocationService livelocation = new LiveLocationService();
        public double cLatitude;
        public double cLongitude;
        private readonly IAgentServices _AgentServices;
        private OpenTicketModel _ticket;
        private List<OpenTicketModel> TicketListLoaded;
        public VOpenTicket()
        {
            _ticket = new OpenTicketModel();
            _AgentServices = new AgentServices();
            TicketListLoaded = new List<OpenTicketModel>();
        }

        public OpenTicketModel Ticket
        {
            get { return _ticket; }
            set
            {
                if (_ticket != value)
                {
                    _ticket = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<OpenTicketModel> AgentListLoaded
        {
            get { return TicketListLoaded; }
            set
            {
                if (TicketListLoaded != value)
                {
                    TicketListLoaded = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
