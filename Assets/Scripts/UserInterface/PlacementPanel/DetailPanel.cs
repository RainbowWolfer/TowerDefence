using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TowerDefence.Placements;
using TowerDefence.Data;

namespace TowerDefence.UserInterface {
	public class DetailPanel: PlacementPanel {
		public static bool DescriptionShown { get; private set; }

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
				} else if(ShowDescription) {
					return 660;
				} else {
					return 450;
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
		private IconButton upgradeButton;
		[SerializeField]
		private IconButton sellButton;
		[SerializeField]
		private IconButton abilityButton;

		[Space]
		[SerializeField]
		private GameObject abilityObject;
		[SerializeField]
		private Image abilityFiller;
		[SerializeField]
		private Image abilityFiller_outline;

		[Space]
		[SerializeField]
		private RectTransform description;

		[Space]
		[SerializeField]
		private GameObject showMoreObject;
		[SerializeField]
		private PointEventHandler showMoreButton;
		[SerializeField]
		private Outline showMoreButton_outline;

		[Space]
		[SerializeField]
		private TextMeshProUGUI attackTypeText;
		[SerializeField]
		private GridsData damageData;
		[SerializeField]
		private GridsData radiusData;
		[SerializeField]
		private GridsData fireRateData;

		protected override void Start() {
			base.Start();

			showMoreButton.MouseEnter += s => showMoreButton_outline.enabled = true;
			showMoreButton.MouseExit += s => showMoreButton_outline.enabled = false;
			showMoreButton.MouseUp += s => {
				ShowDescription = !ShowDescription;
				DescriptionShown = ShowDescription;
			};

			description.sizeDelta = new Vector2(ShowDescription ? DESCRIPTION_FIXED_WIDTH : 0, description.sizeDelta.y);


			UpdateKillCount();
			UpdateExp();
			abilityFiller.fillAmount = 0;
		}

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			if(placement is Tower t) {
				CurrentFieldPlacement = t;
				ShowSpecialAbility = false;
				ShowDescription = DescriptionShown;
			} else if(placement is Emplacement e) {
				CurrentFieldPlacement = e;
				ShowSpecialAbility = true;
				ShowDescription = DescriptionShown;
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}

			UpdateTitle();
			UpdateAttributes();
			UpdateAttackType();

			upgradeButton.OnClick = b => Upgrade();
			upgradeButton.ExternalUpdate = (icon, outline, text) => {
				if(IsUpgraded) {
					text.text = "----";
					text.color = Color.white;
					icon.color = new Color(0, 0.6667f, 1);
					outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
				} else {
					text.text = "$" + CurrentFieldPlacement.info.upgradePrice;
					if(UpgradeAvailiable) {
						text.color = Color.white;
						outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
					} else {
						text.color = Color.red;
						outline.effectColor = Color.red;
					}
				}
			};

			sellButton.OnClick = b => {
				CurrentFieldPlacement.Sell();
			};
			sellButton.ExternalUpdate = (icon, outline, text) => {
				text.text = "$" + (IsUpgraded ?
					CurrentFieldPlacement.info.UpgradedSellPrice :
					CurrentFieldPlacement.info.SellPrice
				);

			};

			abilityButton.OnClick = b => {
				if(CurrentFieldPlacement is Emplacement e) {
					if(e.Ability?.StatusType == AbilityStatusType.Ready
						&& e.Ability?.IsFiring == false) {
						Ability(e);
					}
				}
			};
		}

		public void UpdateAttackType() {
			attackTypeText.text = CurrentFieldPlacement.info.attackType switch {
				TowerInfo.AttackType.Single => "Single",
				TowerInfo.AttackType.Penetrate => "Penetrate",
				TowerInfo.AttackType.Area => "Area",
				_ => "Null",
			};
		}

		public void UpdateAttributes() {
			damageData.Set(CurrentFieldPlacement.info.damageData);
			radiusData.Set(CurrentFieldPlacement.info.radiusData);
			fireRateData.Set(CurrentFieldPlacement.info.fireRateData);
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
		}

		private void Ability(Emplacement e) {
			manager.RequestAbility(e);
		}

		private float cv1;
		//00A9FF - blue for upgraded
		protected override void Update() {
			base.Update();
			UpdateLayout();

			description.anchoredPosition = new Vector2(ShowSpecialAbility ? 525 : 410, 0);
			description.sizeDelta = new Vector2(
				Mathf.SmoothDamp(
					description.sizeDelta.x,
					showDescription ? DESCRIPTION_FIXED_WIDTH : 0,
					ref cv1,
					0.1f
				)
			, description.sizeDelta.y);

			abilityObject.SetActive(showSpecialAbility);
			showMoreObject.SetActive(true);

			UpdateKillCount();
			UpdateExp();
			UpdateAbilityCooldown();

			if(CurrentFieldPlacement is Emplacement e) {
				abilityButton.Outline.effectColor = e.Ability?.StatusType != AbilityStatusType.Ready ? Color.red : new Color(0.6f, 0.6f, 0.6f, 0.5f);
				abilityFiller_outline.color = e.Ability?.StatusType != AbilityStatusType.Ready ? Color.red : Color.white;
			}
		}
	}
}
