using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TowerDefence.Placements;

namespace TowerDefence.UserInterface {
	public class DetailPanel: PlacementPanel {
		//public override Placement CurrentPlacement { get => CurrentFieldPlacement; }
		public FieldPlacement CurrentFieldPlacement { get; private set; }

		public bool IsUpgraded => CurrentFieldPlacement.IsUpgraded;

		public bool UpgradeAvailiable => Level.Cash >= CurrentFieldPlacement.info.upgradePrice;

		[SerializeField]
		private bool showDescription;
		[SerializeField]
		private bool showSpecialAbility;

		public bool ShowDescription {
			get => showDescription;
			set {
				showDescription = value;
				UpdateLayout();
			}
		}

		public bool ShowSpecialAbility {
			get => showSpecialAbility;
			set {
				showSpecialAbility = value;
				UpdateLayout();
			}
		}

		private const float DESCRIPTION_FIXED_WIDTH = 180f;

		public override float Width {
			get {
				if(ShowSpecialAbility && !ShowDescription) {
					return 590;
				} else if(ShowSpecialAbility && ShowDescription) {
					return 775;
				} else {
					return 440;
				}
			}
		}

		[Space]
		[SerializeField]
		private RectTransform titleMask;
		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private TextMeshProUGUI killCountText;
		[SerializeField]
		private Image star1, star2, star3;
		[SerializeField]
		private Image expFiller;

		[Space]
		[SerializeField]
		private PointEventHandler upgradeIcon;
		[SerializeField]
		private Image upgradeIconImage;
		[SerializeField]
		private Outline upgradeIcon_outline;
		[SerializeField]
		private TextMeshProUGUI upgradePriceText;

		[Space]
		[SerializeField]
		private PointEventHandler sellIcon;
		[SerializeField]
		private Outline sellIcon_outline;
		[SerializeField]
		private TextMeshProUGUI sellPriceText;

		[Space]
		[SerializeField]
		private GameObject abilityObject;
		[SerializeField]
		private PointEventHandler abilityIcon;
		[SerializeField]
		private Outline abilityIcon_outline;
		[SerializeField]
		private Image abilityFiller;
		[SerializeField]
		private Image abilityFiller_outline;

		[Space]
		[SerializeField]
		private RectTransform description;
		//[SerializeField]
		//private TextMeshProUGUI descriptionText;

		[Space]
		[SerializeField]
		private GameObject showMoreObject;
		[SerializeField]
		private PointEventHandler showMoreButton;
		[SerializeField]
		private Outline showMoreButton_outline;

		[Space]
		[SerializeField]
		private GridsData damageData;
		[SerializeField]
		private GridsData radiusData;
		[SerializeField]
		private GridsData fireRateData;

		protected override void Start() {
			base.Start();
			upgradeIcon.MouseEnter += s => upgradeIcon_outline.enabled = true;
			upgradeIcon.MouseExit += s => upgradeIcon_outline.enabled = false;

			sellIcon.MouseEnter += s => sellIcon_outline.enabled = true;
			sellIcon.MouseExit += s => sellIcon_outline.enabled = false;

			abilityIcon.MouseEnter += s => abilityIcon_outline.enabled = true;//&& is cd ready
			abilityIcon.MouseExit += s => abilityIcon_outline.enabled = false;

			showMoreButton.MouseEnter += s => showMoreButton_outline.enabled = true;
			showMoreButton.MouseExit += s => showMoreButton_outline.enabled = false;
			showMoreButton.MouseUp += s => ShowDescription = !ShowDescription;

			description.sizeDelta = new Vector2(ShowDescription ? DESCRIPTION_FIXED_WIDTH : 0, description.sizeDelta.y);

			UpdatePrice();

			UpdateKillCount();
			UpdateExp();
			abilityFiller.fillAmount = 0;
		}

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			if(placement is Tower t) {
				CurrentFieldPlacement = t;
				ShowSpecialAbility = false;
				ShowDescription = false;
			} else if(placement is Emplacement e) {
				CurrentFieldPlacement = e;
				ShowSpecialAbility = true;
				ShowDescription = false;
				abilityIcon.MouseUp += s => {
					if(e.Ability?.StatusType == AbilityStatusType.Ready && e.Ability?.IsFiring == false) {
						Ability(e);
					}
				};
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}

