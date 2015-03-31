
//Jermo
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

			Music teme = new Music(@"..\..\Ressources\SanicMusic.wav");
			teme.Volume = 10;
			teme.Loop = true;
            Sound sanicQuote = new Sound();
            sanicQuote.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanicQuote.wav");
			Sound jamp = new Sound();
			jamp.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanic_jamp.wav");
			jamp.Volume = 100;

			Sprite sanic = new Sprite(new Texture(@"..\..\Ressources\sanic.png"));
			sanic.Scale = new Vector2f(0.25f, 0.25f);
			Sprite background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale = new Vector2f(800 / background.GetLocalBounds().Width, 600 / background.GetLocalBounds().Height);

			Vector2f VITESSE_X = new Vector2f(2, 0);
			Vector2f VITESSE_Y = new Vector2f(0, -25);
			Vector2f GRAVITY = new Vector2f(0, 1);
			Vector2f sanic_sped = new Vector2f(0, 0);

			teme.Play();
            sanicQuote.Play();
			while (window.IsOpen)
			{
				if (!Keyboard.IsKeyPressed(Keyboard.Key.Left) && !(Keyboard.IsKeyPressed(Keyboard.Key.Right)))
				{
					sanic_sped.X /= 1.1f;
				}
				else
				{
					if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
					{
						sanic_sped -= VITESSE_X;
					}
					if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
					{
						sanic_sped += VITESSE_X;
					}
				}
				if (sanic.Position.X < 0)
				{
					//sanic.Position = new Vector2f(0, sanic.Position.Y);
					sanic_sped.X = Math.Abs(sanic_sped.X);
				}
				else if (sanic.Position.X + sanic.GetGlobalBounds().Width > window.Size.X)
				{
					//sanic.Position = new Vector2f(window.Size.X - sanic.GetGlobalBounds().Width, sanic.Position.Y);
					sanic_sped.X = -Math.Abs(sanic_sped.X);
				}
				if (sanic.Position.Y + sanic.GetGlobalBounds().Height < window.Size.Y)
				{
					sanic_sped += GRAVITY;
				}
				else
				{
					if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
					{
						if (sanic.Position.Y + sanic.GetGlobalBounds().Height > window.Size.Y)
						{
							jamp.Play();
							sanic_sped += VITESSE_Y;
						}
					}
					sanic.Position = new Vector2f(sanic.Position.X, window.Size.Y - sanic.GetGlobalBounds().Height + 1);
					sanic_sped.Y = sanic_sped.Y < 0 ? sanic_sped.Y : 0;
				}
				sanic.Position += sanic_sped;

				window.DispatchEvents();
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