using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PracticaUnoTi
{
    internal static class Program
    {
        private static IEnumerable<string> Abecedario { get; } = new List<string>
        {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"," "
        };

        private static void Main()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var textoIngles = File.ReadAllText(@"451i.txt");
            var textoFrances = File.ReadAllText(@"451f.txt");

            NormalizarTexto(ref textoIngles);
            NormalizarTexto(ref textoFrances);

            #region Sin memoria Ingles

            var repeticionesIngles = ConteoRepeticionesCaracter(textoIngles);
            var probabilidadesIngles = ProbabilidaxCaracter(repeticionesIngles, textoIngles.Length);
            var informacionIngles = CantidadInformacionxCarcater(probabilidadesIngles);
            var entropiaIngles = Entropia(probabilidadesIngles, informacionIngles);

            #endregion

            #region Pares Ingles

            var paresIngles = ParesTexto(textoIngles);
            var paresProbabilidadIngles = ProbabilidadxPar(paresIngles, repeticionesIngles, probabilidadesIngles);
            var informacionParesIngles = InformacionxPar(paresProbabilidadIngles);
            var entropiaParesIngles = EntropiaPares(paresProbabilidadIngles, informacionParesIngles);

            #endregion

            #region Tercias

            var terciasIngles = TerciasTexto(textoIngles);
            var terciasProbabilidadIngles = ProbabilidadxTercia(terciasIngles, paresIngles,
                paresProbabilidadIngles.ToDictionary(d => d.Par, d => d.Probabilidad));
            var terciasInformacionIngles = InformacionxTercia(terciasProbabilidadIngles);
            var entropiaTerciasIngles = EntropiaTercias(terciasProbabilidadIngles, terciasInformacionIngles);

            #endregion

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs / 360);
            Console.ReadLine();
        }

        #region Metodos Tercias

        private static Dictionary<string, double> TerciasTexto(string texto)
        {
            var tercias = new List<Tercias>();
            for (var i = 0; i < texto.Length - 2; i++)
            {
                var tercia = texto[i].ToString() + texto[i + 1] + texto[i + 2];
                if (tercias.Exists(p => p.Tercia.Equals(tercia)))
                    tercias.First(p => p.Tercia.Equals(tercia)).Cantidad++;
                else
                    tercias.Add(new Tercias { Tercia = tercia, Cantidad = 1 });
            }

            return tercias.ToDictionary(t => t.Tercia, t => t.Cantidad);
        }

        private static List<Tercias> ProbabilidadxTercia(Dictionary<string, double> tercias, Dictionary<string, double> repeticiones,
            Dictionary<string, double> probabilidades)
        {
            var probabilidadesTercias = new List<Tercias>();
            foreach (var terc in tercias)
            {
                var palabra = terc.Key.Remove(terc.Key.Length - 1);
                var numPares = tercias[terc.Key];
                var denPares = repeticiones[palabra];

                var proPalabra = probabilidades[palabra];
                var proCondicinal = numPares / denPares;
                var prob = proPalabra * proCondicinal;

                probabilidadesTercias.Add(new Tercias { Tercia = terc.Key, ProbCondicional = proCondicinal, Probabilidad = prob });
            }

            return probabilidadesTercias;
        }

        private static Dictionary<string, double> InformacionxTercia(List<Tercias> probabilidades)
        {
            var informacionTercias = new Dictionary<string, double>();
            foreach (var tercia in probabilidades)
                informacionTercias.Add(tercia.Tercia, Math.Log(1 / tercia.ProbCondicional, 2));

            return informacionTercias;
        }

        private static double EntropiaTercias(List<Tercias> propabilidad, Dictionary<string, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
            {
                terminos.Add(propabilidad[i].Probabilidad * informacion.ElementAt(i).Value);
            }

            return terminos.Sum();
        }

        #endregion

        #region Metodos Pares

        private static Dictionary<string, double> ParesTexto(string texto)
        {
            var pares = new List<Pares>();
            for (var i = 0; i < texto.Length - 1; i++)
            {
                var par = texto[i].ToString() + texto[i + 1];
                if (pares.Exists(p => p.Par.Equals(par)))
                    pares.First(p => p.Par.Equals(par)).Cantidad++;
                else
                    pares.Add(new Pares { Par = par, Cantidad = 1 });
            }

            return pares.ToDictionary(p => p.Par, p => p.Cantidad);
        }

        private static List<Pares> ProbabilidadxPar(Dictionary<string, double> pares, Dictionary<string, double> repeticiones,
            Dictionary<string, double> probabilidades)
        {
            var probabilidadesPar = new List<Pares>();
            foreach (var pair in pares)
            {
                var palabra = pair.Key.Remove(pair.Key.Length - 1);
                var numPares = pares[pair.Key];
                var denPares = repeticiones[palabra];

                var proPalabra = probabilidades[palabra];
                var proCondicinal = numPares / denPares;
                var prob = proPalabra * proCondicinal;

                probabilidadesPar.Add(new Pares { Par = pair.Key, ProbCondicional = proCondicinal, Probabilidad = prob });
            }

            return probabilidadesPar;
        }

        private static Dictionary<string, double> InformacionxPar(List<Pares> probabilidades)
        {
            var informacionPares = new Dictionary<string, double>();
            foreach (var par in probabilidades)
                informacionPares.Add(par.Par, Math.Log(1 / par.ProbCondicional, 2));

            return informacionPares;
        }

        private static double EntropiaPares(List<Pares> propabilidad, Dictionary<string, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
            {
                terminos.Add(propabilidad[i].Probabilidad * informacion.ElementAt(i).Value);
            }

            return terminos.Sum();
        }

        #endregion

        #region Metodos sin memoria

        private static Dictionary<string, double> ConteoRepeticionesCaracter(string texto)
        {
            var repeticiones = new Dictionary<string, double>();
            foreach (var letra in Abecedario)
                repeticiones.Add(letra, texto.Count(l => l.Equals(char.Parse(letra))));

            return repeticiones;
        }

        private static Dictionary<string, double> ProbabilidaxCaracter(Dictionary<string, double> diccionario, int total)
        {
            var probabilidades = new Dictionary<string, double>();
            foreach (var letra in diccionario)
                probabilidades.Add(letra.Key, letra.Value / total);

            return probabilidades;
        }

        private static Dictionary<string, double> CantidadInformacionxCarcater(Dictionary<string, double> diccionario)
        {
            var entropias = new Dictionary<string, double>();
            foreach (var letra in diccionario)
                entropias.Add(letra.Key, Math.Log(1 / letra.Value, 2));

            return entropias;
        }

        private static double Entropia(Dictionary<string, double> propabilidad, Dictionary<string, double> informacion)
        {
            var terminos = new List<double>();
            for (var i = 0; i < propabilidad.Count; i++)
                terminos.Add(propabilidad.ElementAt(i).Value * informacion.ElementAt(i).Value);

            return terminos.Sum();
        }

        #endregion

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
        public double Probabilidad { get; set; }
        public double ProbCondicional { get; set; }
    }

    internal class Tercias
    {
        public string Tercia { get; set; }
        public double Cantidad { get; set; }
        public double Probabilidad { get; set; }
        public double ProbCondicional { get; set; }
    }
}
