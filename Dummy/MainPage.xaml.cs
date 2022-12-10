using System.ComponentModel;

namespace Dummy;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	int count = 0;

	private string dummyStr;
	public string DummyStr
	{
		get {  return dummyStr; }
		set { dummyStr = value;
			OnPropertyChanged(nameof(DummyStr));

		}
	}


	private DateTime date = DateTime.Now;
	public DateTime Date
	{
		get { return date; }
		set
		{
			date= value;
			OnPropertyChanged(nameof(Date));
		}
	}

    new public event PropertyChangedEventHandler PropertyChanged;
    new void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public MainPage()
	{
		InitializeComponent();
		DateButton.BindingContext = DummyStr;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
		
		DummyStr = CounterBtn.Text;	
		
	}
}

