﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence.Data {
	[Serializable]
	public struct StarValues<T> {
		public T none;
		public T one;
		public T two;
		public T three;
	}
}