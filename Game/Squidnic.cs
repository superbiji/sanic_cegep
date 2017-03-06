using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
	public class Squidnic : Updatable, Drawable
	{
		private enum State
		{
			Standing,
			Running,
			Jumping,
			Ducking,
			Spinning,
		}

		static private readonly Sosn bruiit = new Sosn();
		static private readonly Spiirtes imajes = new Spiirtes();

		private readonly RenderWindow window;
		private Sprite currentSpriteBody;
		private Sprite currentSpriteFace;

		public FloatRect boundaries
		{
			get
			{
				return currentSpriteBody.GetGlobalBounds();
			}
		}
		public Vector2f Size
		{
			get
			{
				return new Vector2f(currentSpriteBody.Origin.X, currentSpriteBody.Origin.Y);
			}
		}
		public Vector2f Origin
		{
			get
			{
				return currentSpriteBody.Origin;
			}
		}

		private int orientation = 1; // 1: orienté à droite, -1: orienté à gauche
		private float Rotation = 0;
	  //  private State state = State.Standing;

		public Vector2f Position = new Vector2f(200, 200);
		private Vector2f Scale = new Vector2f(1, 1);
		public Vector2f Speed = new Vector2f(0, 0);
		private readonly Vector2f ACCELERATION_X = new Vector2f(1, 0);
		private readonly Vector2f GRAVITY = new Vector2f(0, 1);
		public bool Grounded
		{
			get;
			set;
		}

		public Squidnic(RenderWindow rw)
		{
			window = rw;
			currentSpriteBody = imajes.squidBody;
			currentSpriteBody.Origin = new Vector2f(imajes.squidBody.GetLocalBounds().Width / 2, imajes.squidBody.GetLocalBounds().Height / 2);

			currentSpriteFace = imajes.squidFace;
			currentSpriteFace.Origin = new Vector2f(-2+(imajes.squidFace.GetLocalBounds().Width / 2), -1+(imajes.squidFace.GetLocalBounds().Height / 2));

			bruiit.squid_step.Loop = true;
			bruiit.squid_step.Volume = 10;

			Grounded = false;
	}

		public void Draw(RenderTarget target, RenderStates states)
		{
			currentSpriteBody.Draw(target, states);
			currentSpriteFace.Draw(target, states);
		}

		private void UpdateSprite()
		{
			Scale.X = orientation;			//Flip sprite
			currentSpriteBody.Position = Position;
			currentSpriteBody.Rotation = Rotation;
			currentSpriteBody.Scale = Scale;

			currentSpriteFace.Position = Position;
			currentSpriteFace.Scale = Scale;
		}

		private void fall()
		{
			Speed += GRAVITY;
		}

		public void turn()
		{
			orientation *= -1;
		}

		private void collision()
		{
			if (!Grounded)
			{
				fall();
			}
			else
			{
				Speed.Y = Speed.Y < 0 ? Speed.Y : 0;
			}
		}

		private void running()
		{
			Speed += ACCELERATION_X;
		}

		private void run()
		{
			if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
			{
				orientation = 1;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
			{
				orientation = -1;
			}

			bruiit.squid_step.Pitch = 1f + Math.Abs(Speed.X) / ACCELERATION_X.X / 33;
			bruiit.squid_step.Play();
			running();
		}

		private void stand()
		{
			Speed.X /= 1.05f;
		}

		private void jump()
		{
			Speed.Y = -30;
			bruiit.jamp.Play();
		}

		public void playTeme()
		{
			bruiit.gloria.Loop = true;
			bruiit.gloria.Volume = 20;
			bruiit.gloria.Play();
		}

		public void stopTeme()
		{
			bruiit.gloria.Stop();
		}

		public int update(int elapsedMilliseconds)
		{
			if ((Keyboard.IsKeyPressed(Keyboard.Key.Right)) || (Keyboard.IsKeyPressed(Keyboard.Key.Left)))
			{
				run();
			}
			else if ((Keyboard.IsKeyPressed(Keyboard.Key.Up)) && Grounded)
			{
				jump();
			}
			else
			{
				stand();
			}
			bruiit.ren.Pitch = 1f + Math.Abs(Speed.X) / ACCELERATION_X.X / 33;
			bruiit.squid_step.Pitch = 1f + Math.Abs(Speed.X) / ACCELERATION_X.X / 33;


			Rotation = Position.X;
			currentSpriteBody.Rotation = Rotation;
			collision();
			Position.X += orientation * Speed.X;
			Position.Y += Speed.Y;
			UpdateSprite();

			return 0;
		}
	}
}
