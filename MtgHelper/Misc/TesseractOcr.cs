using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace MtgHelper.Misc
{
	public class TesseractOcr
	{
		private string commandpath;
		private string outpath;
		private string tmppath;

		public TesseractOcr( string commandpath )
		{
			this.commandpath = commandpath;
			tmppath = System.Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\out.tif";
			outpath = System.Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\out.txt";
		}
		public string analyze( string filename )
		{
			var args = filename + " " + outpath.Replace( ".txt", "" ) + " -l mtg -psm 7";
			ProcessStartInfo startinfo = new ProcessStartInfo( commandpath, args );
			startinfo.CreateNoWindow = true;
			startinfo.UseShellExecute = false;
			Process.Start( startinfo ).WaitForExit();
			string ret = "";
			using( StreamReader r = new StreamReader( outpath ) )
			{
				string content = r.ReadToEnd();
				ret = content;
			}
			File.Delete( outpath );
			return ret;
		}
		public string OCRFromBitmap( Bitmap bmp )
		{
			bmp.Save( tmppath, System.Drawing.Imaging.ImageFormat.Png );
			string ret = analyze( tmppath );
			File.Delete( tmppath );
			return ret;
		}
		public string OCRFromFile( string filename )
		{
			return analyze( filename );
		}
	}
}