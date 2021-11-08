using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence.Towers {
	public interface IFieldPlacement {
		public bool IsUpgraded { get; set; }
		public abstract void Upgrade();

		
	}
}
