using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Effects;
using TowerDefence.Functions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Emplacements {
	public class StormGeneratorEmplacement: Emplacement {
		public override EmplacementAbility Ability { get; set; }

		[Space]
		[SerializeField]
		private MeshRenderer mainBall;
		[SerializeField]
		private MeshRenderer ball1;
		[SerializeField]
		private MeshRenderer ball2;
		[SerializeField]
		private MeshRenderer ball3;
		[SerializeField]
		private MeshRenderer ball4;

		[Space]
		[SerializeField]
		private Transform skyCylinder;

		private bool ball1FLickered = false;
		private bool ball2FLickered = false;
		private bool ball3FLickered = false;
		private bool ball4FLickered = false;

		private float initialNoiseOffset = 0;
		private float multiplier = 0.5f;
		private float emissionMultiplier = 8;

		[Space]
		[SerializeField]
		private AnimationCurve lightFlickerCurve;

		[SerializeField]
		private GameObject lightningStormPrefab;


		protected override void Start() {
			base.Start();
			initialNoiseOffset = Random.Range(0, 9999);
			Ability = new EmplacementAbility() {
				AbilityRadius = 3,
				Cooldown = 30,
				UpgradedCooldown = 20,
			};
			StartCoroutine(Test());
		}

		protected override void Update() {
			base.Update();
			float percentage = Ability.CooldownPercentage;
			//Debug.Log($"{percentage} - {Ability.IsFiring}");

			float colorSpeed = Time.deltaTime * 10;
			switch(Ability.StatusType) {
				case AbilityStatusType.Idle:

					//mainBall.material.SetColor("_EmissionColor", new Color(0, 0.1f, 0.9f) * 2);
					mainBall.material.ColorLerp("_EmissionColor", new Color(0, 0.1f, 0.9f) * 2, colorSpeed);

					static Color BlackToCyan(float t) {
						return Color.Lerp(Color.gray, new Color(0, 1, 1), t);
					}

					ball1.material.ColorLerp("_BaseColor", BlackToCyan(percentage * 4), colorSpeed);
					ball2.material.ColorLerp("_BaseColor", BlackToCyan((percentage - 0.25f) * 4), colorSpeed);
					ball3.material.ColorLerp("_BaseColor", BlackToCyan((percentage - 0.5f) * 4), colorSpeed);
					ball4.material.ColorLerp("_BaseColor", BlackToCyan((percentage - 0.75f) * 4), colorSpeed);

					break;
				case AbilityStatusType.Ready:
					float v = Mathf.PerlinNoise(Time.time * multiplier + initialNoiseOffset, Time.deltaTime);
					mainBall.material.SetColor("_EmissionColor", Color.Lerp(
						mainBall.material.GetColor("_EmissionColor"),
						new Color(0, 0.1f, 0.87f) * v * emissionMultiplier,
						Time.deltaTime * 25
					));
					break;
				case AbilityStatusType.Firing:
					mainBall.material.ColorLerp("_EmissionColor", Color.red * 5, colorSpeed);
					ball1.material.ColorLerp("_EmissionColor", Color.red * 5, colorSpeed);
					ball2.material.ColorLerp("_EmissionColor", Color.red * 5, colorSpeed);
					ball3.material.ColorLerp("_EmissionColor", Color.red * 5, colorSpeed);
					ball4.material.ColorLerp("_EmissionColor", Color.red * 5, colorSpeed);

					break;
				default:
					break;
			}


			if(percentage >= 0.25f && !ball1FLickered) {
				ball1FLickered = true;
				StartCoroutine(LightFlickerCouroutine(ball1.material));
			} else if(percentage >= 0.5f && !ball2FLickered) {
				ball2FLickered = true;
				StartCoroutine(LightFlickerCouroutine(ball2.material));
			} else if(percentage >= 0.75f && !ball3FLickered) {
				ball3FLickered = true;
				StartCoroutine(LightFlickerCouroutine(ball3.material));
			} else if(percentage >= 1f && !ball4FLickered) {
				ball4FLickered = true;
				StartCoroutine(LightFlickerCouroutine(ball4.material));
			}
		}

		private IEnumerator LightFlickerCouroutine(Material material) {
			float startTime = Time.time;
			while(Time.time - startTime <= lightFlickerCurve.length) {
				material.SetColor("_EmissionColor",
					new Color(0, 0.7f, 0.8f) * lightFlickerCurve.Evaluate(Time.time - startTime)
				);
				yield return null;
			}
		}

		private IEnumerator Test() {
			while(true) {
				yield return new WaitForSeconds(Random.Range(10f, 20));
				multiplier = 10;
				emissionMultiplier = 6;
				yield return new WaitForSeconds(1f);
				multiplier = 0.5f;
				emissionMultiplier = 8;
			}
		}

		public override void Fire(Vector2 coord) {
			StartCoroutine(FireCouroutine(coord));
		}

		private void ValuesReset() {
			ball1FLickered = false;
			ball2FLickered = false;
			ball3FLickered = false;
			ball4FLickered = false;
			new List<MeshRenderer>() {
				ball1, ball2, ball3, ball4,
			}.ForEach(i => {
				i.material.SetColor("_EmissionColor", Color.gray);
				i.material.SetColor("_BaseColor", Color.gray);
			});
		}

		private Coroutine shootToSkyCoroutine;
		private IEnumerator FireCouroutine(Vector2 coord) {
			Ability.IsFiring = true;
			Ability.ResetCooldown();
			shootToSkyCoroutine = StartCoroutine(ShootToSkyCorouine());
			yield return new WaitForSeconds(1f);
			ResetSkyCylinder();

			//generate lightning storm 
			var storm = Instantiate(lightningStormPrefab, new Vector3(coord.x, 0, coord.y), Quaternion.identity).GetComponent<LightningStorm>();
			storm.Radius = Ability.AbilityRadius / 2;

			yield return new WaitForSeconds(4f);
			ValuesReset();
			Ability.IsFiring = false;
		}

		private IEnumerator ShootToSkyCorouine() {
			ResetSkyCylinder();
			while(skyCylinder.transform.localScale.z <= 20) {
				yield return null;
				skyCylinder.transform.localScale = new Vector3(
					skyCylinder.transform.localScale.x,
					skyCylinder.transform.localScale.y,
					Mathf.MoveTowards(
						skyCylinder.transform.localScale.z,
						20,
						Time.deltaTime * 10
					)
				);
			}
		}

		private void ResetSkyCylinder() {
			if(shootToSkyCoroutine != null) {
				StopCoroutine(shootToSkyCoroutine);
				shootToSkyCoroutine = null;
			}
			skyCylinder.transform.localScale = new Vector3(0.1f, 0.1f, 0.5f);
		}
	}
}
