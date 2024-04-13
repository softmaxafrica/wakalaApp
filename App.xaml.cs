namespace mobilewakala
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF1cXmhNYVRpR2Nbe05yfldAalxWVBYiSV9jS3pTdUVrWHtcdnRdTmRcVA==");

            MainPage = new AppShell();
        }
    }
}
