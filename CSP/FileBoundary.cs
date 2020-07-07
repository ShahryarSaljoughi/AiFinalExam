using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSP.Entities;

namespace CSP
{
    public class FileBoundary
    {
        private static string InputPath { get; set; }

        static FileBoundary()
        {
            var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            InputPath = Path.Combine(
                basePath ?? throw new InvalidOperationException("problem with getting path to executable"), "Data",
                "Input.txt");
        }

        public static async Task<int[,]> ReadVisibilityMatrixAsync()
        {
            var fileContent = await ReadFileAsync(InputPath);
            var pattern = @"Visibility:\r\n(?<visibility>((([01],)+[01])+)([\r\n]{2}((([01],)+[01])+))*)";
            var visibilityContent = Regex.Match(fileContent, pattern).Groups["visibility"].Value;

            return ConvertMatrixStringToArray(visibilityContent);
        }

        public static async Task<int[,]> ReadCommunicationsMatrixAsync()
        {
            var fileContent = await ReadFileAsync(InputPath);
            var lines = fileContent.Split(Environment.NewLine);
            var pattern = @"Communication:\r\n(?<communication>((([01],)+[01])+)([\r\n]{2}((([01],)+[01])+))*)";
            var communicationMatrix = Regex.Match(fileContent, pattern).Groups["communication"].Value;

            return ConvertMatrixStringToArray(communicationMatrix);
        }

        public static async Task<int> GetKAsync()
        {
            var fileContent = await ReadFileAsync(InputPath);
            var pattern = @"K:[\r\n]*(?<k>\d+)";
            var k = Regex.Match(fileContent, pattern).Groups["k"].Value;
            return int.Parse(k);
        }

        public static async Task WriteBackResultAsync(Result result)
        {
            var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var outputPath = Path.Combine(
                basePath ?? throw new InvalidOperationException("problem with getting path to executable"), "Data",
                "Output.txt");
            await using var sw = new StreamWriter($"{outputPath}");
            await sw.WriteAsync(result.ToString());
        }

        private static async Task<string> ReadFileAsync(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string result = await sr.ReadToEndAsync();
                    return result;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static int[,] ConvertMatrixStringToArray(string matrix)
        {
            var lines = matrix.Split(Environment.NewLine);
            var numberOfRows = lines.Length;
            var numberOfColumns = lines.First().Split(",").Length;

            if (lines.Any(line => line.Split(",").Length != numberOfColumns))
                throw new Exception("input file is not valid");

            var result = new int[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    result[i, j] = int.Parse(lines[i].Split(",")[j]);
                }
            }

            return result;
        }
    }
}