			UpdateTitle();

			upgradeIcon.MouseUp += s => this.Upgrade();
			sellIcon.MouseUp += s => CurrentFieldPlacement.Sell();
		}

		public void UpdateTitle() {
			title.text = $"{CurrentFieldPlacement.info.TowerName.Replace(' ', '_')} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
		}

		public void UpdateKillCount() {
			killCountText.text = $"{CurrentFieldPlacement.Kills}";
		}

		public void UpdateExp() {
			expFiller.fillAmount = CurrentFieldPlacement.ExpPercentage;
			bool[] stars = CurrentFieldPlacement.Star switch {
				Star.None => new bool[] { false, false, false },
				Star.Star1 => new bool[] { true, false, false },
				Star.Star2 => new bool[] { true, true, false },
				Star.Star3 => new bool[] { true, true, true },
				_ => new bool[] { true, true, true },
			};
			star1.gameObject.SetActive(stars[0]);
			star2.gameObject.SetActive(stars[1]);
			star3.gameObject.SetActive(stars[2]);
		}

		private void UpdateLayout() {
			titleMask.sizeDelta = new Vector2(ShowSpecialAbility ? 505f : 380f, titleMask.sizeDelta.y);
		}

		private void UpdateAbilityCooldown() {
			if(CurrentFieldPlacement is Emplacement em && em.Ability != null) {
				abilityFiller.fillAmount = em.Ability?.CooldownPercentage ?? 0;
			}
		}

		private void Upgrade() {
			if(!UpgradeAvailiable) {
				return;
			}
			CurrentFieldPlacement.Upgrade();
			UpdatePrice();
		}

		private void UpdatePrice() {
			upgradePriceText.text = IsUpgraded ? "----" : ("$" + CurrentFieldPlacement.info.upgradePrice);
			sellPriceText.text = "$" + (IsUpgraded ?
				CurrentFieldPlacement.info.UpgradedSellPrice :
				CurrentFieldPlacement.info.SellPrice
			);
		}

		private void Ability(Emplacement e) {
			manager.RequestAbility(e);
		}

		private float cv1;
		//00A9FF - blue for upgraded
		protected override void Update() {
			base.Update();
			UpdateLayout();
			description.sizeDelta = new Vector2(
				Mathf.SmoothDamp(
					description.sizeDelta.x,
					showDescription ? DESCRIPTION_FIXED_WIDTH : 0,
					ref cv1,
					0.1f
				)
			, description.sizeDelta.y);

			abilityObject.SetActive(showSpecialAbility);
			showMoreObject.SetActive(showSpecialAbility);

			upgradePriceText.color = UpgradeAvailiable ? Color.white : Color.red;
			UpdateKillCount();
			UpdateExp();
			UpdateAbilityCooldown();

			if(CurrentFieldPlacement is Emplacement e) {
				abilityIcon_outline.effectColor = e.Ability?.StatusType != AbilityStatusType.Ready ? Color.red : new Color(0.6f, 0.6f, 0.6f, 0.5f);
				abilityFiller_outline.color = e.Ability?.StatusType != AbilityStatusType.Ready ? Color.red : Color.white;
			}

			if(IsUpgraded) {
				upgradeIconImage.color = new Color(0, 0.6667f, 1);
				upgradeIcon_outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
				upgradePriceText.color = Color.white;
			} else {
				upgradeIconImage.color = Color.white;
				if(UpgradeAvailiable) {
					upgradeIcon_outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
					upgradePriceText.color = Color.white;
				} else {
					upgradeIcon_outline.effectColor = Color.red;
					upgradePriceText.color = Color.red;
				}
			}
		}
	}
}
