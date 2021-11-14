using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Functions {
	public static class Methods {

		public static bool CheckLinerIncrease(bool checkUnsigned, params float[] list) {
			if(checkUnsigned && list.Any(i => i < 0)) {
				return false;
			}
			return list.SequenceEqual(list.OrderBy(i => i).ToArray());
		}

	}
}
