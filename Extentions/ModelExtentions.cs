using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorn.Extentions {
    public static class ModelExtentions {
        public static List<Vector3> Verticies(this Model model) {
            List<Vector3> verticies = new List<Vector3>();
            foreach (ModelMesh mesh in model.Meshes) {
                foreach (ModelMeshPart meshPart in mesh.MeshParts) {
                    VertexPosition[] v = new VertexPosition[meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(v);
                    verticies.AddRange(v.Select(x => x.Position));
                }
            }
            return verticies;
        }

        public static BoundingSphere GetBoundingSphere(this Model model) {
            return BoundingSphere.CreateFromPoints(model.Verticies());
        }
    }
}
