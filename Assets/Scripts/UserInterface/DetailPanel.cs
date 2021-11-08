﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TowerDefence.Towers;

namespace TowerDefence.UserInterface {
	public class DetailPanel: PlacementPanel {
		private Tower tower;
		private Emplacement emplacement;

		[SerializeField] private bool showDescription;
		[SerializeField] private bool showSpecialAbility;


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

		private const float DESCRIPTIONFIXEDWIDTH = 180f;
		public override Placement Placement => tower == null ? (Placement)emplacement : (Placement)tower;

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

		[SerializeField]
		private PointEventHandler upgradeIcon;
		[SerializeField]
		private Outline upgradeIcon_outline;
		[SerializeField]
		private TextMeshProUGUI upgradePriceText;

		[SerializeField]
		private PointEventHandler sellIcon;
		[SerializeField]
		private Outline sellIcon_outline;
		[SerializeField]
		private TextMeshProUGUI sellPriceText;

		[SerializeField]
		private GameObject abilityObject;
		[SerializeField]
		private PointEventHandler abilityIcon;
		[SerializeField]
		private Outline abilityIcon_outline;
		[SerializeField]
		private Image abilityFiller;

		[SerializeField]
		private RectTransform description;
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private GameObject showMoreObject;
		[SerializeField]
		private PointEventHandler showMoreButton;
		[SerializeField]
		private Outline showMoreButton_outline;

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

			description.sizeDelta = new Vector2(ShowDescription ? DESCRIPTIONFIXEDWIDTH : 0, description.sizeDelta.y);
		}

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			if(placement is Tower t) {
				this.tower = t;
				upgradeIcon.MouseUp += s => t.Upgrade();
				sellIcon.MouseUp += s => t.Sell();
				abilityIcon.MouseUp += s => t.Ability();
			} else if(placement is Emplacement e) {
				this.emplacement = e;
				ShowSpecialAbility = true;
				//upgradeIcon.MouseUp += s => e.Upgrade();
				sellIcon.MouseUp += s => e.Sell();
				abilityIcon.MouseUp += s => e.Abibity();
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}
		}

		public void UpdateKillCount(int count) => killCountText.text = $"{count}K";

		private void UpdateLayout() {
			titleMask.sizeDelta = new Vector2(ShowSpecialAbility ? 505f : 380f, titleMask.sizeDelta.y);
		}

		private float cv1;
		//00A9FF - blue for upgraded
		protected override void Update() {
			base.Update();
			UpdateLayout();
			description.sizeDelta = new Vector2(
				Mathf.SmoothDamp(description.sizeDelta.x, showDescription ? DESCRIPTIONFIXEDWIDTH : 0, ref cv1, 0.1f)
			, description.sizeDelta.y);

			abilityObject.SetActive(showSpecialAbility);
			showMoreObject.SetActive(showSpecialAbility);


		}

	}
}
