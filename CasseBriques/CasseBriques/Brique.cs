using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriques
{
    public class Brique : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Boolean marque;
        private Texture2D unebriquenoire;
        private Vector2 position;
        private Vector2 size;
        private SpriteBatch spriteBatch;

        public Brique(Game game,Vector2 p, Vector2 s) : base(game)
        {
            this.position = p;
            this.size = s;
            this.marque = false;

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            unebriquenoire = Game.Content.Load<Texture2D>(@"mesimages\briquetransparente");
            base.LoadContent();
        }

        public Boolean Marque
        {
            get { return marque; }
            set { marque = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
    }
}
