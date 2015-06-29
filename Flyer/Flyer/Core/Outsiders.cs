using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flyer.Core
{
    public static class Outsiders
    {
        public static int detailReload = 0;
        public static bool coordinate_Show = false;

        public static void Update(KeyboardState state)
        {
            detailReload++;
            if (state.IsKeyDown(Keys.F3))
            {
                if (detailReload >= 10)
                {
                    if (coordinate_Show)
                    {
                        coordinate_Show = false;
                        detailReload = 0;

                    }
                    else
                    {
                        coordinate_Show = true;
                        detailReload = 0;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch , SpriteFont universalfont
            ,Vector2 location,Texture2D HPbar,Texture2D redbar,Texture2D bluebar,
            double index1,double index2)
        {
            if (coordinate_Show)
            {
                spriteBatch.DrawString(universalfont, "Ship.X - " + location.X,
                    new Vector2(15, 660), Color.White);
                spriteBatch.DrawString(universalfont, "Ship.Y - " + location.Y,
                    new Vector2(15, 700), Color.White);
            }
            //HARDCODE PLZ
            spriteBatch.DrawString(universalfont, "HP ", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(universalfont, "SP ", new Vector2(10, 40), Color.White);
            spriteBatch.Draw(HPbar, new Vector2(50, 15), new Rectangle(0, 0, 200, HPbar.Height), Color.White);
            spriteBatch.Draw(HPbar, new Vector2(50, 45), new Rectangle(0, 0, 200, HPbar.Height), Color.White);
            spriteBatch.Draw(redbar, new Vector2(50, 2), new Rectangle(0, 0, (int)index1, 27), Color.Red);
            spriteBatch.Draw(bluebar, new Vector2(55, 50), new Rectangle(0, 0, (int)index2, 8), Color.White);
        }
    }
}
