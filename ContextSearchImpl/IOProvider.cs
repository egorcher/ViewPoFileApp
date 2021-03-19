using System.IO;
using System.Linq;

namespace ContextSearchImpl
{
    public interface IIOProvider
    {
        string[] GetNameFiles(string dirPath, string searhPattern);
    }
    public class IOProvider : IIOProvider
    {
        public string[] GetNameFiles(string dirPath, string searhPattern)
        {
            if (!Directory.Exists(dirPath))
                return new string[0];

            var results = Directory.GetFiles(dirPath, searhPattern).Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();
            return results;
        }
    }
}
