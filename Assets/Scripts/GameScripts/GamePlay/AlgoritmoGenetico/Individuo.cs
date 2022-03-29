using System;

namespace AlgoritmoGenetico.Class
{
    public class Individuo
    {
        private char[] cromossomo;                                    //Vetor de bits, possui apenas dois estatos, true ou false  


        //Construtor da classe
        public Individuo()
        {
            //Instancia o cromossomo com a quantidade de bits informados na classe Constants
            this.cromossomo = new char[Constants.sizeCromossomo];

            int i;
            for (i = 0; i < cromossomo.Length; i++)
            {

                this.cromossomo[i] = Convert.ToChar(Constants.random.Next(65, 90));  //Popula o vetor com bits aleatorios
            }
        }

        public char[] getCromossomo()
        {
            return this.cromossomo;
        }

        //Inserir valor booleano em um determinado local do vetor do cromossomo
        public void setGene(int position, char gene)
        {
            this.cromossomo[position] = gene;
        }

        public char getGene(int position)
        {
            return this.cromossomo[position];
        }




        public void mutarBit(int position)
        {
            if (position < cromossomo.Length)
            {
                // cromossomo.Set(position, cromossomo[position] == false ? true : false);
            }
        }



        public int getInt()
        {
            if (this.cromossomo.Length > 32)
                throw new ArgumentException("O comprimento do cromossomo deve ser no máximo 32 bits.");

            int[] array = new int[1];
            this.cromossomo.CopyTo(array, 0);
            return array[0];
        }



    }
}
