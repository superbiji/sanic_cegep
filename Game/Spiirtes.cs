using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
	public class Spiirtes
	{
		public readonly Texture sheet;
		public readonly Sprite sanic;
        public readonly Sprite sanicBall;
        public readonly Sprite sanicBallRedi;
		public readonly Sprite sanicDuck;
		public readonly Sprite squidBody;
		public readonly Sprite squidFace;


		public Spiirtes()
		{
			sheet = new Texture(@"..\..\Ressources\sanic_sheet.png");
			sanic = new Sprite(new Texture(@"..\..\Ressources\sanic.png"));
            sanicBall = new Sprite(new Texture(@"..\..\Ressources\sanic_ball.png"));
            sanicBallRedi = new Sprite(new Texture(@"..\..\Ressources\sanic_ball_redi.png"));
			sanicDuck = new Sprite(new Texture(@"..\..\Ressources\sanicDuck.png"));
			squidBody = new Sprite(new Texture(@"..\..\Ressources\squidnic_Bahdy.png"));
			squidFace = new Sprite(new Texture(@"..\..\Ressources\squidnic_Fasse.png"));
		}
	}
}
