using Day19.TestData;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Day19.DataReaders
{
    internal static class JsonDataReader
    {
        public static IEnumerable<LoginData> ReadLoginData()
        {
            string baseDir = TestContext.CurrentContext.TestDirectory;

            string filePath = Path.Combine(
                baseDir,
                "TestData",
                "loginData.json"
            );

            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<LoginData>>(json)
                   ?? new List<LoginData>();
        }
    }
}
