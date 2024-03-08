
using Microsoft.Maui.Controls;
using static MAUI.SpaceGraphics;

namespace MAUI;

public partial class MainPage : ContentPage
{
	private readonly SpaceGraphics spaceGraphics = new();
		public MainPage()
	{
		InitializeComponent();
		draw.Drawable = spaceGraphics;

		IDispatcherTimer timer = Application.Current.Dispatcher.CreateTimer();
		timer.Interval = TimeSpan.FromMilliseconds(100);
		timer.Tick += Timer_Tick;
		timer.Start();

		// Set up the UI
		OrbitToggle.OnChanged += ToggleOrbits_Clicked;
		NameToggle.OnChanged += ToggleNames_Clicked;

		InitPicker();
		picker.SelectedIndexChanged += Picker_SelectedIndexChanged;

		
	    }

		public void InitPicker()
		{
			picker.Items.Add("Mercury");
			picker.Items.Add("Venus");
			picker.Items.Add("Earth");
			picker.Items.Add("Mars");
			picker.Items.Add("Jupiter");
			picker.Items.Add("Saturn");
			picker.Items.Add("Uranus");
			picker.Items.Add("Neptune");
			picker.Items.Add("Pluto");
			picker.Items.Add("Solar System");
		}

		public void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			Picker picker = (Picker)sender;
			int selectedIndex = picker.SelectedIndex;
			if (selectedIndex != -1 && selectedIndex != 9)
			{
				spaceGraphics.SolarSystem.ZoomIn(spaceGraphics.SolarSystem.Planets[selectedIndex]);
				draw.Invalidate();
			}
			else
			{
				spaceGraphics.SolarSystem.ZoomOut();
				draw.Invalidate();
			}
			draw.Invalidate();
		}

		public void ToggleNames_Clicked(object sender, EventArgs e)
		{
			SwitchCell swichcell = (SwitchCell)sender;

			if (swichcell.On)
			{
				spaceGraphics.SolarSystem.ToggleOffNames();}
			else
			{
				spaceGraphics.SolarSystem.ToggleOnNames();
			}
			draw.Invalidate();
		}

		public void ToggleOrbits_Clicked(object sender, EventArgs e)
		{
			SwitchCell swichcell = (SwitchCell)sender;

			if (swichcell.On)
			{
				spaceGraphics.SolarSystem.ToggleOffOrbit();
			}
			else
			{
				spaceGraphics.SolarSystem.ToggleOnOrbit();	
			}
			draw.Invalidate();
		}



public void Timer_Tick(object sender, EventArgs e)
{
	spaceGraphics.SolarSystem?.DoTick();
	draw.Invalidate();

}
}
