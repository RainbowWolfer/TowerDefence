using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Scripts.Data {

	[CreateAssetMenu(fileName = "Cards Benefits", menuName = "Data/Cards Benefits")]
	[Serializable]
	public class CardsBenefits: ScriptableObject {
		/**
			emplacementsCooldown;
			towerDamage;
			upgradePrice;
			buyPrice;
			sellPrice;
			shovelPrice;
			shovelTools;
			passiveCash;
			cashEarnedMultiplier;
			diamondChances;
			diamondAmount;
			diamondUpgradePrice;
		*/

		public CardData emplacementCooldown;
		public CardData shovelTools;


		[Serializable]
		public class CardData {
			public CurveType type;

			[Header("Benefits")]
			public float startValue;
			public float slopeEachLevel;
			public int maxLevel;
			public float EdgeValue => startValue + slopeEachLevel * maxLevel;

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
				return startValue + slopeEachLevel * level;
			}
		}

		public enum CurveType {
			Percentage, Fixed
		}
	}
}
