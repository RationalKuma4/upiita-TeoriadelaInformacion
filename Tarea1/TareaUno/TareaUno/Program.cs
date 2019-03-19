using System;
using System.Collections.Generic;
using System.Linq;

namespace TareaUno
{
    internal static class Program
    {
        public static void Main()
        {
            double[,] matrizUno = {
                { 1.0/2, 1.0/3, 1.0/6 },
                { 1.0/6, 1.0/2, 1.0/3 },
                { 1.0/3, 1.0/6, 1.0/2 }
            };

            double[,] matrizDos = {
                { 1.0/3, 1.0/6, 1.0/2 },
                { 1.0/6, 1.0/2, 1.0/3 },
                { 1.0/2, 1.0/6, 1.0/6 }
            };

            double[,] matrizTres = {
                { 1.0/3, 1.0/3, 1.0/3 },
                { 1.0/2, 1.0/2, 0.0 },
                { 1.0/2, 1.0/3, 1.0/6 }
            };

            double[] probInicial = { 1.0 / 3, 1.0 / 3, 1.0 / 3 };

            #region Informacion mutua

            var infoMutuaUno = CalcularInformacionMutua(matrizUno, probInicial);
            var infoMutuaDos = CalcularInformacionMutua(matrizDos, probInicial);
            var infoMutuaTres = CalcularInformacionMutua(matrizTres, probInicial);

            #endregion

            #region Capacidad de canal

            var matricesIniciales = MatrizProbIniciales();
            var capacidadCanalUno = CalculaCapacidadCanal(matrizUno, matricesIniciales);
            var capacidadCanalDos = CalculaCapacidadCanal(matrizDos, matricesIniciales);
            var capacidadCanalTres = CalculaCapacidadCanal(matrizTres, matricesIniciales);

            #endregion
        }

        private static CapacidadCanal CalculaCapacidadCanal(double[,] matriz, IEnumerable<double[]> matricesInic)
        {
            var informaciones = new Dictionary<double[], double>();

            foreach (var matrizInicial in matricesInic)
            {
                var informacionMutuaMatriz = CalcularInformacionMutua(matriz, matrizInicial);
                informaciones.Add(matrizInicial, informacionMutuaMatriz);
            }

            var infoMutuaMax = informaciones.Max(i => i.Value);
            var probInfoMutuaMax = informaciones
                .FirstOrDefault(i => i.Value == infoMutuaMax)
                .Key;

            return new CapacidadCanal
            {
                MaxInformacionMutua = infoMutuaMax,
                MaxMatrizInformacionMutua = probInfoMutuaMax,
                ConjuntoMatrices = informaciones
            };
        }

        private static List<double[]> MatrizProbIniciales()
        {
            var probA = 0.0000;
            var probB = 0.0001;
            var probC = 0.9999;
            var probs = new List<double[]> { new[] { probA, probB, probC } };

            for (var i = 0; i < 5001; i++)
            {
                probA += 0.0001;
                probB += 0.0001;
                probC -= 0.0002;

                if (probC < 0) break;
                probs.Add(new[]
                {
                    probA, probB, probC
                });
            }

            return probs;
        }

        private static double CalcularInformacionMutua(double[,] matriz, double[] probInicial)
        {
            double informacionMutua = 0;
            for (var i = 0; i < matriz.GetLength(0); i++)
            {
                for (var j = 0; j < matriz.GetLength(1); j++)
                {
                    var valorSumaInterior = probInicial.Select((t, k) => t * matriz[k, j]).Sum();
                    if (matriz[i, j] == 0.0)
                        informacionMutua += 0.0;
                    else
                        informacionMutua += probInicial[i] * matriz[i, j] * Math.Log(matriz[i, j] / valorSumaInterior, 2);
                }
            }
            return informacionMutua;
        }
    }

    internal class CapacidadCanal
    {
        public double MaxInformacionMutua { get; set; }
        public double[] MaxMatrizInformacionMutua { get; set; }
        public Dictionary<double[], double> ConjuntoMatrices { get; set; }
    }
}


/*

    private static double CalcularInformacionMutua(double[,] matriz, double[] probInicial)
        {
            double informacionMutua = 0;

            for (var i = 0; i < matriz.GetLength(0); i++)
            {
                for (var j = 0; j < matriz.GetLength(1); j++)
                {
                    //Console.WriteLine("Suma incial");
                    double valorSumaInterior = 0;
                    for (var k = 0; k < probInicial.Length; k++)
                    {
                        //Console.WriteLine(probInicial[k]);
                        //Console.WriteLine(matriz[k, j]);
                        //Console.WriteLine();
                        valorSumaInterior += probInicial[k] * matriz[k, j];
                    }

                    //Console.WriteLine("Suma");
                    //Console.WriteLine(probInicial[i]);
                    //Console.WriteLine(matriz[i, j]);
                    //Console.WriteLine();

                    var test = probInicial[i] * matriz[i, j];
                    var test2 = matriz[i, j] * Math.Log(matriz[i, j] / valorSumaInterior, 2);

                    //Caso cuando el valor es cero lo que provoca un inf
                    if (matriz[i, j] == 0.0)
                        informacionMutua += 0.0;
                    else
                        informacionMutua += probInicial[i] * matriz[i, j] * Math.Log(matriz[i, j] / valorSumaInterior, 2);
                }
            }

            return informacionMutua;
        }
 */
