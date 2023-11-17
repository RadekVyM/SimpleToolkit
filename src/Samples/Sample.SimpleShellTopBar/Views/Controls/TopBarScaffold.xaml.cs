namespace Sample.SimpleShellTopBar.Views.Controls;

[ContentProperty("PageContent")]
public partial class TopBarScaffold : ContentView
{
    public static readonly BindableProperty PageContentProperty =
        BindableProperty.Create(nameof(PageContent), typeof(View), typeof(TopBarScaffold), defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty IsBackButtonVisibleProperty =
        BindableProperty.Create(nameof(IsBackButtonVisible), typeof(bool), typeof(TopBarScaffold), defaultValue: true, defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty TitleViewProperty =
        BindableProperty.Create(nameof(TitleView), typeof(View), typeof(TopBarScaffold), defaultBindingMode: BindingMode.OneWay, propertyChanged: OnTitleViewChanged);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TopBarScaffold), defaultBindingMode: BindingMode.OneWay, propertyChanged: OnTitleChanged);

    public View PageContent
    {
        get => (View)GetValue(PageContentProperty);
        set => SetValue(PageContentProperty, value);
    }

    public bool IsBackButtonVisible
    {
        get => (bool)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    public View? TitleView
    {
        get => (View?)GetValue(TitleViewProperty);
        set => SetValue(TitleViewProperty, value);
    }

    public string? Title
    {
        get => (string?)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    public TopBarScaffold()
	{
		InitializeComponent();
	}


    private static void OnTitleViewChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TopBarScaffold scaffold || newValue is not View newTitleView)
            return;

        scaffold.titleContent.Content = newTitleView;
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TopBarScaffold scaffold)
            return;

        if (scaffold.TitleView is not null)
            return;

        scaffold.titleContent.Content = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            Text = newValue.ToString()
        };
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}