using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Mesh : MonoBehaviour
{
    public int xSize = 20, zSize = 20;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    public Material material;
    
    
    public Vector3[] newVertices, newVerts;
    public Vector2[] newUV;
    public int[] newTriangles;

    private float zVal;
    public float offSetx;
    public float offSetz;

    private void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        //GetComponent<MeshCollider>().convex = true;
        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<Rigidbody>();
        cube.transform.position = new Vector3(5f, 20f, 10f);
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        MeshWavePerls(5f, 5f);
    }

    void MeshWavePerls(float lat, float lon)
    {
        mesh = this.gameObject.GetComponent<MeshFilter>().mesh;

        newVerts = new Vector3[(xSize + 1) * (zSize + 1)];
        float amplitude = 2.0f;
        float wavelength = 10f;

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++, i++)
            {
                zVal = amplitude * Mathf.Sin((float)(mesh.vertices[i].x * 2 * Mathf.PI / wavelength) + Time.time) * Mathf.Cos((float)(mesh.vertices[i].z * 2 * Mathf.PI / wavelength) + Time.time);


                float xCoord = offSetx + 333f * x / xSize * amplitude;
                float zCoord = offSetz + 222f * z / zSize * amplitude;

                //zVal = zVal + Mathf.PerlinNoise(xCoord, zCoord);

                newVerts[i] = new Vector3(x, zVal + Mathf.PerlinNoise(xCoord, zCoord), z);
            }
        }
        newVertices = newVerts;
        mesh.vertices = newVerts;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    void CreateShape()
    {
        newVertices = new Vector3[(xSize + 1) * (zSize + 1)];

        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                newVertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        newTriangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                newTriangles[tris + 0] = vert + 0;
                newTriangles[tris + 1] = vert + xSize + 1;
                newTriangles[tris + 2] = vert + 1;
                newTriangles[tris + 3] = vert + 1;
                newTriangles[tris + 4] = vert + xSize + 1;
                newTriangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

                
            }
            vert++;
        }
        

        

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();
    }

    

    
}
