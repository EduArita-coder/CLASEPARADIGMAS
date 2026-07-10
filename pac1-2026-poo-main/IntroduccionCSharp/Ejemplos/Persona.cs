using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroduccionCSharp.Ejemplos
{
    public class Persona
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }

        // Método: Constructor
        /*
            1. Se llama igual que la clase
            2. No retorna datos
            3. Se utiliza para inicializar valores de propiedades de la clase
            4. Se ejecuta cuando se construye el objeto
            5. No se puede llamar por medio de la sintaxis de punto 
        */ 
        public Persona(string nombres, string apellidos, string genero, int edad)
        {
            Nombres = nombres;
            Apellidos = apellidos;
            Genero = genero;
            Edad = edad;
        }

    }
}