using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XNATetris
{
    class TetrisGameplay : TetrisConstants
    {
        //members
	    private long mNextUpdate; 	//time stamp when next update can be called
	    private long mLastGravity;	//allow updates of shape independently from gravity by checking this
        
        //game specific
        private TetrisGrid grid; 			//game play field/grid
        private TetrisShape currentShape;	//current shape controllable by the user
        private int currentAction;			//current game action fired by player
        private int playerScore;
        private String playerName;
        public void SetPlayerName(String n)
        {
            playerName = n;
        }
        
        /**
         * Constructor
         */
        public TetrisGameplay()
        {
    		
		    //inst objs
		    grid = new TetrisGrid();
		    currentShape = new TetrisShape(grid);
    		
		    //initialize
		    init();
	    }

        public void OnSizeChanged(int w, int h)
        {
            grid.setBackGroundDimentions(w,h);
        }

	    /**
         * Initialize members
	     */
	    private void init() {

		    //initialize members
		    currentShape.isInited = false;
		    currentAction = ACTION_NONE;
		    mNextUpdate = 0;
            mLastGravity = 1;
            playerScore = 0;

		    grid.init();

	    }

	    public void restartGame() {
		    init();
		    currentShape.isGameOver = false;
	    }

	    /**
	     * Handle key presses (make sure view is focusable)
	     */
        public void handleInput(Platform.Input.ACTION a)
        {
            int actionToSend = -1;
            switch (a)
            {
                case Platform.Input.ACTION.UP:
                    actionToSend = XNATetris.TetrisConstants.ACTION_ROTATE_L;
                    break;
                case Platform.Input.ACTION.DOWN:
                    actionToSend = XNATetris.TetrisConstants.ACTION_ROTATE_R;
                    break;
                case Platform.Input.ACTION.RIGHT:
                    actionToSend = XNATetris.TetrisConstants.ACTION_STRAFE_RIGHT;
                    break;
                case Platform.Input.ACTION.LEFT:
                    actionToSend = XNATetris.TetrisConstants.ACTION_STRAFE_LEFT;
                    break;
                case Platform.Input.ACTION.SELECT:
                    actionToSend = XNATetris.TetrisConstants.ACTION_MAKE_FALL;
                    break;
            }

            if (actionToSend != -1)
                currentAction = actionToSend;
	    }

            /**
	     * Update user actions
	     * Update engine actions (gravity and line check)
	     */
	    public void update() 
        {
            long time = SpecificTools.GetSystemTime();
    		
		    //manage gameOver
		    if(currentShape.isGameOver)
		    {
                bool isNewHighScore = TetrisGame.highScore.IsHighScore(playerName, playerScore) && playerScore > 0;
                TetrisGame.soundPlayer.FireSoundFX(isNewHighScore ? SOUND_FX.Win : SOUND_FX.Lose);
                TetrisGame.menu.setMenuState(TetrisMenu.MENU_STATE.STATE_MENUGAMEOVER, isNewHighScore );

		    }
		    //normal state
		    else if( time > mNextUpdate )
		    {
			    mNextUpdate = time + 1000 / FRAME_RATE;
			    currentShape.update(currentAction);
			    currentAction = ACTION_NONE;
                //GRAVITY_RATE - playerScore makes it increasingly harder!
                if (time - mLastGravity > GRAVITY_RATE - (playerScore * DIFFICULTY) || currentShape.IsFalling())
			    {
				    mLastGravity = time;
				    bool shapeIsLocked = currentShape.addGravity();
				    if(shapeIsLocked)
				    {
                        //update returns the number of points
					    int points = grid.update();
                        if (points != 0)
                        {
                            //play sound
                            playerScore += points;
                        }
				    }
			    }
		    }
		    return;
	    }
    	
	    /**
	     * Paint game
	     */
	    public void draw() {
		    //paint elements
		    grid.paint();
    		
		    //paint HUD from top right anchor
            TetrisHud.paintRightHud(TetrisGame.graphics.PreferredBackBufferWidth, 0, playerScore, currentShape.getNextType());
	    }
    }
}
