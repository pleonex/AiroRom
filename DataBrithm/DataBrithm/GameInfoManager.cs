//
//  GameInfoManager.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DataBrithm
{
	public class GameInfoManager
	{
		static readonly string FileDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		static GameInfoManager CurrentInstance;
		readonly Dictionary<Device, IEnumerable<XElement>> gameDb = 
			new Dictionary<Device, IEnumerable<XElement>>();

		GameInfoManager()
		{
			gameDb.Add(Device.NintendoDS, ParseXml("ADVANsCEne_NDS.xml"));
			gameDb.Add(Device.PSP, ParseXml("ADVANsCEne_PSP.xml"));
		}

		public static GameInfoManager Instance {
			get {
				if (CurrentInstance == null)
					CurrentInstance = new GameInfoManager();

				return CurrentInstance;
			}
		}

		public GameInfo GetGameInfo(Device dev, int releaseNum)
		{
			XElement xinfo = gameDb[dev]
				.FirstOrDefault(n => n.Element("releaseNumber").Value == releaseNum.ToString());
			if (xinfo == null)
				return null;

			return GameInfo.FromXml(dev, xinfo);
		}

		IEnumerable<XElement> ParseXml(string filename)
		{
			string filePath = System.IO.Path.Combine(FileDir, filename);
			XDocument doc = XDocument.Load(filePath);
			return doc.Root.Element("games").Elements();
		}
	}
}

