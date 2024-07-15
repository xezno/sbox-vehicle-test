using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace RacingGame;

[Category( "Racing Game" )]
[Title( "Car" )]
[Icon( "directions_car" )]
public sealed class Car : Component
{
	[RequireComponent] public Rigidbody Rigidbody { get; set; }

	[Property] public float Torque { get; set; } = 15000f;

	private List<Wheel> _wheels;

	protected override void OnEnabled()
	{
		_wheels = Components.GetAll<Wheel>( FindMode.EverythingInSelfAndDescendants ).ToList();
	}

	protected override void OnFixedUpdate()
	{
		float verticalInput = Input.AnalogMove.x;
		float torque = verticalInput * Torque;

		foreach ( Wheel wheel in _wheels )
		{
			wheel.ApplyMotorTorque( torque );
		}
	}
}
