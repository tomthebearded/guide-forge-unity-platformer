using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip hurtClip;

    private AudioSource source;
    private PlayerMotor motor;
    private PlayerStats stats;
    private PlayerHealth health;

    private int livesLastSeen;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        motor = GetComponent<PlayerMotor>();
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
    }

    private void Start()=>
        livesLastSeen = health.LivesRemaining;

    private void OnEnable()
    {
        motor.Jumped += PlayJump;
        motor.Landed += PlayLand;
        motor.Dashed += PlayDash;
        stats.CoinsChanged += PlayCoin;
        health.LivesChanged += PlayHurtIfLifeLost;
    }

    private void OnDisable()
    {
        motor.Jumped -= PlayJump;
        motor.Landed -= PlayLand;
        motor.Dashed -= PlayDash;
        stats.CoinsChanged -= PlayCoin;
        health.LivesChanged -= PlayHurtIfLifeLost;
    }

    private void PlayJump() => Play(jumpClip);
    private void PlayLand() => Play(landClip);
    private void PlayDash() => Play(dashClip);

    private void PlayCoin(int coinsCollected) => Play(coinClip);

    private void PlayHurtIfLifeLost(int livesRemaining)
    {
        if (livesRemaining < livesLastSeen)
            Play(hurtClip);

        livesLastSeen = livesRemaining;
    }

    private void Play(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }
}
