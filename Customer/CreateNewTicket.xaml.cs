
using Syncfusion.Maui.Inputs;
using System.Collections.ObjectModel;
using mobilewakala.Models;

namespace mobilewakala.Customer
{
    public partial class CreateNewTicket : ContentPage
    {
        // Properties for binding
        public string EnteredPhoneNumber { get; set; }
        public string EnteredDescription { get; set; }
        public ObservableCollection<string> NetworkItems { get; set; }
        public ObservableCollection<string> ServiceItems { get; set; }

        public string SelectedNetwork;
        public string SelectedService;

        public CreateNewTicket()
        {
            InitializeComponent();
            // Initialize collections
            NetworkItems = new ObservableCollection<string> { "Airtel", "Halotel", "Tigo", "Vodacom", "Ttcl" };
            ServiceItems = new ObservableCollection<string> { "Usajili Wa Laini", "Miamala Ya Kifedha" };

            // Set the binding context to this page
            BindingContext = this;
        }

        private void GoToFetchAgent(object sender, EventArgs e)
        {

            PreparedCustomerTicket.Instance.phoneNumber = LoggedCustomer.loggedCust.CustomerId;
            PreparedCustomerTicket.Instance.description = EnteredDescription;
            PreparedCustomerTicket.Instance.network = SelectedNetwork;
            PreparedCustomerTicket.Instance.serviceRequested = SelectedService;
            Navigation.PushAsync(new SelectAgent());

        }

        private void OnNetworkComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (SfComboBox)sender;
            SelectedNetwork = comboBox.SelectedItem?.ToString();
        }

        private void OnServiceComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (SfComboBox)sender;
            SelectedService = comboBox.SelectedItem?.ToString();
        }
    }
}
