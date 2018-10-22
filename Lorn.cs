using Lorn.Views;
using Lorn.Extentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lorn.Desktop {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Lorn : Game {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Camera GameCamera;
        Camera ViewCamera;

        Camera ViewCameraO;

        Model cameraModel;
        Model sphere;

        Texture2D pixel;

        public Lorn() {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.Window.AllowUserResizing = true;
        }

        private VertexPositionColor[] FloorVerticies;
        private BasicEffect effect;

        protected override void Initialize() {

            this.pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            this.pixel.SetData(new[] { Color.White });

            this.FloorVerticies = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(-20, -20, 0), Color.Red),
                new VertexPositionColor(new Vector3(-20, 20, 0), Color.Red),
                new VertexPositionColor(new Vector3(20, -20, 0), Color.Red),
                new VertexPositionColor(new Vector3(-20, 20, 0), Color.Orange),
                new VertexPositionColor(new Vector3(20, 20, 0), Color.Orange),
                new VertexPositionColor(new Vector3(20, -20, 0), Color.Orange)
            };
            this.effect = new BasicEffect(this.graphics.GraphicsDevice);

            this.IsMouseVisible = true; //false;
            //// Center mouse
            //Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            this.GameCamera = new Camera(this.graphics, new Vector3(0, 40, 20));
            this.ViewCamera = new Camera(this.graphics, new Vector3(0, 80, 40));

            base.Initialize();
        }

        void DrawGround() {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.

            this.effect.View = this.ViewCamera.ViewMatrix;
            this.effect.Projection = this.ViewCamera.ProjectionMatrix;

            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = false;

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes) {
                pass.Apply();

                this.graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    this.FloorVerticies,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }


        }

        public void DrawModel(Model model, Matrix world) {
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (BasicEffect meshEffect in mesh.Effects) {
                    meshEffect.EnableDefaultLighting();
                    meshEffect.PreferPerPixelLighting = true;

                    meshEffect.World = world;
                    meshEffect.View = this.ViewCamera.ViewMatrix;
                    meshEffect.Projection = this.ViewCamera.ProjectionMatrix;

                    //meshEffect.VertexColorEnabled = true;
                    //meshEffect.AmbientLightColor = new Vector3(100);
                }

                mesh.Draw();
            }
        }

        public void DrawCamera(Camera camera) =>
            this.DrawModel(this.cameraModel, Matrix.CreateWorld(camera.Position, camera.Forward, camera.Up));

        public void DrawSphere(Vector3 position) =>
            this.DrawModel(this.sphere, Matrix.CreateWorld(position, Vector3.UnitX, Vector3.UnitZ));

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.font = this.Content.Load<SpriteFont>("Fonts/SourceCodePro");
            this.cameraModel = this.Content.Load<Model>("Models/Basic/Rectangle");
            this.sphere = this.Content.Load<Model>("Models/Basic/Sphere");
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime) {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboard.IsKeyDown(Keys.Escape)
                || (keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) && keyboard.IsKeyDown(Keys.F4)) {
                this.Exit();
            }

            if (this.IsActive) {
                if (keyboard.IsKeyDown(Keys.W)) {
                    this.GameCamera.Position -= new Vector3(0, 1, 0);
                }
                if (keyboard.IsKeyDown(Keys.S)) {
                    this.GameCamera.Position += new Vector3(0, 1, 0);
                }
                if (keyboard.IsKeyDown(Keys.A)) {
                    this.GameCamera.Position += new Vector3(1, 0, 0);
                }
                if (keyboard.IsKeyDown(Keys.D)) {
                    this.GameCamera.Position -= new Vector3(1, 0, 0);
                }
                if (keyboard.IsKeyDown(Keys.Space)) {
                    this.GameCamera.Position += new Vector3(0, 0, 1);
                }
                if (keyboard.IsKeyDown(Keys.LeftShift)) {
                    this.GameCamera.Position -= new Vector3(0, 0, 1);
                }


                //  TODO: Instead of changing forward vector, calculate it based on pitch yaw and roll
                // TODOLLL: Gonna need some trig or calculus or some angle shit

                if (keyboard.IsKeyDown(Keys.Q)) {
                    this.GameCamera.Forward.Y += .1f;
                }
                if (keyboard.IsKeyDown(Keys.E)) {
                    this.GameCamera.Forward.Y -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.Left)) {
                    this.GameCamera.Forward.X += .1f;
                }
                if (keyboard.IsKeyDown(Keys.Right)) {
                    this.GameCamera.Forward.X -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.Up)) {
                    this.GameCamera.Forward.Z += .1f;
                }
                if (keyboard.IsKeyDown(Keys.Down)) {
                    this.GameCamera.Forward.Z -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.Enter)) {
                    this.GameCamera.Forward = Vector3.UnitY;
                }
                if (keyboard.IsKeyDown(Keys.RightShift)) {
                    this.GameCamera.Position = new Vector3(0, 40, 20);
                }

                if (keyboard.IsKeyDown(Keys.NumPad0) && this.ViewCameraO == null) {
                    this.ViewCameraO = this.ViewCamera;
                    this.ViewCamera = this.GameCamera;
                }
                if (keyboard.IsKeyUp(Keys.NumPad0) && this.ViewCameraO != null) {
                    this.ViewCamera = this.ViewCameraO;
                    this.ViewCameraO = null;
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
            this.GraphicsDevice.Clear(Color.Black);

            this.DrawGround();
            this.DrawCamera(this.GameCamera);
            this.DrawSphere(this.GameCamera.LookAt);

            this.spriteBatch.Begin();

            this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(10, 0, 0), Color.Red);
            this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(0, 10, 0), Color.Green);
            this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(0, 0, 10), Color.Blue);

            this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+X", this.GameCamera.Position + new Vector3(11, 0, 0), Color.Red, Color.White);
            this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+Y", this.GameCamera.Position + new Vector3(0, 11, 0), Color.Green, Color.White);
            this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+Z", this.GameCamera.Position + new Vector3(0, 0, 11), Color.Blue, Color.White);

            this.spriteBatch.DrawString3D(this.font, this.ViewCamera, $"Camera: {this.GameCamera.Position}", this.GameCamera.Position, Color.White, Color.Black);
            this.spriteBatch.DrawString3D(this.font, this.ViewCamera, $"LookAt: {this.GameCamera.LookAt.Round(3)}", this.GameCamera.LookAt, Color.White, Color.Black);

            this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.LookAt, Color.Orange);

            this.spriteBatch.DrawString(this.font,
                $"Camera Position:  {this.GameCamera.Position}\n" +
                $"LookAt:           {this.GameCamera.LookAt.Round(3)}\n" +
                $"Yaw:              {Math.Round(MathHelper.ToDegrees(this.GameCamera.Yaw) * 10e2) / 10e2} Deg\n" +
                $"Pitch:            {Math.Round(MathHelper.ToDegrees(this.GameCamera.Pitch) * 10e2) / 10e2} Deg\n" +
                $"Roll:             {Math.Round(MathHelper.ToDegrees(this.GameCamera.Roll) * 10e2) / 10e2} Deg\n" +
                $"Up:               {this.GameCamera.Up.Round(3)}\n" +
                $"Forward:          {this.GameCamera.Forward.Round(3)}",
                new Vector2(2, 5), Color.Black, Color.White, new Vector2(2, 5));

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
