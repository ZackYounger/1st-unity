using UnityEngine;
using System.Collections;

public class MeshDisplayer
{

	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public void DrawMesh(MeshData meshData)//, Texture2D texture)
	{
		Mesh lol = meshData.CreateMesh();
		//meshFilter.sharedMesh = meshData.CreateMesh();
		//meshRenderer.sharedMaterial.mainTexture = texture;
	}
}