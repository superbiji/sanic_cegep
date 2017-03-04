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

		public SanicScene(RenderWindow window)
			: base(window)
		{
			window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

			background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale *= 3;
			boundaries = background.GetGlobalBounds();
	
			sanic = new Sanic(boundaries);
			camera = new View(new Vector2f(sanic.Position.X + (sanic.Size.X / 2), sanic.Position.Y + (sanic.Size.Y / 2)), new Vector2f(window.Size.X, window.Size.Y));
			Music teme = sanic is Squidnic ? new Music(@"..\..\Ressources\gloria.wav") : new Music(@"..\..\Ressources\SanicMusic.wav");

			teme.Volume = 10;
			teme.Loop = true;

			teme.Play();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			camera.Center = new Vector2f(sanic.Position.X + (sanic.Size.X / 2), sanic.Position.Y + (sanic.Size.Y / 2));
			//camera.Rotation = sanic.Rotation; //LOLOLOLOL
			target.SetView(camera);

			background.Draw(target, states);
			sanic.Draw(target, states);
		}

		public new int update(int elapsedMilliseconds)
		{
			sanic.update(elapsedMilliseconds);

			return base.update(elapsedMilliseconds);
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
