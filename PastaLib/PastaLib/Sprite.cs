using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;
using PastaGameLibrary.Components;

namespace PastaLibrary
{
    /// <summary>
    /// A sprite is an entity: it has a position and a direction (its rotation angle).
    /// 
    /// How to use the sprite class:
    /// 
    /// 1. Do not directly create a sprite instance, even though it could work, use the SpriteManager's MakeSprite() method.
    /// 2. A sprite can either be animated or not depending on which initialisation is done.
    /// 3. You should not really have to worry about what's inside of the sprite class, as everything (updating and drawing) is handled by the SpriteManager class.
    /// 4. You can get/alter some parameters like the color, alpha, width, height, depth, position, rotation etc using the accessors/mutators.
    /// 
    /// </summary> 

}
