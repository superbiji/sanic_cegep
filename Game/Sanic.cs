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
		public Vector2f Position = new Vector2f(0, 0);
		public Vector2f Scale = new Vector2f(1, 1);
		public float Rotation = 0;

		private Sprite currentSprite;
        private bool asMoved = false;
        private bool isSpenning = false;
        private float spen_sped = 0f;
		private Vector2f sanic_sped = new Vector2f(0, 0);

		private readonly RenderWindow window;
		private readonly Sprite sanic;
		private readonly Sprite sanicBall;
		private readonly Sound sanicQuote = new Sound();
		private readonly Sound jamp = new Sound();
		private readonly Sound ren = new Sound();
		private readonly Vector2f VITESSE_X = new Vector2f(2, 0);
		private readonly Vector2f VITESSE_Y = new Vector2f(0, -25);
		private readonly Vector2f GRAVITY = new Vector2f(0, 1);

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

		public Sanic(RenderWindow rw)
		{
			window = rw;

			sanic = new Sprite(new Texture(@"..\..\Ressources\sanic.png"));
			sanicBall = new Sprite(new Texture(@"..\..\Ressources\sanic_ball.png"));
			sanicQuote.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanicQuote.wav");
			jamp.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanic_jamp.wav");
			ren.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanic_ren.wav");

			ren.Loop = true;


			sanic.Origin = new Vector2f(sanic.GetLocalBounds().Width / 2, sanic.GetLocalBounds().Height / 2);
			sanicBall.Origin = new Vector2f(sanicBall.GetLocalBounds().Width / 2, sanicBall.GetLocalBounds().Height / 2);
			currentSprite = sanicBall;
		}

		public void Quote()
		{
			sanicQuote.Play();
		}

		public bool IsGrounded()
		{
			return Position.Y + Size.Y >= window.Size.Y;
		}

		public bool IsMoving()
		{
			return sanic_sped.X != 0;
		}

		public bool isStopped()
		{
			return IsGrounded() && !IsMoving();
		}

		private int Face()
		{
			return sanic_sped.X < 0 ? -1 : 1;
		}

        public void spen(Action pDirection)
        {
            isSpenning = true;
			switch (pDirection)
			{
				case Action.Left:
					Scale = new Vector2f(-1, 1);
					break;
				case Action.Right:
					Scale = new Vector2f(1, 1);
					break;
			}
        }

		public void Jump()
		{
			if (IsGrounded())
			{
				jamp.Play();
				sanic_sped += VITESSE_Y;
			}
		}

		public void Move(Action direction)
		{
			switch (direction)
			{
				case Action.Right:
					Move(-VITESSE_X);
					break;
				case Action.Left:
					Move(VITESSE_X);
					break;
				case Action.Up:
					Jump();
					break;
				default:
					break;
			}
		}

		public void Move(Vector2f speed)
		{
			sanic_sped -= speed;
			asMoved = true;
		}

		public void Update()
		{
			if (!asMoved)
			{
				sanic_sped.X = Math.Round(sanic_sped.X) == 0 ? 0 : sanic_sped.X / 1.1f;
			}

			if (Position.X < 0)
			{
				//sanic.Position = new Vector2f(0, sanic.Position.Y);
				sanic_sped.X = Math.Abs(sanic_sped.X);
			}
			else if (Position.X + Size.X > window.Size.X)
			{
				//sanic.Position = new Vector2f(window.Size.X - sanic.GetGlobalBounds().Width, sanic.Position.Y);
				sanic_sped.X = -Math.Abs(sanic_sped.X);
			}

			if (IsGrounded())
			{
				Position = new Vector2f(Position.X, window.Size.Y - Size.Y);
				sanic_sped.Y = sanic_sped.Y < 0 ? sanic_sped.Y : 0;
                
                if (!isSpenning)
                {
                    currentSprite = sanic;
				    if (Math.Abs(Rotation) > 180)
				    {
					    Rotation -= Math.Sign(Rotation) * 360;
				    }
				    Rotation /= 1.1f;
                    spen_sped = 0;
                }
                else
                {
                    currentSprite = sanicBall;
                    Rotation += Scale.X*(15 + spen_sped);
                    if (spen_sped < 60)
                    {
                        spen_sped += 0.3f;
                    }
                }
			}
			else
			{
				currentSprite = sanicBall;
				sanic_sped += GRAVITY;
				Rotation += 2.5f * sanic_sped.X / VITESSE_X.X;
			}

			if (IsGrounded() && IsMoving())
			{
				ren.Pitch = 1f + Math.Abs(sanic_sped.X) / VITESSE_X.X / 33;
				if (ren.Status == SoundStatus.Stopped)
				{
					ren.Play();
				}
			}
			else
			{
				ren.Stop();
			}

			//Flip sprite
			if (IsMoving())
			{
				Scale = new Vector2f(Face(), 1);
			}
			
			Position += sanic_sped;

			UpdateSprite();
			 
			asMoved = false;
            isSpenning = false;
		}

		private void UpdateSprite()
		{
			currentSprite.Position = Position + currentSprite.Origin;
			currentSprite.Rotation = Rotation;
			currentSprite.Scale = Scale;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			currentSprite.Draw(target, states);
		}
	}
}
