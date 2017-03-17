using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using SFML.System;

namespace Game
{
	public class Sanic : Player
	{
		public enum State
		{
			Standing,
			Running,
			Jumping,
			Ducking,
			Spinning,
		}

		protected int orientation = 1; // 1: orienté à droite, -1: orienté à gauche
		protected float rotation = 0;
		protected State state = State.Standing;

		//DEBUG
		public State getState()
		{
			return state;
		}

		public Vector2f Speed = new Vector2f(0, 0);

		private bool boostRedi = false;

		private Animation sheet;
		private Sprite currentSprite;
		private float spen_sped = 0f;

		public FloatRect SpriteRect
		{
			get
			{
				return currentSprite.GetGlobalBounds();
			}
		}
		private readonly Sprite sanic;
		private readonly Sprite sanicBall;
		private readonly Sprite sanicBallRedi;
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

		public Sanic(Vector2f position)
			: base(new FloatRect(position, new Vector2f(100, 170)))
		{
			sheet = new Animation(imaje.sheet, new IntRect(0, 0, 162, 170));

			sanic = imaje.sanic;
			sanicBall = imaje.sanicBall;
			sanicBallRedi = imaje.sanicBallRedi;
			sanicDuck = imaje.sanicDuck;

			sanicQuote = bruiit.sanicQuote;
			jamp = bruiit.jamp;
			ren = bruiit.ren;
			spenSound = bruiit.spenSound;
			bump = bruiit.bump;

			bump.Loop = false;
			ren.Loop = true;
			spenSound.Loop = true;

			sanic.Origin = new Vector2f(sanic.GetLocalBounds().Width / 2, sanic.GetLocalBounds().Height / 2);
			sanicBall.Origin = new Vector2f(sanicBall.GetLocalBounds().Width / 2, sanicBall.GetLocalBounds().Height / 2);
			sanicBallRedi.Origin = new Vector2f(sanicBallRedi.GetLocalBounds().Width / 2, sanicBallRedi.GetLocalBounds().Height / 2);
			sanicDuck.Origin = new Vector2f(sanicDuck.GetLocalBounds().Width / 2, sanicDuck.GetLocalBounds().Height / 2);
			sheet.Origin = new Vector2f(sheet.GetLocalBounds().Width / 2, sheet.GetLocalBounds().Height / 2);

			stand();
		}

		private void boost()
		{
			if (spen_sped > 60)
			{
				Speed.X = orientation * (spen_sped + 20);
			}
			run();
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
				orientation = Keyboard.IsKeyPressed(Keyboard.Key.D) ? 1 : -1;
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

		protected void fall()
		{
			currentSprite = sanicBall;
			ren.Stop();
			state = State.Jumping;
		}

		public override bool isFalling()
		{
			return Speed.Y >= 0;
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
			rotation += 2.5f * Speed.X / ACCELERATION_X.X;

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
				if (sound.Status == SoundStatus.Playing)
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
			if (Math.Abs(rotation) > 180)
			{
				rotation -= Math.Sign(rotation) * 360;
			}
			rotation /= 1.1f;
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
			rotation += orientation * (15 + spen_sped);
			spenSound.Pitch = 1 + (spen_sped / 30);

			if (spen_sped < 60)/*
			if (true)  //*///For funny wierd shit
			{
				spen_sped += 0.3f;
				boostRedi = false;
			}
			else if (!boostRedi)
			{
				currentSprite = sanicBallRedi;
				Quote(2);
				boostRedi = true;
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

		public void collisions()
		{
			Grounded = false;
			while (collisionables.Count > 0)
			{
				Tuple<Collisionable, CollisionDirection> tuple = collisionables.Dequeue();
				Collisionable collisionable = tuple.Item1;
				CollisionDirection collisionDirection = tuple.Item2;

				if ((collisionable is Plateforme) && (collisionDirection == CollisionDirection.DOWN))
				{
					Grounded = true;

					Plateforme plateforme = collisionable as Plateforme;
					collisionRect.Top = plateforme.GetGlobalBounds().Top - collisionRect.Height + 1;
				}
				else if (collisionable is SanicLevel.Boundaries)
				{
					FloatRect boundaries = (collisionable as SanicLevel.Boundaries).CollisionRect;

					switch (collisionDirection)
					{
						case CollisionDirection.DOWN:
							{
								Grounded = true;

								collisionRect.Top = boundaries.Top + boundaries.Height - collisionRect.Height;
							}
							break;
						case CollisionDirection.RIGHT:
							{
								orientation = -1;
								collisionRect.Left = boundaries.Left + boundaries.Width - collisionRect.Width;
								Speed.X = orientation * Math.Abs(Speed.X) * 0.9f;

								if (Math.Abs(Speed.X) > 3)
								{
									bump.Play();
								}
								else
								{
									orientation = 1;
									Speed.X = 0;
								}
							}
							break;
						case CollisionDirection.LEFT:
							{
								orientation = 1;
								collisionRect.Left = boundaries.Left;
								Speed.X = orientation * Math.Abs(Speed.X) * 0.9f;

								if (Math.Abs(Speed.X) > 3)
								{
									bump.Play();
								}
								else
								{
									orientation = -1;
									Speed.X = 0;
								}
							}
							break;
					}
				}
			}

			if (!Grounded)
			{
				fall();
			}
			else
			{
				Speed.Y = Speed.Y < 0 ? Speed.Y : 0;
			}
		}

		public override int update(int elapsedMilliseconds)
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

			collisionRect.Left += Speed.X;
			collisionRect.Top += Speed.Y;
			collisions();

			return 0;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			//Sprite centré. Va falloir trouver une autre facon générique de gérer les hotspots
			currentSprite.Position = new Vector2f(collisionRect.Left - (collisionRect.Width / 2), collisionRect.Top) + currentSprite.Origin;
			currentSprite.Rotation = rotation;
			currentSprite.Scale = new Vector2f(orientation, currentSprite.Scale.Y);  //Flip sprite

			currentSprite.Draw(target, states);

			/*RectangleShape collisionShape = new RectangleShape(new Vector2f(CollisionRect.Width, CollisionRect.Height));
			collisionShape.Position = new Vector2f(CollisionRect.Left, CollisionRect.Top);
			collisionShape.FillColor = Color.Yellow;
			collisionShape.Draw(target, states);*/
		}
	}
}
