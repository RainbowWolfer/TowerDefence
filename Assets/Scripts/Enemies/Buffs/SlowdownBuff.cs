namespace TowerDefence.Enemies.Buffs {
	public class SlowdownBuff: BaseBuff {
		public float SlowdownPercentage { get; private set; }
		public SlowdownBuff(float duration, float slowdownPercentage) : base(duration) {
			SlowdownPercentage = slowdownPercentage;
		}

		public override void Update(Enemy enemy) {
			base.Update(enemy);
			enemy.speedMultiplier = SlowdownPercentage;
		}

		public override void OnLost(Enemy enemy) {
			enemy.speedMultiplier = 1f;
		}
	}
}
