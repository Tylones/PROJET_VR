using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class testPlanTriangles : MonoBehaviour
{
    private bool corrigerBug = true;
    public test ScriptTest;
    private string fonction;
    public Mesh mesh;
    private int cpt;
    private bool cptAsc;
    private const int TAILLE_FCT = 4;
    private float lastValidValue;

    private int borne_min_precedente;
    private int borne_max_precedente;
    private int echantillonage_precedent;
    public int echantillonage; // valeur visible par l'utilisateur, qui ne peut être de type Decimal

    private int verticesNumber;
    public int borne_min;
    public int borne_max;
    private GameObject objetPlan;


    private void Awake()
    {
        ScriptTest = GetComponent<test>();
    }

    void Start()
    {
        objetPlan = GameObject.Find("Plan");

        ScriptTest = GetComponent<test>();
        //Dessiner("");
    }


    private void Update()
    {
        if (cptAsc)
        {
            if (cpt > 50 && cpt < 150)
                transform.position += new Vector3(0, 0.001f, 0);
            else
                transform.position += new Vector3(0, 0.0007f, 0);
            cpt++;
            if (cpt == 200)
                cptAsc = false;
        }
        else
        {
            if (cpt < 150 && cpt > 50)
                transform.position += new Vector3(0, -0.001f, 0);
            else
                transform.position += new Vector3(0, -0.0007f, 0);
            cpt--;
            if (cpt == 0)
                cptAsc = true;
        }
    }


    public void Dessiner(string fonction)
    {
        cpt = 0;
        cptAsc = true;
        this.fonction = fonction;
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        borne_min = -2;
        borne_min_precedente = borne_min;
        borne_max = 2;
        borne_max_precedente = borne_max;
        echantillonage = 92;
        echantillonage_precedent = echantillonage;

        drawPlan(mesh);

    }

    void LateUpdate()
    {
        /*Si l'échantillonage ou les bornes d'affichage sont modifiées, alors on redessine la fonction */
        if ((echantillonage != echantillonage_precedent || borne_min != borne_min_precedente || borne_max != borne_max_precedente) && mesh != null)
        {
            echantillonage_precedent = echantillonage;
            borne_min_precedente = borne_min;
            borne_max_precedente = borne_max;
            drawPlan(mesh);
        }
    }

    void drawPlan(Mesh mesh)
    {
        mesh.Clear();

        Vector3[] newVertices;
        int[] newTriangles;

        int largeur_plan = borne_max - borne_min;

        decimal pas = (decimal)(borne_max - borne_min) / (decimal)echantillonage;

        //if (pas.ToString().Length == 30)
        //{

        //    int difference = 1;
        //    bool found = false;
        //    while (!found)
        //    {
        //        if (((decimal)(borne_max - borne_min) / ((decimal)echantillonage + difference)).ToString().Length < 30)
        //        {
        //            echantillonage += difference;
        //            echantillonage_precedent = echantillonage;
        //            pas = (decimal)(borne_max - borne_min) / (decimal)echantillonage;
        //            found = true;
        //        }
        //        else
        //        {
        //            if (((decimal)(borne_max - borne_min) / ((decimal)echantillonage - difference)).ToString().Length < 30)
        //            {
        //                echantillonage -= difference;
        //                echantillonage_precedent = echantillonage;
        //                pas = (decimal)(borne_max - borne_min) / (decimal)echantillonage;
        //                found = true;
        //            }
        //            else
        //                difference++;
        //        }
        //    }
        //}

        /* Initialisation Vertices*/
        newVertices = new Vector3[(echantillonage * echantillonage)];
        verticesNumber = newVertices.Length;
        int indice = 0;

        ScriptTest.InitialiserArbre(fonction);
        decimal ratio = 0;
        bool isReducted = false;
        float valeur_max = 0, valeur_min = 0;


        for (decimal i = borne_min; i < borne_max; i += pas)
        {
            for (decimal j = borne_min; j < borne_max; j += pas)
            {
                float tmp = (float)ScriptTest.calculerArbre(ScriptTest.racine, (double)j, (double)i);
                if (tmp > valeur_max)
                {
                    valeur_max = tmp;
                }
                else if (tmp < valeur_min)
                {
                    valeur_min = tmp;
                }

            }
        }
        if (valeur_min < -3)
        {
            ratio = 3 / Math.Abs((decimal)valeur_min);
            isReducted = true;
        }
        if (valeur_max > 3 && (decimal)valeur_max > Math.Abs((decimal)valeur_min))
        {
            ratio = 3 / (decimal)valeur_max;
            isReducted = true;
        }

        for (decimal i = borne_min, i2 = 0, i3 = 0; i3 < echantillonage; i += (decimal)(borne_max - borne_min) / (decimal)echantillonage, i2 += (decimal)TAILLE_FCT / (decimal)echantillonage, i3++)
        {
            for (decimal j = borne_min, j2 = 0, j3 = 0; j3 < echantillonage; j += (decimal)(borne_max - borne_min) / (decimal)echantillonage, j2 += TAILLE_FCT / (decimal)echantillonage, j3++)
            {
                float value = (float)ScriptTest.calculerArbre(ScriptTest.racine, (double)j, (double)i);
                try
                {
                    if (isReducted)
                        newVertices[indice] = new Vector3((float)(j2), (float)ratio * value, (float)(i2));
                    else
                        newVertices[indice] = new Vector3((float)(j2), value, (float)(i2));

                    lastValidValue = value;
                }
                catch
                {
                    Debug.Log(indice);
                    if (isReducted)
                        newVertices[indice] = new Vector3((float)(j2), (float)ratio * lastValidValue, (float)(i2));
                    else
                        newVertices[indice] = new Vector3((float)(j2), lastValidValue, (float)(i2));

                }
                indice++;
            }
        }

        /*Initialisation triangles*/
        newTriangles = new int[echantillonage * echantillonage * 2 * 3]; // 2 = nombre de triangle par case, 3 = nombre de points par triangle
        indice = -1;
        for (int i = 0; i < echantillonage * (echantillonage - 1); i++)
        {
            if ((i + 1) % echantillonage != 0)
            {
                newTriangles[++indice] = i;
                newTriangles[++indice] = i + (int)echantillonage;
                newTriangles[++indice] = i + 1;

                newTriangles[++indice] = i + 1;
                newTriangles[++indice] = i + (int)echantillonage;
                newTriangles[++indice] = i + (int)echantillonage + 1;
            }
        }

        /*Initialisation UVs*/
        Vector2[] uvs = new Vector2[newVertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(newVertices[i].x, newVertices[i].z);
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.uv = uvs;

        if (gameObject.GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().convex = true;
        }

        mesh.RecalculateNormals();

        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        if (corrigerBug)
        {
            objetPlan.transform.position = new Vector3(293.1f, 1.814f, -40.623f);
            corrigerBug = !corrigerBug;
        }
    }

}