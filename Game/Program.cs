// NEGA production
// Present : Sanic au pays de la roulette !
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Game
{
	static class Program
	{
		static void Main()
		{
            //Intro Nega de NEGA nigga bitch nega!
            Sprite nega = new Sprite(new Texture(@"..\..\Ressources\nEGA.png"));
            RenderWindow splashScreen = new RenderWindow(new VideoMode((uint)(nega.GetGlobalBounds().Width + 300), 
                                                                       (uint)(nega.GetGlobalBounds().Height + 300)), 
                                                         "", Styles.None);
            splashScreen.Clear(new Color(0, 0, 255));
            
            nega.Position = new Vector2f(150, 150);
            splashScreen.Draw(nega);
            Sound scream = new Sound(new SoundBuffer(@"..\..\Ressources\Intro.wav"));
            scream.Loop = false;
            splashScreen.Display();
            scream.Play();
            
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
			sanic.Quote();

			while (window.IsOpen)
			{
				window.DispatchEvents();


				sanic.Update();

				window.Clear();
				window.Draw(background);
				window.Draw(sanic);
				window.Display();
			}
		}

		static void OnClose(object sender, EventArgs e)
		{
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}
	}
}