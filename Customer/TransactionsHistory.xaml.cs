using mobilewakala.Models;
using mobilewakala.Services;

namespace mobilewakala.Customer;

public partial class TransactionsHistory : ContentPage
{
    private readonly ICustomerServices _customerServices;
    public OpenTicketModel mapData;
    private List<OpenTicketModel> TicketListLoaded;
    public TransactionsHistory()
    {
        InitializeComponent();
        TicketListLoaded = new List<OpenTicketModel>();
        _customerServices = new CustomerServices();
        LoadTicketHistory();
    }


    #region Load TransactionsHistory to collection

    private async void LoadTicketHistory()
    {
        try
        {
            if (LoggedCustomer.loggedCust == null)
            {
               await  Navigation.PushAsync(new CustomerLogin());
                return;
            }
            string customerId = LoggedCustomer.loggedCust.CustomerId;

            if (_customerServices == null)
            {
                return;
            }

            var history = await _customerServices.GetTransactionsHistory(customerId);

            if (history == null)
            {
                Console.WriteLine("Open tickets collection is null.");
                return;
            }
            TicketListLoaded.Clear();

            foreach (var ticket in history)
            {
                TicketListLoaded.Add(ticket);
            }
            TicketHistoryListLoaded.ItemsSource = TicketListLoaded;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading open tickets: {ex.Message}");
        }
    }
    #endregion
    private void Frame_Tapped(object sender, EventArgs e)
    {
        mapData = new OpenTicketModel();
        // Check if an item is selected
        if (sender is Frame frame && frame.BindingContext is OpenTicketModel selectedTicket)
        {

            string phoneNumber = selectedTicket.phoneNumber;
            string network = selectedTicket.network;
            string serviceRequested = selectedTicket.serviceRequested;
            string description = selectedTicket.description;
            string ticketStatus = selectedTicket.ticketStatus;

            PreparedCustomerTicket.Instance.phoneNumber = phoneNumber;
            PreparedCustomerTicket.Instance.description = description;
            PreparedCustomerTicket.Instance.network = network;
            PreparedCustomerTicket.Instance.serviceRequested = serviceRequested;

            respondToTicket();



            //            
        }

        async void respondToTicket()
        {

            bool answer = await DisplayAlert("Maombi Ya Huduma", "Je, Unabadili wakala  Huduma Hii Kwa Sasa", "Ndio", "Hapana");
            if (answer)
            {
                Navigation.PushAsync(new SelectAgent());
            }
            else
            {
                if (answer == false)
                {

                }

            }
        }


    }
}