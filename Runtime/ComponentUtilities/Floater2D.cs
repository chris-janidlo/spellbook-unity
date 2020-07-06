using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
public class Floater2D : MonoBehaviour
{
    [System.Serializable]
    public class Sin
    {
        public float Period, Amplitude;
        public Vector2 RandomStartPhaseRange;

        public float Offset { get; set; } = 0;

        float? _phase;
        float phase
        {
            get
            {
                if (_phase == null)
                {
                    _phase = RandomExtra.Range(RandomStartPhaseRange);
                }

                return (float) _phase;
            }
        }

        public float CurrentValue ()
        {
            return Amplitude * Mathf.Sin((2 * Mathf.PI / Period) * Time.time - phase) + Offset;
        }
    }

    public Sin VertWave, RotWave;

    void Start ()
    {
        VertWave.Offset = transform.position.y;
        RotWave.Offset = transform.rotation.eulerAngles.z;
    }

    void Update ()
    {
        transform.position = new Vector2(transform.position.x, VertWave.CurrentValue());

        transform.rotation = Quaternion.Euler(Vector3.forward * RotWave.CurrentValue());
    }
}
}
