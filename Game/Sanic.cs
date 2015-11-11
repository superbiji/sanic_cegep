using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class Sanic : Drawable
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
		private float Rotation = 0;
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

		private Sprite currentSprite;
		private float spen_sped = 0f;
		private Vector2f sanic_sped = new Vector2f(0, 0);

		private readonly RenderWindow window;
		private readonly Sprite sanic;
		private readonly Sprite sanicBall;
		private readonly Sprite sanicDuck;

        private static Sosn bruiit = new Sosn();
        private static Spiirtes imaje = new Spiirtes();

		private readonly List<Sound> sanicQuote = new List<Sound>();
		private readonly Sound spenSound = new Sound();
		private readonly Sound jamp = new Sound();
		private readonly Sound ren = new Sound();
        private readonly Sound bump;
		private readonly Vector2f ACCELERATION_X = new Vector2f(2, 0);
		private readonly Vector2f GRAVITY = new Vector2f(0, 1);

		public Sanic(RenderWindow rw)
		{
			window = rw;

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


			sanic.Origin = new Vector2f(sanic.GetLocalBounds().Width / 2, sanic.GetLocalBounds().Height / 2);
			sanicBall.Origin = new Vector2f(sanicBall.GetLocalBounds().Width / 2, sanicBall.GetLocalBounds().Height / 2);
			sanicDuck.Origin = new Vector2f(sanicDuck.GetLocalBounds().Width / 2, sanicDuck.GetLocalBounds().Height / 2);

			stand();
		}



		private void boost()
		{
			if (spen_sped > 60)
			{
				sanic_sped = new Vector2f(Face() * (spen_sped + 20), sanic_sped.Y);
			}
			run();
		}

		public void collisions()
		{
			if (!isGrounded())
			{
				fall();
			}
			if (Position.X < 0)
			{
                sanic_sped.X = Math.Abs(sanic_sped.X);
				//sanic_sped.X = (int)(Math.Abs(sanic_sped.X) * 0.9); //corrige le bug d'accélération après une collision en forçant un ralentissement... mais c'est moins drôle
                bump.Play();
				orientation = 1;
			}
			else if (Position.X + Size.X > window.Size.X)
			{
                sanic_sped.X = -Math.Abs(sanic_sped.X);
                //sanic_sped.X = (int)(-Math.Abs(sanic_sped.X) * 0.9); //corrige le bug d'accélération après une collision en forçant un ralentissement... mais c'est moins drôle
                bump.Play();
				orientation = -1;
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
			sanic_sped = new Vector2f(0, sanic_sped.Y);
			state = State.Ducking;
		}

		private void ducking()
		{
			raise();
			if ((Keyboard.IsKeyPressed(Keyboard.Key.Right)) || (Keyboard.IsKeyPressed(Keyboard.Key.Left)))
			{
				spin();
			}
			else if (!Keyboard.IsKeyPressed(Keyboard.Key.Down))
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

		private void fall()
		{
			currentSprite = sanicBall;
			ren.Stop();
			state = State.Jumping;
		}

		private bool isGrounded()
		{
			return Position.Y + Size.Y >= window.Size.Y;
		}

		private bool isMovingX()
		{
			return sanic_sped.X != 0;
		}

		private void jump()
		{
			currentSprite = sanicBall;
			ren.Stop();
			sanic_sped += new Vector2f(0, -25);
			jamp.Play();
			state = State.Jumping;
		}

		private void jumping()
		{
			sanic_sped += GRAVITY;
			Rotation += 2.5f * sanic_sped.X / ACCELERATION_X.X;

			if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) 
			{
				sanic_sped += ACCELERATION_X;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
			{
				sanic_sped -= ACCELERATION_X;
			}
			else
			{
				sanic_sped.X = Math.Round(sanic_sped.X) == 0 ? 0 : sanic_sped.X / 1.1f;
			}

			if (isGrounded())
			{
				Position = new Vector2f(Position.X, window.Size.Y - Size.Y);
				sanic_sped.Y = sanic_sped.Y < 0 ? sanic_sped.Y : 0;
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
			currentSprite = sanic;
			ren.Play();
			spen_sped = 0;
			spenSound.Stop();
			state = State.Running;
		}

		private void running()
		{
			raise();
			ren.Pitch = 1f + Math.Abs(sanic_sped.X) / ACCELERATION_X.X / 33;
			if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
			{
				jump();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
			{
				duck();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) 
			{
				sanic_sped += ACCELERATION_X;
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
			{
				sanic_sped -= ACCELERATION_X;
			}
			else
			{
				stand();
			}

			if (sanic_sped.X > 0)
			{
				orientation = 1;
			}
			else if (sanic_sped.X < 0)
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

			//if (spen_sped < 60)
			if (true)  //For funny wierd shit
			{
				spen_sped += 0.3f;
			}
			if (!Keyboard.IsKeyPressed(Keyboard.Key.Down))
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
			sanic_sped.X = Math.Round(sanic_sped.X) == 0 ? 0 : sanic_sped.X / 1.1f;
			if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
			{
				jump();
			}
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
			{
				duck();
			}
			else if ((Keyboard.IsKeyPressed(Keyboard.Key.Right)) || (Keyboard.IsKeyPressed(Keyboard.Key.Left)))
			{
				run();
			}
		}

		public void Update()
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
			Position += sanic_sped;

			UpdateSprite();
		}

		private void UpdateSprite()

		{
			Scale = new Vector2f(orientation,Scale.Y);			//Flip sprite
			currentSprite.Position = Position + currentSprite.Origin;
			currentSprite.Rotation = Rotation;
			currentSprite.Scale = Scale;
		}
	}
}
