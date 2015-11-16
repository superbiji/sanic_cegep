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
    public class Squidnic : Drawable
    {
        private enum State
        {
            Standing,
            Running,
            Jumping,
            Ducking,
            Spinning,
        }

        private readonly RenderWindow window;
        private readonly Spiirtes imajes = new Spiirtes();
        private Sprite currentSpriteBody;
        private Sprite currentSpriteFace;

        public Vector2f Size
        {
            get
            {
                return new Vector2f(currentSpriteBody.Origin.X, currentSpriteBody.Origin.Y);
            }
        }

        private int orientation = 1; // 1: orienté à droite, -1: orienté à gauche
        private float Rotation = 0;
        private State state = State.Standing;

        public Vector2f Position = new Vector2f(200, 200);
        private Vector2f Scale = new Vector2f(1, 1);
        private Vector2f squid_sped = new Vector2f(0, 0);
        private readonly Vector2f ACCELERATION_X = new Vector2f(2, 0);
        private readonly Vector2f GRAVITY = new Vector2f(0, 1);

        public Squidnic(RenderWindow rw)
        {
            window = rw;
            currentSpriteBody = imajes.squidBody;
            currentSpriteBody.Origin = new Vector2f(imajes.squidBody.GetLocalBounds().Width / 2, imajes.squidBody.GetLocalBounds().Height / 2);

            currentSpriteFace = imajes.squidFace;
            currentSpriteFace.Origin = new Vector2f((imajes.squidFace.GetLocalBounds().Width / 2), imajes.squidFace.GetLocalBounds().Height / 2);
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
            currentSpriteFace.Rotation = Rotation;
            currentSpriteFace.Scale = Scale;
        }

        private void fall()
        {
            squid_sped += GRAVITY;
        }

        private bool isGrounded()
        {
            return Position.Y + currentSpriteBody.Origin.Y >= window.Size.Y;
        }

        private void collision()
        {
            if (!isGrounded())
            {
                fall();
            }
            else
            {
                Position.Y = window.Size.Y - Size.Y;
                squid_sped.Y = squid_sped.Y < 0 ? squid_sped.Y : 0;
            }
        }

        public void update()
        {

            collision();
            Position += squid_sped;
            UpdateSprite();
        }
    }
}
