using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using mobilewakala.Models;
using mobilewakala.Services;
using mobilewakala.Shared;
namespace mobilewakala.ViewModels
{
    public class CustomerTicketVm : INotifyPropertyChanged
    {
        private TicketDataModel _ticket;
        private readonly ICustomerServices _customerServices;

        private List<AgentDataModel> _agentListLoaded;

        public CustomerTicketVm()
        {
            _ticket = new TicketDataModel();
            _customerServices = new CustomerServices();
            _agentListLoaded = new List<AgentDataModel>();

        }
        public TicketDataModel Ticket
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
        public List<AgentDataModel> AgentListLoaded
        {
            get { return _agentListLoaded; }
            set
            {
                if (_agentListLoaded != value)
                {
                    _agentListLoaded = value;
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
