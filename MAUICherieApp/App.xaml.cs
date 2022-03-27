namespace MAUICherieApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Resumed += Window_Resumed;
        window.Activated += Window_Resumed;

        return window;
    }

    private void Window_Resumed(object sender, EventArgs e)
    {
        MessagingCenter.Instance.Send(this, "RefreshCollectionView");
    }
}