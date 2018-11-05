using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorn.Extentions {
    public static class MeshExtentions {
        public static List<Vector3> Verticies(this ModelMesh mesh) {
            List<Vector3> verticies = new List<Vector3>();
            foreach (ModelMeshPart meshPart in mesh.MeshParts) {
                VertexPosition[] v = new VertexPosition[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData(v);
                verticies.AddRange(v.Select(x => x.Position));
            }
            return verticies;
        }

        public static BoundingSphere GetBoundingSphere(this ModelMesh mesh) {
            return BoundingSphere.CreateFromPoints(mesh.Verticies());
        }
    }
}
