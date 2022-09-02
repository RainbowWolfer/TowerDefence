using System;

namespace TowerDefence.Local {
	[Serializable]
	public class Cards {
		public CardInfo emplacementsCooldown;
		public CardInfo towerDamage;
		public CardInfo upgradePrice;
		public CardInfo buyPrice;
		public CardInfo sellPrice;
		public CardInfo shovelPrice;
		public CardInfo shovelTools;
		public CardInfo passiveCash;
		public CardInfo cashEarnedMultiplier;
		public CardInfo diamondChances;
		public CardInfo diamondAmount;
		public CardInfo diamondUpgradePrice;
		public CardInfo cardsEarnedChances;
		public CardInfo cardsEarnedAmount;
	}

	[Serializable]
	public class CardInfo {
		public int currentLevel;
		public int cardsCount;
	}
}
