using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Interfaces
{
    interface IDrawable
    {
        Texture2D Texture { get; set; }

        Vector2 Location { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
