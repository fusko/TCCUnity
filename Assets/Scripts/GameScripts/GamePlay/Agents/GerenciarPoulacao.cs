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
    [SerializeField] private GameObject contentGene;
    [SerializeField] private GameObject contentView;
    [SerializeField] private GameObject contentMenu;
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
          
            GameObject gene = new GameObject("gene"+j);
            gene.transform.SetParent(contentGene.transform);
            gene.AddComponent<Text>().text =  (j.ToString() + "----" + Constants.leCromossomo(populacao.getPopulacao()[j].getCromossomo()));
            gene.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
          
          
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

    public void ShowOrCloseMenu(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
}
