namespace MAUICherieApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        MessagingCenter.Instance.Send(this, "RefreshCollectionView");
    }
}