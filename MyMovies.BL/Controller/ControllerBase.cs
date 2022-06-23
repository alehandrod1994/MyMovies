using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyMovies.BL.Controller
{
    public abstract class ControllerBase
    {
        protected List<T> Load<T>() where T : class
        {
            var formatter = new BinaryFormatter();
            var fileName = $"{typeof(T).Name}s.bin";

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return fs.Length > 0 && formatter.Deserialize(fs) is List<T> items ? items : new List<T>();
            }
        }

        protected void Save<T>(List<T> item) where T : class
        {
            var formatter = new BinaryFormatter();
            var fileName = $"{typeof(T).Name}s.bin";

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, item);
            }
        }

    }
}
