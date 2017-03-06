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
		public static readonly int BACKGROUND = 0;
		public static readonly int MIDDLEGROUND = 1;
		public static readonly int FOREGROUND = 2;

		public static readonly int NO_STATE = 0;
		public static readonly int EXIT_STATE = 1;

		protected RenderWindow window;
		protected List<Updatable> updatables = new List<Updatable>();
		protected SortedList<int, Drawable> drawables = new SortedList<int, Drawable>(new DuplicateKeyComparer<int>());
		protected int state = NO_STATE;

		public Scene(RenderWindow rw)
		{
			window = rw;
		}

		public virtual int update(int elapsedMilliseconds)
		{
			foreach (Updatable updatable in updatables)
			{
				updatable.update(elapsedMilliseconds);
			}

			return state;
		}

		public virtual void Draw(RenderTarget target, RenderStates states)
		{
			foreach (Drawable drawable in drawables.Values)
			{
				drawable.Draw(target, states);
			}
		}

		public void exit()
		{
			state = EXIT_STATE;
		}

		protected class DuplicateKeyComparer<TKey>
		: IComparer<TKey> where TKey : IComparable
		{
			public int Compare(TKey x, TKey y)
			{
				int result = x.CompareTo(y);

				if (result == 0)
					return 1;   // Handle equality as beeing greater
				else
					return result;
			}
		}
	}
}
