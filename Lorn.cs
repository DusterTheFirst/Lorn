using System.Diagnostics;
using Lorn.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lorn.Desktop {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lorn : Game {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Camera camera;

        Model sphere;

        public Lorn() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private VertexPositionColor[] FloorVerticies;
        private BasicEffect effect;

        protected override void Initialize() {
            FloorVerticies = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(-20, -20, 0), Color.Red),
                new VertexPositionColor(new Vector3(-20, 20, 0), Color.Red),
                new VertexPositionColor(new Vector3(20, -20, 0), Color.Red),
                new VertexPositionColor(new Vector3(-20, 20, 0), Color.Orange),
                new VertexPositionColor(new Vector3(20, 20, 0), Color.Orange),
                new VertexPositionColor(new Vector3(20, -20, 0), Color.Orange)
            };
            effect = new BasicEffect(graphics.GraphicsDevice);

            IsMouseVisible = true; //false;
            //// Center mouse
            //Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            camera = new Camera(new Vector3(0, 40, 20));

            sphere = Content.Load<Model>("Models/Basic/Sphere");

            base.Initialize();
        }

        void DrawGround() {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.

            effect.View = Matrix.CreateLookAt(new Vector3(50, 70, 20), new Vector3(0, 40, 20), Vector3.UnitZ);// camera.ViewMatrix;

            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 2000);

            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    FloorVerticies,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }


        }

        public void DrawSphere(Vector3 position) {
            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

            foreach (ModelMesh mesh in sphere.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Matrix.CreateWorld(position, Vector3.UnitX, Vector3.UnitZ);

                    effect.View = Matrix.CreateLookAt(new Vector3(50, 70, 20), new Vector3(0, 40, 20), Vector3.UnitZ);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 2000);
                }

                mesh.Draw();
            }
        }

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/Ubuntu");
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime) {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboard.IsKeyDown(Keys.Escape)
                || (keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) && keyboard.IsKeyDown(Keys.F4)) {
                Exit();
            }

            if (IsActive) {
                if (keyboard.IsKeyDown(Keys.W)) {
                    camera.Position -= new Vector3(0, 1, 0);
                }
                if (keyboard.IsKeyDown(Keys.S)) {
                    camera.Position += new Vector3(0, 1, 0);
                }
                if (keyboard.IsKeyDown(Keys.A)) {
                    camera.Position += new Vector3(1, 0, 0);
                }
                if (keyboard.IsKeyDown(Keys.D)) {
                    camera.Position -= new Vector3(1, 0, 0);
                }
                if (keyboard.IsKeyDown(Keys.Space)) {
                    camera.Position += new Vector3(0, 0, 1);
                }
                if (keyboard.IsKeyDown(Keys.LeftShift)) {
                    camera.Position -= new Vector3(0, 0, 1);
                }
                if (keyboard.IsKeyDown(Keys.Q)) {
                    camera.Roll += .1f;
                }
                if (keyboard.IsKeyDown(Keys.E)) {
                    camera.Roll -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.Left)) {
                    camera.Yaw += .1f;
                }
                if (keyboard.IsKeyDown(Keys.Right)) {
                    camera.Yaw -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.Up)) {
                    camera.Pitch += .1f;
                }
                if (keyboard.IsKeyDown(Keys.Down)) {
                    camera.Pitch -= .1f;
                }

                //// Mouse movement
                //if (keyboard.IsKeyUp(Keys.LeftControl)) {
                //    IsMouseVisible = false;

                //    Vector3 mouseOffset = new Vector3(graphics.PreferredBackBufferWidth / 2, 0, graphics.PreferredBackBufferHeight / 2);

                //    Point delta = mouse.Position;
                //    Mouse.SetPosition((int)mouseOffset.X, (int)mouseOffset.Z);

                //    Vector3 changeVector = new Vector3(delta.X, 0, delta.Y) - mouseOffset;

                //    //Debug.WriteLine(changeVector.ToString());

                //    camera.Yaw += changeVector.X * .05f;
                //    camera.Pitch += changeVector.Z * .05f;
                //    //cameraTarget -= changeVector;
                //} else {
                //    IsMouseVisible = true;
                //}
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            DrawGround();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, $"Camera Position: {camera.Position}", new Vector2(2, 5), Color.Black, Color.White, new Vector2(2, 5));
            spriteBatch.DrawString(font, $"Yaw: {MathHelper.ToDegrees(camera.Yaw)} Pitch: {MathHelper.ToDegrees(camera.Pitch)} Roll: {MathHelper.ToDegrees(camera.Roll)}", new Vector2(2, 30), Color.Black, Color.White, new Vector2(2, 5));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class SpriteBatchExtentions {
        public static Texture2D WhiteRectangle;
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color) {
            if (WhiteRectangle == null) {
                WhiteRectangle = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                WhiteRectangle.SetData(new[] { Color.White });
            }

            Vector2 coor = new Vector2(10, 20);
            spriteBatch.Draw(WhiteRectangle, rectangle, color);
        }
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color background) => spriteBatch.DrawString(font, text, position, color, background, Vector2.Zero);
        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color background, Vector2 padding) {
            Vector2 size = font.MeasureString(text);
            spriteBatch.DrawRectangle(new Rectangle((position - padding).ToPoint(), (size + (padding * 2)).ToPoint()), background);
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}
