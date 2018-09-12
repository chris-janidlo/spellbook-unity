using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
// SmoothDamps the particles 
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
	public Transform Target;
	public float TravelTime;
	[Tooltip("Minimum distance from target where a particle will die")]
	public float KillDistance = 0.01f;

	public int NumAlive { get; private set; }

	ParticleSystem system;
	Vector3[] particleVelocities;
	ParticleSystem.Particle[] particles;

	bool initialized;

	void LateUpdate ()
	{
		Initialize();

		NumAlive = system.GetParticles(particles);

		for (int i = 0; i < NumAlive; i++)
		{
			var pos = transform.TransformPoint(particles[i].position);

			particles[i].position = transform.InverseTransformPoint(Vector3.SmoothDamp(pos, Target.position, ref particleVelocities[i], TravelTime));

			if (Vector3.Distance(pos, Target.position) <= KillDistance)
			{
				particles[i].remainingLifetime = 0;
			}
		}
		system.SetParticles(particles, NumAlive);
	}

	public void Initialize ()
	{
		if (!initialized)
		{
			initialized = true;
			system = GetComponent<ParticleSystem>();
			particleVelocities = new Vector3[system.main.maxParticles];
			particles = new ParticleSystem.Particle[system.main.maxParticles];
		}
	}
}
}