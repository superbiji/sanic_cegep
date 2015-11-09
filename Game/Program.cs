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
			//Intro Nega de NEGA nigga bitch nega!
			Random rand = new Random((int)Math.Round((Double)(Mouse.GetPosition().X / (DateTime.Today.Second+3))));
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
			//fin de l'intro-------------------------------------------------------------------

			RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SANIC SPED!!", Styles.Close);
			window.Closed += new EventHandler(OnClose);
			window.SetFramerateLimit(60);
			window.SetKeyRepeatEnabled(false);

			Music teme = new Music(@"..\..\Ressources\SanicMusic.wav");
			teme.Volume = 10;
			teme.Loop = true;
		   
			Sprite background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
			background.Scale = new Vector2f(800 / background.GetLocalBounds().Width, 600 / background.GetLocalBounds().Height);

			Sanic sanic = new Sanic(window) { Position = new Vector2f(window.Size.X / 2, 0)};

			teme.Play();
			sanic.Quote(0);

			while (window.IsOpen)
			{
				window.DispatchEvents();


				sanic.Update();

				window.Clear();
				window.Draw(background);
				window.Draw(sanic);
				window.Display();
			}
            return 0;
		}

		static void OnClose(object sender, EventArgs e)
		{
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}
	}
}