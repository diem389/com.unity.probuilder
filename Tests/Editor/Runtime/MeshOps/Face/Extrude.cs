﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using NUnit.Framework;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.ProBuilder.Test;
using UnityEditor;
using UnityEngine.TestTools;

namespace UnityEngine.ProBuilder.RuntimeTests.MeshOps.Face
{
	public class Extrude
	{
		static System.Random m_Random = new System.Random();

		[Test]
		public static void ExtrudeAllFaces_FaceNormal()
		{
			using(var shapes = new TestUtility.BuiltInPrimitives())
			{
				foreach (var pb in (IEnumerable<ProBuilderMesh>) shapes)
				{
					pb.Extrude(pb.faces, ExtrudeMethod.FaceNormal, .5f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
#if PB_CREATE_TEST_MESH_TEMPLATES
					pb_TestUtility.SaveAssetTemplate(pb.msh, pb.name);
#endif
					Mesh template = TestUtility.GetAssetTemplate<Mesh>(pb.name);
					TestUtility.AssertAreEqual(pb.mesh, template);
				}
			}
		}

		[Test]
		public static void ExtrudeAllFaces_IndividualFaces()
		{
			using(var shapes = new TestUtility.BuiltInPrimitives())
			{
				foreach (var pb in (IEnumerable<ProBuilderMesh>) shapes)
				{
					int vertexCountBeforeExtrude = pb.vertexCount;
					pb.Extrude(pb.faces, ExtrudeMethod.IndividualFaces, .5f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					Assert.AreNotEqual(vertexCountBeforeExtrude, pb.vertexCount);
					TestUtility.AssertMeshAttributesValid(pb.mesh);
#if PB_CREATE_TEST_MESH_TEMPLATES
					pb_TestUtility.SaveAssetTemplate(pb.msh, pb.name);
#endif
					Mesh template = TestUtility.GetAssetTemplate<Mesh>(pb.name);
					TestUtility.AssertAreEqual(pb.mesh, template);
				}
			}
		}

		[Test]
		public static void ExtrudeAllFaces_VertexNormal()
		{
			using(var shapes = new TestUtility.BuiltInPrimitives())
			{
				foreach (var pb in (IEnumerable<ProBuilderMesh>) shapes)
				{
					pb.Extrude(pb.faces, ExtrudeMethod.VertexNormal, .5f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
#if PB_CREATE_TEST_MESH_TEMPLATES
					pb_TestUtility.SaveAssetTemplate(pb.msh, pb.name);
#endif
					Mesh template = TestUtility.GetAssetTemplate<Mesh>(pb.name);
					TestUtility.AssertAreEqual(pb.mesh, template);
				}
			}
		}

		static void ExtrudeSingleFace(ExtrudeMethod method, float distance = 1f)
		{
			using(var shapes = new TestUtility.BuiltInPrimitives())
			{
				foreach (var pb in (IEnumerable<ProBuilderMesh>) shapes)
				{
					int initialVertexCount = pb.vertexCount;
					UnityEngine.ProBuilder.Face face = pb.faces[m_Random.Next(0, pb.faces.Length)];
					pb.Extrude(new UnityEngine.ProBuilder.Face[] {face}, method, 1f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
#if PB_CREATE_TEST_MESH_TEMPLATES
					pb_TestUtility.SaveAssetTemplate(pb.msh, pb.name);
#endif
					Assert.AreEqual(initialVertexCount + face.edges.Length * 4, pb.vertexCount);
				}
			}
		}

		[Test]
		public static void ExtrudeSingleFace_FaceNormal()
		{
			ExtrudeSingleFace(ExtrudeMethod.FaceNormal);
		}

		[Test]
		public static void ExtrudeSingleFace_VertexNormal()
		{
			ExtrudeSingleFace(ExtrudeMethod.VertexNormal);
		}

		[Test]
		public static void ExtrudeSingleFace_IndividualFaces()
		{
			ExtrudeSingleFace(ExtrudeMethod.IndividualFaces);
		}

		[Test]
		public static void ExtrudeSingleFace_NegativeDirection_FaceNormal()
		{
			ExtrudeSingleFace(ExtrudeMethod.FaceNormal, -.5f);
		}

		[Test]
		public static void ExtrudeSingleFace_NegativeDirection_VertexNormal()
		{
			ExtrudeSingleFace(ExtrudeMethod.VertexNormal, -.5f);
		}

		[Test]
		public static void ExtrudeSingleFace_NegativeDirection_IndividualFaces()
		{
			ExtrudeSingleFace(ExtrudeMethod.IndividualFaces, -.5f);
		}

		[Test]
		public static void ExtrudeFaceMultipleTimes()
		{
			using (var shapes = new TestUtility.BuiltInPrimitives())
			{
				foreach (ProBuilderMesh pb in shapes)
				{
					int initialVertexCount = pb.vertexCount;
					UnityEngine.ProBuilder.Face face = pb.faces[m_Random.Next(0, pb.faces.Length)];
					pb.Extrude(new UnityEngine.ProBuilder.Face[] {face}, ExtrudeMethod.FaceNormal, 1f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
					Assert.AreEqual(initialVertexCount + face.edges.Length * 4, pb.vertexCount);

					initialVertexCount = pb.vertexCount;
					pb.Extrude(new UnityEngine.ProBuilder.Face[] {face}, ExtrudeMethod.VertexNormal, 1f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
					Assert.AreEqual(initialVertexCount + face.edges.Length * 4, pb.vertexCount);

					initialVertexCount = pb.vertexCount;
					pb.Extrude(new UnityEngine.ProBuilder.Face[] {face}, ExtrudeMethod.IndividualFaces, 1f);
					pb.ToMesh();
					pb.Refresh();
					LogAssert.NoUnexpectedReceived();
					TestUtility.AssertMeshAttributesValid(pb.mesh);
					Assert.AreEqual(initialVertexCount + face.edges.Length * 4, pb.vertexCount);
				}
			}
		}
	}
}
