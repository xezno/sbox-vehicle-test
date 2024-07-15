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

	private float _currentTorque;

	protected override void OnFixedUpdate()
	{
		float verticalInput = Input.AnalogMove.x;
		float targetTorque = verticalInput * Torque;

		bool isBraking = (targetTorque < 0f);
		float lerpRate = isBraking ? 5.0f : 1.0f; // Brake applies quicker

		_currentTorque = _currentTorque.LerpTo( targetTorque, lerpRate * Time.Delta );
		_currentTorque = _currentTorque.Clamp( 0, float.MaxValue );

		foreach ( Wheel wheel in _wheels )
		{
			wheel.ApplyMotorTorque( _currentTorque );
		}
	}
}
