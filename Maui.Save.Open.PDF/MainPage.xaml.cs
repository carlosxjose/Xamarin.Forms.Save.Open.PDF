using Plugin.MauiSaveOpenPDFPackage;

namespace Maui.Save.Open.PDF
{
    public partial class MainPage : ContentPage
    {
        private string _uri = "https://dgii.gov.do/legislacion/decretos/Documents/2018/Decreto-299-18.pdf";//"https://officialdoniald.com/testpdf.pdf";
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        /// <summary>
        /// Save and open the PDF file in the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new();

            var uri = new Uri(_uri);

            Stream content;
            MemoryStream stream = new();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStreamAsync();
                content.CopyTo(stream);
            }

            // Validar permisos
            await CheckAndRequestLocationPermission();

            await CrossMauiSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", stream, PDFOpenContext.InApp);

        }

        /// <summary>
        /// Save and open the PDF file with "choose app".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            HttpClient client = new();

            var uri = new Uri(_uri);

            Stream content;
            MemoryStream stream = new MemoryStream();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStreamAsync();
                content.CopyTo(stream);
            }

            // Validar permisos
            await CheckAndRequestLocationPermission();

            await CrossMauiSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", stream, PDFOpenContext.ChooseApp);
        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }
    }
}