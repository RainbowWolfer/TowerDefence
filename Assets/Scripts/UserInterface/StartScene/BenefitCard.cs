using System;
using TMPro;
using TowerDefence.Functions;
using TowerDefence.Local;
using TowerDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class BenefitCard: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[HideInInspector]
		public BenefitCard[] others;

		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private TextMeshProUGUI description;
		[SerializeField]
		private Image border;
		[SerializeField]
		private Image image;
		[SerializeField]
		private TextMeshProUGUI levelNumber;
		[SerializeField]
		private ThreeDotsIndicator dots;
		[SerializeField]
		private Image progressFiller;
		[SerializeField]
		private TextMeshProUGUI progressText;
		[SerializeField]
		private AboutDetailButton upgrade;
		[SerializeField]
		private AboutDetailButton buy;

		[Space]
		[SerializeField]
		private Image progressborder;

		[Space]
		public Range<Color> colorRange;

		[field: Space]
		[field: SerializeField]
		public bool IsMouesOnCard { get; private set; }
		[field: SerializeField]
		public bool IsMouesOnProgress { get; private set; }
		[field: SerializeField]
		public bool Open { get; set; }


		public CardsBenefits.CardData Data { get; set; }
		public CardInfo CardInfo { get; set; }

		private float cv1;
		private float cv2;

		private void Awake() {
			buy.OnClick = () => {
				int diamondsCost = Data.GetDiamondCost(CardInfo.currentLevel);
				int diamondsOwn = Player.Current.diamond;
				if(diamondsOwn < diamondsCost) {
					return;
				}
				CardInfo.cardsCount++;
				Player.Current.diamond -= diamondsCost;
			};

			upgrade.OnClick = () => {
				//cards stuff
				int cardsCost = Data.GetCardsCount(CardInfo.currentLevel);
				int cardsOwn = CardInfo.cardsCount;
				if(cardsOwn < cardsCost) {
					return;
				}
				CardInfo.currentLevel++;
				CardInfo.cardsCount -= cardsCost;
			};
		}

		private void Update() {
			IsMouesOnCard = UIRayCaster.HasElement(border.gameObject);
			IsMouesOnProgress = UIRayCaster.HasElement(progressborder.gameObject);

			//action
			if((IsMouesOnCard || IsMouesOnCard) && Input.GetMouseButtonUp(0)) {
				Open = !Open;
				foreach(BenefitCard card in others) {
					card.Open = false;
				}
			}

			//animation
			Rt.sizeDelta = new Vector2(
				Mathf.SmoothDamp(Rt.sizeDelta.x,
					Open ? 500 : 200,
					ref cv1, 0.1f
				)
			, Rt.sizeDelta.y);

			border.color = Color.Lerp(border.color,
				IsMouesOnCard ? colorRange.from : colorRange.to,
			Time.deltaTime * 15);

			progressborder.color = Color.Lerp(progressborder.color,
				IsMouesOnProgress ? colorRange.from : colorRange.to,
			Time.deltaTime * 15);

			progressFiller.color = progressborder.color;

			//values
			if(Data == null || CardInfo == null) {
				return;
			}
			int level = CardInfo.currentLevel;
			switch(Data.type) {
				case CardsBenefits.CurveType.Number: {
					string current = $"{Data.GetBenefit(level)}";
					string sign = Data.slopeBenefit > 0 ? "+" : "";
					string next = level < Data.maxLevel ? $"{sign}{Data.slopeBenefit}" : "MAX";
					description.text = Data.description.Format(current, next);
					levelNumber.gameObject.SetActive(true);
					dots.gameObject.SetActive(false);
					levelNumber.text = $"X{level}";
				}
				break;
				case CardsBenefits.CurveType.Percentage: {
					string current = $"{Data.GetBenefit(level) * 100}%";
					string sign = Data.slopeBenefit > 0 ? "+" : "";
					string next = level < Data.maxLevel ? $"{sign}{Data.slopeBenefit * 100}%" : "MAX";
					description.text = Data.description.Format(current, next);
					levelNumber.gameObject.SetActive(true);
					dots.gameObject.SetActive(false);
					levelNumber.text = $"X{level}";
				}
				break;
				case CardsBenefits.CurveType.Fixed: {
					description.text = Data.descriptions[level];
					levelNumber.gameObject.SetActive(false);
					dots.gameObject.SetActive(true);
					dots.Count = level;
				}
				break;
				default:
					throw new Exception($"New type ({Data.type}) not implemented");
			}

			if(level < Data.maxLevel) {


				int diamondsCost = Data.GetDiamondCost(level);
				buy.SetText(diamondsCost);
				if(Player.Current.diamond < diamondsCost) {
					buy.foregroundColorRange.from = Color.red;
					buy.foregroundColorRange.to = Color.red;
				} else {
					buy.foregroundColorRange.from = Color.black;
					buy.foregroundColorRange.to = Color.white;
				}

				int cardsMax = Data.GetCardsCount(level);
				progressText.text = $"{CardInfo.cardsCount} / {cardsMax}";
				if(CardInfo.cardsCount < cardsMax) {
					buy.gameObject.SetActive(true);
					upgrade.gameObject.SetActive(false);
				} else {
					buy.gameObject.SetActive(false);
					upgrade.gameObject.SetActive(true);
				}

				progressFiller.fillAmount = Mathf.SmoothDamp(
					progressFiller.fillAmount,
					CardInfo.cardsCount / (float)cardsMax,
					ref cv2, 0.1f
				);

			} else {
				buy.gameObject.SetActive(false);
				upgrade.gameObject.SetActive(false);
				progressText.text = "MAX";
				progressFiller.fillAmount = 1;
			}

		}

		public void SetTitle(string title) {
			this.title.text = title;
		}

	}
}
