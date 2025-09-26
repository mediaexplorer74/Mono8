
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Enums;

namespace GameManager
{

    public class Scroll8
    {

        // Scroll Properties
        protected float _zoom;
        protected Matrix _transform;
        protected float _rotation;
        protected Vector2 _position;

        public Scroll8()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }

        // GSetters
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; }
        }
        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        // https://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/

    }
}