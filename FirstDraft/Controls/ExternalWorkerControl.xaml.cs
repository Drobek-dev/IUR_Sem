namespace FirstDraft.Controls;

public partial class ExternalWorkerControl : ContentView
{
	public BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ExternalWorkerControl));


	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	} 
	public ExternalWorkerControl()
	{
		InitializeComponent();
	}
}