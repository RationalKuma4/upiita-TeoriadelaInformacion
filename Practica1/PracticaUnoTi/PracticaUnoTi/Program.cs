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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var textoIngles = File.ReadAllText(@"451i.txt");
            var textoFrances = File.ReadAllText(@"451f.txt");

            NormalizarTexto(ref textoIngles);
            NormalizarTexto(ref textoFrances);

            #region Sin memoria

            var repeticionesIngles = ConteoRepeticionesCaracter(textoIngles);
            var repeticionesFrances = ConteoRepeticionesCaracter(textoFrances);

            var probabilidadesIngles = ProbabilidaxCaracter(repeticionesIngles, textoIngles.Length);
            var probabilidadesFrances = ProbabilidaxCaracter(repeticionesFrances, textoFrances.Length);

            var informacionIngles = CantidadInformacionxCarcater(probabilidadesIngles);
            var informacionFrances = CantidadInformacionxCarcater(probabilidadesFrances);

            var entropiaIngles = Entropia(probabilidadesIngles, informacionIngles);
            var entropiaFrances = Entropia(probabilidadesFrances, informacionFrances);

            #endregion


            #region Pares

            var paresIngles = ParesTexto(textoIngles);
            var paresFrances = ParesTexto(textoFrances);

            var paresProbabilidadIngles = ProbabilidadxPar(paresIngles, repeticionesIngles, probabilidadesIngles);
            var paresProbabilidadFrances = ProbabilidadxPar(paresFrances, repeticionesFrances, probabilidadesFrances);

            var informacionParesIngles = InformacionxPar(paresProbabilidadIngles);
            var informacionParesFrances = InformacionxPar(paresProbabilidadFrances);

            var entropiaParesIngles = EntropiaPares(paresProbabilidadIngles, informacionParesIngles);
            var entropiaParesFrances = EntropiaPares(paresProbabilidadFrances, informacionParesFrances);

            #endregion

            /*
            #region Tercias

            var terciasIngles = TerciasTexto(textoIngles);
            var terciasFrances = TerciasTexto(textoFrances);

            var terciasProbabilidadIngles = ProbabilidadxTercia(terciasIngles, paresIngles.ToDictionary(t => t.Par, t => t.Cantidad),
                paresProbabilidadIngles);
            var terciasProbabilidadFrances = ProbabilidadxTercia(terciasFrances, paresFrances.ToDictionary(t => t.Par, t => t.Cantidad),
                paresProbabilidadFrances);

            var terciasInformacionIngles = InformacionxTercia(terciasProbabilidadIngles);
            var terciasInformacionFrances = InformacionxTercia(terciasProbabilidadFrances);

            var entropiaTerciasIngles = EntropiaTercias(terciasProbabilidadIngles, terciasInformacionIngles);
            var entropiaTerciasFrances = EntropiaTercias(terciasProbabilidadFrances, terciasInformacionFrances);

            #endregion
            */

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs / 360);
            Console.ReadLine();
        }

        private static double EntropiaTercias(Dictionary<string, double> propabilidad, Dictionary<string, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
            {
                terminos.Add(propabilidad.ElementAt(i).Value * informacion.ElementAt(i).Value);
            }

            var terminoL = new List<double>();
            foreach (var termino in terminos)
            {
                if (double.IsNaN(termino))
                {
                    terminoL.Add(0);
                }
                else
                {
                    terminoL.Add(termino);
                }
            }

            return terminoL.Sum();
        }

        private static Dictionary<string, double> InformacionxTercia(Dictionary<string, double> probabilidades)
        {
            var informacionTercias = new Dictionary<string, double>();
            foreach (var tercia in probabilidades)
            {
                if (tercia.Value == 0)
                    informacionTercias.Add(tercia.Key, 0);
                else
                    informacionTercias.Add(tercia.Key, Math.Log(1 / tercia.Value, 2));
            }

            return informacionTercias;
        }

        private static double EntropiaPares(Dictionary<string, double> propabilidad, Dictionary<string, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
            {
                terminos.Add(propabilidad.ElementAt(i).Value * informacion.ElementAt(i).Value);
            }

            return terminos.Sum();
        }

        private static Dictionary<string, double> ProbabilidadxTercia(List<Tercias> tercias, Dictionary<string, double> repeticiones,
            Dictionary<string, double> probabilidades)
        {
            var probabilidadesTercia = new Dictionary<string, double>();
            foreach (var tercia in tercias)
            {
                var palabra = tercia.Tercia.Remove(tercia.Tercia.Length - 1);
                var numPares = tercias.First(p => p.Tercia.Equals(tercia.Tercia)).Cantidad;
                var denPares = repeticiones[palabra];

                var propabilidadPalabra = probabilidades[palabra];
                var probCondicional = numPares / denPares;

                var prob = propabilidadPalabra * probCondicional;

                probabilidadesTercia.Add(tercia.Tercia, prob);
            }

            return probabilidadesTercia;
        }

        private static Dictionary<string, double> InformacionxPar(Dictionary<string, double> probabilidades)
        {
            var informacionPares = new Dictionary<string, double>();
            foreach (var par in probabilidades)
            {
                if (par.Value == 0)
                    informacionPares.Add(par.Key, 0);
                else
                    informacionPares.Add(par.Key, Math.Log(1 / par.Value, 2));
            }

            return informacionPares;
        }

        private static List<Tercias> TerciasTexto(string texto)
        {
            var tercias = new List<Tercias>();
            for (var i = 0; i < texto.Length - 2; i++)
            {
                var tercia = texto[i].ToString() + texto[i + 1] + texto[i + 2];
                if (tercias.Exists(p => p.Tercia.Equals(tercia)))
                    tercias.First(p => p.Tercia.Equals(tercia)).Cantidad++;
                else
                    tercias.Add(new Tercias { Tercia = tercia, Cantidad = 0 });
            }

            return tercias;
        }




        private static Dictionary<string, double> ProbabilidadxPar(List<Pares> pares, Dictionary<char, double> repeticiones,
            Dictionary<char, double> probabilidades)
        {
            var probabilidadesPar = new Dictionary<string, double>();
            foreach (var par in pares)
            {
                var palabra = par.Par.Remove(par.Par.Length - 1);
                var numPares = pares.First(p => p.Par.Equals(par.Par)).Cantidad;
                var denPares = repeticiones[char.Parse(palabra)];

                var propabilidadPalabra = probabilidades[char.Parse(palabra)];
                var probCondicional = numPares / denPares;

                var prob = propabilidadPalabra * probCondicional;

                probabilidadesPar.Add(par.Par, prob);
            }

            return probabilidadesPar;
        }

        private static List<Pares> ParesTexto(string texto)
        {
            var pares = new List<Pares>();
            for (var i = 0; i < texto.Length - 1; i++)
            {
                var par = texto[i].ToString() + texto[i + 1];
                if (pares.Exists(p => p.Par.Equals(par)))
                    pares.First(p => p.Par.Equals(par)).Cantidad++;
                else
                    pares.Add(new Pares { Par = par, Cantidad = 0 });
            }

            return pares;
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

        private static Dictionary<char, double> CantidadInformacionxCarcater(Dictionary<char, double> diccionario)
        {
            var entropias = new Dictionary<char, double>();
            foreach (var letra in diccionario)
            {
                entropias.Add(letra.Key, Math.Log(1 / letra.Value, 2));
            }

            return entropias;
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

        private static Dictionary<char, double> ConteoRepeticionesCaracter(string texto)
        {
            var repeticiones = new Dictionary<char, double>();
            foreach (var letra in Abecedario)
            {
                repeticiones.Add(letra, texto.Count(l => l.Equals(letra)));
            }
            return repeticiones;
        }

        private static void NormalizarTexto(ref string texto)
        {
            texto = texto.ToLower();
            var rgx = new Regex("[^a-zA-Z ]+?");
            texto = rgx.Replace(texto, string.Empty);
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

    internal class Pares
    {
        public string Par { get; set; }
        public double Cantidad { get; set; }
    }

    internal class Tercias
    {
        public string Tercia { get; set; }
        public double Cantidad { get; set; }
    }
}
