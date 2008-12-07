using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace XNATetris
{
    enum SOUND_FX
    {
        Fall,
        Rotate,
        Win,
        Lose,
    }

    class XNASound
    {

                
        private SoundEffect fall;
        private SoundEffect rotate;
        private SoundEffect win;
        private SoundEffect lose;

        public void LoadContent(ContentManager content)
        {
            fall = content.Load<SoundEffect>(@"sounds\fall");
            rotate = content.Load<SoundEffect>(@"sounds\rotate");
            win = content.Load<SoundEffect>(@"sounds\win");
            lose = content.Load<SoundEffect>(@"sounds\lose");
        }

        public void FireSoundFX(SOUND_FX s)
        {
            switch (s)
            {
                case SOUND_FX.Rotate: rotate.Play(0.3f, 0.0f, 0.0f, false);; break;
                case SOUND_FX.Fall: fall.Play(0.5f, 0.0f, 0.0f, false);; break;
                case SOUND_FX.Win: win.Play(1.0f, 0.0f, 0.0f, false);; break;
                case SOUND_FX.Lose: lose.Play(1.0f, 0.0f, 0.0f, false);; break;
            }
        }
    }
}
