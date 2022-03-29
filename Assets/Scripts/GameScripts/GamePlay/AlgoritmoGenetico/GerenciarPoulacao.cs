using AlgoritmoGenetico.Class;
using UnityEngine;
using UnityEngine.UI;

public class GerenciarPoulacao : MonoBehaviour
{
    private Populacao populacao;
    private AlgoritmoGenetico.Class.AlgoritmoGenetico algoritmoGenetico;
    [SerializeField]
    private Text labeGeracao;
    [SerializeField]
    private Text labelPopulacao;
    private int geracao = 0;
    void Start()
    {
        ShowGeracao();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreatePopulacao()
    {
        populacao = new Populacao();
        for (int j = 0; j < Constants.sizePopulacao; j++)
        {
            print(labelPopulacao.text + "\n" + (j.ToString() + "----" + Constants.leCromossomo(populacao.getPopulacao()[j].getCromossomo())));
            labelPopulacao.text = labelPopulacao.text + "\n" + (j.ToString() + "----" + Constants.leCromossomo(populacao.getPopulacao()[j].getCromossomo()));
        }
    }
    /*    public void ExecutarAG()
        {
            labelPopulacao.text = "";
            algoritmoGenetico = new AlgoritmoGenetico.Class.AlgoritmoGenetico(double.Parse("0.80"), double.Parse("0.01"));


                populacao = algoritmoGenetico.executaAG(populacao);

            geracao++;
            for (int i = 0; i < Constants.sizePopulacao; i++)
            {
                 labelPopulacao.text = labelPopulacao.text + "\n" +(i.ToString() + "----" + Constants.leCromossomo(populacao.getPopulacao()[i].getCromossomo()));
            }
            ShowGeracao();


        }*/
    public void ShowGeracao()
    {
        labeGeracao.text = geracao.ToString();
    }
}
