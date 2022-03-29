using System;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace AlgoritmoGenetico.Class
{
    public class AgenteInteligente : Agent
    {
        [Tooltip("state machine")]
        public ActionsType actionsType;
        [Tooltip("state machine")]
        public GameObject agentPrefab;
        [Tooltip("state machine")]
        public Animator animator;

        [Tooltip("Force to apply when moving")]
        public float moveForce = 2f;

        [Tooltip("Speed to pitch up or down")]
        public float pitchSpeed = 100f;

        [Tooltip("Speed to rotate around the up axis")]
        public float yawSpeed = 100f;

        [Tooltip("Speed to rotate around the up axis")]
        public bool playerControl;

        [Tooltip("The agent's camera")]
        public Camera agentCamera;

        [Tooltip("Whether this is training mode or gameplay mode")]
        public bool trainingMode;

        
        new private Rigidbody rigidbody;
        [SerializeField]
        public float EnergyAmount { get; private set; }

        private float smoothPitchChange = 0f;

        [SerializeField]
        private float smoothYawChange = 0f;


        private const float MaxPitchAngle = 80f;


        [Tooltip("Speed to rotate around the up axis")]
        public Bio bio;
        [Tooltip("Speed to rotate around the up axis")]
        public FoodType foodType;

        private bool frozen = false;

        [Tooltip("Whether this is training mode or gameplay mode")]
        public Bio nearestBio;
        private Environment environment;

        private float moveCust=0.1f;
        [Tooltip("Speed to rotate around the up axis")]
        public float energyBase;
        /// <summary>
        /// The amount of nectar the agent has obtained this episode
        /// </summary>
        /// 
        [Tooltip("Speed to rotate around the up axis")]
        public float energyRatefood;
        public float BioMassObtained { get; private set; }
        public Health health;
        public float payForMove = 0f;
        private void Awake()
        {
          
        }

        private void Update()
        {
            //print(EnergyAmount);
            //  print(BioMassObtained);
            // Draw a line from the beak tip to the nearest flower
            if (nearestBio != null)
                Debug.DrawLine(transform.position, nearestBio.BioCenterPosition, Color.green);

        }
        private void FixedUpdate()
        {

            UpdateAction();
            //print(StepCount);
        }



        private void OnCollisionEnter(Collision collision)
        {
            
       if (trainingMode && collision.collider.CompareTag("boundary"))
            {


                AddReward(-.5f);
            }

            if (trainingMode && collision.collider.CompareTag("carnivore"))
            {

                if (foodType == FoodType.herb) { 
                AddReward(-.5f);
                }
            }
        }
       

        private void TriggerEnterOrStay(Collider collider)
        {

            switch (foodType)
            {
                case FoodType.herb:
                    feedHerb(collider);
                    break;
                case FoodType.meet:
                    feedMeet(collider);
                    break;
            }
        }


        private void OnTriggerStay(Collider other)
        {
            TriggerEnterOrStay(other);
        }
 
        private void feedHerb(Collider collider)
        {
            if (collider.CompareTag("bio"))
            {

                Bio bio = collider.GetComponentInParent<Bio>();

                float dot = Vector3.Dot((transform.localPosition - bio.transform.localPosition).normalized, transform.forward.normalized);

                if (dot < 0.6f)
                {

                    float foodReceived = bio.Feed(.1f);

                    // Keep track of nectar obtained
                    BioMassObtained += foodReceived;
                    CovertFoodtoEnergy(foodReceived);
                    if (trainingMode && EnergyAmount < energyBase)
                    {
                        // Calculate reward for getting nectar
                        float bonus = .02f * Mathf.Clamp01(dot);

                        AddReward(.01f + bonus);

                    }

                    // If flower is empty, update the nearest flower
                    if (!bio.HasBioMass)
                    {
                        UpdateNearestBio();
                    }

                }
            }
        }
        private void feedMeet(Collider collider)
        {
            if (collider.CompareTag("herbivorous"))
            {

                Bio bio = collider.GetComponentInParent<Bio>();

                float dot = Vector3.Dot((transform.localPosition - bio.transform.localPosition).normalized, transform.forward.normalized);

                if (dot < 0.6f)
                {

                    float foodReceived = bio.Feed(.1f);

                    // Keep track of nectar obtained
                    BioMassObtained += foodReceived;
                    CovertFoodtoEnergy(foodReceived);
                    if (trainingMode && EnergyAmount < energyBase)
                    {
                        // Calculate reward for getting nectar
                        float bonus = .02f * Mathf.Clamp01(dot);

                        AddReward(.01f + bonus);

                    }

                    if (!bio.HasBioMass)
                    {
                        UpdateNearestBio();
                    }

                }
            }
        }
        public void CovertFoodtoEnergy(float foodReceived)
        {
            if (EnergyAmount < energyBase) { 
            EnergyAmount += foodReceived * energyRatefood;
               
            }
           

        }
        public override void OnActionReceived(float[] vectorAction)
        {
            // Don't take actions if frozen
            if (frozen) return;

            if (EnergyAmount<=0) return;
            // Calculate movement vector
            Vector3 move = new Vector3(0, 0, 0);
            payForMove = 0;
            if (vectorAction[0] > 0) move= transform.forward.normalized;
               else if (vectorAction[0] < 0 )move = -transform.forward.normalized;

            payForMove += vectorAction[0] == 0 ?  0f : moveCust;

            payForMove += vectorAction[1] == 0 ? 0f : moveCust/2;

            payForMove += moveCust/5;
            // Add force in the direction of the move vector
            rigidbody.velocity=(move * moveForce);
           
        
            Vector3 rotationVector = transform.rotation.eulerAngles;

            float yawChange = vectorAction[1];


            smoothYawChange = Mathf.MoveTowards(smoothYawChange, yawChange, 2f * Time.fixedDeltaTime);

            float yaw = rotationVector.y + smoothYawChange * Time.fixedDeltaTime * yawSpeed;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(rotationVector.x, yaw, 0f);

            UseEnergy(payForMove);
            health.UpdateHealth(EnergyAmount);
        }

        public override void Heuristic(float[] actionsOut)
        {
            if (!playerControl)
                return;
            // Create placeholders for all movement/turning
            float forward=0;
            float left = 0;
            Vector3 up = Vector3.zero;

            float yaw = 0f;


            // Forward/backward
            if (Input.GetKey(KeyCode.W)) forward = 1;
            else if (Input.GetKey(KeyCode.S)) forward = -1;


            // Turn left/right
            if (Input.GetKey(KeyCode.Q)) yaw = -1f;
            else if (Input.GetKey(KeyCode.E)) yaw = 1f;

           

            actionsOut[0] = forward;
          //  actionsOut[1] = combined.y;
            //actionsOut[2] =left;
            actionsOut[1] = yaw;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
           
            sensor.AddObservation(transform.localRotation.normalized);
            sensor.AddObservation(transform.localPosition.normalized);
            sensor.AddObservation(0);
            sensor.AddObservation(energyBase-EnergyAmount);

        }
        public override void Initialize()
        {
            rigidbody = GetComponent<Rigidbody>();
            environment = GetComponentInParent<Environment>();
            health= GetComponent<Health>();
            health.maxHealth = energyBase;
            health.currentHealth = energyBase;
           bio = GetComponent<Bio>();
            // If not training mode, no max step, play forever
            if (!trainingMode) MaxStep = 0;
        }


        public void FreezeAgent()
        {
            Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
            frozen = true;
            rigidbody.Sleep();
        }

 
        public void UnfreezeAgent()
        {
            Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
            frozen = false;
            rigidbody.WakeUp();
        }

        public override void OnEpisodeBegin()
        {
            if (trainingMode)
            {
                // Only reset flowers in training when there is one agent per area
                environment.ResetBios();
            }
            
            bio.ResetBio();
            // Reset nectar obtained
            BioMassObtained = 0f;
            EnergyAmount = energyBase;
            // Zero out velocities so that movement stops before a new episode begins
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            // Default to spawning in front of a flower
            bool inFrontOfFlower = true;
            if (trainingMode)
            {
                // Spawn in front of flower 50% of the time during training
                inFrontOfFlower = UnityEngine.Random.value > .5f;
            }

            // Move the agent to a new random position
            MoveToSafeRandomPosition(inFrontOfFlower);

            // Recalculate the nearest flower now that the agent has moved
            UpdateNearestBio();
            
        }
        

        public void MoveToSafeRandomPosition(bool inSafePosition)
        {
            bio.MoveToSafeRandomPosition();
        }

        private void UpdateAction()
        {
            switch (actionsType)
            {
                case ActionsType.research:
                    UpdateNearestBio();
                    break;
                case ActionsType.pair:
                    break;
                case ActionsType.fight:
                    break;
                case ActionsType.flee:
                    break;
            }


        }


        private void UpdateNearestBio()
        {
            if (nearestBio != null && !nearestBio.HasBioMass)
            {
                
            foreach (Bio bio in environment.Bios)
            {
                if (bio != this.bio)
                {
                    if (nearestBio == null && bio.HasBioMass)
                    {
                        // No current nearest flower and this flower has nectar, so set to this flower
                        nearestBio = bio;
                    }
                    else if (bio.HasBioMass)
                    {
                        // Calculate distance to this flower and distance to the current nearest flower
                        float distanceToFlower = Vector3.Distance(bio.transform.position, transform.position);
                        float distanceToCurrentNearestFlower = Vector3.Distance(nearestBio.transform.position, transform.position);

                        // If current nearest flower is empty OR this flower is closer, update the nearest flower
                        if (!nearestBio.HasBioMass || (distanceToFlower < distanceToCurrentNearestFlower))
                        {
                            nearestBio = bio;
                        }
                    }
                }
            }

            if (nearestBio && !nearestBio.HasBioMass)
            {
                   
                nearestBio = null;
            }
            }
        }


        public void UseEnergy(float amount)
        {
            // Track how much nectar was successfully taken (cannot take more than is available)
            float energyUse = Mathf.Clamp(amount, 0f, EnergyAmount);

            // Subtract the nectar
            EnergyAmount -= energyUse;

            //print(EnergyAmount);
            if (EnergyAmount <= 0)
            {
                // No nectar remaining
                EnergyAmount = 0;



            }
            
        }



        public bool reproduzir()
        {    
            return true;
        }
        public bool copular(string data)
        {
            return true;
        }
        public bool copula(string data)
        {
            criar();
            return true;
        }
        private void criar()
        {
            GameObject clone = Instantiate(agentPrefab, this.transform.position, Quaternion.identity) as GameObject;
            Debug.Log("criado");
        }
        private void Cortejo(Bio bio)
        {

            bool reproduzir = bio.GetComponentInParent<AgenteInteligente>().reproduzir();
            if (!reproduzir)
                return;
            bio.GetComponentInParent<AgenteInteligente>().copula("data");
            
        }
    }
}