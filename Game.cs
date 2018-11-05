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
    public class Game : Microsoft.Xna.Framework.Game {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Camera GameCamera;
        Camera ViewCamera;

        Camera ViewCameraO;

        Model cameraModel;
        Model sphere;

        Texture2D pixel;

        public Game() {
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

            this.GameCamera = new Camera(this.graphics, new Vector3(0, 40, 20), Vector3.Zero, MathHelper.PiOver2);
            this.ViewCamera = new Camera(this.graphics, new Vector3(0, 90, 70), new Vector3(MathHelper.Pi, -MathHelper.PiOver4, 0));

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

        public void DrawCamera(Camera camera) =>
            ViewCamera.DrawModel(this.cameraModel, Matrix.CreateWorld(camera.Position, camera.Forward, camera.Up));

        public void DrawSphere(Vector3 position) => this.DrawSphere(position, Vector3.UnitY, Vector3.UnitZ);
        public void DrawSphere(Vector3 position, Vector3 forward) => this.DrawSphere(position, forward, Vector3.UnitZ);
        public void DrawSphere(Vector3 position, Vector3 forward, Vector3 up) =>
            ViewCamera.DrawModel(this.sphere, Matrix.CreateWorld(position, forward, up));

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.font = this.Content.Load<SpriteFont>("Fonts/SourceCodePro");
            //this.cameraModel = this.Content.Load<Model>("Models/Editor/Camera");
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
                    this.GameCamera.Position += this.GameCamera.FlatForward * new Vector3(.1f);
                }
                if (keyboard.IsKeyDown(Keys.S)) {
                    this.GameCamera.Position -= this.GameCamera.FlatForward * new Vector3(.1f);
                }
                if (keyboard.IsKeyDown(Keys.A)) {
                    this.GameCamera.Position += this.GameCamera.FlatLeft * new Vector3(.1f, .1f, 0);
                }
                if (keyboard.IsKeyDown(Keys.D)) {
                    this.GameCamera.Position -= this.GameCamera.FlatLeft * new Vector3(.1f, .1f, 0);
                }
                if (keyboard.IsKeyDown(Keys.Space)) {
                    this.GameCamera.Position += new Vector3(0, 0, 1);
                }
                if (keyboard.IsKeyDown(Keys.LeftShift)) {
                    this.GameCamera.Position -= new Vector3(0, 0, 1);
                }

                if (keyboard.IsKeyDown(Keys.Q)) {
                    this.GameCamera.Roll -= .1f;
                }
                if (keyboard.IsKeyDown(Keys.E)) {
                    this.GameCamera.Roll += .1f;
                }
                if (keyboard.IsKeyDown(Keys.Left)) {
                    this.GameCamera.Yaw += MathHelper.PiOver4 / 20;
                }
                if (keyboard.IsKeyDown(Keys.Right)) {
                    this.GameCamera.Yaw -= MathHelper.PiOver4 / 20;
                }
                if (keyboard.IsKeyDown(Keys.Up)) {
                    this.GameCamera.Pitch += MathHelper.PiOver4 / 20;
                }
                if (keyboard.IsKeyDown(Keys.Down)) {
                    this.GameCamera.Pitch -= MathHelper.PiOver4 / 20;
                }
                if (keyboard.IsKeyDown(Keys.Enter)) {
                    this.GameCamera.Yaw = 0;
                    this.GameCamera.Pitch = 0;
                    this.GameCamera.Roll = 0;
                }
                if (keyboard.IsKeyDown(Keys.Back)) {
                    this.GameCamera.Position = new Vector3(0, 40, 20);
                }

                if (keyboard.IsKeyDown(Keys.RightShift) && this.ViewCameraO == null) {
                    this.ViewCameraO = this.ViewCamera;
                    this.ViewCamera = this.GameCamera;
                }
                if (keyboard.IsKeyUp(Keys.RightShift) && this.ViewCameraO != null) {
                    this.ViewCamera = this.ViewCameraO;
                    this.ViewCameraO = null;
                }

                // Mouse movement
                if (keyboard.IsKeyUp(Keys.LeftControl)) {
                    IsMouseVisible = false;

                    Vector3 mouseOffset = new Vector3(graphics.PreferredBackBufferWidth / 2, 0, graphics.PreferredBackBufferHeight / 2);

                    Point delta = mouse.Position;
                    Mouse.SetPosition((int)mouseOffset.X, (int)mouseOffset.Z);

                    Vector3 changeVector = new Vector3(delta.X, 0, delta.Y) - mouseOffset;

                    //Debug.WriteLine(changeVector.ToString());

                    GameCamera.Yaw -= changeVector.X * .01f;
                    GameCamera.Pitch -= changeVector.Z * .01f;
                    //cameraTarget -= changeVector;
                } else {
                    IsMouseVisible = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);

            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            this.DrawGround();
            this.DrawCamera(this.GameCamera);

            this.spriteBatch.Begin();

            if (this.ViewCameraO == null) {

                //this.DrawSphere(this.GameCamera.Forward * 10 + this.GameCamera.Position, GameCamera.Forward, GameCamera.Up);
                //this.DrawSphere(this.GameCamera.Left * 10 + this.GameCamera.Position, GameCamera.Forward, GameCamera.Up);
                //this.DrawSphere(this.GameCamera.Up * 10 + this.GameCamera.Position, GameCamera.Forward, GameCamera.Up);
                //this.DrawSphere(this.GameCamera.FlatForward * 10 + this.GameCamera.Position, GameCamera.Forward, GameCamera.Up);

                //this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(10, 0, 0), Color.Red);
                //this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(0, 10, 0), Color.Green);
                //this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Position + new Vector3(0, 0, 10), Color.Blue);

                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+X", this.GameCamera.Position + new Vector3(11, 0, 0), Color.Red, Color.White);
                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+Y", this.GameCamera.Position + new Vector3(0, 11, 0), Color.Green, Color.White);
                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, "+Z", this.GameCamera.Position + new Vector3(0, 0, 11), Color.Blue, Color.White);

                //this.spriteBatch.DrawString3D(this.font, this.ViewCamera, $"Camera: {this.GameCamera.Position}", this.GameCamera.Position, Color.White, Color.Black);
                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, $"Forward: {this.GameCamera.Forward.Round(3)}", this.GameCamera.Forward * 10 + this.GameCamera.Position, Color.White, Color.Black);
                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, $"Left: {this.GameCamera.Left.Round(3)}", this.GameCamera.Left * 10 + this.GameCamera.Position, Color.White, Color.Black);
                //this.spriteBatch.DrawStringCentered3D(this.font, this.ViewCamera, $"Up: {this.GameCamera.Up.Round(3)}", this.GameCamera.Up * 10 + this.GameCamera.Position, Color.White, Color.Black);

                this.ViewCamera.DrawLine3D(this.GameCamera.Position, this.GameCamera.Forward * 10 + this.GameCamera.Position, Color.Orange);
                //this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.Left * 10 + this.GameCamera.Position, Color.Yellow);
                this.ViewCamera.DrawLine3D(this.GameCamera.Position, this.GameCamera.Up * 10 + this.GameCamera.Position, Color.Purple);
                //this.spriteBatch.DrawLine3D(this.ViewCamera, this.GameCamera.Position, this.GameCamera.FlatForward * 10 + this.GameCamera.Position, Color.SandyBrown);

                this.ViewCamera.DrawLine3D(this.GameCamera.Position,
                                               Vector3.Transform(this.GameCamera.Forward, 
                                                                 Matrix.CreateFromAxisAngle(this.GameCamera.Up, this.GameCamera.FOV / 2))
                                           * 10 
                                           + this.GameCamera.Position,
                                           Color.Green);
                this.ViewCamera.DrawLine3D(this.GameCamera.Position,
                                               Vector3.Transform(this.GameCamera.Forward, 
                                                                 Matrix.CreateFromAxisAngle(this.GameCamera.Up, -this.GameCamera.FOV / 2))
                                           * 10 
                                           + this.GameCamera.Position, 
                                           Color.Green);
            }

            //BoundingFrustum frustum = this.ViewCamera.ViewFrustrum;

            //this.ViewCamera.DrawLine3D(frustum.Left.Normal, frustum.Left.Normal * 50);
            //this.ViewCamera.DrawLine3D(frustum.Right.Normal, frustum.Right.Normal * 50);
            //this.ViewCamera.DrawLine3D(frustum.Top.Normal, frustum.Top.Normal * 50);
            //this.ViewCamera.DrawLine3D(frustum.Bottom.Normal, frustum.Bottom.Normal * 50);

            this.spriteBatch.DrawString(this.font,
                $"Camera Position:  {this.GameCamera.Position}\n" +
                $"Yaw:              {Math.Round(MathHelper.ToDegrees(this.GameCamera.Yaw) * 10e2) / 10e2} Deg\n" +
                $"Pitch:            {Math.Round(MathHelper.ToDegrees(this.GameCamera.Pitch) * 10e2) / 10e2} Deg\n" +
                $"Roll:             {Math.Round(MathHelper.ToDegrees(this.GameCamera.Roll) * 10e2) / 10e2} Deg",
                new Vector2(2, 5), Color.White, Color.Black * .5f, new Vector2(2, 5));

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
