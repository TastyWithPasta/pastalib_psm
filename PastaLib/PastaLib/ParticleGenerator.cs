using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PastaLibrary
{
    public interface IParticle
    {
        bool RemoveMe();
        //void AssignFastTexture(Texture2D particleTexture);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public class ParticleGenerator<T> : Entity
        where T : IParticle
    {
        int _size;
        bool _visible = true;
        bool _automatic = false;
        int _generationInterval = 100;
        int _generationTimer;
        IParticle[] _particles;

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public bool Automatic
        {
            get { return _automatic; }
            set { _automatic = value; }
        }
        public int GenerationInterval
        {
            get { return _generationInterval; }
            set { _generationInterval = value; }
        }
        public ParticleGenerator(int size) : base()
        {
            _size = size;
            _particles = new IParticle[size];
            _generationTimer = _generationInterval;
        }
        public void Clear()
        {
            _particles = new IParticle[_size];
        }
        public void ResetGenerationTimer()
        {
            _generationTimer = _generationInterval;
        }

        /// <summary>
        /// Add a particle manually.
        /// </summary>
        /// <param name="particle">The particle to add.</param>
        public void AddParticle(T particle)
        {
            for (int i = 0; i < _particles.Length; ++i)
                if (_particles[i] == null)
                {
                    _particles[i] = particle;
                    break;
                }
        }
        /// <summary>
        /// Generate a certain amount of particles. The particles need a constructor that takes their generator as sole parameter.
        /// </summary>
        /// <param name="amount">The amount of particles to generate</param>
        public void Generate(int amount)
        {
            int lastIndex = 0;
            for (int j = 0; j < amount; ++j)
                for (int i = lastIndex; i < _particles.Length; ++i)
                    if (_particles[i] == null)
                    {
                        _particles[i] = (IParticle)Activator.CreateInstance(typeof(T), this);
                        lastIndex = i + 1;
                        break;
                    }
        }
        /// <summary>
        /// Generate a particle. The particles need a constructor that takes their generator as sole parameter.
        /// </summary>
        public void Generate()
        {
            for (int i = 0; i < _particles.Length; ++i)
                if (_particles[i] == null)
                {
                    _particles[i] = (IParticle)Activator.CreateInstance(typeof(T), this);
                    break;
                }
        }
        public void Update(GameTime gameTime)
        {
            if (_automatic)
            {
                _generationTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                while (_generationTimer <= 0)
                {
                    _generationTimer = _generationInterval + _generationTimer;
                    Generate();
                }
            }

            for (int i = 0; i < _particles.Length; ++i)
            {
                if (_particles[i] != null)
                {
                    _particles[i].Update(gameTime);
                    if (_particles[i].RemoveMe())
                        _particles[i] = null;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_visible)
                for (int i = 0; i < _particles.Length; ++i)
                    if (_particles[i] != null)
                        _particles[i].Draw(spriteBatch);
        }
    }
}
