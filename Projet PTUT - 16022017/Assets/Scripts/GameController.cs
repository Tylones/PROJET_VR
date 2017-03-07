using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{




    public Canvas mainCanvas;     // Canvas du menu principal
    public GameObject plan;
    public string textePlan;
    public InputField input;
    public Canvas clavierCanvas; // Canvas du clavier
 
  
    public AudioSource mainAudio;
    public AudioClip audioRaeggae;
    public AudioClip audioElectro;
    public AudioClip audioFunny;

    public Slider sliderVolume;
    public Slider sliderEchantillonage;

    public GameObject panel_bumps;
    public GameObject panel_cone;
    public GameObject panel_fences;
    public GameObject panel_menu_fonctions;

    public GameObject porte;
    public static int cpt_porte=0;

    public GameObject inputMinTxt;
    public GameObject inputMaxTxt;













    void Awake()
    {

       

        // Pour envoyer la structure de la fonction

        var se = new InputField.SubmitEvent();
        se.AddListener(setTextePlan);
        input.onEndEdit = se;

      //  clavierCanvas.enabled = false;


        sliderVolume.value = mainAudio.volume ;


        inputMinTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_min.ToString();

        inputMaxTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_max.ToString(); 


    }




    public void setTextePlan(string chaine)
    {
        textePlan = chaine;
        Debug.Log("passed");


        plan.GetComponent<testPlanTriangles>().Dessiner(textePlan);

        inputMinTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_min.ToString();

        inputMaxTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_max.ToString();

    }

    public void setCouleurRed()
    {
        Recolor.theme = 2;
    }

    public void setCouleurDamier()
    {
        Recolor.theme = 4;
    }

    public void setCouleurBleu()
    {
        Recolor.theme = 1;
    }

    public void setCouleurVert()
    {
        Recolor.theme = 3;
    }

    public void setCouleurMultiColor()
    {
        Recolor.theme = 5;
    }

    public void setCouleurNormale()
    {
        Recolor.theme = 0;
    }

    /*
    // Fonction qui est lancée par les "listeners" pour changer la couleur du plan.
    void modificationColorOnClick(int value)
    {
        Recolor.theme = value;
    }
    */


    /* Update pour
     * - Apparition du mainMenuPanel
     * 
     * */


    public void Gestion_porte()
    {
        if (cpt_porte%2==0 && porte.GetComponent<Animation>().isPlaying == false )
        {
            porte.GetComponent<Animation>().Play("Close");
            cpt_porte++;
        }
        else{
            if (porte.GetComponent<Animation>().isPlaying == false)
            {
                porte.GetComponent<Animation>().Play("Open");
                cpt_porte++;
            }
            
        }
        
    }

    public void UnderPanelControl()
    {

        if (panel_cone.GetComponent<CanvasGroup>().alpha.ToString() == "1")
        {
            panel_cone.GetComponent<Animator>().SetTrigger("Close");
        }

        if (panel_fences.GetComponent<CanvasGroup>().alpha.ToString() == "1")
        {
            panel_fences.GetComponent<Animator>().SetTrigger("Close");
        }

        if (panel_menu_fonctions.GetComponent<CanvasGroup>().alpha.ToString() == "1")
        {
            panel_menu_fonctions.GetComponent<Animator>().SetTrigger("Close");
        }

        if (panel_bumps.GetComponent<CanvasGroup>().alpha.ToString() == "1")
        {
            panel_bumps.GetComponent<Animator>().SetTrigger("Close");
        }



    }





    void Update()
    {


        if (Input.GetKeyDown("space"))
        {
            mainCanvas.enabled = false;
        }

        if (Input.GetKeyDown("a"))
        {
            mainCanvas.enabled = true;
        }

        


    }

    public void detecteInputClavier(string chaine)
    {
        //  Debug.Log("PASSE 1");
        string currentText = input.text.ToString();
        string newText = currentText + chaine;
        input.text = newText;

        inputMinTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_min.ToString();

        inputMaxTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_max.ToString();



        //  Debug.Log(input.text.ToString());
    }

    public void deleteInputClavier()
    {
        input.text = input.text.Remove(input.text.Length - 1);
        input.text = input.text.Remove(input.text.Length - 1);



    }


    /*
    public void popOutVolume()
    {
        float currentTime = Time.time;

        while(currentTime != currentTime + 10)
        {

        }

        confirmationVolumePanneau.SetActive(true);

        INFINITE LOOP PUT A STATIC VARIABLE
    }
    */


    public void OnValueChanged()
    {
        mainAudio.volume = sliderVolume.value;
    }

    public void changeAudio(int theme)
    {
        switch (theme)
        {

            case 0:
                mainAudio.clip = audioRaeggae;
                mainAudio.Play();
                break;


            case 1:
                mainAudio.clip = audioElectro;
                mainAudio.Play();
                break;

            case 2:
                mainAudio.clip = audioFunny;
                mainAudio.Play();
                break;
        }
    }


    public void ModificationBornes(int value)
    {

        int borne_min = plan.GetComponent<testPlanTriangles>().borne_min;
        int borne_max = plan.GetComponent<testPlanTriangles>().borne_max;

        switch (value)
        {
            case 0:
                borne_min--;
                break;
            case 1:
                borne_min-=10;
                break;
            case 2:
                borne_min-=20;
                break;
            case 3:
                borne_min++;
                break;
            case 4:
                borne_min+=10;
                break;
            case 5:
                borne_min+=20;
                break;
            case 6:
                borne_max--;
                break;
            case 7:
                borne_max -= 10;
                break;
            case 8:
                borne_max -= 20;
                break;
            case 9:
                borne_max++;
                break;
            case 10:
                borne_max += 10;
                break;
            case 11:
                borne_max+=20;
                break;

        }
        
        
         plan.GetComponent<testPlanTriangles>().borne_min = borne_min ;
         plan.GetComponent<testPlanTriangles>().borne_max = borne_max;

        inputMinTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_min.ToString();

        inputMaxTxt.GetComponent<InputField>().placeholder.GetComponent<Text>().text = plan.GetComponent<testPlanTriangles>().borne_max.ToString();
    }

    public void OnValueChangedEchantillonage()
    {
        plan.GetComponent<testPlanTriangles>().echantillonage = sliderEchantillonage.value;
    }
}



