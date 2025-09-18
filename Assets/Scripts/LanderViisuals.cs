using UnityEngine;

public class LanderViisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;

    private Lander lander;
    private void Awake() {
        lander = GetComponent<Lander>();

        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e) {
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, true);
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled) {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
