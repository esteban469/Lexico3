using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Data;
using ExcelDataReader;
using System.Text;

namespace Lexico_3
{
    public class Lexico : Token, IDisposable
    {
        public StreamReader archivo;
        public StreamWriter log;
        public StreamWriter asm;
        public int linea = 1;
        const int F = -1;

        const int E = -2;

        int[,] TRAND;

        public Lexico(bool leerDesdeExcel)
        {
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;

            if (leerDesdeExcel)
            {
                TRAND = LeerMatrizDesdeExcel("C:\\Users\\Esteban\\Documents\\ITQ\\AUTOMATAS\\Lexico3\\TRAND.xlsx");
            }
            else
            {
                TRAND = new int[,] {
                        { 0,  1,  2,  33, 1,  12, 14, 8,  9,  10, 11, 23, 16, 16, 18, 20, 21, 26, 25, 27, 29, 32, 34, 0,  F, 33},
                        { F,  1,  1,  F,  1,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  2,  3,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { E,  E,  4,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E, E},
                        { F,  F,  4,  F,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { E,  E,  7,  E,  E,  6,  6,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E, E},
                        { E,  E,  7,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E, E},
                        { F,  F,  7,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  13, F,  F,  F,  F,  F,  13, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  13, F,  F,  F,  F,  13, F,  F,  F,  F,  F,  F,  15, F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  17, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  19, F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  19, F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  22, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  24, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  24, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  24, F,  F,  F,  F,  F,  F,  24, F,  F,  F,  F,  F,  F, F},
                        {27,  27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 28, 27, 27, 27, 27, E, 27},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        {30,  30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30,30},
                        { E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  31, E,  E,  E,  E, E},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  32, F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, F},
                        { F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  17, 36, F,  F,  F,  F,  F,  F,  F,  F,  F,  35, F,  F, F},
                        {35,  35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 0,  35,35},
                        {36,  36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36,36},
                        {36,  36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36, 0,  36, 36,36},
                };
            }

            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe", log);
            }
        }


        // METODO PARA LEER MATRIZ DE EXCEL
        private int[,] LeerMatrizDesdeExcel(string rutaArchivo)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int[,] matrizExcel = new int[38, 26];

            using (var stream = File.Open(rutaArchivo, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[0];

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        string cellValue = dataTable.Rows[i][j].ToString();

                        if (cellValue == "F")
                        {
                            matrizExcel[i, j] = -1;
                        }
                        else if (cellValue == "E")
                        {
                            matrizExcel[i, j] = -2;
                        }
                        else
                        {
                            matrizExcel[i, j] = int.Parse(cellValue);
                        }
                    }
                }
            }

            return matrizExcel;
        }



        public Lexico(string nombreArchivo)
        {
            string nombreArchivoWithoutExt = Path.GetFileNameWithoutExtension(nombreArchivo);
            if (File.Exists(nombreArchivo))
            {
                log = new StreamWriter(nombreArchivoWithoutExt + ".log");
                asm = new StreamWriter(nombreArchivoWithoutExt + ".asm");
                log.AutoFlush = true;
                asm.AutoFlush = true;
                archivo = new StreamReader(nombreArchivo);
            }
            else if (Path.GetExtension(nombreArchivo) != ".cpp")
            {
                throw new ArgumentException("El archivo debe ser de extensión .cpp");
            }
            else
            {
                throw new FileNotFoundException("La extensión " + Path.GetExtension(nombreArchivo) + " no existe");
            }
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
            asm.Close();
        }

        private int Columna(char c)
        {

            if (c == '\n')
            {
                return 23;
            }
            else if (finArchivo())
            {
                return 24;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.ToLower(c) == 'e')
            {
                return 4;
            }
            else if (char.IsLetter(c))
            {
                return 1;
            }
            else if (char.IsDigit(c))
            {
                return 2;
            }
            else if (c == '.')
            {
                return 3;
            }
            else if (c == '+')
            {
                return 5;
            }
            else if (c == '-')
            {
                return 6;
            }
            else if (c == ';')
            {
                return 7;
            }
            else if (c == '{')
            {
                return 8;
            }
            else if (c == '}')
            {
                return 9;
            }
            else if (c == '?')
            {
                return 10;
            }
            else if (c == '=')
            {
                return 11;
            }
            else if (c == '*')
            {
                return 12;
            }
            else if (c == '%')
            {
                return 13;
            }
            else if (c == '&')
            {
                return 14;
            }
            else if (c == '|')
            {
                return 15;
            }
            else if (c == '!')
            {
                return 16;
            }
            else if (c == '<')
            {
                return 17;
            }
            else if (c == '>')
            {
                return 18;
            }
            else if (c == '"')
            {
                return 19;
            }
            else if (c == '\'')
            {
                return 20;
            }
            else if (c == '#')
            {
                return 21;
            }
            else if (c == '/')
            {
                return 22;
            }

            return 25;
        }
        private void Clasifica(int estado)
        {
            switch (estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.FinSentencia); break;
                case 9: setClasificacion(Tipos.InicioBloque); break;
                case 10: setClasificacion(Tipos.FinBloque); break;
                case 11: setClasificacion(Tipos.OperadorTernario); break;

                case 12: // OPERADOR TERMINO
                case 14: setClasificacion(Tipos.OperadorTermino); break;

                case 13: setClasificacion(Tipos.IncrementoTermino); break;
                case 15: setClasificacion(Tipos.Puntero); break;

                case 16: // OPERADOR FACTOR
                case 34: setClasificacion(Tipos.OperadorFactor); break;

                case 17: setClasificacion(Tipos.IncrementoFactor); break;

                case 18: // CARACTER
                case 20:
                case 29:
                case 32:
                case 33: setClasificacion(Tipos.Caracter); break;

                case 19: //OPERADOR LOGICO
                case 21: setClasificacion(Tipos.OperadorLogico); break;

                case 22: // OPERADOR RELACIONAL
                case 24:
                case 25:
                case 26: setClasificacion(Tipos.OperadorRelacional); break;

                case 23: setClasificacion(Tipos.Asignacion); break;

                case 27: setClasificacion(Tipos.Cadena); break;

            }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";
            int estado = 0;
            while (estado >= 0)
            {
                c = (char)archivo.Peek();
                estado = TRAND[estado, Columna(c)];
                Clasifica(estado);
                if (estado >= 0)
                {
                    archivo.Read();
                    if (c == '\n')
                    {
                        linea++;
                    }
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }
            if (estado == E)
            {
                if (getClasificacion() == Tipos.Cadena)
                {
                    throw new Error("léxico, se esperaba un cierre de cadena", log, linea);
                }
                else if (getClasificacion() == Tipos.Caracter)
                {
                    throw new Error("léxico, se esperaba un cierre de comilla simple", log, linea);
                }
                else if (getClasificacion() == Tipos.Numero)
                {
                    throw new Error("léxico, se esperaba un dígitos", log, linea);
                }
                else
                {
                    throw new Error("léxico, se espera fin de comentario", log, linea);
                }
            }
            if (!finArchivo())
            {
                setContenido(buffer);
                log.WriteLine(getContenido() + " __ " + getClasificacion());
            }
        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }

    }
}

