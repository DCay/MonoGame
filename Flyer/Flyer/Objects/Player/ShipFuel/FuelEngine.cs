using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Flyer
{
    class FuelEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<BurnedFuel> particles;
        public readonly List<Texture2D> textures;
        public Color Color;

        public FuelEngine(List<Texture2D> textures,Vector2 emitterLocation,Color color)
        {
            EmitterLocation = emitterLocation;
            this.random = new Random();
            this.particles = new List<BurnedFuel>();
            this.textures = textures;
            this.Color = color;
        }

        public BurnedFuel Generator()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                1f*(float)(random.NextDouble() * 2 - 1),
                1f*(float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f*(float) (random.NextDouble()*2 - 1);
            //Color color = new Color(
            //    (float)random.NextDouble(),
            //    (float)random.NextDouble(),
            //    (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int lifespan = 15 + random.Next(40);

            return new BurnedFuel(texture, position, velocity, angle, angularVelocity, Color, size, lifespan);
        }

        public void Update()
        {
            Build();
            Destroy();
        }

        public void Build()
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(Generator());
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].Lifespan <= 0)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Draw(spriteBatch);
            }
        }
    }
}
