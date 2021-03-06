﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    [SerializeField]
    GameObject roughTerrain;

    public Matrix4x4 mapPointMatrix;

    Mesh mesh;

    public Vector3[] vertices;
    int[] triangles;

    public int squareMeshVerticeBaseSize = 20;

    public int frequencyOfRoughTerrain = 75; //a number between 1-100. The closer to 100, the less frequent the rough terrain
    public bool meshingDone = false;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    //passing in an x and z value calculated from the dimention ratio of the selected table surface
    //hopefully this leads to less scaling fuckery than the first test build had 
    public void createShape(int sizeX, int sizeZ) {
        vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];

        //centering math
        int sizeXHalf = sizeX / 2;
        int sizeZHalf = sizeZ / 2;
        //nested loop to create the vertices
        for (int i = 0, z = -sizeZHalf; z <= sizeZHalf; z++) {
            for (int x = -sizeXHalf; x <= sizeXHalf; x++) {
                //float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f);
                float y = 0.01f;
                vertices[i] = new Vector3(x, y, z);
                ++i;
            }
        }

        //draw triangles between the vertices
        triangles = new int[sizeX * sizeZ * 6];
        int vert = 0;
        int tris = 0;

        for (int z = -sizeZHalf; z < sizeZHalf; z++) {
            for (int x = -sizeXHalf; x < sizeXHalf; x++) {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + sizeX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + sizeX + 1;
                triangles[tris + 5] = vert + sizeX + 2;

                ++vert;
                tris += 6;
            }
            ++vert;
        }

        UpdateMesh();
        gameObject.AddComponent<MeshCollider>();
        BakeNavMesh();

        //set the vertices to matrix points for spawning, etc.
        mapPointMatrix = transform.localToWorldMatrix;

        GenerateTerrainFeatures();
        
        meshingDone = true;
    }

    //update mesh to reflect the generated verticies + triangles 
    private void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        
    }

    private void GenerateTerrainFeatures() {
        //here we will add difficult or impassible terrain
        for (int i = 0; i < mesh.vertices.Length; ++i) {
            int randomnum = Random.Range(0, 100);
            if (randomnum > frequencyOfRoughTerrain) {
                //Matrix4x4 matrix = transform.localToWorldMatrix;
                Instantiate(roughTerrain, mapPointMatrix.MultiplyPoint3x4(mesh.vertices[i]), roughTerrain.transform.rotation, transform);
            }
        }
    }

    private void BakeNavMesh() {
        //create the nav mesh
        NavMeshSurface surface = GetComponentInParent<NavMeshSurface>();
        //NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
