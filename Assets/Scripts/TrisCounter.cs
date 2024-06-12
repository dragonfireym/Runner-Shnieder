
// using UnityEngine;
// using UnityEditor;

// public class TrisCounter : MonoBehaviour
// {
//     [MenuItem("Tools/Count Triangles in Scene")]
//         public static void CountTriangles()
//         {
//             GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
//             int totalTriangles = 0;

//             foreach (GameObject obj in allObjects)
//             {
//                 MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
//                 if (meshFilter != null && meshFilter.sharedMesh != null)
//                 {
//                     int triangleCount = meshFilter.sharedMesh.triangles.Length / 3;
//                     Debug.Log(obj.name + " has " + triangleCount + " triangles");
//                     totalTriangles += triangleCount;
//                 }
//             }

//             Debug.Log("Total triangles in scene: " + totalTriangles);
//         }
// }
