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
	public class Sanic : Updatable, Drawable
	{
		private enum State
		{
			Standing,
			Running,
			Jumping,
			Ducking,
			Spinning,
		}

		public Vector2f Position = new Vector2f(0, 0);
		private Vector2f Scale = new Vector2f(1, 1);
		private int orientation = 1; // 1: orienté à droite, -1: orienté à gauche
		public float Rotation
		{
			get;
			private set;
		}
		private State state = State.Standing;
		
		public Vector2f Size
		{
			get
			{
				return new Vector2f(currentSprite.GetLocalBounds().Width, currentSprite.GetLocalBounds().Height);
			}
			set
			{
				currentSprite.Scale = new Vector2f(value.X / currentSprite.GetLocalBounds().Width, value.Y / currentSprite.GetLocalBounds().Height);
			}
		}

		private bool grounded = false;
		public bool Grounded
		{
			get
			{
				return grounded;
			}
			set
			{
				grounded = value;
				if (grounded)
				{
					Speed.Y = 0;
				}
			}
		}

		private Animation sheet;
		private Sprite currentSprite;
		private float spen_sped = 0f;
		public Vector2f Speed;

		public FloatRect boundaries
		{
			get
			{
				return currentSprite.GetGlobalBounds();
			}
			private set
			{
			}
		}
		private readonly Sprite sanic;
		private readonly Sprite sanicBall;
		private readonly Sprite sanicDuck;

		static private Sosn bruiit = new Sosn();
		private static Spiirtes imaje = new Spiirtes();

		private readonly List<Sound> sanicQuote = new List<Sound>();
		private readonly Sound spenSound;
		private readonly Sound jamp;
		private readonly Sound ren;
		private readonly Sound bump;
		private readonly Vector2f ACCELERATION_X = new Vector2f(0.5f, 0);
		private readonly Vector2f GRAVITY = new Vector2f(0, 1);

		public Sanic()
		{
			sheet = new Animation(imaje.sheet, new IntRect(0, 0, 162, 170));

			sanic = imaje.sanic;
			sanicBall = imaje.sanicBall;
			sanicDuck = imaje.sanicDuck;			
			
			sanicQuote = bruiit.sanicQuote;
			jamp = bruiit.jamp;
			ren = bruiit.ren;
			spenSound = bruiit.spenSound;
			bump = bruiit.bump;

			bump.Loop = false;
			ren.Loop = true;
			spenSound.Loop = true;

			Rotation = 0;
			Speed = new Vector2f(0, 0);
			sanic.Origin = new Vector2f(sanic.GetLocalBounds().Width / 2, sanic.GetLocalBounds().Height / 2);
			sanicBall.Origin = new Vector2f(sanicBall.GetLocalBounds().Width / 2, sanicBall.GetLocalBounds().Height / 2);
			sanicDuck.Origin = new Vector2f(sanicDuck.GetLocalBounds().Width / 2, sanicDuck.GetLocalBounds().Height / 2);
			sheet.Origin = new Vector2f(sheet.GetLocalBounds().Width / 2, sheet.GetLocalBounds().Height / 2);

			stand();
		}

		private void boost()
		{
			if (spen_sped > 60)
			{
				Speed.X = Face() * (spen_sped + 20);
			}
			run();
		}

		public void turnLeft()
		{
			Speed.X = -Math.Abs(Speed.X);
			//sanic_sped.X = (int)(-Math.Abs(sanic_sped.X) * 0.9); //corrige le bug d'accélération après une collision en forçant un ralentissement... mais c'est moins drôle
			bump.Play();
			orientation = -1;
		}

		public void turnRight()
		{
			Speed.X = Math.Abs(Speed.X);
			//sanic_sped.X = (int)(Math.Abs(sanic_sped.X) * 0.9); //corrige le bug d'accélération après une collision en forçant un ralentissement... mais c'est moins drôle
			bump.Play();
			orientation = 1;
		}

		public void collisions()
		{
			if (!Grounded)
			{
				fall();
			}
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			currentSprite.Draw(target, states);
		}
	
		private void duck()
		{
			currentSprite = sanicDuck;
			ren.Stop();
			spen_sped = 0;
			spenSound.Stop();
			Speed.X = 0;
			state = State.Ducking;
		}

		private void ducking()
		{
			raise();
			if ((Keyboard.IsKeyPressed(Keyboard.Key.D)) || (Keyboard.IsKeyPressed(Keyboard.Key.A)))
			{
				spin();
			}
			else if (!Keyboard.IsKeyPressed(Keyboard.Key.S))
			{
				stand();
			}
		}

		public int duckWidth()
		{
			return (int)(sanicDuck.Texture.Size.X);
		}

		private int Face()
		{
			return Scale.X < 0 ? -1 : 1;
		}

		protected void fall()
		{
			currentSprite = sanicBall;
			ren.Stop();
			state = State.Jumping;
		}

		private bool isMovingX()
		{
			return Speed.X != 0;
		}

		private void jump()
		{
			currentSprite = sanicBall;
			ren.Stop();
			Speed.Y += -30;
			jamp.Play();
			state = State.Jumping;
		}

		private void jumping()
		{
			Speed += GRAVITY;
			Rotation += 2.5f * Speed.X / ACCELERATION_X.X;

			if (Keyboard.IsKeyPressed(Keyboard.Key.D)) 
			{
				Speed += ACCELERATION_X;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
			{
				Speed -= ACCELERATION_X;
			}
			else
			{
				Speed.X = Math.Round(Speed.X) == 0 ? 0 : Speed.X / 1.1f;
			}

			if (Grounded)
			{
				Speed.Y = Speed.Y < 0 ? Speed.Y : 0;
				if (isMovingX())
				{
					run();
				}
				else
				{
					stand();
				}
			}
		}

		public void Quote()
		{
			bool playing = false;
			foreach (Sound sound in sanicQuote)
			{
				if(sound.Status == SoundStatus.Playing)
				{
					playing = true;
					break;
				}
			}
			if (!playing)
			{
				Random r = new Random();
				Quote(r.Next(sanicQuote.Count));
			}
		}

		public void Quote(int i)
		{
			sanicQuote.ElementAt(i).Play();
		}

		private void raise()
		{
			if (Math.Abs(Rotation) > 180)
			{
				Rotation -= Math.Sign(Rotation) * 360;
			}
			Rotation /= 1.1f;
		}

		private void run()
		{
			currentSprite = sheet;
			ren.Play();
			spen_sped = 0;
			spenSound.Stop();
			state = State.Running;
		}

		private void running()
		{
			raise();
			ren.Pitch = 1f + Math.Abs(Speed.X) / ACCELERATION_X.X / 33;
			sheet.next(0.5);
			if (Keyboard.IsKeyPressed(Keyboard.Key.W))
			{
				jump();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
			{
				duck();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.D)) 
			{
				Speed += ACCELERATION_X;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
			{
				Speed -= ACCELERATION_X;
			}
			else
			{
				stand();
			}

			if (Speed.X > 0)
			{
				orientation = 1;
			}
			else if (Speed.X < 0)
			{
				orientation = -1;
			}
		}

		private void spin()
		{
			currentSprite = sanicBall;
			if (spenSound.Status == SoundStatus.Stopped)
			{
				spenSound.Play();
			}
			state = State.Spinning;
		}

		private void spinning()
		{
			if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
			{
				orientation = 1;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
			{
				orientation = -1;
			}
			Rotation += orientation * (15 + spen_sped);
			spenSound.Pitch = 1 + (spen_sped / 30);

			if (spen_sped < 60)
			//if (true)  //For funny wierd shit
			{
				spen_sped += 0.3f;
			}
			if (!Keyboard.IsKeyPressed(Keyboard.Key.S))
			{
				boost();
			}
		}

		private void stand()
		{
			currentSprite = sanic;
			ren.Stop();
			state = State.Standing;
		}

		private void standing()
		{
			raise();
			Speed.X = Math.Round(Speed.X) == 0 ? 0 : Speed.X / 1.1f;
			if (Keyboard.IsKeyPressed(Keyboard.Key.W))
			{
				jump();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
			{
				duck();
			}
			else if ((Keyboard.IsKeyPressed(Keyboard.Key.D)) || (Keyboard.IsKeyPressed(Keyboard.Key.A)))
			{
				run();
			}
		}
		

		private void UpdateSprite()
		{
			Scale.X = orientation;			//Flip sprite
			currentSprite.Scale = Scale;

			currentSprite.Position = Position + currentSprite.Origin;
			currentSprite.Rotation = Rotation;
		}

		public int update(int elapsedMilliseconds)
		{
			switch (state)
			{
				case State.Standing:
					standing();
					break;
				case State.Running:
					running();
					break;
				case State.Jumping:
					jumping();
					break;
				case State.Ducking:
					ducking();
					break;
				case State.Spinning:
					spinning();
					break;
			}

			if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
			{
				Quote();
			}

			collisions();
			Position += Speed;

			UpdateSprite();

			return 0;
		}
	}
}
