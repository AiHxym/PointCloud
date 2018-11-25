using UnityEngine;
using System.Collections;
using System.IO;

public class DisplayPointCloudByGpu : MonoBehaviour
{
    int numPoints = 60000;
    [SerializeField] string dataPath;
    BinaryReader br;

    void Start()
    {
        try
        {
            br = new BinaryReader(new FileStream(dataPath, FileMode.Open));
        }
        catch (IOException e)
        {

            Debug.Log(e.Message + "\n Cannot open file.");
            return;
        }

        // 1. 读取数据
        ArrayList arrayListXYZ = new ArrayList();
        ArrayList arrayListRGB = new ArrayList();
        try
        {
            while (true)
            {
                float x = br.ReadInt32();
                float y = br.ReadInt32();
                float z = br.ReadInt32();
                Vector3 xyz = new Vector3(x, y, z);
                xyz /= 10000.0f;
                arrayListXYZ.Add(xyz);
                Color colorRGB = Color.gray;
                arrayListRGB.Add(colorRGB);
            }
        }
        catch (IOException e)
        {

        }

        float avx = 0;
        float avy = 0;
        float avz = 0;

        for (int i = 0; i < arrayListXYZ.Count; ++i)
        {
            Vector3 xyz = (Vector3)arrayListXYZ[i];
            avx += xyz.x / arrayListXYZ.Count;
            avy += xyz.y / arrayListXYZ.Count;
            avz += xyz.z / arrayListXYZ.Count;
        }



        // 2. 渲染
        int num = arrayListRGB.Count;

        int meshNum = num / numPoints;
        int leftPointsNum = num % numPoints;

        for (int i = 0; i < meshNum; i++)
        {
            GameObject obj = new GameObject();
            obj.name = i.ToString();
            obj.transform.position = new Vector3(-avx, -avy, -avz);
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            Mesh tempMesh = new Mesh();
            CreateMesh(ref tempMesh, ref arrayListXYZ, ref arrayListRGB, i * numPoints, numPoints);
            Material material = new Material(Shader.Find("Custom/VertexColor"));
            obj.GetComponent<MeshFilter>().mesh = tempMesh;
            obj.GetComponent<MeshRenderer>().material = material;
        }
        GameObject objLeft = new GameObject();
        objLeft.name = meshNum.ToString();
        objLeft.transform.position = new Vector3(-avx, -avy, -avz);
        //objLeft.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        objLeft.AddComponent<MeshFilter>();
        objLeft.AddComponent<MeshRenderer>();
        Mesh tempMeshLeft = new Mesh();
        CreateMesh(ref tempMeshLeft, ref arrayListXYZ, ref arrayListRGB, meshNum * numPoints, leftPointsNum);
        Material materialLeft = new Material(Shader.Find("Custom/VertexColor"));
        objLeft.GetComponent<MeshFilter>().mesh = tempMeshLeft;
        objLeft.GetComponent<MeshRenderer>().material = materialLeft;
        
    }
    void CreateMesh(ref Mesh mesh, ref ArrayList arrayListXYZ, ref ArrayList arrayListRGB, int beginIndex, int pointsNum)
    {
        Vector3[] points = new Vector3[pointsNum];
        Color[] colors = new Color[pointsNum];
        int[] indecies = new int[pointsNum];
        for (int i = 0; i < pointsNum; ++i)
        {
            points[i] = (Vector3)arrayListXYZ[beginIndex + i];
            indecies[i] = i;
            colors[i] = (Color)arrayListRGB[beginIndex + i];
        }

        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);

    }
}

