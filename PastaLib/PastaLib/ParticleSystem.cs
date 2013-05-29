using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PastaLibrary
{
    public class ParticleSystem : PComponent, IPDrawableComponent, IPUpdatableComponent
    {
        const int MAX_TEXTURES = 5;
        int MAX_PARTICLES;

        Random random;
        Texture2D[] _textures;
        Particle[] _particles;

        private bool _isOn;
        private float _alphaIncrement, _alphaDecrement;
        private int _ttl, _storedTextures, _incrementTick, _decrementTick;
        private Vector2 _spawnBox, _acceleration;
        private float _spawnCone;
        private float _minSpeed, _maxSpeed;
        private float _minRotSpeed, _maxRotSpeed;
        private float _minRotInit, _maxRotInit;
        private float _depth;

        #region Accessors/Mutators

        public bool IsOn
        {
            get { return _isOn; }
            set { _isOn = value; }
        }
        public int TTL
        {
            get { return _ttl; }
            set { _ttl = value; }
        }
        public Vector2 SpawnBox
        {
            get { return _spawnBox; }
            set { _spawnBox = value; }
        }
        public float XAcceleration
        {
            get { return _acceleration.X; }
            set { _acceleration.X = value; }
        }
        public float YAcceleration
        {
            get { return _acceleration.Y; }
            set { _acceleration.Y = value; }
        }
        public float SpawnCone
        {
            get { return _spawnCone; }
            set { _spawnCone = value; }
        }
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                _maxSpeed = value;
                if (_maxSpeed < _minSpeed)
                    _minSpeed = _maxSpeed;
            }
        }
        public float MinSpeed
        {
            get { return _minSpeed; }
            set
            {
                _minSpeed = value;
                if (_minSpeed > _maxSpeed)
                    _maxSpeed = _minSpeed;
            }
        }
        public float MaxRotSpeed
        {
            get { return _maxRotSpeed; }
            set
            {
                _maxRotSpeed = value;
                if (_maxRotSpeed < _minRotSpeed)
                    _minRotSpeed = _maxRotSpeed;
            }
        }
        public float MinRotSpeed
        {
            get { return _minRotSpeed; }
            set
            {
                _minRotSpeed = value;
                if (_minRotSpeed > _maxRotSpeed)
                    _maxRotSpeed = _minRotSpeed;
            }
        }
        public float MaxRotInit
        {
            get { return _maxRotInit; }
            set
            {
                _maxRotInit = value;
                if (_maxRotInit < _minRotInit)
                    _minRotInit = _maxRotInit;
            }
        }
        public float MinRotInit
        {
            get { return _minRotInit; }
            set
            {
                _minRotInit = value;
                if (_minRotInit > _maxRotInit)
                    _maxRotInit = _minRotInit;
            }
        }

        public int FadeOutTime
        {
            get { return _decrementTick; }
            set
            {
                _alphaDecrement = 1.0f / (float)value;
                _decrementTick = value;
                //if (_incrementTick < _decrementTick)
                //    throw new Exception("Increment and decrement are intersecting.");
            }
        }
        public int FadeInTime
        {
            get { return _ttl - _incrementTick; }
            set
            {
                _alphaIncrement = 1.0f / (float)value;
                _incrementTick = _ttl - value;

                //if (_incrementTick < _decrementTick)
                //    throw new Exception("Increment and decrement are intersecting.");
            }
        }

        public float Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        #endregion

        public ParticleSystem(int maxParticles)
        {

            MAX_PARTICLES = maxParticles;

            random = new Random();
            _textures = new Texture2D[MAX_TEXTURES];
            _particles = new Particle[MAX_PARTICLES];

            for (int i = 0; i < MAX_PARTICLES; ++i)
            {
                _particles[i] = new Particle(null, _ttl, 0, 0, 0, 0, 0, 0);
                _particles[i]._exists = false;
            }

            _spawnBox.X = 0;
            _spawnBox.Y = 0;
            _minRotSpeed = 0;
            _maxRotSpeed = 0;
            _minRotInit = 0;
            _maxRotInit = 0;
            Angle = 0;
            _spawnCone = (float)Math.PI * 2f;
            _minSpeed = 10;
            _maxSpeed = 10;

            _alphaDecrement = 0.2f;
            _decrementTick = 5;

            _alphaIncrement = 0.2f;
            _incrementTick = 5;

            _isOn = true;
        }

        public void Clear()
        {
            for (int i = 0; i < MAX_PARTICLES; ++i)
                _particles[i]._exists = false;
        }

        public void Remove()
        {
            for (int i = 0; i < MAX_TEXTURES; ++i)
                _textures[i] = null;
            for (int i = 0; i < MAX_PARTICLES; ++i)
                _particles[i]._exists = false;

            _storedTextures = 0;
            _isOn = false;
        }
        public void AddTexture(Texture2D texture)
        {
            for (int i = 0; i < MAX_TEXTURES; ++i)
            {
                if (_textures[i] == null)
                {
                    _textures[i] = texture;
                    _storedTextures++;
                    break;
                }
            }
        }

        public void AddParticle(int textureIndex)
        {
            bool isArrayFull = true;

            Particle newParticle = null;
            foreach (Particle particle in _particles)
            {
                if (!particle._exists)
                {
                    newParticle = particle;
                    isArrayFull = false;
                    break;
                }
            }

            if (isArrayFull)
                return;

            if (textureIndex < 0)
                textureIndex = random.Next(0, _storedTextures);

            float speed = (float)(_minSpeed + random.NextDouble() * (_maxSpeed - _minSpeed));
            float angle = (float)(Angle + random.NextDouble() * _spawnCone - _spawnCone / 2f);
            float randomRotation = (float)(_minRotInit + random.NextDouble() * (_maxRotInit - _minRotInit));
            float randomRotationSpeed = (float)(_minRotSpeed + random.NextDouble() * (_maxRotSpeed - _minRotSpeed));

            newParticle.Initialise(_textures[textureIndex],
                _ttl,
                (int)ScreenX + random.Next(0, (int)_spawnBox.X + 1),
                (int)ScreenY + random.Next(0, (int)_spawnBox.Y + 1),
                (float)(speed * Math.Cos(angle)),
                (float)(speed * Math.Sin(angle)),
                randomRotation,
                randomRotationSpeed);
        }

        public void GenerateParticles(int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                AddParticle(-1);
            }
        }
        public void GenerateParticles(int amount, int textureIndex)
        {
            for (int i = 0; i < amount; ++i)
            {
                AddParticle(textureIndex);
            }
        }

        public void Update()
        {

            foreach (Particle par in _particles)
            {
                if (par._exists)
                {
                    par._velocity.X += _acceleration.X;
                    par._velocity.Y += _acceleration.Y;
                    par._position.X += par._velocity.X;
                    par._position.Y += par._velocity.Y;
                    par._destinationRectangle.X = (int)par._position.X;
                    par._destinationRectangle.Y = (int)par._position.Y;
                    par._rotation += par._rotationSpeed;

                    if (par._ttl > _incrementTick)
                    {
                        if (par._alpha + _alphaIncrement > 1)
                            par._alpha = 1;
                        else
                            par._alpha += _alphaIncrement;
                    }
                    else
                    {
                        if (par._ttl < _decrementTick)
                        {
                            if (par._alpha - _alphaDecrement < 0)
                                par._alpha = 0;
                            else
                                par._alpha -= _alphaDecrement;
                        }
                    }

                    par._ttl--;

                    if (par._ttl == 0)
                        par._exists = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle par in _particles)
            {
                if (par._exists)
                {
                    spriteBatch.Draw(par._texture, par._destinationRectangle, par._sourceRectangle, Color.White * par._alpha, par._rotation, par._origin, SpriteEffects.None, _depth);
                }
            }
        }
    }
}

