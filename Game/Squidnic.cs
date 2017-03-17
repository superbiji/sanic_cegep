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
	public enum Orientation { GAUCHE = -1, DROITE = 1 };

	public class Squidnic : Player
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
		
		private Sprite currentSpriteBody;
		private Sprite currentSpriteFace;

		public Vector2f Speed = new Vector2f(0, 0);

		public FloatRect SpriteRect
		{
			get
			{
				return currentSpriteBody.GetGlobalBounds();
			}
		}

		private Orientation orientation = Orientation.GAUCHE;
		private float Rotation = 0;
		//  private State state = State.Standing;

		private readonly Vector2f ACCELERATION_X = new Vector2f(0.8f, 0);
		private readonly Vector2f GRAVITY = new Vector2f(0, 1);

		public Squidnic(Vector2f position)
			: base(new FloatRect(position, new Vector2f(180, 170)))
		{
			currentSpriteBody = imajes.squidBody;
			currentSpriteBody.Origin = new Vector2f(imajes.squidBody.GetLocalBounds().Width / 2, imajes.squidBody.GetLocalBounds().Height / 2);

			currentSpriteFace = imajes.squidFace;
			currentSpriteFace.Origin = new Vector2f(-2 + (imajes.squidFace.GetLocalBounds().Width / 2), -1 + (imajes.squidFace.GetLocalBounds().Height / 2));

			bruiit.squid_step.Loop = true;
			bruiit.squid_step.Volume = 30;

			Grounded = false;
		}

		private void fall()
		{
			Speed += GRAVITY;
		}

		private void collision()
		{
			if (!Grounded)
			{
				fall();
			}
			else
			{
				Speed.Y = Math.Min(Speed.Y, 0);
			}
		}

		private void run(Orientation pOri)
		{
			if (true)
			{
				orientation = pOri;
			}

			if (Math.Abs(Speed.X) <= 60)
			{
				Speed += ((int)orientation)*ACCELERATION_X;
			}
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

		public override bool isFalling()
		{
			return Speed.Y >= 0;
		}

		public override void collision(Collisionable collisionable, CollisionDirection collisionDirection)
		{
			if (collisionDirection == CollisionDirection.NONE)
			{
				Grounded = false;
			}
			else if ((collisionable is Plateforme) && (collisionDirection == CollisionDirection.DOWN))
			{
				Grounded = true;

				Plateforme plateforme = collisionable as Plateforme;
				collisionRect.Top = plateforme.GetGlobalBounds().Top - collisionRect.Height + 1;
			}
			else if (collisionable is SanicLevel.Boundaries)
			{
				switch (collisionDirection)
				{
					case CollisionDirection.DOWN:
						{
							Grounded = true;

							SanicLevel.Boundaries boundaries = collisionable as SanicLevel.Boundaries;
							collisionRect.Top = boundaries.CollisionRect.Height - collisionRect.Height;
						}
						break;
					case CollisionDirection.LEFT:
						{
							orientation = Orientation.GAUCHE;
							Speed.X = (((int)orientation) * Math.Max(Math.Abs(Speed.X), 1f)) * 0.9f;
							bruiit.bump.Play();
						}
						break;
					case CollisionDirection.RIGHT:
						{
							orientation = Orientation.DROITE;
							Speed.X = (((int)orientation) * Math.Max(Math.Abs(Speed.X), 1f)) * 0.9f;
							bruiit.bump.Play();
						}
						break;
				}
			}
		}

		private void updateStep()
		{
			bruiit.squid_step.Pitch = 1f + Math.Abs(Speed.X) / 60;
			if ((Math.Abs(Speed.X) < 1) || (!Grounded))
			{
				bruiit.squid_step.Stop();
			}
			else if (bruiit.squid_step.Status == SoundStatus.Stopped)
			{
				bruiit.squid_step.Play();
			}
		}

		public override int update(int elapsedMilliseconds)
		{
			if ((Keyboard.IsKeyPressed(Keyboard.Key.Right)) || (Keyboard.IsKeyPressed(Keyboard.Key.Left)))
			{
				run(Keyboard.IsKeyPressed(Keyboard.Key.Right) ? Orientation.DROITE : Orientation.GAUCHE);
			}
			else
			{
				stand();
			}
			if ((Keyboard.IsKeyPressed(Keyboard.Key.Up)) && Grounded)
			{
				jump();
			}
			updateStep();
			collision();
			collision(null, CollisionDirection.NONE);

			collisionRect.Left += Speed.X;
			collisionRect.Top += Speed.Y;

			return 0;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			currentSpriteBody.Position = new Vector2f(CollisionRect.Left, CollisionRect.Top) + currentSpriteBody.Origin;
			currentSpriteBody.Rotation = CollisionRect.Left;
			currentSpriteBody.Scale = new Vector2f((float)orientation, currentSpriteBody.Scale.Y); //Flip sprite

			currentSpriteFace.Position = new Vector2f(CollisionRect.Left, CollisionRect.Top) + currentSpriteBody.Origin;
			currentSpriteFace.Scale = new Vector2f((float)orientation, currentSpriteFace.Scale.Y);//Flip sprite

			currentSpriteBody.Draw(target, states);
			currentSpriteFace.Draw(target, states);
		}
	}
}
