using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Mono8.System;

namespace Mono8;

/*
    About

    A simple little Chip8 EMU in C#.

    CHIP-8 is an interpreted programming language, developed by Joseph Weisbecker on his 1802 microprocessor. 
    It was initially used on the COSMAC VIP and Telmac 1800, which were 8-bit microcomputers made in the mid-1970s. (Wikipedia)
    
    Support:

    CHIP-8 - Finished (Passes 16/16 tests of CORAX at the momment)

    SUPER-CHIP-8 - Not Support

    XO-CHIP - Not supported

    Credits
    
    Nicholas B. Brooks

    https://en.wikipedia.org/wiki/CHIP-8#Memory (CHIP-8 summary I used in about.)

    https://multigesture.net/articles/how-to-write-an-emulator-chip-8-interpreter/ (A C Pico8 EMU)

    https://github.com/brokenprogrammer/CHIP-8-Emulator/tree/master (A Java Chip8 EMU)
*/

public class Game1 : Game
{
    // MonoGame Stuff
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Paths
    public string romPath = "/home/ksportalcraft/Documents/C#/Mono8/roms/TETRIS.ch8";

    // Classes
    public SystemBase system;

    private readonly Screen8 screen;
    private readonly Sound8 sound;
    private readonly Keyboard8 keyboard;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set Classes
        screen = new Screen8(_graphics, Content);
        sound = new Sound8();
        keyboard = new Keyboard8();

        // Set Proper 'system' (for 8, super, and xo)
        var ext = Path.GetExtension(romPath);

        switch(ext) {

            case ".ch8" :
            case ".chip8":
                system = new System8(screen, sound, keyboard);
            break;

            default:
                Console.WriteLine($"\nError: File type {ext} not supported!");
                Console.WriteLine($"Info: CHIP-8 roms must end in .ch8 or .chip8! Right now we only support System 8! File extensions determine what 'system' to load!");
            
                Environment.Exit(0);
            break;
        }

        // MonoGame Setup
        this.IsFixedTimeStep = true;
        this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 180.0); 
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Load Rom
        Console.WriteLine($"\nRom Name: {Path.GetFileName(romPath)}");
        Console.WriteLine($"Rom Size:  {Utils.GetFileSize(romPath)}k");
        Console.WriteLine("\n");

        system.LoadProgram(romPath);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        screen.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        keyboard.Update();
        system.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        screen.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}