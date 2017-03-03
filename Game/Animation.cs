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
		public Animation(Texture texture, IntRect textureRect)
			: base(texture, textureRect)
		{
		}
	}
}
