using CsvHelper;
using System.Globalization;

namespace Library.Api.Helpers
{
    public static class Csv
    {
        public static string GetDataPath(string path)
        {
            return Path.Combine(AppContext.BaseDirectory, path);
        }

        public static string ReadFile(string path)
        {
            var fullPath = GetDataPath(path);
            return File.ReadAllText(fullPath);
        }

        /// <summary>
        /// Read the uploaded file and return mapped object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<T> ReadFile<T>(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await foreach (var record in csv.GetRecordsAsync<T>())
            {
                yield return record; //to avoid fully load everything to memory
            }
        }
    }
}