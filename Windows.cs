using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DiskWars
{

    public static class Windows
    {
        public static bool ReadyToDisplayNewMessage = true;
        public static bool DisplayingMessage = false;
        public static int MessageFrame = 0;
        //public static int 

        public static void Draw(int x1, int y1, int sizex, int sizey)
        {
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + 2, y1 + 2, sizex - 4, sizey - 4), new Rectangle(1, 4, 14, 9), Color.White); //main blue windows
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1, y1 + 4, 4, sizey - 8), new Rectangle(26, 1, 4, 4), Color.White); //long borders
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + sizex - 4, y1 + 4, 4, sizey - 8), new Rectangle(26, 1, 4, 4), Color.White);
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + 4, y1, sizex - 8, 4), new Rectangle(17, 0, 4, 4), Color.White);
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + 4, y1 + sizey - 4, sizex - 8, 4), new Rectangle(17, 0, 4, 4), Color.White);

            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1, y1 + sizey - 4, 4, 4), new Rectangle(26, 8, 4, 4), Color.White); //corners
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + sizex - 4, y1 + sizey - 4, 4, 4), new Rectangle(26, 12, 4, 4), Color.White); //corners
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1, y1, 4, 4), new Rectangle(18, 8, 4, 4), Color.White); //corners
            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(x1 + sizex - 4, y1, 4, 4), new Rectangle(18, 12, 4, 4), Color.White); //corners


        }
        public static void DisplayMessage(string Message)
        {
            Vector2 stringDim = Game1.gameFont.MeasureString(Message);
            DisplayingMessage = true;
            MessageFrame++;
            if (MessageFrame > 300)
            {
                //ReadyToDisplayNewMessage = true;
                MessageFrame = -1;
                DisplayingMessage = false;
                return;
            }
            if (MessageFrame <= 12)
            {
                Draw(4, 4 + 6 - (MessageFrame / 2), (int)stringDim.X + 12, MessageFrame * 2);
            }
            else if (MessageFrame > 12 && MessageFrame < 288)
            {
                Draw(4, 4, (int)stringDim.X + 12, 24);
                if (MessageFrame > 12 + Message.Length)
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, Message, new Vector2(11, 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, Message, new Vector2(10, 10), Color.White);
                }
                else
                {
                    string ShortenedMessage = Message.Substring(0, MessageFrame - 12);
                    Game1.spriteBatch.DrawString(Game1.gameFont, ShortenedMessage, new Vector2(11, 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, ShortenedMessage, new Vector2(10, 10), Color.White);
                }
            }
            else if (MessageFrame >= 288)
            {
                Draw(4, 4 + 6 - ((300-MessageFrame) / 2), (int)stringDim.X + 12, (300 - MessageFrame) * 2);
            }

        }
    }

}
