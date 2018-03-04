using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssemblyCSharp
{
    public class ProceduralTreeGeneratorAvecPerlin : MonoBehaviour
    {
        [Tooltip("Modèles à utiliser pour générer les arbres")]
        public GameObject[] modeleArbres;
        [Tooltip("Limite associée à chaque modèle. -1 signifie aucune limite.")]
        public int[] limiteModele;
        [Tooltip("Distance minimale entre les arbres")]
        public int distMinGen = 5;
        [Tooltip("Jeu sur une partie de l'algo qui permet de générer des points plus ou moins vite")]
        public int nbPtsEssai = 10;
        [Tooltip("Distance de recherche dans les cellules avoisinantes de la grille")]
        public int sqSize = 5;

        [Tooltip("Distance de recherche dans les cellules avoisinantes de la grille")]
        public int scalePerlin = 5;

        [Tooltip("Joue avec la probabilité de générer un motton d'arbre. Plus près de 0 donne moins d'arbres.")]
        public float multiplicateurChance = 1.1f;

        private Vector2 randomStartPerlin;
        public float multiplicateurPerlin = 1;
        public float biaisPerlin = 0;
        public float biaisChance = 0;
        public bool afficherPlan = false;

        void Start()
        {
            randomStartPerlin = new Vector2(Random.value * 100 - 50, Random.value * 100 - 50);
            var poisson = generate_poisson((int)transform.localScale.x, (int)transform.localScale.z, distMinGen, nbPtsEssai, sqSize);
            foreach (var p in poisson)
            {
                if (Mathf.PerlinNoise(Remap(p.x, 0, transform.localScale.x, 0, scalePerlin) + randomStartPerlin.x, Remap(p.z, 0, transform.localScale.z, 0, scalePerlin) + randomStartPerlin.y) * multiplicateurPerlin + biaisPerlin > Random.value * multiplicateurChance + biaisChance)
                {
                    var index_modele_arbres = Random.Range(0, modeleArbres.Length);
                    
                    bool peut_generer_arbre = (index_modele_arbres < limiteModele.Length && (limiteModele[index_modele_arbres] > 0 || limiteModele[index_modele_arbres] == -1)) || index_modele_arbres >= limiteModele.Length;
                    int index_avant_recherche = index_modele_arbres;
                    while (!peut_generer_arbre)
                    {
                        index_modele_arbres = (index_modele_arbres + 1) % modeleArbres.Length;
                        peut_generer_arbre = (index_modele_arbres < limiteModele.Length && (limiteModele[index_modele_arbres] > 0 || limiteModele[index_modele_arbres] == -1)) || index_modele_arbres >= limiteModele.Length;
                        if(index_avant_recherche == index_modele_arbres)
                        {
                            //plus aucun modèle d'arbre disponible :(
                            return;
                        }
                    }
                    var modele = modeleArbres[index_modele_arbres];
                    if (index_modele_arbres < limiteModele.Length && limiteModele[index_modele_arbres] != -1)
                    {
                        limiteModele[index_modele_arbres]--;
                    }
                    var instance = Instantiate(modele, transform.position + p - (transform.localScale / 2), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
					instance.transform.SetParent (this.transform);
                }
                
            }
            if (afficherPlan)
            {
                var k = new Texture2D(1000, 1000);
                for (int y = 0; y < k.height; y++)
                {
                    for (int x = 0; x < k.width; x++)
                    {
                        float p_Val = Mathf.PerlinNoise(Remap(x, 0, k.width, 0, scalePerlin) + randomStartPerlin.x, Remap(y, 0, k.height, 0, scalePerlin) + randomStartPerlin.y);
                        k.SetPixel(x, y, new Color(p_Val, p_Val, p_Val));
                    }
                }
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.localScale = new Vector3(10, 0, 10);
                // Position and rotation plane
                plane.transform.position = new Vector3(0, 6.074f, 0);
                plane.transform.rotation = Quaternion.Euler(0, 180, 0);
                var materiel_plan = plane.GetComponent<MeshRenderer>().material;
                materiel_plan.shader = Shader.Find("Unlit/Texture");
                materiel_plan.SetTexture("_MainTex", k);
                k.filterMode = FilterMode.Point;
                k.wrapMode = TextureWrapMode.Repeat;
                k.Apply();
            }
            
        }
        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }

        Vector2 generateRandomPointAround(Vector2 point, int mindist)
        {
            var r1 = Random.value;
            var r2 = Random.value;
            var radius = mindist * (r1 + 1);
            var angle = 2 * Mathf.PI * r2;
            var newX = point.x + radius * Mathf.Cos(angle);
            var newY = point.y + radius * Mathf.Sin(angle);
            return new Vector2(newX, newY);
        }

        Vector2 imageToGrid(Vector2 point, double cellSize)
        {
            var gridX = (int)(point.x / cellSize);
            var gridY = (int)(point.y / cellSize);
            return new Vector2(gridX, gridY);
        }

        bool inNeighbourhood(Vector2[,] grid, Vector2 gridPoint, int mindist, int squareSize, float cellSize, int width, int height)
        {
            var cellsAroundPoint = squareAroundPoint(grid, gridPoint, cellSize, squareSize, width, height);
            foreach(var cell in cellsAroundPoint)
            {
                if (Vector2.Distance(cell, gridPoint) < mindist)
                {
                    return true;
                }
                                
            }
            return false;
        }

        Vector2[] squareAroundPoint(Vector2[,] grid, Vector2 gridPoint, float cellSize, int size, int width, int height)
        {
            var qIndex = imageToGrid(gridPoint, cellSize);
            var pointList = new List<Vector2>();
            int lowerHalf = Mathf.FloorToInt(size / 2f);
            int upperHalf = Mathf.CeilToInt(size / 2f);
            for (var i = (int)Mathf.Max(0, qIndex.x - lowerHalf); i < Mathf.Min(width, qIndex.x + upperHalf); i++)
            {
                for (var j = (int)Mathf.Max(0, qIndex.y - lowerHalf); j < Mathf.Min(height, qIndex.y + upperHalf); j++)
                {
                    pointList.Add(grid[i, j]);
                }
            }
            return pointList.ToArray();
        }

        List<Vector3> generate_poisson(int width, int height, int min_dist, int new_points_count, int squareSize)
        {
            float cellSize = min_dist / Mathf.Sqrt(2);
            int gridW = Mathf.CeilToInt(width / cellSize);
            int gridH = Mathf.CeilToInt(height / cellSize);
            var grid = new Vector2[gridW, gridH];

            var processList = new List<Vector2>();
            var samplePoints = new List<Vector3>();
            
            var firstPoint = new Vector2(Random.Range(0, width), Random.Range(0,height));
            processList.Add(firstPoint);
            samplePoints.Add(new Vector3(firstPoint.x, 0, firstPoint.y));
            var index = imageToGrid(firstPoint, cellSize);
            grid[(int)index.x, (int)index.y] = firstPoint;

            while (processList.Count != 0)
            {
                int choix = Random.Range(0, processList.Count);
                var point = processList[choix];
                processList.RemoveAt(choix);
                for (int i = 0; i < new_points_count; i++)
                {
                    var newPoint = generateRandomPointAround(point, min_dist);
                    if (newPoint.x >= 0 && newPoint.x <= width && newPoint.y >= 0 && newPoint.y <= height && !inNeighbourhood(grid, newPoint, min_dist, squareSize, cellSize, gridW, gridH))
                    {
                        processList.Add(newPoint);
                        samplePoints.Add(new Vector3(newPoint.x, 0, newPoint.y));
                        var indexNew = imageToGrid(newPoint, cellSize);
                        grid[(int)indexNew.x, (int)indexNew.y] = newPoint;
                    }
                }
            }
            return samplePoints;
        }
    }
}