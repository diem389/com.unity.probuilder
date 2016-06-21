using UnityEngine;
using System.Collections.Generic;
using ProBuilder2.Common;
using System.Linq;

namespace ProBuilder2.MeshOperations
{
	public static class pb_AppendPolygon
	{
		/**
		 *	FillHole differs from CreatePolygon in that CreatePolygon expects vertices to be passed
		 *	with the correct winding order already applied.  FillHole projects and attempts to figure
		 *	out the winding order.
		 */
		public static pb_ActionResult FillHole(this pb_Object pb, IList<int> indices, out pb_Face face)
		{
			pb_IntArray[] sharedIndices = pb.sharedIndices;
			Dictionary<int, int> lookup = sharedIndices.ToDictionary();
			HashSet<int> common = pb_IntArrayUtility.GetCommonIndices(lookup, indices);
			List<pb_Vertex> vertices = new List<pb_Vertex>(pb_Vertex.GetVertices(pb));
			List<pb_Vertex> append_vertices = new List<pb_Vertex>();

			foreach(int i in common)
			{
				int index = sharedIndices[i][0];
				append_vertices.Add(new pb_Vertex(vertices[index]));
			}

			pb_FaceRebuildData data = FaceWithVertices(append_vertices);

			if(data != null)
			{
				data.sharedIndices = common.ToList();
				List<pb_Face> faces = new List<pb_Face>(pb.faces);
				pb_FaceRebuildData.Apply(new pb_FaceRebuildData[] { data }, vertices, faces, lookup, null);
				pb.SetVertices(vertices);
				pb.SetFaces(faces.ToArray());
				pb.SetSharedIndices(lookup);
				pb.ToMesh();

				// find an adjacent faces and test that the normals are correct
				List<pb_WingedEdge> wings = pb_WingedEdge.GetWingedEdges(pb);
				pb_WingedEdge newFace = wings.FirstOrDefault(x => x.face == data.face);
				face = newFace.face;

				pb_WingedEdge orig = newFace;

				// grab first edge with a valid opposite face
				while(newFace.opposite == null)
				{
					newFace = newFace.next;
					if(newFace == orig) break;
				}

				if(orig.opposite != null)
				{
					if(pb_ConformNormals.ConformOppositeNormal(orig.opposite))
						pb.ToMesh();
				}

				return new pb_ActionResult(Status.Success, "Fill Hole");
			}

			face = null;

			return new pb_ActionResult(Status.Failure, "Insufficient Points");
		}

		public static pb_FaceRebuildData FaceWithVertices(List<pb_Vertex> vertices)
		{
			List<int> triangles;

			if(pb_Triangulation.TriangulateVertices(vertices, out triangles))
			{
				pb_FaceRebuildData data = new pb_FaceRebuildData();
				data.vertices = vertices;
				data.face = new pb_Face(triangles.ToArray());
				return data;
			}

			return null;
		}
	}
}
