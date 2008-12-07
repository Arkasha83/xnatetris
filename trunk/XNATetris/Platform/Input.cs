using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNATetris.Platform
{
    class Input
    {
        public enum ACTION
        {
            NONE,
            EXIT,
            LEFT,
            DOWN,
            UP,
            RIGHT,
            SELECT,
        }

        KeyboardState oldKState = new KeyboardState();
        GamePadState oldGState = new GamePadState();
        public ACTION QueryInput()
        {
            ACTION actionToSend = ACTION.NONE;
            KeyboardState kState = Keyboard.GetState();
            GamePadState gState = GamePad.GetState(PlayerIndex.One);
            if (IsKeyPressed(kState, oldKState, Keys.Left)
                    || IsKeyPressed(kState, oldKState, Keys.NumPad4))
                actionToSend = ACTION.LEFT;
            else if (IsKeyPressed(kState, oldKState, Keys.Right)
                    || IsKeyPressed(kState, oldKState, Keys.NumPad6))
                actionToSend = ACTION.RIGHT;
            else if (IsKeyPressed(kState, oldKState, Keys.Up)
                    || IsKeyPressed(kState, oldKState, Keys.NumPad8))
                actionToSend = ACTION.UP;
            else if (IsKeyPressed(kState, oldKState, Keys.Down)
                    || IsKeyPressed(kState, oldKState, Keys.NumPad2))
                actionToSend = ACTION.DOWN;
            else if (IsKeyPressed(kState, oldKState, Keys.Space)
                        || IsKeyPressed(kState, oldKState, Keys.NumPad5)
                        || IsKeyPressed(kState, oldKState, Keys.Enter))
                actionToSend = ACTION.SELECT;
            else if (IsKeyPressed(kState, oldKState, Keys.Escape))
                actionToSend = ACTION.EXIT;


            oldKState = kState;
            oldGState = gState;

            return actionToSend;
        }

        private bool IsKeyPressed(KeyboardState newKS, KeyboardState oldKS, Keys k)
        {
            return newKS.IsKeyDown(k) && !oldKS.IsKeyDown(k);
        }
    }
}
