//
//  GameInfo.cs
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
using System.Xml.Linq;

namespace DataBrithm
{
	[Flags]
	public enum GameRegion : byte {

	}

	[Flags]
	public enum GameLanguage : byte {

	}

	public class GameInfo
	{
		GameInfo()
		{
		}

		public Device Device    { get; private set; }
		public int    ReleaseId { get; private set; }
		public string Title   { get; private set; }
		public string Company { get; private set; }
		public long   Size    { get; private set; }
		public GameRegion   Region   { get; private set; }
		public GameLanguage Language { get; private set; }
		public string SaveType  { get; private set; }

		public static GameInfo FromXml(Device dev, XElement xentry)
		{
			GameInfo info = new GameInfo();
			info.Device = dev;

			info.ReleaseId = Convert.ToInt32(xentry.Element("releaseNumber").Value);
			info.Title   = xentry.Element("title").Value;
			info.Company = xentry.Element("publisher").Value;
			info.Size    = Convert.ToInt64(xentry.Element("romSize").Value);
			info.Region   = (GameRegion)Convert.ToInt32(xentry.Element("location").Value);
			info.Language = (GameLanguage)Convert.ToInt32(xentry.Element("language").Value);
			info.SaveType = xentry.Element("saveType").Value;

			return info;
		}
	}
}

