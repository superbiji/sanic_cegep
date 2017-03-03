using SFML.Audio;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace Game
{
	class Sosn
	{
		public List<Sound> sanicQuote = new List<Sound>();
		public readonly Sound spenSound;
		public readonly Sound jamp;
		public readonly Sound ren;
		public readonly Sound squid_step;
		public readonly Sound bump;
		public readonly Sound teme;
		public readonly Sound gloria;

		public Sosn()
		{
			sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\sanicQuote.wav")));
			sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\Intro.wav")));
			sanicQuote.Add(new Sound(new SoundBuffer(@"..\..\Ressources\GottaGoFast.wav")));
			jamp = new Sound(new SoundBuffer(@"..\..\Ressources\sanic_jamp.wav"));
			ren = new Sound(new SoundBuffer(@"..\..\Ressources\sanic_ren.wav"));
			squid_step = new Sound(new SoundBuffer(@"..\..\Ressources\squid_step.wav"));
			spenSound = new Sound(new SoundBuffer(@"..\..\Ressources\SanicSpen.wav"));
			bump = new Sound(new SoundBuffer(@"..\..\Ressources\bump.wav"));
			teme = new Sound(new SoundBuffer(@"..\..\Ressources\SanicMusic.wav"));
			gloria = new Sound(new SoundBuffer(@"..\..\Ressources\Gloria.wav"));
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
