using System.Collections.Generic;
using UnityEngine;

namespace AlgoritmoGenetico.Class
{
    public class Environment : MonoBehaviour
    {
        

        public Dictionary<Collider, Bio> bioDictionary;
        public const float AreaDiameter = 20f;

        public List<Bio> Bios { get; private set; }

        public Bio GetBioFromMass(Collider collider)
        {
            return bioDictionary[collider];
        }

        [SerializeField]
        public GameObject staticAgent;

        [SerializeField]
        public int numOfStaticAgents = 0;

        [SerializeField]
        public GameObject agentHerb;
        [SerializeField]
        public GameObject agentHerbTrain;
        [SerializeField]
        public int numOfgentHerb = 0;
        [SerializeField]
        public GameObject agentCarn;
        [SerializeField]
        public GameObject agentCarnTrain;

        [SerializeField]
        public int numOfgentCarn = 0;
        [SerializeField]
        public bool isTrainner;
        public execType execMode;

        bool running = false;
        private void Start()
        {
            bioDictionary = new Dictionary<Collider, Bio>();
            Bios = new List<Bio>();
            AgenteInteligente.OnAgentDied += CountAgentDie;
            Demo();


        }
        [ContextMenu("start")]
        public void Demo()
        {
            if (running)
                return;

            running = true;
            for (int i = 0; i < numOfStaticAgents; i++)
            {

                GameObject agentStatic = Instantiate(staticAgent, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                Bio bio = agentStatic.GetComponent<Bio>();
                Collider bioCollider = agentStatic.GetComponent<Collider>();
                Bios.Add(bio);


                bioDictionary.Add(bioCollider, bio);


            }
            switch (execMode)
            {
                case execType.trainHerb:
                    TrainingHerb();
                    DemoCarn();
                    break;
                case execType.demoHerb:
                    DemoHerb();
                    break;
                case execType.trainCarn:
                    DemoHerb();
                    TrainingCarn();
                    break;
                case execType.demoCarn:
                    DemoHerb();
                    DemoCarn();
                    break;
            }

        }


        private void FindChildBio(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                if (child.CompareTag("bio")|| child.CompareTag("herbivorous"))
                {
                    Bio bio = child.GetComponent<Bio>();
                    Collider bioCollider = child.GetComponent<Collider>();

                    if (bio != null)
                    {
                        // Found a flower, add it to the Flowers list
                        Bios.Add(bio);

                        // Add the nectar collider to the lookup dictionary

                        bioDictionary.Add(bioCollider, bio);


                    }

                }

            }
        }
        public void ResetBios()
        {

            // Reset each flower
            foreach (Bio bio in Bios)
            {
                bio.ResetBio();
               
            }
        }

        // Update is called once per frame

        private void TrainingHerb()
        {
            for (int i = 0; i < numOfgentHerb; i++)
            {

               GameObject herb =Instantiate(agentHerbTrain, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                Bio bio = herb.GetComponent<Bio>();
                Collider bioCollider = herb.GetComponent<Collider>();
                if (bio != null)
                {
                    // Found a flower, add it to the Flowers list
                    Bios.Add(bio);

                    // Add the nectar collider to the lookup dictionary

                    bioDictionary.Add(bioCollider, bio);


                }
            }
        }
        private void DemoHerb()
        {

            for (int i = 0; i < numOfgentHerb; i++)
            {
               GameObject herb= Instantiate(agentHerb, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                Bio bio = herb.GetComponent<Bio>();
                Collider bioCollider = herb.GetComponent<Collider>();
                if (bio != null)
                {
                    // Found a flower, add it to the Flowers list
                    Bios.Add(bio);

                    // Add the nectar collider to the lookup dictionary

                    bioDictionary.Add(bioCollider, bio);


                }
            }

        }
        private void TrainingCarn()
        {
            for (int i = 0; i < numOfgentCarn; i++)
            {

                GameObject carn=Instantiate(agentCarnTrain, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                Bio bio = carn.GetComponent<Bio>();
                Collider bioCollider = carn.GetComponent<Collider>();

                if (bio != null)
                {
                    // Found a flower, add it to the Flowers list
                    Bios.Add(bio);

                    // Add the nectar collider to the lookup dictionary

                    bioDictionary.Add(bioCollider, bio);


                }
            }
        }
        private void DemoCarn()
        {
            for (int i = 0; i < numOfgentCarn; i++)
            {

               GameObject carn= Instantiate(agentCarn, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                Bio bio = carn.GetComponent<Bio>();
                Collider bioCollider = carn.GetComponent<Collider>();
                if (bio != null)
                {
                    // Found a flower, add it to the Flowers list
                    Bios.Add(bio);

                    // Add the nectar collider to the lookup dictionary

                    bioDictionary.Add(bioCollider, bio);


                }
            }

        }
        private void CountAgentDie(string agent)
        {
            print(agent);
        }
    }
}