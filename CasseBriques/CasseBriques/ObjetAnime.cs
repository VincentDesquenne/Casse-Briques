using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriques
{
    public class ObjetAnime
    {
        // On définit les propriétés de l'objet animé
        // Certains objets animés auront une vitesse nulle
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _size;
        private Vector2 _vitesse;
        

        public Vector2 Vitesse
        {
            get { return _vitesse; }
            set { _vitesse = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        
        public ObjetAnime(Texture2D texture, Vector2 position, Vector2 size, Vector2 vitesse)
        {
            this._texture = texture;
            this._position = position;
            this._size = size;
            this._vitesse = vitesse;
        }
    }

}

