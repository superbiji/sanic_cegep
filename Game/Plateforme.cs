using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class Plateforme : RectangleShape, Collisionable
	{
		public FloatRect CollisionRect
		{
			get
			{
				return new FloatRect(Position, Size);
			}
			protected set
			{
				Position = new Vector2f(value.Left, value.Top);
				Size = new Vector2f(value.Width, value.Height);
			}
		}

		public Plateforme(Vector2f size)
			: base(size)
		{
		}

		public Plateforme(RectangleShape copy)
			: base(copy)
		{
		}

		public void collision(Collisionable collisionable, CollisionDirection collisionDirection)
		{
		}
	}
}
