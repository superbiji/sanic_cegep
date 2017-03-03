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
		protected Sprite background;
		protected Sanic sanic;

		public SanicScene(RenderWindow window)
			: base(window)
		{
			window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

			sanic = new Sanic(window);

			Music teme = sanic is Squidnic ? new Music(@"..\..\Ressources\gloria.wav") : new Music(@"..\..\Ressources\SanicMusic.wav");
			teme.Volume = 30;
			teme.Loop = true;

			background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale = new Vector2f(window.Size.X / background.GetLocalBounds().Width, window.Size.Y / background.GetLocalBounds().Height);			
			
			teme.Play();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			background.Draw(target, states);
			sanic.Draw(target, states);
		}

		public new int update(int elapsedMilliseconds)
		{
			sanic.update();
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
