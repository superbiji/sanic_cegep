using SFML.Audio;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Sosn
    {
        public readonly List<Sound> sanicQuote = new List<Sound>();
	    public readonly Sound spenSound = new Sound();
	    public readonly Sound jamp = new Sound();
	    public readonly Sound ren = new Sound();
        public readonly Sound bump;
        public readonly Sound teme;

        public Sosn()
        {
		    sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\sanicQuote.wav")));
		    sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\Intro.wav")));
		    sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\GottaGoFast.wav")));
		    jamp.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanic_jamp.wav");
		    ren.SoundBuffer = new SoundBuffer(@"..\..\Ressources\sanic_ren.wav");
		    spenSound.SoundBuffer = new SoundBuffer(@"..\..\Ressources\SanicSpen.wav");
            bump = new Sound(new SoundBuffer(@"..\..\Ressources\bump.wav"));
            teme = new Sound(new SoundBuffer(@"..\..\Ressources\SanicMusic.wav"));
        }

        public void playTeme()
        {
            teme.Volume = 10;
            teme.Loop = true;
            teme.Play();
        }

        public void stopTeme()
        {
            teme.Stop();
        }
    }
}
