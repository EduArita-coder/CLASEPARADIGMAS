using IntroduccionCSharp.Ejemplos;

Calculadora calculadora = new Calculadora();


Console.WriteLine("==== SUMAR ====");
Console.WriteLine("");

string operación = "";
int n1 = 0;
int n2 = 0;

try
{
    Console.WriteLine("Que operación quiere realizar se permite (+, -, x, ÷)");
    operación = Console.ReadLine();

    Console.WriteLine("Ingrese el primer numero");
    n1 = int.Parse(Console.ReadLine()); // "2" != 2
    Console.WriteLine("Ingrese el segundo numero");
    n2 = int.Parse(Console.ReadLine()); // "1, 2..."
}
catch
{
    Console.WriteLine("Error de formato en el texto ingresado.");
}

int resultado = 0;

switch (operación)
{
    case "+":
        resultado = calculadora.Sumar(n1, n2);
        break;
    case "-":
        resultado = calculadora.Restar(n1, n2);
        break;
    case "x":
        resultado = calculadora.Multiplicar(n1, n2);
        break;
    case "÷":
        resultado = calculadora.Dividir(n1, n2);
        break;
    default:
    throw new Exception("No ingreso una operación valida.");
}

Console.WriteLine("El resultado es: " + resultado);

// Persona persona1 = new Persona("Juan", "Perez", "M", 21);

// Console.WriteLine("Hola " + persona1.Nombres + " " 
//     + persona1.Apellidos + " usted tiene " + persona1.Edad + " años");

// class Program 
// {
//     static void Main()
//     {
//         Console.WriteLine("Hola mundo");

//         Console.WriteLine("¿Cómo te llamas?");
//         string nombre =  Console.ReadLine();

//         Console.WriteLine("¿Qué edad tienes?");
//         string edad = Console.ReadLine();

//         Console.WriteLine("\nHola " + nombre + " tienes " + edad + " años.");

//     }
// }