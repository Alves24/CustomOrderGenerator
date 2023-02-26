
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Engine.DataProcessors
{
    public static class PathManager
    {
        private static string GetDropboxPath()
        {
            // Devuelve la ruta del dropbox desde cualquier pc.

            // Busco la ruta de 'info.json'
            string possiblepath1 = Environment.GetFolderPath(
                                   Environment.SpecialFolder.ApplicationData) + @"\Dropbox\info.json";

            string possiblepath2 = Environment.GetFolderPath(
                                   Environment.SpecialFolder.LocalApplicationData) + @"\Dropbox\info.json";

            string path = null;

            if (File.Exists(possiblepath1)) { path = possiblepath1; }

            if (File.Exists(possiblepath2)) { path = possiblepath2; }

            if (path == null)
            {
                return "Ruta del dropbox no encontrada (info.json)";
            }

            // Paso info.json a string.
            string json;
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }

            // Interpreto y leo el archivo en string y paro y leo cuando encuentro el path.
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "path")
                    {
                        reader.Read();
                        return reader.Value.ToString();
                    }
                }
            }
            return "Ruta de Dropbox no encontrada (path in json deserializer)";
        }
        public  static string GetDropboxPath_MEP()
        {
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // OLD CRAP --> return GetDropboxPath() + @"\Pedidos MEP";
            // 
            // 
            // 17-02-22
            // CHAD AND SIMPLE VERSION OF GETDROPBOXPATH!!!
            // Now we can change dropboxpath from 'Paths.json' file.
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            var text = File.ReadAllText(path: @".\Files\Paths.json");

            JsonTextReader reader = new JsonTextReader(new StringReader(text));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.Value.ToString() == "DropBoxPath_MEP")
                    {
                        reader.Read();
                        return reader.Value.ToString();
                    }
                }
            }
            return "Ruta o Archivo no encontrado (GETDROPBOXPATH_MEP)";
        }
        public static List<string> GetOrderFilePaths()
        {
            List<string> AllFilePaths = new List<string>();
            string path = GetDropboxPath_MEP();
            var LastFilePaths = new List<string>();
            // Obtengo las Direcciones ( Paths ) de las ordenes a partir de una fecha
            // "Ultimos 2 MESES"...
            DateTime FechaDeInicio = DateTime.Now;
            FechaDeInicio = FechaDeInicio.AddMonths(-2);

            if (Directory.Exists(path))
            {
                // Obtengo las direcciones de los archivos del dropbox
                AllFilePaths = Directory.EnumerateFiles(path) //<--- .NET 4.5
                                        .Where(file => file.ToLower().EndsWith("xlsx") && (File.GetLastWriteTime(file) > FechaDeInicio))
                                        .ToList();
            }
            else
            {
                new InfoBox("Ruta de Dropbox no encontrada!", $"path: {path}");
                return null;
            }
            return AllFilePaths;
        }
        public static int GetLastOrderNumber()
        {
            string numberSeccion, lastOrderPath;
            var FilePaths = GetOrderFilePaths();
            if (FilePaths == null) return 0;
            int Length = FilePaths.Count;
            int i = 0;

            do{
                lastOrderPath = FilePaths[Length - ++i];
                numberSeccion = Path.GetFileName(lastOrderPath).Substring(0, 5);
            } while (numberSeccion[0] == '~');

            return int.Parse(Regex.Match(numberSeccion, @"\d+").Value);
        }
    }
}
