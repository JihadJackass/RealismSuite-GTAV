using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace BusinessV.Core
{
	internal static class JsonFile
	{
		private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer
		{
			MaxJsonLength = 4194304,
			RecursionLimit = 64
		};

		public static T Read<T>(string path)
		{
			return Serializer.Deserialize<T>(File.ReadAllText(path, Encoding.UTF8));
		}

		public static T Deserialize<T>(string json)
		{
			return Serializer.Deserialize<T>(json);
		}

		public static string Serialize<T>(T value)
		{
			return Serializer.Serialize(value);
		}

		public static void WriteNew(string path, string json)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(false)))
				{
					streamWriter.Write(json);
				}
			}
		}
	}
}
