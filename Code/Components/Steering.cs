using Sandbox;
using System.Collections.Generic;

namespace RacingGame;

public sealed class Steering : Component
{
	[Property] public List<GameObject> Wheels { get; set; }
	[Property] public float MaxSteeringAngle { get; set; } = 20f;
	[Property] public float SteeringSmoothness { get; set; } = 10f;
	[Property] public Angles Offset { get; set; }

	protected override void OnFixedUpdate()
	{
		if ( Scene.IsEditor )
			return;

		if ( IsProxy )
			return;

		foreach ( var wheel in Wheels )
		{
			var targetRotation = Rotation.FromYaw( MaxSteeringAngle * Input.AnalogMove.y ) * Rotation.From( Offset );
			wheel.Transform.LocalRotation = Rotation.Lerp( wheel.Transform.LocalRotation, targetRotation, Time.Delta * SteeringSmoothness );
		}
	}
}
