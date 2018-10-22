using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Lorn.Views {
    public class Camera {
        private GraphicsDeviceManager GraphicsManager;
        public GraphicsDevice Graphics => this.GraphicsManager.GraphicsDevice;

        /// <summary>
        /// The cameras position
        /// </summary>
        public Vector3 Position { get; set; }

        public float FOV { get; private set; }
        public float AspectRatio => this.Graphics.Viewport.AspectRatio;

        public float NearPlaneDistance { get; private set; }
        public float FarPlaneDistance { get; private set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Vector3 Up => Vector3.Transform(new Vector3(0, 0, 1), Matrix.CreateRotationZ(this.Roll));
        public Vector3 Forward;

        public Vector3 LookAt => this.Position + this.Forward; /*{
            get {
                Vector3 lookAtVector = new Vector3(0, -1, -.5f);
                // We'll create a rotation matrix using our angle
                Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(this.Yaw, this.Pitch, this.Roll);
                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += this.Position;
                return lookAtVector;
            }
        }*/

        public Matrix ViewMatrix => Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);

        public Matrix ProjectionMatrix => Matrix.CreatePerspectiveFieldOfView(this.FOV, this.AspectRatio, this.NearPlaneDistance, this.FarPlaneDistance);



        public Camera(GraphicsDeviceManager graphics)
            : this(graphics, Vector3.UnitZ, Vector3.Zero, MathHelper.PiOver4, 1, 2000) { }

        public Camera(GraphicsDeviceManager graphics, Vector3 position)
            : this(graphics, position, Vector3.Zero, MathHelper.PiOver4, 1, 2000) { }

        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll) 
            : this(graphics, position, yawPitchRoll, MathHelper.PiOver4, 1, 2000) { }

        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll, float FOV) 
            : this(graphics, position, yawPitchRoll, FOV, 1, 2000) { }

        public Camera(GraphicsDeviceManager graphics, Vector3 position, Vector3 yawPitchRoll, float FOV, float nearPlane, float farPlane) {
            this.Yaw = yawPitchRoll.X;
            this.Pitch = yawPitchRoll.Y;
            this.Roll = yawPitchRoll.Z;

            this.Position = position;
            this.FOV = FOV;
            this.GraphicsManager = graphics;

            this.NearPlaneDistance = nearPlane;
            this.FarPlaneDistance = farPlane;

            this.Forward = -position;
        }
    }
}
