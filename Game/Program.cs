using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

//DICK_BUTT
//RAINBOW PIECE OF FUCK
//TEST

namespace Game
{
	static class Program
	{
		static RenderWindow window;
		static Sprite sanic;
		static Vector2f sanic_sped = new Vector2f(0, 0);
		static Vector2f VITESSE_X = new Vector2f(2, 0);
		static Vector2f VITESSE_Y = new Vector2f(0, -25);

		static void Main()
		{
			window = new RenderWindow(new VideoMode(800, 600), "SANIC SPED!!");
			window.SetFramerateLimit(60);
			window.Closed += new EventHandler(OnClose);

			Vector2f GRAVITY = new Vector2f(0, 1);
			sanic = new Sprite(new Texture(@"..\..\Ressources\sanic.png"));
			sanic.Scale = new Vector2f(0.25f, 0.25f);
            Texture Background = new Texture(@"..\..\Ressources\Background.jpg");
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
					sanic_sped.X = -Math.Abs(sanic_sped.X); ;
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
							sanic_sped += VITESSE_Y;
						}
					}
					sanic.Position = new Vector2f(sanic.Position.X, window.Size.Y - sanic.GetGlobalBounds().Height + 1);
					sanic_sped.Y = sanic_sped.Y < 0 ? sanic_sped.Y : 0;
				}
				sanic.Position += sanic_sped;

				window.DispatchEvents();
				window.Clear(Color.Green);
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