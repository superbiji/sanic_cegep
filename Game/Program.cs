
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Game
{
	static class Program
	{
		static void Main()
		{
			RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SANIC SPED!!", Styles.Close);
			window.Closed += new EventHandler(OnClose);
			window.SetFramerateLimit(60);
			window.SetKeyRepeatEnabled(false);

			Music teme = new Music(@"..\..\Ressources\SanicMusic.wav");
			teme.Volume = 10;
			teme.Loop = true;
           
			Sprite background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale = new Vector2f(800 / background.GetLocalBounds().Width, 600 / background.GetLocalBounds().Height);

			Sanic sanic = new Sanic(window);
			sanic.Position = new Vector2f(window.Size.X / 2, 0);

			teme.Play();
			sanic.Quote();

			while (window.IsOpen)
			{
				window.DispatchEvents();

				//TODO: better input manager
				if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
				{
					sanic.Move(Direction.Left);
				}
				if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
				{
					sanic.Move(Direction.Right);
				}
				if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
				{
					sanic.Jump();
				}
				if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
				{
					sanic.Quote();
				}

				sanic.Update();

				window.Clear();
				window.Draw(background);
				window.Draw(sanic);
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