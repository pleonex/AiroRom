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
using Libgame;

namespace BinMesG
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var xconf = System.Xml.Linq.XDocument.Load(
				"/home/benito/Dropbox/Ninokuni español/NinoPatcher/Ninokuni español.xml");
			Configuration.Initialize(xconf);

			var root = GameFolderFactory.FromPath("/home/benito/workdir");

			string filename = "n195_jp15_dorayaki_s";
			var file = root.Files[filename + ".bmg"] as GameFile;
			file.SetFormat<Bmg>();
			file.Format.Read();
			System.IO.File.Delete("/home/benito/workdir/" + filename + ".xml");
			file.Format.Export("/home/benito/workdir/" + filename + ".xml");
		}
	}
}
