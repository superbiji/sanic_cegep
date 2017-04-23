using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public abstract class Player : Collisionable, Updatable, Drawable
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
		protected Queue<Tuple<Collisionable, CollisionDirection>> collisionables = new Queue<Tuple<Collisionable, CollisionDirection>>();
		public Vector2f Speed = new Vector2f(0, 0);

		public Player(FloatRect collisionRect)
		{
			CollisionRect = new FloatRect(collisionRect.Left, collisionRect.Top, collisionRect.Width, collisionRect.Height);
		}

		public virtual void collision(Collisionable collisionable, CollisionDirection collisionDirection)
		{
			collisionables.Enqueue(new Tuple<Collisionable, CollisionDirection>(collisionable, collisionDirection));
		}

		public abstract int update(int elapsedMilliseconds);
		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
