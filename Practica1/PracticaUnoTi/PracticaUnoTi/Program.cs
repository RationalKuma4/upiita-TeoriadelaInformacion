using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PracticaUnoTi
{
    internal static class Program
    {
        private static IEnumerable<char> Abecedario { get; } = new List<char>
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',' '
        };

        private static void Main()
        {
            var textoIngles = File.ReadAllText(@"451i.txt");
            var textoFrances = File.ReadAllText(@"451f.txt");

            NormalizarTexto(ref textoIngles);
            NormalizarTexto(ref textoFrances);

            var repeticionesIngles = ConteoRepeticionesCaracter(textoIngles);
            var repeticionesFrances = ConteoRepeticionesCaracter(textoFrances);

            var probabilidadesIngles = ProbabilidaxCaracter(repeticionesIngles, textoIngles.Length);
            var probabilidadesFrances = ProbabilidaxCaracter(repeticionesFrances, textoFrances.Length);

            var informacionIngles = CantidadInformacionxCarcater(probabilidadesIngles);
            var informacionFrances = CantidadInformacionxCarcater(probabilidadesFrances);

            var entropiaIngles = Entropia(probabilidadesIngles, informacionIngles);
            var entropiaFrances = Entropia(probabilidadesFrances, informacionFrances);

            Console.ReadLine();
        }

        private static void NormalizarTexto(ref string texto)
        {
            texto = texto.ToLower();
            var rgx = new Regex("[^a-zA-Z0-9 ]+");
            texto = rgx.Replace(texto, string.Empty);
        }

        private static Dictionary<char, double> ConteoRepeticionesCaracter(string texto)
        {
            var repeticiones = new Dictionary<char, double>();
            foreach (var letra in Abecedario)
            {
                repeticiones.Add(letra, texto.Count(l => l.Equals(letra)));
            }
            return repeticiones;
        }

        private static Dictionary<char, double> ProbabilidaxCaracter(Dictionary<char, double> diccionario, int total)
        {
            var probabilidades = new Dictionary<char, double>();
            foreach (var letra in diccionario)
            {
                probabilidades.Add(letra.Key, letra.Value / total);
            }

            return probabilidades;
        }

        private static Dictionary<char, double> CantidadInformacionxCarcater(Dictionary<char, double> diccionario)
        {
            var entropias = new Dictionary<char, double>();
            foreach (var letra in diccionario)
            {
                entropias.Add(letra.Key, Math.Log(1 / letra.Value, 2));
            }

            return entropias;
        }

        private static double Entropia(Dictionary<char, double> propabilidad, Dictionary<char, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
            {
                terminos.Add(propabilidad.ElementAt(i).Value * informacion.ElementAt(i).Value);
            }

            return terminos.Sum();
        }

        private static void Imprimir<T>(T objeto, string formato = "")
        {
            if (objeto is Dictionary<char, double>)
            {
                var diccionario = objeto as Dictionary<char, double>;
                foreach (var entry in diccionario)
                {
                    Console.WriteLine($@"{formato} {entry.Key} {entry.Value}");
                }
            }
        }
    }
}
