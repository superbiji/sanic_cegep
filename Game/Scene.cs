using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public abstract class Scene : Updatable, Drawable
	{
		public static readonly int NO_STATE = 0;
		public static readonly int EXIT_STATE = 1;

		protected RenderWindow window;
		protected int state = NO_STATE;

		public Scene(RenderWindow rw)
		{
			window = rw;
		}

		public int update(int elapsedMilliseconds)
		{
			return state;
		}

		public void exit()
		{
			state = EXIT_STATE;
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
