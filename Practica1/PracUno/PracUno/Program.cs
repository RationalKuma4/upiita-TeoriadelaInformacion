using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PracUno
{
    internal static class Program
    {
        private static void Main()
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.FullName;
            var textoIngles = File.ReadAllText($@"{path}/Textos/451i.txt");
            var textoFrances = File.ReadAllText($@"{path}/Textos/451f.txt");

            NormalizarTexto(ref textoIngles);
            NormalizarTexto(ref textoFrances);

            #region Sin memoria

            var repeticionesIngles = ConteoRepeticionesCaracter(textoIngles);
            var probabilidadesIngles = ProbabilidaxCaracter(repeticionesIngles, textoIngles.Length);
            var informacionIngles = CantidadInformacionxCarcater(probabilidadesIngles);
            var entropiaIngles = Entropia(probabilidadesIngles, informacionIngles);

            WriteDicInfo(probabilidadesIngles, informacionIngles, entropiaIngles, path, nameof(textoIngles));

            var repeticionesFrances = ConteoRepeticionesCaracter(textoFrances);
            var probabilidadesFrances = ProbabilidaxCaracter(repeticionesFrances, textoFrances.Length);
            var informacionFrances = CantidadInformacionxCarcater(probabilidadesFrances);
            var entropiaFrances = Entropia(probabilidadesFrances, informacionFrances);

            WriteDicInfo(probabilidadesFrances, informacionFrances, entropiaFrances, path, nameof(textoFrances));

            #endregion

            #region Pares

            var paresIngles = ParesTexto(textoIngles);
            var paresProbabilidadIngles = ProbabilidadxPar(paresIngles, repeticionesIngles, probabilidadesIngles);
            var informacionParesIngles = InformacionxPar(paresProbabilidadIngles);
            var entropiaParesIngles = EntropiaPares(paresProbabilidadIngles, informacionParesIngles);

            WriteDicInfo(paresProbabilidadIngles.ToDictionary(d => d.Par, d => d.ProbCondicional), informacionParesIngles, entropiaParesIngles, path, nameof(paresIngles));

            var paresFrances = ParesTexto(textoFrances);
            var paresProbabilidadFrances = ProbabilidadxPar(paresFrances, repeticionesFrances, probabilidadesFrances);
            var informacionParesFrances = InformacionxPar(paresProbabilidadFrances);
            var entropiaParesFrances = EntropiaPares(paresProbabilidadFrances, informacionParesFrances);

            WriteDicInfo(paresProbabilidadFrances.ToDictionary(d => d.Par, d => d.ProbCondicional), informacionParesFrances, entropiaParesFrances, path, nameof(paresFrances));

            #endregion

            #region Tercias

            var terciasIngles = TerciasTexto(textoIngles);
            var terciasProbabilidadIngles = ProbabilidadxTercia(terciasIngles, paresIngles,
                paresProbabilidadIngles.ToDictionary(d => d.Par, d => d.Probabilidad));
            var terciasInformacionIngles = InformacionxTercia(terciasProbabilidadIngles);
            var entropiaTerciasIngles = EntropiaTercias(terciasProbabilidadIngles, terciasInformacionIngles);

            WriteDicInfo(terciasProbabilidadIngles.ToDictionary(d => d.Tercia, d => d.ProbCondicional), terciasInformacionIngles, entropiaTerciasIngles, path, nameof(terciasIngles));

            var terciasFrances = TerciasTexto(textoFrances);
            var terciasProbabilidadFrances = ProbabilidadxTercia(terciasFrances, paresFrances,
                paresProbabilidadFrances.ToDictionary(d => d.Par, d => d.Probabilidad));
            var terciasInformacionFrances = InformacionxTercia(terciasProbabilidadFrances);
            var entropiaTerciasFrances = EntropiaTercias(terciasProbabilidadFrances, terciasInformacionFrances);

            WriteDicInfo(terciasProbabilidadFrances.ToDictionary(d => d.Tercia, d => d.ProbCondicional), terciasInformacionFrances, entropiaTerciasFrances, path, nameof(terciasFrances));

            #endregion

            #region Reportar datos

            Console.WriteLine($@"Libro: the martian chronicles
Cantidad de caracteres ingles: {textoIngles.Length} 
Cantidad de caracteres frances: {textoFrances.Length}");

            Console.WriteLine();
            Console.WriteLine("Ingles");
            Console.WriteLine($"Entropia sin memoria: {entropiaIngles} bit/simbolo \n");
            Console.WriteLine($"Segundo orden: {entropiaParesIngles} bit/simbolo");
            Console.WriteLine("Casos mas frecuentes Ingles");
            foreach (var keyValuePair in paresIngles.OrderByDescending(p => p.Value)
                .Take(10))
            {
                Console.WriteLine($"Pareja: {keyValuePair.Key}, Repeticiones: {keyValuePair.Value}");
            }

            Console.WriteLine();
            Console.WriteLine($"Tercer orden: {entropiaTerciasIngles} bit/simbolo");
            Console.WriteLine("Casos mas frecuentes Ingles");
            foreach (var keyValuePair in terciasIngles.OrderByDescending(p => p.Value)
                .Take(10))
            {
                Console.WriteLine($"Tercia: {keyValuePair.Key}, Repeticiones: {keyValuePair.Value}");
            }


            Console.WriteLine();
            Console.WriteLine("Frances");
            Console.WriteLine($"Entropia sin memoria: {entropiaFrances} bit/simbolo \n");
            Console.WriteLine($"Primer orden: {entropiaParesFrances} bit/simbolo");
            foreach (var keyValuePair in paresFrances.OrderByDescending(p => p.Value)
                .Take(10))
            {
                Console.WriteLine($"Pareja: {keyValuePair.Key}, Repeticiones: {keyValuePair.Value}");
            }

            Console.WriteLine();

            Console.WriteLine($"Segundo orden: {entropiaTerciasFrances} bit/simbolo");
            foreach (var keyValuePair in terciasFrances.OrderByDescending(p => p.Value)
                .Take(10))
            {
                Console.WriteLine($"Pareja: {keyValuePair.Key}, Repeticiones: {keyValuePair.Value}");
            }
            Console.WriteLine();
            Console.ReadLine();

            #endregion
        }

        #region Util

        private static void NormalizarTexto(ref string texto)
        {
            texto = texto.ToLower();
            var rgx = new Regex("[^a-zA-Z ]+?");
            texto = rgx.Replace(texto, string.Empty);
        }

        private static void WriteDicInfo(Dictionary<string, double> probabilidad, Dictionary<string, double> informacion,
            double entropia, string filePath, string nombre)
        {
            var csv = new StringBuilder();

            for (var i = 0; i < probabilidad.Count; i++)
            {
                var newLine = $"{probabilidad.ElementAt(i).Key},{probabilidad.ElementAt(i).Value},{informacion.ElementAt(i).Value}";
                csv.AppendLine(newLine);
            }
            csv.AppendLine(entropia.ToString(CultureInfo.CurrentCulture));
            File.WriteAllText($"{filePath}/Textos/{nombre}.csv", csv.ToString());
        }

        #endregion

        #region Metodos sin memoria

        private static Dictionary<string, double> ConteoRepeticionesCaracter(string texto)
        {
            var abecedario = new List<string>
            {
                "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"," "
            };
            var repeticiones = new Dictionary<string, double>();
            foreach (var letra in abecedario)
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

                var proPalabra = probabilidades[palabra]; //p(x_{i-1})
                var proCondicinal = numPares / denPares; //p(x_{i} | x_{i-1})
                var prob = proPalabra * proCondicinal; //p(x_{i-1})*p(x_{i} | x_{i-1})

                probabilidadesPar.Add(new Pares { Par = pair.Key, ProbCondicional = proCondicinal, Probabilidad = prob });
            }

            return probabilidadesPar;
        }

        private static Dictionary<string, double> InformacionxPar(List<Pares> probabilidades)
        {
            var informacionPares = new Dictionary<string, double>();
            foreach (var par in probabilidades)
                informacionPares.Add(par.Par, Math.Log(1 / par.ProbCondicional, 2)); //log_{2}*(1)*p(x_{i} | x_{i-1})

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
