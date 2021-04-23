using UnityEngine.UI;
using Leopotam.Ecs;

public class SidebarUI : ReactiveUI<Gold, Health, Wave>
{
    public Text Gold;
    public Text Health;
    public Text Wave;

    public Button NextWaveButton;
    public Button Tower1;
    public Button Tower2;
    public Button Tower3;

    public EntityLink GameStateLink;

    public override void OnChanged(in EcsEntity entity, in Gold gold, in Health health, in Wave wave)
    {
        Gold.text = "Gold: " + gold.Value;
        Health.text = "Health: " + health.Value;
        Wave.text = "Wave: " + wave.Value;
        Tower1.enabled = gold.Value >= 100;
        Tower2.enabled = gold.Value >= 200;
        Tower3.enabled = gold.Value >= 300;
        NextWaveButton.enabled = !entity.Has<WaveInProgress>();
    }

    public void BuyTower(int tower)
    {
        GameStateLink.Link.Set(new BuyTowerEvent { Value = tower });
    }

    public void NextWave()
    {
        GameStateLink.Link.Set<NextWaveEvent>();
    }
}
