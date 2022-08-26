using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data.Instance;
using TowerDefence.Enemies.Instances;
using TowerDefence.Local;
using TowerDefence.Scripts.Data;
using UnityEngine;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_UserPanel: MonoBehaviour {
		[SerializeField]
		private AboutDetailButton rank;
		[SerializeField]
		private AboutDetailButton username;
		[SerializeField]
		private AboutDetailButton diamond;


		[Header("Cards")]
		public BenefitCard emplacementCoolDown;
		public BenefitCard towerDamage;
		public BenefitCard upgradePrice;
		public BenefitCard buyPrice;

		public BenefitCard sellPrice;
		public BenefitCard shovelPrice;
		public BenefitCard shovelTools;
		public BenefitCard passiveCash;

		public BenefitCard cashEarnedMultiplier;
		public BenefitCard diamondChances;
		public BenefitCard diamondAmount;
		public BenefitCard diamondUpgradePrice;

		public BenefitCard[] AllCards { get; private set; }

		public CardsBenefits Datas => GameData.Instance.Cards;
		public Cards PlayerCards => Player.Current.cards;

		private void Awake() {
			AllCards = new BenefitCard[] {
				emplacementCoolDown,
				towerDamage,
				upgradePrice,
				buyPrice,
				sellPrice,
				shovelPrice,
				shovelTools,
				passiveCash,
				cashEarnedMultiplier,
				diamondChances,
				diamondAmount,
				diamondUpgradePrice,
			};

			foreach(BenefitCard item in AllCards) {
				item.others = AllCards.Where(c => c != item).ToArray();
			}
		}

		private void Start() {
			emplacementCoolDown.SetTitle("Ability CD");
			towerDamage.SetTitle("Tower Damage");
			upgradePrice.SetTitle("Upgrade Price");
			buyPrice.SetTitle("Buy Price");
			sellPrice.SetTitle("Sell Price");
			shovelPrice.SetTitle("Shovel Price");
			shovelTools.SetTitle("Shovel Tools");
			passiveCash.SetTitle("Passive Cash");
			cashEarnedMultiplier.SetTitle("Cash Multiplier");
			diamondChances.SetTitle("Diamond Chances");
			diamondAmount.SetTitle("Diamond Amount");
			diamondUpgradePrice.SetTitle("Card Price");

			emplacementCoolDown.Data = Datas.emplacementCooldown;
		}

		private void Update() {
			rank.SetText(Player.Current.rank);
			username.SetText(Player.Current.username);
			diamond.SetText(Player.Current.diamond);

			emplacementCoolDown.CardInfo = PlayerCards.emplacementsCooldown;
			towerDamage.CardInfo = PlayerCards.towerDamage;
			upgradePrice.CardInfo = PlayerCards.upgradePrice;
			buyPrice.CardInfo = PlayerCards.buyPrice;
			sellPrice.CardInfo = PlayerCards.sellPrice;
			shovelPrice.CardInfo = PlayerCards.shovelPrice;
			shovelTools.CardInfo = PlayerCards.shovelTools;
			passiveCash.CardInfo = PlayerCards.passiveCash;
			cashEarnedMultiplier.CardInfo = PlayerCards.cashEarnedMultiplier;
			diamondChances.CardInfo = PlayerCards.diamondChances;
			diamondAmount.CardInfo = PlayerCards.diamondAmount;
			diamondUpgradePrice.CardInfo = PlayerCards.diamondUpgradePrice;

		}

	}
}
