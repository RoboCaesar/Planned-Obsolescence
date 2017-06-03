using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiskWars
{
    static public class Tile
    {
        static public Texture2D TileSetTexture;
        static public Rectangle GetSourceRectangle(int tileIndex)
        {
            return new Rectangle((tileIndex % 10) * 16, Convert.ToInt16(tileIndex/10) * 16, 16, 16);
        }
    }
}