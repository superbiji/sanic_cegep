﻿// NEGA production
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
        const int nbrSanic = 500;
        const int largFen = 1120;
        const float ratioVoulu = 16f / 9f;

        private static RenderWindow window;
        private static Sprite background;

		static int Main()
        {
            intro();
            initFenetre();

            //Liste pour le loll MOUHAHAHAHAHA
            List<Sanic> sanic = new List<Sanic>();
            for (int ji = 0; ji < nbrSanic; ji++ )
            {
                sanic.Add(new Sanic(window));
                float posX = ((window.Size.X - sanic[ji].Size.X) * (ji + 1) / (1 + nbrSanic));
                sanic[ji].Position = new Vector2f(posX, 0);
            }
            
            sanic[0].playTeme();
            sanic[0].Quote(0);

			while (window.IsOpen)
			{
				window.Clear();
				window.Draw(background);
                foreach (Sanic s in sanic)
                {
                    s.Update();
				    window.Draw(s);
                }
				window.Display();
				window.DispatchEvents();
			}

            sanic[0].stopTeme();
            sanic.Clear();
            window.Dispose();
            return 0;
		}

        static void intro()
        {
            //Intro Nega de NEGA nigga bitch nega!
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

            splashScreen.Display();
            scream.ElementAt(i).Play();
            System.Threading.Thread.Sleep(2000);
            
            scream.Clear();
            nega.Dispose();
            splashScreen.Close();
            splashScreen.Dispose();
        }

        static void initFenetre()
        {
            window = new RenderWindow(new VideoMode((uint)largFen, (uint)(Math.Round(largFen / ratioVoulu))), "SANIC SPED!!", Styles.Close);
            window.Closed += new EventHandler(OnClose);
            window.SetFramerateLimit(60);
            window.SetKeyRepeatEnabled(false);

            background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
            background.Scale = new Vector2f(window.Size.X / background.GetLocalBounds().Width, window.Size.Y / background.GetLocalBounds().Height);
        }



        //évènements
        static void OnClose(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }
	}
}