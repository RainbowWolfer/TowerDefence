using System;
using UnityEngine;

namespace TowerDefence.Scripts.Data {

	[CreateAssetMenu(fileName = "Cards Benefits", menuName = "Data/Cards Benefits")]
	[Serializable]
	public class CardsBenefits: ScriptableObject {
		public CardData emplacementCooldown;
		public CardData towerDamage;
		public CardData upgradePrice;
		public CardData buyPrice;
		public CardData sellPrice;
		public CardData shovelPrice;
		public CardData shovelTools;
		public CardData passiveCash;
		public CardData cashEarnedMultiplier;
		public CardData diamondChances;
		public CardData diamondAmount;
		public CardData diamondUpgradePrice;
		public CardData cardsEarnedChances;
		public CardData cardsEarnedAmount;


		[Serializable]
		public class CardData {
			public CurveType type;

			[Header("Benefits")]
			public float startBenefit;
			public float slopeBenefit;
			public int maxLevel;
			public float EdgeValue => startBenefit + slopeBenefit * maxLevel;

			[Header("Diamond")]
			public int startDiamondCost;
			public int slopeDiamondCost;
			public float exponentDiamondCost;

			[Header("Cards")]
			public int startCardsCount;
			public int slopeCardsCount;


			[Space]
			[TextArea]
			public string description;

			[TextArea]
			public string[] descriptions;

			public int GetDiamondCost(int level) {
				level = Mathf.Clamp(level, 0, maxLevel);
				return startDiamondCost + (int)Mathf.Pow(slopeDiamondCost * level, exponentDiamondCost);
			}

			public int GetCardsCount(int level) {
				level = Mathf.Clamp(level, 0, maxLevel);
				return startCardsCount + slopeCardsCount * level;
			}

			public float GetBenefit(int level) {
				level = Mathf.Clamp(level, 0, maxLevel);
				return startBenefit + slopeBenefit * level;
			}
		}

		public enum CurveType {
			Percentage, Fixed, Number
		}
	}
}
