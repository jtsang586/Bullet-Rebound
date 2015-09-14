using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bullet_Rebound
{
    class InteractiveMusic
    {
        public FMOD.RESULT result;

        public FMOD.EventSystem eventsystem = null;
        public FMOD.MusicSystem musicsystem = null;
        public FMOD.MusicPrompt cue_titleScreen;
        public FMOD.MusicPrompt cue_main;
        public FMOD.MusicPrompt cue_gameOver;

        // From the minimusic.h 
        /*
        Music parameter ids
        */
        public const uint MUSICPARAM_BULLETREBOUND_LIVESREMAINING = 1;
        public const uint MUSICPARAM_BULLETREBOUND_PLAYERSCORE = 2;
        public const uint MUSICPARAM_BULLETREBOUND_GAMESTART = 3;

        /*
            Music cue ids
        */
        const uint MUSICCUE_BULLETREBOUND_TITLESCREEN = 2;
        const uint MUSICCUE_BULLETREBOUND_MAIN = 3;
        const uint MUSICCUE_BULLETREBOUND_GAMEOVER = 4;

        public InteractiveMusic()
        {
            Initialize();
        }

        public void Initialize()
        {
            result = FMOD.Event_Factory.EventSystem_Create(ref eventsystem);
            ERRCHECK(result);
            result = eventsystem.init(64, FMOD.INITFLAGS.NORMAL, (IntPtr)null, FMOD.EVENT_INITFLAGS.NORMAL);
            ERRCHECK(result);

            ERRCHECK(eventsystem.setMediaPath("./Media/"));
            ERRCHECK(result);
            ERRCHECK(eventsystem.load("BulletRebound.fev"));
            ERRCHECK(result);

            result = eventsystem.getMusicSystem(ref musicsystem);
            ERRCHECK(result);

            result = musicsystem.setVolume(1.0f);
            ERRCHECK(result);
            result = musicsystem.setMute(false);
            ERRCHECK(result);
            result = musicsystem.setPaused(false);
            ERRCHECK(result);

            result = musicsystem.prepareCue(MUSICCUE_BULLETREBOUND_TITLESCREEN, ref cue_titleScreen);
            ERRCHECK(result);
            result = musicsystem.prepareCue(MUSICCUE_BULLETREBOUND_MAIN, ref cue_main);
            ERRCHECK(result);
            result = musicsystem.prepareCue(MUSICCUE_BULLETREBOUND_GAMEOVER, ref cue_gameOver);
            ERRCHECK(result);
        }

        private void ERRCHECK(FMOD.RESULT result)
        {
            
        }

        public void UnloadContent()
        {
            ERRCHECK(musicsystem.freeSoundData(true));
            ERRCHECK(eventsystem.release());
        }

        public void fmod_music_Load(object sender, EventArgs e)
        {
        }

    }
}
