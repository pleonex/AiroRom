//
//  GfsaBlock.cs
//
//  Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Mono.Addins;
using Libgame;
using Libgame.IO;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace Layton4
{
	[Extension]
	public class GfsaBlock : Format
	{
		static readonly string ExecutablePath = Path.GetDirectoryName(
			Assembly.GetExecutingAssembly().Location);

		static readonly Dictionary<uint, Tuple<byte, string>> Encoders = new Dictionary<uint, Tuple<byte, string>> {
			{ 0, Tuple.Create((byte)0x00, "") },
			{ 1, Tuple.Create((byte)0x10, "lzss.exe") },
			{ 2, Tuple.Create((byte)0x24, "huffman.exe") },
			{ 3, Tuple.Create((byte)0x28, "huffman.exe") },
			{ 4, Tuple.Create((byte)0x30, "rle.exe") }
		};

		public GfsaBlock()
		{
		}

		public override string FormatName {
			get { return "Layton4.GfsaBlock"; }
		}

		public override void Read(DataStream strIn)
		{
			uint header = new DataReader(strIn).ReadUInt32();
			uint encoder = header & 0x3;
			uint size = header >> 3;

			if (encoder == 0) {
				File.AddFile(new GameFile(File.Name + ".dec", new DataStream(strIn, 4, strIn.Length)));
				return;
			}

			// Export file to decode
			string tempFile = Path.Combine(ExecutablePath, File.Name);
			var tempStream = new DataStream(new MemoryStream(), 0, 0);
			strIn.WriteTo(tempStream);
			tempStream.Seek(0, SeekMode.Origin);

			// Set header
			uint stdHeader = (Encoders[encoder].Item1) | (size << 8);
			tempStream.Write(BitConverter.GetBytes(stdHeader), 0, 4);

			// Export
			tempStream.WriteTo(tempFile);
			tempStream.Dispose();

			// Decode
			ExecuteProgram(Encoders[encoder].Item2, "-d \"" + tempFile + "\"");

			// Read data
			var memStream = new MemoryStream();
			using (var fs = new FileStream(tempFile, FileMode.Open))
				fs.CopyTo(memStream);

			// Remove file
			System.IO.File.Delete(tempFile);

			// Create subfile
			var subStream = new DataStream(memStream, 0, memStream.Length);
			var subFile = new GameFile(File.Name + ".dec", subStream);
			File.AddFile(subFile);
		}

		void ExecuteProgram(string path, string args)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix) {
				args = string.Format("\"{0}\" {1}", path, args);
				path = "wine";
			}

			var startInfo = new ProcessStartInfo();
			startInfo.FileName  = path;
			startInfo.Arguments = args;
			startInfo.UseShellExecute = false;
			startInfo.CreateNoWindow  = true;
			startInfo.ErrorDialog = false;

			var program = new Process();
			program.StartInfo = startInfo;
			program.Start();
			program.WaitForExit();
			program.Dispose();
		}

		public override void Write(DataStream strOut)
		{
			throw new NotImplementedException();
		}

		public override void Import(params DataStream[] strIn)
		{
			throw new NotImplementedException();
		}

		public override void Export(params DataStream[] strOut)
		{
			throw new NotImplementedException();
		}

		protected override void Dispose(bool freeManagedResourcesAlso)
		{
		}
	}
}

