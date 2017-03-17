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
	public class SanicLevel : Scene
	{
		private bool playTeme = true; //quand on est tannés des tounes


		protected float ratio;
		protected View camera;
		protected FloatRect boundaries;
		protected Sprite background;
		protected Sanic sanic;
		protected Squidnic squidnic;
		protected List<RectangleShape> plateformes = new List<RectangleShape>();

		public SanicLevel(RenderWindow window)
			: base(window)
		{
			window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

			ratio = (float)window.Size.X / (float)window.Size.Y;

			background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale *= 3;
			boundaries = background.GetGlobalBounds();

			sanic = new Sanic(new Vector2f(0, 0));
			squidnic = new Squidnic(new Vector2f(0, 0));
			camera = new View(new Vector2f(sanic.SpriteRect.Left + (sanic.SpriteRect.Width / 2), sanic.SpriteRect.Top + (sanic.SpriteRect.Height / 2)), new Vector2f(window.Size.X, window.Size.Y) * 1.5f);

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
				if (sanic.CollisionRect.Top + sanic.CollisionRect.Height >= boundaries.Height)
				{
					sanic.Grounded = true;
					sanic.CollisionRect.Top = boundaries.Height - sanic.CollisionRect.Height;
				}
				else
				{
					foreach (RectangleShape plateforme in plateformes)
					{
						if (sanic.CollisionRect.Intersects(plateforme.GetGlobalBounds()))
						{
							sanic.Grounded = true;
							sanic.CollisionRect.Top = plateforme.GetGlobalBounds().Top - sanic.CollisionRect.Height + 1;
						}
					}
				}
			}

			if (sanic.CollisionRect.Left < 0)
			{
				sanic.turnRight();
			}
			else if (sanic.CollisionRect.Left + sanic.CollisionRect.Width > boundaries.Width)
			{
				sanic.turnLeft();
			}

			//DOUBLE THE TROUBLE
			squidnic.Grounded = false;

			if (squidnic.Speed.Y >= 0)
			{
				if (squidnic.Position.Y + squidnic.SpriteRect.Height >= boundaries.Height)
				{
					squidnic.Grounded = true;
					squidnic.Position.Y = boundaries.Height - squidnic.SpriteRect.Height;
				}
				else
				{
					foreach (RectangleShape plateforme in plateformes)
					{
						if (squidnic.SpriteRect.Intersects(plateforme.GetGlobalBounds()))
						{
							squidnic.Grounded = true;
							squidnic.Position.Y = plateforme.GetGlobalBounds().Top - squidnic.SpriteRect.Height + 1;
						}
					}
				}
			}

			if (squidnic.CollisionRect.Left < 0)
			{
				squidnic.bounce(Orientation.DROITE);
			}
			else if (squidnic.CollisionRect.Left + squidnic.CollisionRect.Width >= boundaries.Width)
			{
				squidnic.bounce(Orientation.GAUCHE);
			}

			return updateResult;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			float x1 = Math.Min(sanic.CollisionRect.Left, squidnic.Position.X);
			float x2 = Math.Max(sanic.CollisionRect.Left + sanic.CollisionRect.Width, squidnic.Position.X + squidnic.SpriteRect.Width);
			float y1 = Math.Min(sanic.CollisionRect.Top, squidnic.Position.Y);
			float y2 = Math.Max(sanic.CollisionRect.Top + sanic.CollisionRect.Height, squidnic.Position.Y + squidnic.SpriteRect.Height);
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
