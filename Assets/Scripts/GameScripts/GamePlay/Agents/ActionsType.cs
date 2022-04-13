using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AlgoritmoGenetico.Class
{
    public enum AgentType
    {
        friend,
        enemy,
        partnner

    }


    public enum ActionsType
    {
        research,
        pair,
        fight,
        flee,
        breed,
        conceive,
        friend

    }
    public enum FoodType
    {
       herb,
       meet

    }
    public enum execType
    {
        trainHerb,
        demoHerb,
        trainCarn,
        demoCarn
    }
}