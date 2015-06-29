using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Interfaces
{
    public interface IProjectile
    {
        Texture2D Texture { get;}

        Direction Direction { get; }

        Vector2 Location { get;}

        bool ToDraw { get; set; }

        //int Damage { get; set; }

        //int Speed { get; set; }

        //void SetPosition(Direction direction, Vector2 location);

        void Update();

        void Draw(SpriteBatch spriteBatch);
    }
}
