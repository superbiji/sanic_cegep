using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public abstract class Player : Updatable, Drawable
	{
		public FloatRect CollisionRect;

		public Player(FloatRect collisionRect)
		{
			CollisionRect = new FloatRect(collisionRect.Left, collisionRect.Top, collisionRect.Width, collisionRect.Height);
		}

		public abstract int update(int elapsedMilliseconds);

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
