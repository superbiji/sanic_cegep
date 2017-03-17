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
		public class Boundaries : Collisionable
		{
			protected FloatRect collisionRect;
			public FloatRect CollisionRect
			{
				get
				{
					return collisionRect;
				}
				protected set
				{
					collisionRect = value;
				}
			}

			public Boundaries(FloatRect floatRect)
			{
				CollisionRect = floatRect;
			}

			public void collision(Collisionable collisionable, CollisionDirection collisionDirection)
			{
			}
		}

		private bool playTeme = true; //quand on est tannés des tounes

		Text debug = new Text("debug", new Font(@"..\..\Ressources\DigitalDream.ttf"));

		protected float ratio;
		protected View camera;
		protected Boundaries boundaries;
		protected Sprite background;
		protected List<Player> players = new List<Player>();
		protected List<Plateforme> plateformes = new List<Plateforme>();

		public SanicLevel(RenderWindow window)
			: base(window)
		{
			window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

			ratio = (float)window.Size.X / (float)window.Size.Y;

			debug.Color = Color.Red;
			debug.Style = Text.Styles.Bold;

			background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale *= 3;
			boundaries = new Boundaries(background.GetGlobalBounds());

			Sanic sanic = new Sanic(new Vector2f(0, 0));
			Squidnic squidnic = new Squidnic(new Vector2f(boundaries.CollisionRect.Left + boundaries.CollisionRect.Width - 200, 0));
			camera = new View(new Vector2f(sanic.SpriteRect.Left + (sanic.SpriteRect.Width / 2), sanic.SpriteRect.Top + (sanic.SpriteRect.Height / 2)), new Vector2f(window.Size.X, window.Size.Y) * 1.5f);

			Plateforme box = new Plateforme(new Vector2f(300, 50));
			box.Position = new Vector2f(boundaries.CollisionRect.Left + boundaries.CollisionRect.Width - box.CollisionRect.Width, 800);
			box.FillColor = Color.Magenta;
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

			players.Add(sanic);
			players.Add(squidnic);
			updatables.Add(sanic);
			updatables.Add(squidnic);
			drawables.Add(FOREGROUND, sanic);
			drawables.Add(FOREGROUND, squidnic);
			drawables.Add(BACKGROUND, background);
			foreach (Plateforme plateforme in plateformes)
			{
				drawables.Add(MIDDLEGROUND, plateforme);
			}
		}

		public override int update(int elapsedMilliseconds)
		{
			int updateResult = base.update(elapsedMilliseconds);

			foreach (Player player in players)
			{
				if (player.Speed.Y >= 0)
				{
					if (player.CollisionRect.Top + player.CollisionRect.Height >= boundaries.CollisionRect.Height)
					{
						player.collision(boundaries, CollisionDirection.DOWN);
					}
					else
					{
						foreach (Plateforme plateforme in plateformes)
						{
							if (player.CollisionRect.Intersects(plateforme.GetGlobalBounds())) //à chier
							{
								player.collision(plateforme, CollisionDirection.DOWN);
							}
						}
					}
				}

				if (player.CollisionRect.Left < boundaries.CollisionRect.Left)
				{
					player.collision(boundaries, CollisionDirection.LEFT);
				}
				else if (player.CollisionRect.Left + player.CollisionRect.Width > boundaries.CollisionRect.Width)
				{
					player.collision(boundaries, CollisionDirection.RIGHT);
				}
			}

			return updateResult;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			/*float x1 = Math.Min(sanic.CollisionRect.Left, squidnic.CollisionRect.Left);
			float x2 = Math.Max(sanic.CollisionRect.Left + sanic.CollisionRect.Width, squidnic.CollisionRect.Left + squidnic.SpriteRect.Width);
			float y1 = Math.Min(sanic.CollisionRect.Top, squidnic.CollisionRect.Top);
			float y2 = Math.Max(sanic.CollisionRect.Top + sanic.CollisionRect.Height, squidnic.CollisionRect.Top + squidnic.SpriteRect.Height);
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

			camera.Size = new Vector2f(cameraWidth, cameraHeight);*/
			FloatRect playerRect = players.First().CollisionRect;
			camera.Center = new Vector2f(playerRect.Left + (playerRect.Width / 2), playerRect.Top + (playerRect.Height / 2));
			//camera.Rotation = sanic.Rotation; //LOLOLOLOL
			target.SetView(camera);

			base.Draw(target, states);

			/*Sanic sanic = players.First() as Sanic;
			debug.DisplayedString = "state : " + sanic.getState().ToString() + "\n";
			debug.DisplayedString += "speed x : " + sanic.Speed.X + "\n";
			debug.DisplayedString += "x : " + sanic.CollisionRect.Left + "\n";
			debug.DisplayedString += "y : " + sanic.CollisionRect.Top + "\n";
			debug.Position = new Vector2f(camera.Center.X - camera.Size.X / 2, camera.Center.Y - camera.Size.Y / 2);
			debug.Draw(target, states);*/
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
