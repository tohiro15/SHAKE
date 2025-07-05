using System.IO;

namespace ES3Internal
{
	public class ES3FileStream : FileStream
	{
		private bool isDisposed;

		public ES3FileStream(string path, ES3FileMode fileMode, int bufferSize, bool useAsync)
			: base(GetPath(path, fileMode), GetFileMode(fileMode), GetFileAccess(fileMode), FileShare.None, bufferSize, useAsync)
		{
		}

		protected static string GetPath(string path, ES3FileMode fileMode)
		{
			string directoryPath = ES3IO.GetDirectoryPath(path);
			if (fileMode != 0 && directoryPath != ES3IO.persistentDataPath)
			{
				ES3IO.CreateDirectory(directoryPath);
			}
			if (fileMode == ES3FileMode.Write)
			{
				switch (fileMode)
				{
				case ES3FileMode.Append:
					break;
				default:
					return path;
				case ES3FileMode.Write:
					return path + ".tmp";
				}
			}
			return path;
		}

		protected static FileMode GetFileMode(ES3FileMode fileMode)
		{
			switch (fileMode)
			{
			case ES3FileMode.Read:
				return FileMode.Open;
			case ES3FileMode.Write:
				return FileMode.Create;
			default:
				return FileMode.Append;
			}
		}

		protected static FileAccess GetFileAccess(ES3FileMode fileMode)
		{
			if (fileMode == ES3FileMode.Read)
			{
				return FileAccess.Read;
			}
			return FileAccess.Write;
		}

		protected override void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				isDisposed = true;
				base.Dispose(disposing);
			}
		}
	}
}
