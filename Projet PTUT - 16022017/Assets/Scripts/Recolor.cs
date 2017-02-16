using UnityEngine;
using System.Collections;

public class Recolor : MonoBehaviour {

    public static int theme;
    private int theme_precedent;
    Material mat;
    
    void Start ()
    {
        Renderer rend = GetComponent<Renderer>();
        theme_precedent = -1;
        recolor(theme, mat, rend);
    }

    void LateUpdate()
    {
        Renderer rend = GetComponent<Renderer>();
        recolor(theme, mat, rend);
    }

    public void recolor(int theme, Material newMat, Renderer rend)
    {
        if (theme != theme_precedent)
        {
            switch (theme)
            {
                //Multicolour
                case 0:
                    newMat = Resources.Load("Couleur", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;

                //Blue
                case 1:
                    newMat = Resources.Load("Bleu", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;

                case 2:
                    newMat = Resources.Load("Rouge", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;

                case 3:
                    newMat = Resources.Load("Vert", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;

                case 4:
                    newMat = Resources.Load("Texture1", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;

                case 5:
                    newMat = Resources.Load("Multicolour", typeof(Material)) as Material;
                    if (rend != null)
                    {
                        rend.material = newMat;
                    }
                    break;
            }
            theme_precedent = theme;
        }
    }
}
