using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence {
	public static class Local {
		private static string path;
		private static string MapInfoPath => $"{path}/maps/";
		static Local() {
			path = Application.persistentDataPath;
			Debug.Log(path);
		}

		public static void SaveMapInfo(MapInfo mapInfo) {
			if(!Directory.Exists(MapInfoPath)) {
				Directory.CreateDirectory(MapInfoPath);
			}
			using var stream = new FileStream(MapInfoPath + $"{mapInfo.name}.map", FileMode.OpenOrCreate);
			new BinaryFormatter().Serialize(stream, mapInfo);
		}

		public static MapInfo LoadMapInfo(string name) {
			string filePath = MapInfoPath + $"{name}.map";
			if(!File.Exists(filePath)) {
				return null;
			}
			using var stream = new FileStream(filePath, FileMode.Open);
			var map = new BinaryFormatter().Deserialize(stream) as MapInfo;
			return map;
		}

		public static List<MapInfo> LoadAllMapInfos() {
			var result = new List<MapInfo>();
			foreach(string item in Directory.GetFiles(MapInfoPath)) {
				using var stream = new FileStream(item, FileMode.Open);
				result.Add(new BinaryFormatter().Deserialize(stream) as MapInfo);
			}
			return result;
		}
	}
}
