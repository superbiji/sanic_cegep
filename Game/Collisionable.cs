using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public enum CollisionDirection
	{
		NONE,
		LEFT,
		RIGHT,
		UP,
		DOWN
	}

	public interface Collisionable
	{
		FloatRect CollisionRect
		{
			get;
		}

		void collision(Collisionable collisionable, CollisionDirection collisionDirection);
	}
}