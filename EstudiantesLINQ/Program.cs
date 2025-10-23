using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EstudiantesLINQ
{
    public class Estudiante
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Curso { get; set; }
        public double Nota { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string ruta = "datos.csv";
            var estudiantes = new List<Estudiante>();

            // Verificar que el archivo existe
            if (!File.Exists(ruta))
            {
                Console.WriteLine("No se encontró el archivo 'datos.csv'.");
                return;
            }

            try
            {
                var lineas = File.ReadAllLines(ruta).Skip(1); // Saltar encabezado

                foreach (var linea in lineas)
                {
                    var partes = linea.Split(',');
                    if (partes.Length < 5) continue; // Evitar líneas incompletas

                    var estudiante = new Estudiante
                    {
                        Curso = partes[0],
                        Id = partes[1],
                        Nombre = $"{partes[2]} {partes[3]}",
                        Nota = double.Parse(partes[4])
                    };

                    estudiantes.Add(estudiante);
                }

                Console.WriteLine($"Datos cargados correctamente. Total de estudiantes: {estudiantes.Count}\n");

                // -----------------------------
                // CONSULTAS CON LINQ
                // -----------------------------

                // a) Estudiantes aprobados (nota >= 60)
                var aprobados = estudiantes.Where(e => e.Nota >= 60);
                Console.WriteLine("a) Estudiantes aprobados:");
                foreach (var e in aprobados)
                    Console.WriteLine($" - {e.Nombre} ({e.Curso}) -> {e.Nota:F2}");
                Console.WriteLine();

                // b) Promedio general
                var promedioGeneral = estudiantes.Average(e => e.Nota);
                Console.WriteLine($"b) Promedio general: {promedioGeneral:F2}\n");

                // c) Estudiante con la nota más alta
                var mejorEstudiante = estudiantes.OrderByDescending(e => e.Nota).First();
                Console.WriteLine($"c) Mejor estudiante: {mejorEstudiante.Nombre} ({mejorEstudiante.Curso}) -> {mejorEstudiante.Nota:F2}\n");

                // d) Agrupar por curso y mostrar promedio por curso
                Console.WriteLine("d) Promedio por curso:");
                var promedioPorCurso = estudiantes
                    .GroupBy(e => e.Curso)
                    .Select(g => new { Curso = g.Key, Promedio = g.Average(e => e.Nota) });

                foreach (var c in promedioPorCurso)
                    Console.WriteLine($" - {c.Curso}: {c.Promedio:F2}");
                Console.WriteLine();

                // e) Top 5 notas más altas
                Console.WriteLine("e) Top 5 estudiantes con mejores notas:");
                var top5 = estudiantes.OrderByDescending(e => e.Nota).Take(5);
                foreach (var e in top5)
                    Console.WriteLine($" - {e.Nombre} ({e.Curso}) -> {e.Nota:F2}");
                Console.WriteLine();

                // f) Estudiantes reprobados (nota < 60)
                var reprobados = estudiantes.Where(e => e.Nota < 60);
                Console.WriteLine("f) Estudiantes reprobados:");
                foreach (var e in reprobados)
                    Console.WriteLine($" - {e.Nombre} ({e.Curso}) -> {e.Nota:F2}");
                Console.WriteLine();

                // g) Contar cuántos aprobaron por curso
                Console.WriteLine("g) Aprobados por curso:");
                var aprobadosPorCurso = estudiantes
                    .Where(e => e.Nota >= 60)
                    .GroupBy(e => e.Curso)
                    .Select(g => new { Curso = g.Key, Cantidad = g.Count() });

                foreach (var c in aprobadosPorCurso)
                    Console.WriteLine($" - {c.Curso}: {c.Cantidad} aprobados");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al procesar el archivo: {ex.Message}");
            }

            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
