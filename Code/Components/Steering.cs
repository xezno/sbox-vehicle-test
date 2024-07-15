using Sandbox;
using System;
using System.Collections.Generic;

namespace RacingGame;

[Category( "Racing Game" )]
[Title( "Steering" )]
[Icon( "screen_rotation" )]
public sealed class Steering : Component
{
	[Property] public List<GameObject> Wheels { get; set; }
    [Property] public float MaxSteeringAngle { get; set; } = 20f;
    [Property] public float SteeringSmoothness { get; set; } = 10f;

	protected override void OnFixedUpdate()
	{
		if ( Scene.IsEditor )
			return;

		foreach ( var wheel in Wheels )
		{
			var targetRotation = Rotation.FromYaw( MaxSteeringAngle * Input.AnalogMove.y );
			wheel.Transform.LocalRotation = Rotation.Lerp( wheel.Transform.LocalRotation, targetRotation, Time.Delta * SteeringSmoothness );
		}
	}
}
