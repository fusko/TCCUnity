using System;
using AlgoritmoGenetico.Enum;
using System.Text.RegularExpressions;

public class Cell
    {
        public string value;
         public TypesCaracteristicas typesCaracteristicas;

    public Cell(string value, TypesCaracteristicas typesCaracteristicas)
    {
        this.value = value;
        this.typesCaracteristicas = typesCaracteristicas;
    }
      
    }
