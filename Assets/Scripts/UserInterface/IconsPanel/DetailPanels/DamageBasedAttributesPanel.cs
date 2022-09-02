using TMPro;
using TowerDefence.Data;
using TowerDefence.Functions;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class DamageBasedAttributesPanel: MonoBehaviour {
		public TowerInfo Info { get; private set; }

		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private GridsData damage;
		[SerializeField]
		private GridsData radius;
		[SerializeField]
		private GridsData fireRate;
		[SerializeField]
		private TextMeshProUGUI powers;
		[SerializeField]
		private TextMeshProUGUI prices;
		[SerializeField]
		private TextMeshProUGUI size;
		[SerializeField]
		private TextMeshProUGUI description;
		[SerializeField]
		private Image specialBorder;
		[SerializeField]
		private TextMeshProUGUI specialTitle;
		[SerializeField]
		private Image specialIcon;

		private void Update() {
			if(Info != null) {
				UpdateCashAndPowersColor(Info);
			}
		}

		private void UpdateCashAndPowersColor(TowerInfo info) {
			const string green = "green";
			const string red = "red";

			//cash
			bool cashSufficient = Level.Cash >= Info.price;
			bool cashUpgradedSufficient = Level.Cash >= Info.upgradePrice;

			string cashString = $"<color=\"{(cashSufficient ? green : red)}\">{info.price}</color>";
			string cashUpgradedString = $"<color=\"{(cashUpgradedSufficient ? green : red)}\">{info.upgradePrice}</color>";

			prices.text = $"{cashString}/{cashUpgradedString}";

			//powers
			bool powerSufficient = Info.powers > 0 || Level.MaxPowers - Level.CurrentPowers >= -Info.powers;
			bool powerUpgradedSufficient = info.powers > 0 || Level.MaxPowers - Level.CurrentPowers >= -Info.upgradedPowers;

			string powersSign = Info.powers > 0 ? "+" : "";

			string powersString = $"<color=\"{(powerSufficient ? green : red)}\">{powersSign}{info.powers}</color>";
			string powersUpgradedString = $"<color=\"{(powerUpgradedSufficient ? green : red)}\">{powersSign}{info.upgradedPowers}</color>";

			powers.text = $"{powersString}/{powersUpgradedString}";
		}

		public void Set(TowerInfo info) {
			Info = info;

			title.text = info.TowerName;

			damage.Set(info.damageData);
			radius.Set(info.radiusData);
			fireRate.Set(info.fireRateData);

			UpdateCashAndPowersColor(Info);
			size.text = $"{info.size.x}x{info.size.y}";

			description.text = info.description;

			if(info.specials != null) {
				specialIcon.sprite = info.specials.icon;
				specialIcon.SetNativeSizeByHeight(40f);
				specialTitle.text = info.specials.name;
				specialBorder.color = info.specials.tier switch {
					SpecialsData.Tier.Rare => new Color(0.212f, 0.79f, 1),
					_ => Color.white,
				};
			}
		}
	}
}
