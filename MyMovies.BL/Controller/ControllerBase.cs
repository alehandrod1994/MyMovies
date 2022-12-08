using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace MyMovies.BL.Controller
{
	/// <summary>
	/// Базовый контроллер.
	/// </summary>
	public abstract class ControllerBase
	{
		/// <summary>
		/// Загрузить данные.
		/// </summary>
		/// <returns> Список элементов. </returns>
		protected List<T> Load<T>() where T : class
		{
			var formatter = new BinaryFormatter();
			var fileName = $"{typeof(T).Name}s.bin";

			using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				return fs.Length > 0 && formatter.Deserialize(fs) is List<T> items ? items : new List<T>();
			}
		}	

		/// <summary>
		/// Сохранить данные.
		/// </summary>
		/// <param name="item"> Список элементов для сохранения. </param>
		protected void Save<T>(List<T> item) where T : class
		{
			var formatter = new BinaryFormatter();
			var fileName = $"{typeof(T).Name}s.bin";

			using (var fs = new FileStream(fileName, FileMode.Create))
			{
				formatter.Serialize(fs, item);
			}
		}
	}
}
