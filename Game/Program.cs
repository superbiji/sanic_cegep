// NEGA production
// Present : Sanic au pays de la roulette !
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
	static class Program
	{
		static int Main()
		{
			UInt32 hautFen = 600;
			double ratioVoulu = 16.0 / 9.0;
			UInt32 largeurFen = (UInt32)(hautFen * ratioVoulu);

			intro();

			Game game = new Game(largeurFen, hautFen, "test");

			game.loop();

			return 0;
		}

		static void intro()
		{
			Random rand = new Random((int)Math.Round((Double)(Mouse.GetPosition().X / (DateTime.Today.Second + 3))));
			Sprite nega = new Sprite(new Texture(@"..\..\Ressources\nEGA.png"));
			RenderWindow splashScreen = new RenderWindow(new VideoMode((uint)(nega.GetGlobalBounds().Width + 300),
																	   (uint)(nega.GetGlobalBounds().Height + 300)),
																		"", Styles.None);
			splashScreen.Clear(Color.Blue);

			nega.Position = new Vector2f(150, 150);
			splashScreen.Draw(nega);

			List<Sound> scream = new List<Sound>();
			scream.Add(new Sound(new SoundBuffer(@"..\..\Ressources\Intro.wav")));
			scream.Add(new Sound(new SoundBuffer(@"..\..\Ressources\Intro2.wav")));

			int i = rand.Next(scream.Count);
			scream.ElementAt(i).Loop = false;

			scream.ElementAt(i).Play();

			splashScreen.Display();


			System.Threading.Thread.Sleep(2000);
			splashScreen.Close();
		}
	}
}