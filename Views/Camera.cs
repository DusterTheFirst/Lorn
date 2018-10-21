using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorn.Views {
    public class Camera {
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// The cameras position
        /// </summary>
        public Vector3 Position { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Matrix ViewMatrix {
            get {
                var lookAtVector = new Vector3(0, -1, -.5f);
                // We'll create a rotation matrix using our angle
                var rotationMatrix = Matrix.CreateFromYawPitchRoll(MathHelper.WrapAngle(Yaw), MathHelper.WrapAngle(Pitch), 0);
                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += Position;

                var upVector = Vector3.Transform(new Vector3(0, 0, 1), Matrix.CreateRotationZ(Roll));
                Debug.WriteLine(upVector);
                Debug.WriteLine(Roll);


                return Matrix.CreateLookAt(Position, lookAtVector, upVector);
            }
        }

        public Camera() : this(Vector3.UnitZ, Vector3.Zero) { }
        public Camera(Vector3 position) : this(position, Vector3.Zero) { }
        public Camera(Vector3 position, float yaw, float pitch, float roll) : this(position, new Vector3(yaw, pitch, roll)) { }
        public Camera(Vector3 position, Vector3 yawPitchRoll) {
            Yaw = yawPitchRoll.X;
            Pitch = yawPitchRoll.Y;
            Roll = yawPitchRoll.Z;

            Position = position;
        }
    }
}
