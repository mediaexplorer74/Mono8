
using System;
using Microsoft.Xna.Framework.Input;

namespace Mono8;

public class Keyboard8 {

    private bool[] keys = new bool[16]; // Very simple; it's hex based

    public Keyboard8() {
        // Set the board
        for (int i = 0; i < 15; i++) {
            keys[i] = true;
        }
    }


    /// <summary>
    /// Method <c>SetKeyDown</c> sets the key stack? I guess i'd call it for keys that are down.
    /// </summary>
	public bool SetKeyDown() {

        KeyboardState keyboard = Keyboard.GetState();

        // I will try to figure how to to make it into switch staments 
        // But Monogame is a bit weird on input (I cannot figured out how to get the current keyindex, this is good enough though just ugly)
        // Maybe a loop and such, but I don't think the keycodes really line up in a way that can fit into 16 without a lot of work

        if (keyboard.IsKeyDown(Keys.D1))
        {
            keys[0] = true;
        }
  
        if (keyboard.IsKeyDown(Keys.D2))
        {
            keys[1] = true;
        }

        if (keyboard.IsKeyDown(Keys.D3))
        {
            keys[2] = true;
        }
  
        if (keyboard.IsKeyDown(Keys.D4))
        {
            keys[3] = true;
        }

        
        if(keyboard.IsKeyDown(Keys.Q)) {
            keys[4] = true;
        }

        if(keyboard.IsKeyDown(Keys.W)) {
            keys[5] = true;
        }

        if(keyboard.IsKeyDown(Keys.E)) {
            keys[6] = true;
        }

        if(keyboard.IsKeyDown(Keys.R)) {
            keys[7] = true;
        }

        if(keyboard.IsKeyDown(Keys.A)) {
            keys[8] = true;
        }

        if(keyboard.IsKeyDown(Keys.S)) {
            keys[9] = true;
        }

        if(keyboard.IsKeyDown(Keys.D)) {
            keys[10] = true;
        }

        if(keyboard.IsKeyDown(Keys.F)) {
            keys[11] = true;
        }

        if(keyboard.IsKeyDown(Keys.Z)) {
            keys[12] = true;
        }

        if(keyboard.IsKeyDown(Keys.X)) {
            keys[13] = true;
        }
        
        if(keyboard.IsKeyDown(Keys.C)) {
            keys[14] = true;
        }
        if(keyboard.IsKeyDown(Keys.V)) {
            keys[15] = true;
        }

        return true;
	}

    /// <summary>
    /// Method <c>SetKeyUp</c> sets the key stack? I guess i'd call it for keys that are up.
    /// </summary>
	public bool SetKeyUp() {

        KeyboardState keyboard = Keyboard.GetState();

        // I will try to figure how to to make it into switch staments 
        // But Monogame is a bit weird on input (I cannot figured out how to get the current keyindex, this is good enough though just ugly)
        // Maybe a loop and such, but I don't think the keycodes really line up in a way that can fit into 16 without a lot of work

        if (keyboard.IsKeyUp(Keys.D1))
        {
            keys[0] = false;
        }
  
        if (keyboard.IsKeyUp(Keys.D2))
        {
            keys[1] = false;
        }

        if (keyboard.IsKeyUp(Keys.D3))
        {
            keys[2] = false;
        }
  
        if (keyboard.IsKeyUp(Keys.D4))
        {
            keys[3] = false;
        }


        if(keyboard.IsKeyUp(Keys.Q)) {
            keys[4] = false;
        }

        if(keyboard.IsKeyUp(Keys.W)) {
            keys[5] = false;
        }

        if(keyboard.IsKeyUp(Keys.E)) {
            keys[6] = false;
        }

        if(keyboard.IsKeyUp(Keys.R)) {
            keys[7] = false;
        }

        if(keyboard.IsKeyUp(Keys.A)) {
            keys[8] = false;
        }

        if(keyboard.IsKeyUp(Keys.S)) {
            keys[9] = false;
        }

        if(keyboard.IsKeyUp(Keys.D)) {
            keys[10] = false;
        }

        if(keyboard.IsKeyUp(Keys.F)) {
            keys[11] = false;
        }

        if(keyboard.IsKeyUp(Keys.Z)) {
            keys[12] = false;
        }

        if(keyboard.IsKeyUp(Keys.X)) {
            keys[13] = false;
        }
        
        if(keyboard.IsKeyUp(Keys.C)) {
            keys[14] = false;
        }
        if(keyboard.IsKeyUp(Keys.V)) {
            keys[15] = false;
        }  

        return true;
	}

    /// <summary>
    /// Method <c>Update</c> updates duh
    /// </summary>
    public void Update() {
        SetKeyDown();
        SetKeyUp();
    }

    /// <summary>
    /// Method <c>isKeyPressed</c> returns if key is pressed
    /// </summary>
    public bool IsKeyPressed(int k) {
        return keys[k];
	}
}