﻿using Microsoft.Xna.Framework;
using System;

namespace Lorn.Extentions {
    public static class Vector3Extentions {
        /// <summary>
        /// Rounds Vector3.
        /// </summary>
        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2) {
            float multiplier = (float)Math.Pow(10, decimalPlaces);
            return new Vector3(
                (float)Math.Round(vector3.X * multiplier) / multiplier,
                (float)Math.Round(vector3.Y * multiplier) / multiplier,
                (float)Math.Round(vector3.Z * multiplier) / multiplier);
        }
        public static Vector3 Normalized(this Vector3 vector3) {
            Vector3 clone = new Vector3(vector3.X, vector3.Y, vector3.Z);
            clone.Normalize();
            return clone;
        }
    }
}
