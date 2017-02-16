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
    






    void Awake()
    {

       

        // Pour envoyer la structure de la fonction

        var se = new InputField.SubmitEvent();
        se.AddListener(setTextePlan);
        input.onEndEdit = se;

        clavierCanvas.enabled = false;






        sliderVolume.value = mainAudio.volume ;


    }


    public void setTextePlan(string chaine)
    {
        textePlan = chaine;
        Debug.Log("passed");


        plan.GetComponent<testPlanTriangles>().Dessiner(textePlan);

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
}



