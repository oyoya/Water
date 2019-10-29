using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetOrAddComponent<T>(this GameObject gameobject) where T : Component
    {
        T component = gameobject.GetComponent<T>();
        if(!component)
        {
            component = gameobject.AddComponent<T>();
        }
        return component;
    }


    public static Mesh GenerateMesh(float width, float length, float cellSize)
    {
        int xsize = Mathf.RoundToInt(width / cellSize);
        int ysize = Mathf.RoundToInt(length / cellSize);

        Mesh mesh = new Mesh();

        List<Vector3> vertexList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();
        List<Vector3> normalList = new List<Vector3>();
        List<int> indexList = new List<int>();
        float xcellsize = width / xsize;
        float uvxcellsize = 1.0f / xsize;
        float ycellsize = length / ysize;
        float uvycellsize = 1.0f / ysize;

        for (int i = 0; i <= ysize; i++)
        {
            for (int j = 0; j <= xsize; j++)
            {
                vertexList.Add(new Vector3(-width * 0.5f + j * xcellsize, 0, -length * 0.5f + i * ycellsize));
                uvList.Add(new Vector2(j * uvxcellsize, i * uvycellsize));
                normalList.Add(Vector3.up);

                if (i < ysize && j < xsize)
                {
                    indexList.Add(i * (xsize + 1) + j);
                    indexList.Add((i + 1) * (xsize + 1) + j);
                    indexList.Add((i + 1) * (xsize + 1) + j + 1);

                    indexList.Add(i * (xsize + 1) + j);
                    indexList.Add((i + 1) * (xsize + 1) + j + 1);
                    indexList.Add(i * (xsize + 1) + j + 1);
                }
            }
        }

        mesh.SetVertices(vertexList);
        mesh.SetUVs(0, uvList);
        mesh.SetNormals(normalList);
        mesh.SetTriangles(indexList, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }
}
