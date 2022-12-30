namespace FirstDraft.Controls;

public partial class InternetConnectionControl : ContentView
{
	public BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(InternetConnectionControl));


	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	} 
	public InternetConnectionControl()
	{
		InitializeComponent();
	}
}