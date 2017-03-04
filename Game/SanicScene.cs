using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class SanicScene : Scene
	{
		protected View camera;
		protected FloatRect boundaries;
		protected Sprite background;
		protected Sanic sanic;
		protected Squidnic squidnic;
		protected List<RectangleShape> plateformes = new List<RectangleShape>();

		public SanicScene(RenderWindow window)
			: base(window)
		{
			window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

			background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale *= 3;
			boundaries = background.GetGlobalBounds();
	
			sanic = new Sanic();
			squidnic = new Squidnic(window);
			camera = new View(new Vector2f(sanic.Position.X + (sanic.Size.X / 2), sanic.Position.Y + (sanic.Size.Y / 2)), new Vector2f(window.Size.X, window.Size.Y) * 1.5f);

			RectangleShape box = new RectangleShape(new Vector2f(300, 50));
			box.Position = new Vector2f(0, 800);
			box.FillColor = Color.Red;
			plateformes.Add(box);

			Music teme;
			if (new Random().Next(8) == 0)
			{
				teme = new Music(@"..\..\Ressources\SanicMusic.wav");
				teme.Volume = 10;
			}
			else
			{
				teme = new Music(@"..\..\Ressources\gloria.wav");
				teme.Volume = 30;
			}			
			teme.Loop = true;

			teme.Play();

			updatables.Add(sanic);
			updatables.Add(squidnic);
			drawables.Add(FOREGROUND, sanic);
			drawables.Add(FOREGROUND, squidnic);
			drawables.Add(BACKGROUND, background);
			foreach (RectangleShape plateforme in plateformes)
			{
				drawables.Add(MIDDLEGROUND, plateforme);
			}
		}

		public override int update(int elapsedMilliseconds)
		{
			base.update(elapsedMilliseconds);

			sanic.Grounded = false;

			if (sanic.Speed.Y >= 0)
			{
				if (sanic.Position.Y + sanic.Size.Y >= boundaries.Height)
				{
					sanic.Grounded = true;
					sanic.Position.Y = boundaries.Height - sanic.Size.Y;
				}
				else
				{
					foreach (RectangleShape plateforme in plateformes)
					{
						if (sanic.boundaries.Intersects(plateforme.GetGlobalBounds()))
						{
							sanic.Grounded = true;
							sanic.Position.Y = plateforme.GetGlobalBounds().Top - sanic.Size.Y + 1;
						}
					}
				}
			}

			if (sanic.Position.X < 0)
			{
				sanic.turnRight();
			}
			else if (sanic.Position.X + sanic.Size.X > boundaries.Width)
			{
				sanic.turnLeft();
			}

			//DOUBLE THE TROUBLE
			squidnic.Grounded = false;

			if (squidnic.Speed.Y >= 0)
			{
				if (squidnic.Position.Y + squidnic.Size.Y >= boundaries.Height)
				{
					squidnic.Grounded = true;
					squidnic.Position.Y = boundaries.Height - squidnic.Size.Y;
				}
				else
				{
					foreach (RectangleShape plateforme in plateformes)
					{
						if (squidnic.boundaries.Intersects(plateforme.GetGlobalBounds()))
						{
							squidnic.Grounded = true;
							squidnic.Position.Y = plateforme.GetGlobalBounds().Top - squidnic.Size.Y + 1;
						}
					}
				}
			}

			if ((squidnic.Position.X - squidnic.Origin.X <= 0) ||
				(squidnic.Position.X + squidnic.Origin.X >= boundaries.Width))
			{
				squidnic.turn();
			}

			return 0;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			camera.Center = new Vector2f(sanic.Position.X + (sanic.Size.X / 2), sanic.Position.Y + (sanic.Size.Y / 2));
			//camera.Rotation = sanic.Rotation; //LOLOLOLOL
			target.SetView(camera);

			background.Draw(target, states);

			base.Draw(target, states);
		}

		protected void OnKeyPressed(object sender, EventArgs e)
		{
			KeyEventArgs keyEventArgs = (KeyEventArgs)e;
			if (keyEventArgs.Code == Keyboard.Key.Escape)
			{
				exit();
			}
		}
	}
}
