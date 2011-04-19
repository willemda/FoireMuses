using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace FoireMuses.Core.Utils
{
	public class TemporaryFile : IDisposable
	{


		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(TemporaryFile));


		private static readonly char[] theInvalidChars = System.IO.Path.GetInvalidFileNameChars();



		private bool isDisposed;

		private bool mustKeep;

		private string thePath;



		public TemporaryFile()


			: this(false)
		{


		}



		public TemporaryFile(bool shortLived)



			: this(null, shortLived)
		{


		}



		public TemporaryFile(string extension, bool shortLived)
		{



			thePath = CreateTemporaryFile(extension, shortLived);


		}



		public string Path
		{



			get { return thePath; }

		}


		public bool Keep
		{



			get { return mustKeep; }


			set { mustKeep = value; }


		}


		~TemporaryFile()
		{



			Dispose(false);


		}


		public void Dispose()
		{



			Dispose(false);



			GC.SuppressFinalize(this);


		}


		protected virtual void Dispose(bool disposing)
		{



			if (!isDisposed)
			{




				isDisposed = true;



				if (!mustKeep)
				{





					TryDelete();




				}



			}


		}



		private void TryDelete()
		{



			try
			{




				if (!String.IsNullOrEmpty(thePath))
				{





					File.Delete(thePath);





					theLogger.DebugFormat("File '{0}' Deleted", thePath);



				}



			}



			catch (IOException ioex)
			{




				theLogger.Warn(String.Format("Error while deleting temporary file '{0}'", thePath), ioex);


			}



			catch (UnauthorizedAccessException uaex)
			{




				theLogger.Warn(String.Format("Error while deleting temporary file '{0}'", thePath), uaex);


			}


		}



		private static string CreateTemporaryFile(string ext, bool shortLived)
		{



			string tempPath = @"G:\Temp\";//ConfigurationManager.AppSettings["tempPath"];
            //string tempPath = @"C:\Projects\Temp\";


			if (String.IsNullOrEmpty(tempPath))
			{




				tempPath = System.IO.Path.GetTempPath();



			}



			if (!Directory.Exists(tempPath))
			{




				Directory.CreateDirectory(tempPath);
			}

			string tempFileName = System.IO.Path.Combine(tempPath, System.IO.Path.GetRandomFileName());

			if (!String.IsNullOrEmpty(ext) && IsValidExtension(ext))
			{
				tempFileName = System.IO.Path.ChangeExtension(tempFileName, ext);
			}
			//Create the temp File 
			File.Create(tempFileName).Close();
			if (shortLived)
			{
				// Set the temporary attribute, meaning the file will live in memory and will not be written to disk  

				File.SetAttributes(tempFileName, File.GetAttributes(tempFileName) | FileAttributes.Temporary);

			}
			theLogger.DebugFormat("Temporary file '{0}' created", tempFileName);
			return tempFileName;
		}

		private static bool IsValidExtension(string ext)
		{
			for (int i = 0; i < ext.Length; i++)
			{
				if (theInvalidChars.Contains(ext[i]))
					return false;
			}
			return true;

		}

	}
}