using System.Collections.Generic;
using UnityEngine;
using AlgoritmoGenetico.Enum;
using System;

namespace AlgoritmoGenetico.Class
{
    public class Bio : MonoBehaviour
    {

        public float BioMass = 2;
        public Vector3 SizeScale = new Vector3(1f, 1f, 1f);
        [Tooltip("Force to apply when moving")]
        public AgenteInteligente agenteInteligente;
        public Shader shader;
        public Color color;
        [SerializeField]
        public Renderer rend;
        [SerializeField]
        public List<Cell> cells;
        public string bioType;
        public char sexualType;
        public List<string> bioMass;
        public float BioMassAmount { get; private set; }
        [SerializeField]
        public Collider bioCollider;

        public bool HasBioMass
        {
            get
            {
                return BioMassAmount > 0f;
            }
        }

        public Vector3 BioCenterPosition
        {
            get
            {
                return transform.position;
            }
        }

        public Vector3 BioUpVector
        {
            get
            {
                return transform.up;
            }
        }


        public void MoveToSafeRandomPosition()
        {
            bool safePositionFound = false;
            int attemptsRemaining = 100; // Prevent an infinite loop
            Vector3 potentialPosition = Vector3.zero;
            Quaternion potentialRotation = new Quaternion();

            // Loop until a safe position is found or we run out of attempts
            while (!safePositionFound && attemptsRemaining > 0)
            {
                attemptsRemaining--;



                
                float radius = UnityEngine.Random.Range(-45f, 45f);

              //  if (radius > 40 || radius < -40) print("limit");
                // Pick a random direction rotated around the y axis
                Quaternion direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f);

                // Combine height, radius, and direction to pick a potential position
                potentialPosition =  Vector3.up * 0.5f + direction * Vector3.forward * radius;
               
                // Choose and set random starting pitch and yaw

              //  float yaw = UnityEngine.Random.Range(-180f, 180f);


                Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.05f);

                // Safe position has been found if no colliders are overlapped
                safePositionFound = colliders.Length == 0;
            }
            Debug.Assert(safePositionFound, "Could not find a safe position to spawn");

            // Set the position and rotation
            transform.localPosition = potentialPosition;
            transform.rotation = potentialRotation;
        }
            public float Feed(float amount)
        {
            // Track how much nectar was successfully taken (cannot take more than is available)
            float BioMassTaken = Mathf.Clamp(amount, 0f, BioMassAmount);

            // Subtract the nectar
            BioMassAmount -= amount;

            if (BioMassAmount <= 0)
            {
                // No nectar remaining
                BioMassAmount = 0;


                bioCollider.gameObject.SetActive(false);

            }
           // BioMass = BioMassAmount;
            // Return the amount of nectar that was taken
            return BioMassTaken;
             }
            /*            public int Feed(float amount)
                    {
                        // Track how much nectar was successfully taken (cannot take more than is available)
                        int BioMassTaken = int.Parse(Mathf.Clamp(amount, 0f, bioMass.Count).ToString());

                        string[] mass = new string[BioMassTaken];
                        for (int i = 0; i <= BioMassTaken; i++)
                        {
                            mass[i] = bioMass[0];
                            bioMass.RemoveAt(0);
                        }
                        BioMassAmount -= amount;

                        if (BioMassAmount <= 0)
                        {
                            // No nectar remaining
                            BioMassAmount = 0;


                            bioCollider.gameObject.SetActive(false);

                        }

                        // Return the amount of nectar that was taken
                        return BioMassTaken;
                    }*/

            public Bio()
        {

        }
        private void Awake()
        {
            ResetBio();


            UnityEngine.Random.Range(0f, 1f);
            rend.material = new Material(Shader.Find("Standard"));
            color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
            rend.material.color = color;
            this.transform.localScale = SizeScale;



        }


        public void ResetBio()
        {
            // Refill the nectar
            BioMassAmount = BioMass;

           
            bioCollider.gameObject.SetActive(true);

            MoveToSafeRandomPosition();

        }


    }


}