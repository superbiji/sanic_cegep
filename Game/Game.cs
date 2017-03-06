using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game
{
	public class Game
	{
		protected RenderWindow window;
		protected SanicScene sanicScene;

		public Game(UInt32 width, UInt32 height, string title, Styles style, UInt32 framerateLimit)
		{
			window = new RenderWindow(new VideoMode(width, height), title, style);
			window.SetFramerateLimit(60);
			window.SetKeyRepeatEnabled(false);
			window.Closed += new EventHandler(OnClose);

			sanicScene = new SanicScene(window);
		}

		public Game(UInt32 width, UInt32 height, string title, Styles style = Styles.Close)
			: this(width, height, title, style, 60)
		{
		}

		void renderingThread()
		{
			//while (window.IsOpen)
			{
				window.Clear();
				window.Draw(sanicScene);
				window.Display();
			}
		}


		//TODO: Implement good game loop
		public void loop()
		{
			Clock clock = new Clock();

			//Thread thread = new Thread(renderingThread);
			//thread.Start();

			while (window.IsOpen)
			{
				window.DispatchEvents();

				Time elapsed = clock.Restart();
				int elapsedMilliseconds = elapsed.AsMilliseconds();
				int state = sanicScene.update(elapsedMilliseconds);

				if (state == Scene.EXIT_STATE)
				{
					window.Close();
				}

				renderingThread();			
			}
		}

		private void OnClose(object sender, EventArgs e)
		{
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}
	}
}
