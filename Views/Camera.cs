using Lorn.Extentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Lorn.Views {
    public class Camera {
        private GraphicsDeviceManager GraphicsManager;
        public GraphicsDevice Graphics => this.GraphicsManager.GraphicsDevice;

        /// <summary> The cameras position </summary>
        public Vector3 Position { get; set; }

        /// <summary> The cameras Field of View </summary>
        public float FOV { get; private set; }
        /// <summary> The viewport aspect ratio </summary>
        public float AspectRatio => this.Graphics.Viewport.AspectRatio;

        /// <summary> The closest a model will be rendered </summary>
        public float NearPlaneDistance { get; private set; }
        /// <summary> The farthest a model will be rendered </summary>
        public float FarPlaneDistance { get; private set; }


        private float _yaw;
        /// <summary> The cameras yaw </summary>
        public float Yaw {
            get => _yaw;
            set => _yaw = MathHelper.WrapAngle(value);
        }
        private float _pitch;
        /// <summary> The cameras pitch </summary>
        public float Pitch {
            get => _pitch;
            set => _pitch = MathHelper.WrapAngle(value);
        }
        private float _roll;
        /// <summary> The cameras roll </summary>
        public float Roll {
            get => _roll;
            set => _roll = MathHelper.WrapAngle(value);
        }

        /// <summary> The up direction without roll </summary>
        public Vector3 FlatUp => Vector3.Transform(this.Forward, Matrix.CreateFromAxisAngle(this.FlatLeft, -MathHelper.PiOver2)).Normalized();
        /// <summary> The up direction with pitch and roll </summary>
        public Vector3 Up => Vector3.Transform(this.FlatUp, Matrix.CreateFromAxisAngle(this.Forward, this.Roll)).Normalized();
        /// <summary> The forward direction with no Z value (just yaw) </summary>
        public Vector3 FlatForward => Vector3.Transform(Vector3.UnitY, Matrix.CreateRotationZ(this.Yaw)).Normalized();
        /// <summary> The forward direction with pitch and yaw </summary>
        public Vector3 Forward => Vector3.Transform(this.FlatForward, Matrix.CreateFromAxisAngle(this.FlatLeft, -this.Pitch)).Normalized();
        /// <summary> The left diretion with no Z value (just yaw) </summary>
        public Vector3 FlatLeft => Vector3.Transform(this.FlatForward, Matrix.CreateRotationZ(MathHelper.PiOver2)).Normalized();
        /// <summary> The left diretion with pitch yaw and roll </summary>
        public Vector3 Left => Vector3.Transform(this.FlatLeft, Matrix.CreateFromAxisAngle(this.Forward, this.Roll)).Normalized();

        /// <summary> The point the camera is looking at </summary>
        public Vector3 LookAt => this.Position + this.Forward;


        /// <summary> The View matrix that the camera can see </summary>
        public Matrix ViewMatrix => Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
        /// <summary> The Projection matrix that is the cameras perspective </summary>
        public Matrix ProjectionMatrix => Matrix.CreatePerspectiveFieldOfView(this.FOV, this.AspectRatio, this.NearPlaneDistance, this.FarPlaneDistance);

        /// <summary> The area in which the camera can see </summary>
        public BoundingFrustum ViewFrustrum => new BoundingFrustum(this.ViewMatrix * this.ProjectionMatrix);

        public Camera(GraphicsDeviceManager graphics): this(graphics, Vector3.UnitZ) { }
        public Camera(GraphicsDeviceManager graphics, Vector3 position): this(graphics, position, Vector3.Zero) { }
        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll): this(graphics, position, yawPitchRoll, MathHelper.PiOver4) { }
        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll, float FOV): this(graphics, position, yawPitchRoll, FOV, 1, 2000) { }
        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll, float FOV, float nearPlane, float farPlane) {
            this.Yaw = yawPitchRoll.X;
            this.Pitch = yawPitchRoll.Y;
            this.Roll = yawPitchRoll.Z;

            this.Position = position;
            this.FOV = FOV;
            this.GraphicsManager = graphics;

            this.NearPlaneDistance = nearPlane;
            this.FarPlaneDistance = farPlane;
        }

        /// <summary> Checks if the bounding sphere is within the cameras view </summary>
        public bool InView(BoundingSphere bounds) => ViewFrustrum.Intersects(bounds);
        public bool InView(BoundingBox bounds) => ViewFrustrum.Intersects(bounds);
        public bool InView(BoundingFrustum bounds) => ViewFrustrum.Intersects(bounds);

        /// <summary> Draw a model at the given world using the camera </summary>
        public void DrawModel(Model model, Matrix world) {

            foreach (ModelMesh mesh in model.Meshes) {

                BoundingSphere boundingSphere = model.GetBoundingSphere();

                boundingSphere = boundingSphere.Transform(world);

                //if (!InView(boundingSphere)) return;

                foreach (BasicEffect meshEffect in mesh.Effects) {
                    meshEffect.EnableDefaultLighting();
                    meshEffect.PreferPerPixelLighting = true;

                    meshEffect.World = world;
                    meshEffect.View = this.ViewMatrix;
                    meshEffect.Projection = this.ProjectionMatrix;

                    //meshEffect.VertexColorEnabled = true;
                    //meshEffect.AmbientLightColor = new Vector3(100);
                }

                mesh.Draw();
            }
        }

        static BasicEffect BasicEffect;
        public void DrawLine3D(Vector3 start, Vector3 end) => DrawLine3D(start, end, Color.White);
        public void DrawLine3D(Vector3 start, Vector3 end, Color color) {
            if (BasicEffect == null) {
                BasicEffect = new BasicEffect(Graphics) {
                    View = ViewMatrix,
                    Projection = ProjectionMatrix,
                    VertexColorEnabled = true
                };
            }

            BasicEffect.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] vertices = new[] { new VertexPositionColor(start, color), new VertexPositionColor(end, color) };
            Graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }
    }
}
