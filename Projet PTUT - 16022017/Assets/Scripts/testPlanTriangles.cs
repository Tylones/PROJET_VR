using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class testPlanTriangles : MonoBehaviour
{

    public test ScriptTest;
    private string fonction;
    public Mesh mesh;
    private int cpt;
    private bool cptAsc;
    private int toto;

    private int borne_min_precedente;
    private int borne_max_precedente;
    private decimal echantillonage_precedent;

    private int verticesNumber;
    public int borne_min;
    public int borne_max;
    public decimal echantillonage;
    public float echan;

    private void Awake()
    {
         ScriptTest = GetComponent<test>();
    }

    void Start()
    {
        ScriptTest = GetComponent<test>();
        Dessiner("");
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
        echan = 0.1f;
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
        echantillonage = 0.1m;
        echantillonage_precedent = echantillonage;

        drawPlan(mesh);

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void LateUpdate()
    {
        /*Si l'échantillonage ou les bornes d'affichage sont modifiées, alors on redessine la fonction */
        if ((decimal)echan != echantillonage_precedent || borne_min != borne_min_precedente || borne_max != borne_max_precedente)
        {
            drawPlan(mesh);
            echantillonage_precedent = (decimal)echan;
            borne_min_precedente = borne_min;
            borne_max_precedente = borne_max;
        }
    }

    void drawPlan(Mesh mesh)
    {
        Debug.Log(++toto);
        echantillonage = (decimal)echan;
        Vector3[] newVertices;
        int[] newTriangles;

        int largeur_plan = (int)((borne_max - borne_min) / echantillonage);

        /* Initialisation Vertices*/
        newVertices = new Vector3[(largeur_plan * largeur_plan)];
        verticesNumber = newVertices.Length;
        int indice = 0;

        decimal ratio = (decimal)2 / ((decimal)(borne_max) - (decimal)(borne_min));
        
        string fct = ";x;2;pow;y;2;pow;+;0.5;pow;-1;*;";

        ScriptTest.InitialiserArbre(fonction);

        for (decimal i = borne_min; i < borne_max; i += echantillonage)
        {
            for (decimal j = borne_min; j < borne_max; j += echantillonage)
            {
                float tmp1 = getValue_Function("", j, i);
                float tmp = (float)ScriptTest.calculerArbre(ScriptTest.racine, (double)j, (double)i);
                newVertices[indice] = new Vector3((float)(j)*(float)ratio, tmp, (float)(i)*(float)ratio);
                indice++;
            }
        }

        /*Initialisation triangles*/
        newTriangles = new int[(largeur_plan * largeur_plan) * 2 * 3]; // 2 = nombre de triangle par case, 3 = nombre de points par triangle
        indice = -1;
        for (int i = 0; i < largeur_plan * (largeur_plan - 1); i++)
        {
            if ((i + 1) % largeur_plan != 0)
            {
                newTriangles[++indice] = i;
                newTriangles[++indice] = i + largeur_plan;
                newTriangles[++indice] = i + 1;

                newTriangles[++indice] = i + 1;
                newTriangles[++indice] = i + largeur_plan;
                newTriangles[++indice] = i + largeur_plan + 1;
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