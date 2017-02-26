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
            int nbrSanic = 0;

            int hautFen = 900;
            bool fullscreen = false;
            float ratioVoulu = 16f / 9f;

            intro();

            RenderWindow window = new RenderWindow(new VideoMode((uint)(Math.Round(hautFen * ratioVoulu)), (uint)hautFen), "SANIC SPED!!", fullscreen ? Styles.Fullscreen : /**/Styles.None); //Changer "Styles.None" pour "Styles.Close" pour récupérer le bouton pour fermer la fenêtre

            window.Closed += new EventHandler(OnClose);
            window.SetFramerateLimit(60);
            window.SetKeyRepeatEnabled(false);

            Music teme = nbrSanic <= 0 ? new Music(@"..\..\Ressources\gloria.wav") : new Music(@"..\..\Ressources\SanicMusic.wav");
            teme.Volume = 10;
            teme.Loop = true;

            Sprite background = new Sprite(new Texture(@"..\..\Ressources\Background.jpg"));
            background.Scale = new Vector2f(window.Size.X / background.GetLocalBounds().Width, window.Size.Y / background.GetLocalBounds().Height);


            if (nbrSanic > 0)
            {
                teme.Volume = 10;
                //Liste pour le loll MOUHAHAHAHAHA
                List<Sanic> sanic = new List<Sanic>();
                for (int ji = 0; ji < nbrSanic; ji++)
                {
                    sanic.Add(new Sanic(window));
                    float posX = ((window.Size.X - sanic[ji].Size.X) * (ji + 1) / (1 + nbrSanic));
                    sanic[ji].Position = new Vector2f(posX, 0);
                }
                teme.Play();
                sanic[0].Quote(0);

                while (window.IsOpen)
                {
                    window.DispatchEvents();

                    window.Clear();
                    window.Draw(background);
                    foreach (Sanic s in sanic)
                    {
                        s.Update();
                        window.Draw(s);
                    }
                    window.Display();
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        window.Close();
                    }
                }
            }
            else
            {
                teme.Volume = 60;
                Squidnic s = new Squidnic(window);
                teme.Play();

                while (window.IsOpen)
                {
                    window.DispatchEvents();

                    window.Clear();
                    window.Draw(background);
                    s.update();
                    window.Draw(s);
                    window.Display();
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        window.Close();
                    }
                }

            }
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

        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}