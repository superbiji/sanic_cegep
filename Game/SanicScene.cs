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
		protected float ratio;
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

			ratio = (float)window.Size.X / (float)window.Size.Y;

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
			int updateResult = base.update(elapsedMilliseconds);

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

			return updateResult;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			float x1 = Math.Min(sanic.Position.X, squidnic.Position.X);
			float x2 = Math.Max(sanic.Position.X + sanic.Size.X, squidnic.Position.X + squidnic.Size.X);
			float y1 = Math.Min(sanic.Position.Y, squidnic.Position.Y);
			float y2 = Math.Max(sanic.Position.Y + sanic.Size.Y, squidnic.Position.Y + squidnic.Size.Y);
			float cameraWidth = Math.Max(x2 - x1 + 300, 500);
			float cameraHeight = Math.Max(y2 - y1 + 300, 500); ;
			if (cameraHeight < cameraWidth / ratio)
			{
				cameraHeight = cameraWidth / ratio;
			}
			else
			{
				cameraWidth = cameraHeight * ratio;
			}

			camera.Size = new Vector2f(cameraWidth, cameraHeight);
			camera.Center = new Vector2f(((x2 - x1) / 2) + x1, ((y2 - y1) / 2) + y1);
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
