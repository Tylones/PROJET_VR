using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public Composant racine;

    public abstract class Composant : MonoBehaviour
    {
        protected System.Object value;

        public System.Object getValue()
        {
            return value;
        }

        public void setValue(System.Object o)
        {
            value = o;
        }
    }

    

    class Nombre : Composant
    {
        public Nombre(double nb)
        {
            value = nb;
        }


    }


    class Variable : Composant
    {


        public Variable(string name)
        {
            value = name;

        }


        public void setName(string n)
        {
            value = n;
        }

    }

    class Operateur : Composant
    {

        private List<Composant> next;
        private bool special;

        public Operateur(string val, bool speciale)
        {
            special = speciale;
            value = val;
            next = new List<Composant>();
        }

        public Operateur()
        {

        }

        public bool isSpecial()
        {
            return special;
        }

        public Composant getInd(int index)
        {
            return next[index];
        }

        public List<Composant> getNext()
        {
            return next;
        }

        public void addNext(Composant c)
        {
            next.Add(c);
        }
    }

    // Use this for initialization
    void Start () {
        /*
        string input = ";5;8;*;9;5;*;+;";
        racine = createTree(ref input, null, true);
        Debug.Log("On affiche l'arbre : ");
        AfficherArbre(racine);
        Debug.Log("");
        Debug.Log("On calcul l'arbre : ");
        Debug.Log(calculerArbre(racine, 2, 3));
        */
        return;
    }

    public void InitialiserArbre(string input)
    {
        racine = createTree(ref input, null, true);
    }


    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    static string nextStr(string fct)
    {
        string tmp = "";
        int ind = fct.Length - 2;
        while (fct[ind].ToString() != ";")
        {
            tmp = fct[ind].ToString() + tmp;
            ind--;
        }

        return tmp;
    }

    static string deleteLast(string fct)
    {
        string toDelete = nextStr(fct) + ";";
        toDelete = Reverse(toDelete);
        fct = Reverse(fct);

        int index = fct.IndexOf(toDelete);
        string cleanPath = (index < 0)
            ? fct
            : fct.Remove(index, toDelete.Length);
        cleanPath = Reverse(cleanPath);

        return cleanPath;

    }

    static bool hasNext(string fct)
    {
        return fct.Length > 1;
    }

    static Composant createComposant(string str)
    {
        if (str == "/" || str == "+" || str == "*" || str == "-" || str == "pow")
            return new Operateur(str, false);
        else if (str == "x" || str == "y")
            return new Variable(str);
        else if (str == "e" || str == "log" || str == "sin" || str == "cos")
            return new Operateur(str, true);
        else
            return new Nombre(double.Parse(str));
    }

    static Composant createTree(ref string fct, Composant racine, bool first)
    {



        if (first)
        {
            if (hasNext(fct))
            {
                string toProcede = nextStr(fct);
                if (toProcede == "/" || toProcede == "+" || toProcede == "*" || toProcede == "-" || toProcede == "pow")
                    racine = new Operateur(toProcede, false);
                else
                    racine = new Operateur(toProcede, true);
                fct = deleteLast(fct);
            }

            first = !first;
        }

        if (racine.GetType() == typeof(Operateur))
        {
            if (((Operateur)racine).isSpecial())
            {
                string left = nextStr(fct);
                fct = deleteLast(fct);

                ((Operateur)racine).addNext(createComposant(left));
                createTree(ref fct, ((Operateur)racine).getInd(0), false);
            }
            else
            {
                string right = nextStr(fct);
                fct = deleteLast(fct);
                ((Operateur)racine).addNext(createComposant(right));
                createTree(ref fct, ((Operateur)racine).getInd(0), false);
                string left = nextStr(fct);
                fct = deleteLast(fct);

                ((Operateur)racine).addNext(createComposant(left));


                createTree(ref fct, ((Operateur)racine).getInd(1), false);

            }
        }
        Console.WriteLine(racine.getValue());
        return racine;

    }

    static Composant setBranche(string value)
    {


        if (value.CompareTo("x") == 0 || value.CompareTo("y") == 0)
        {
            return new Variable(value);
        }
        else
        {
            return new Nombre(float.Parse(value));
        }

    }

    static void AfficherArbre(Composant racine)
    {
        if (racine.GetType() == typeof(Operateur))
        {
            if (!((Operateur)racine).isSpecial())
                AfficherArbre(((Operateur)racine).getInd(1));
            AfficherArbre(((Operateur)racine).getInd(0));
        }
        Console.Write(racine.getValue());


    }

    public double calculerArbre(Composant racine, double x, double y)
    {

        double gauche = 0;
        if (racine.GetType() == typeof(Operateur))
        {

            double droite = calculerArbre(((Operateur)racine).getInd(0), x, y);
            if (!((Operateur)racine).isSpecial())
            {
                gauche = calculerArbre(((Operateur)racine).getInd(1), x, y);
            }

            switch ((string)racine.getValue())
            {
                case "*":
                    return droite * gauche;

                case "-":
                    return droite - gauche;

                case "/":
                    return gauche / droite;

                case "+":
                    return droite + gauche;

                case "pow":
                    return Math.Pow(gauche, droite);

                case "e":
                    return Math.Exp(droite);

                case "log":
                    return Math.Log(droite);

                case "sin":
                    return Math.Sin(droite);

                case "cos":
                    return Math.Cos(droite);


            }
        }
        else if (racine.GetType() == typeof(Nombre))
        {
            return (double)racine.getValue();

        }
        else if (racine.GetType() == typeof(Variable))
        {
            if (racine.getValue().ToString() == "x")
            {
                return x;
            }
            else
                return y;
        }

        return 0;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
