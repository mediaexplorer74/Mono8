
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Enums;
using System.Diagnostics;

namespace GameManager
{

    public class Screen8
    {

        // Screen Properties

        private static readonly int WINDOW_WIDTH = 512;
        private static readonly int WINDOW_HEIGHT = 256;

        private static int INTERNAL_WIDTH = 64; // CHIP-8 base is 32 (This is scaled 8x)
        private static int INTERNAL_HEIGHT = 32; // CHIP-8 base is 64 (This is scaled 8X)

        private int GAME_SCALE = 8;

        // Misc
        public bool[,] pixelStates = new bool[INTERNAL_HEIGHT, INTERNAL_WIDTH];

        // Classes
        public GraphicsDeviceManager _graphics;
        public ContentManager _contentManager;
        public Scroll8 _scroll8;

        // Pixel (We just need for black/white)
        public Texture2D pixel;


        public Screen8(GraphicsDeviceManager graphics, ContentManager contentManager)
        {

            // Set Classes
            _graphics = graphics;
            _contentManager = contentManager;

            // Set Display
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

        }

        public void LoadContent()
        {
            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Method <c>CheckPixelState</c> sets all pixels to off
        /// </summary>
        public void Clear()
        {
            for (int y = 0; y < INTERNAL_HEIGHT; y++)
            {
                for (int x = 0; x < INTERNAL_WIDTH; x++)
                {

                    pixelStates[y, x] = false;
                }
            }
        }

        /// <summary>
        /// Method <c>SetDisplaySize</c> sets internal height and width.
        /// </summary>
        public void SetDisplaySize(int height, int width)
        {

            var previousHeight = INTERNAL_HEIGHT;
            var previousWidth = INTERNAL_WIDTH;

            var previousPixelStates = pixelStates;

            pixelStates = new bool[height, width];

            INTERNAL_WIDTH = width;
            INTERNAL_HEIGHT = height;

            Clear();

            for (int y = 0; y < previousHeight; y++)
            {
                for (int x = 0; x < previousWidth; x++)
                {

                    pixelStates[y, x] = previousPixelStates[y, x];
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    pixelStates[y, x] = true;
                }
            }

            Debug.WriteLine(pixelStates[1, 1]);
        }

        /// <summary>
        /// Method <c>Scroll</c> scrolls offscreen from direction type.
        /// </summary>
        public void Scroll(ScrollDirection direction)
        {

        }

        /// <summary>
        /// Method <c>CheckPixelState</c> sets a single pixel.
        /// </summary>
        public void SetPixel(int y, int x, bool state = true)
        {
            if (y < INTERNAL_HEIGHT)
            {
                if (x < INTERNAL_WIDTH)
                {
                    pixelStates[y, x] ^= state;
                }
            }
        }

        /// <summary>
        /// Method <c>CheckPixelState</c> sets an area of pixels from y,x width and height 
        /// </summary>
        public void SetPixelArea(int y, int x, int width, int height, bool state = true)
        {
            if (y < INTERNAL_HEIGHT)
            {
                if (x < INTERNAL_WIDTH)
                {

                    for (int aY = 0; aY < height; aY++)
                    {
                        for (int aX = 0; aX < width; aX++)
                        {
                            pixelStates[y + aY, x + aX] = state;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method <c>CheckPixelState</c> checks if a pixel state is on or off (black or white, true false etc)
        /// </summary>
        public bool CheckPixelState(int y, int x)
        {
            if (y < INTERNAL_HEIGHT)
            {
                if (x < INTERNAL_WIDTH)
                {
                    return pixelStates[y, x];
                }
            }

            return false;
        }

        // <summary>
        /// Method <c>SetGameScale</c> sets gamescale.
        /// </summary>
        public void SetGameScale(int gameScale)
        {
            if (gameScale < 1)
            {
                GAME_SCALE = 1;
            }
            else if (gameScale > 10)
            {
                GAME_SCALE = 10;
            }
            else
            {
                GAME_SCALE = gameScale;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < INTERNAL_HEIGHT; y++)
            {
                for (int x = 0; x < INTERNAL_WIDTH; x++)
                {
                    if (pixelStates[y, x])
                    {
                        spriteBatch.Draw(pixel, new Rectangle(x * (GAME_SCALE), y * (GAME_SCALE), 1 * (GAME_SCALE), 1 * (GAME_SCALE)), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(pixel, new Rectangle(x * (GAME_SCALE), y * (GAME_SCALE), 1 * (GAME_SCALE), 1 * (GAME_SCALE)), Color.Black);
                    }
                }
            }
        }

    }
}