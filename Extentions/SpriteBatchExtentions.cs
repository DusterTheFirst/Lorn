using Lorn.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorn.Extentions {
    public static class SpriteBatchExtentions {
        public static Texture2D Pixel;

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color) {
            if (Pixel == null) {
                Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });
            }

            Vector2 coor = new Vector2(10, 20);
            spriteBatch.Draw(Pixel, rectangle, color);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color background) => spriteBatch.DrawString(font, text, position, color, background, Vector2.One);
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color background, Vector2 padding) {
            Vector2 size = font.MeasureString(text);
            spriteBatch.DrawRectangle(new Rectangle((position - padding).ToPoint(), (size + (padding * 2)).ToPoint()), background);
            spriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawString3D(this SpriteBatch spriteBatch, SpriteFont font, Camera camera, string text, Vector3 position, Color color, Color background) => spriteBatch.DrawString3D(font, camera, text, position, color, background, Vector2.One);
        public static void DrawString3D(this SpriteBatch spriteBatch, SpriteFont font, Camera camera, string text, Vector3 position, Color color, Color background, Vector2 padding) {
            // Find screen equivalent of 3D location in world
            Vector3 screenLocation = camera.Graphics.Viewport.Project(position, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            spriteBatch.DrawString(font, text, new Vector2(screenLocation.X, screenLocation.Y), color, background, padding);
        }

        public static void DrawStringCentered(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 center, Color color, Color background) => spriteBatch.DrawStringCentered(font, text, center, color, background, Vector2.One);
        public static void DrawStringCentered(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 center, Color color, Color background, Vector2 padding) {
            Vector2 size = font.MeasureString(text);

            // Center
            Vector2 position = center - size / 2;

            spriteBatch.DrawRectangle(new Rectangle((position - padding).ToPoint(), (size + (padding * 2)).ToPoint()), background);
            spriteBatch.DrawString(font, text, position, color, background, padding);
        }

        public static void DrawStringCentered3D(this SpriteBatch spriteBatch, SpriteFont font, Camera camera, string text, Vector3 center, Color color, Color background) => spriteBatch.DrawStringCentered3D(font, camera, text, center, color, background, Vector2.One);
        public static void DrawStringCentered3D(this SpriteBatch spriteBatch, SpriteFont font, Camera camera, string text, Vector3 center, Color color, Color background, Vector2 padding) {
            // Find screen equivalent of 3D location in world
            Vector3 screenLocation = camera.Graphics.Viewport.Project(center, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            spriteBatch.DrawStringCentered(font, text, new Vector2(screenLocation.X, screenLocation.Y), color, background, padding);
        }

        public static void DrawPoint3D(this SpriteBatch spriteBatch, Camera camera, Vector3 postion) => DrawPoint3D(spriteBatch, camera, postion, Color.White);
        public static void DrawPoint3D(this SpriteBatch spriteBatch, Camera camera, Vector3 postion, Color color) {
            // Find screen equivalent of 3D location in world
            Vector3 screenLocation = camera.Graphics.Viewport.Project(postion, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

            // Draw our pixel texture there
            spriteBatch.Draw(Pixel, new Vector2(screenLocation.X, screenLocation.Y), Color.White);
        }
    }
}
