using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class testPlanTriangles : MonoBehaviour
{

    public test ScriptTest;
    private string fonction;
    public Mesh mesh;
    private int cpt;
    private bool cptAsc;
    private int toto;
    private const int TAILLE_FCT = 4;

    private int borne_min_precedente;
    private int borne_max_precedente;
    private float echantillonage_precedent;
    public float echantillonage; // valeur visible par l'utilisateur, qui ne peut être de type Decimal

    private int verticesNumber;
    public int borne_min;
    public int borne_max;
    public decimal echan; // valeur en decimal invisible pour l'utilisateur

    private void Awake()
    {
         ScriptTest = GetComponent<test>();
    }

    void Start()
    {
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
        echan = 40m;
        echantillonage = 40;
        echantillonage_precedent = echantillonage;

        drawPlan(mesh);

    }

    void LateUpdate()
    {
        /*Si l'échantillonage ou les bornes d'affichage sont modifiées, alors on redessine la fonction */
        if (echantillonage != echantillonage_precedent || borne_min != borne_min_precedente || borne_max != borne_max_precedente)
        {
            echan = (decimal)echantillonage;        
            echantillonage_precedent = echantillonage;
            borne_min_precedente = borne_min;
            borne_max_precedente = borne_max;
            drawPlan(mesh);
        }
    }

    void drawPlan(Mesh mesh)
    {
        Debug.Log(++toto);
        Vector3[] newVertices;
        int[] newTriangles;

        int largeur_plan = borne_max - borne_min;

        /* Initialisation Vertices*/
        newVertices = new Vector3[(int)(echan * echan)];
        verticesNumber = newVertices.Length;
        int indice = 0;

        //decimal ratio = (decimal)2 / ((decimal)(borne_max) - (decimal)(borne_min));
        decimal pas = (borne_max - borne_min) / echan;
        string fct = ";x;2;pow;y;2;pow;+;0.5;pow;-1;*;";

        ScriptTest.InitialiserArbre(fonction);
        decimal ratio = 0;
        bool isReducted = false;
        float valeur_max = 0, valeur_min = 0;

        for (decimal i = borne_min; i < borne_max; i += pas)
        {
            for (decimal j = borne_min; j < borne_max; j += pas)
            {
                float tmp = (float)ScriptTest.calculerArbre(ScriptTest.racine, (double)j, (double)i);
                if(tmp > valeur_max)
                {
                    valeur_max = tmp;
                }
                else if(tmp< valeur_min)
                {
                    valeur_min = tmp;
                }

            }
        }
        if(valeur_min < -3)
        {
            ratio = 3 / Math.Abs((decimal)valeur_min);
            isReducted = true;
        }
        if(valeur_max > 3 && (decimal)valeur_max> Math.Abs((decimal)valeur_min))
        {
            ratio = 3 / (decimal)valeur_max;
            isReducted = true;
        }

                for (decimal i = borne_min, i2=0; i < borne_max; i += pas ,i2+=(decimal)TAILLE_FCT/(decimal)echantillonage)
        {
            for (decimal j = borne_min, j2=0; j < borne_max; j += pas, j2+=TAILLE_FCT/(decimal)echantillonage)
            {
                float tmp1 = getValue_Function("", j, i);
                float tmp = (float)ScriptTest.calculerArbre(ScriptTest.racine, (double)j, (double)i);
                try
                {
                    if (isReducted)
                        newVertices[indice] = new Vector3((float)(j2), (float)ratio * tmp, (float)(i2));
                    else
                        newVertices[indice] = new Vector3((float)(j2),  tmp, (float)(i2));

                }
                catch
                {
                    Debug.Log("i = " + i + " j =" + j + "pas = " + pas); 
                }
                    indice++;

            }
        }

        /*Initialisation triangles*/
        newTriangles = new int[((int)(echan * echan)) * 2 * 3]; // 2 = nombre de triangle par case, 3 = nombre de points par triangle
        indice = -1;
        for (int i = 0; i < echan * (echan - 1); i++)
        {
            if ((i + 1) % echan != 0)
            {
                newTriangles[++indice] = i;
                newTriangles[++indice] = i + (int)echan;
                newTriangles[++indice] = i + 1;

                newTriangles[++indice] = i + 1;
                newTriangles[++indice] = i + (int)echan;
                newTriangles[++indice] = i + (int)echan + 1;
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

        mesh.RecalculateNormals();


    }

    float getValue_Function(string fonction, decimal x, decimal z)
    {

        float result = 0;

         result = (float)(Mathf.Sin((float)(10 * (Mathf.Pow((float)x, 2) + Mathf.Pow((float)z, 2)))) / 10);

        // result = 1 / (15 * (Mathf.Pow((float)x, 2) + Mathf.Pow((float)z, 2)));

        // result = (Mathf.Sin(5 * (float)x) * Mathf.Cos(5 * (float)z)) / 5;

        return result;
    }

    
}