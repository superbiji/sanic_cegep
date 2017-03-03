using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class Animation : Sprite
	{
		private double counter = 0;

		public Animation(Texture texture, IntRect textureRect)
			: base(texture, textureRect)
		{
		}

		public void next()
		{
			TextureRect = new IntRect((TextureRect.Left + TextureRect.Width) % (int)Texture.Size.X, TextureRect.Top, TextureRect.Width, TextureRect.Height);
		}

		public void next(double partial)
		{
			counter += partial;

			if (counter > 1)
			{
				next();

				counter %= 1;
			}
		}
	}
}
