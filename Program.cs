using System;
using System.Collections.Generic;
using System.IO;

namespace Lexico_3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool leerDesdeExcel = true; // true = matriz de excel, false = matriz TRAND
                using (Lexico lexico = new Lexico(leerDesdeExcel))
                {
                    while (!lexico.finArchivo())
                    {
                        lexico.nextToken();
                    }
                    lexico.log.WriteLine("\n-----------------------------------\n");
                    lexico.log.WriteLine("Líneas del archivo: " + lexico.linea);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
