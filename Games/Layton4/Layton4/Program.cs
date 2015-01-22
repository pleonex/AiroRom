//
//  Program.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using Libgame;

namespace Layton4
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
				return;

			string executableDir = args[0];
			var root = GameFolderFactory.FromPath(executableDir);
			FileManager.Initialize(root, new FileInfoCollection());

			var container = root.Files["lt4_main_sp.fa"] as GameFile;
			if (container == null)
				return;

			container.SetFormat<Gfsa>();
			container.Format.Read();
			foreach (var block in container.Files.Cast<GameFile>())
				block.Stream.WriteTo(Path.Combine(executableDir, block.Name));
		}
	}
}
