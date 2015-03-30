using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

//DICK_BUTT
//TEST

namespace Game
{
	static class Program
	{
		static void Main()
		{
			RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Untitled");
			window.SetFramerateLimit(60);
			window.Closed += new EventHandler(OnClose);

			while (window.IsOpen)
			{
				window.DispatchEvents();
				window.Clear();
				//window.Draw(Drawable);
				window.Display();
			}
		}

		static void OnClose(object sender, EventArgs e)
		{
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}
	}
}