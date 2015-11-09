using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Spiirtes
    {
        public readonly Sprite sanic;
        public readonly Sprite sanicBall;
        public readonly Sprite sanicDuck;


        public Spiirtes()
        {
            sanic = new Sprite(new Texture(@"..\..\Ressources\sanic.png"));
            sanicBall = new Sprite(new Texture(@"..\..\Ressources\sanic_ball.png"));
            sanicDuck = new Sprite(new Texture(@"..\..\Ressources\sanicDuck.png"));
        }
    }
}
