using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RacingGame;

[Category( "Racing Game" )]
[Title( "Car" )]
[Icon( "directions_car" )]
public sealed class Car : Component
{
	[RequireComponent] public Rigidbody Rigidbody { get; set; }

	[Property, Group( "Vehicle" )] public float Torque { get; set; } = 15000f;
	[Property, Group( "Vehicle" )] public float AccelerationRate { get; set; } = 1.0f;
	[Property, Group( "Vehicle" )] public float DecelerationRate { get; set; } = 0.5f;
	[Property, Group( "Vehicle" )] public float BrakingRate { get; set; } = 2.0f;
	[Property] public GameObject CameraTarget { get; set; }

	private List<Wheel> _wheels;

	private static List<Car> All { get; } = new();
	public static Car Local => All.FirstOrDefault( x => !x.Network.IsProxy );

	protected override void OnEnabled()
	{
		All.Add( this );
		_wheels = Components.GetAll<Wheel>( FindMode.EverythingInSelfAndDescendants ).ToList();
	}

	protected override void OnDisabled()
	{
		All.Remove( this );
	}

	private float _currentTorque;

	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;

		float verticalInput = Input.AnalogMove.x;
		float targetTorque = verticalInput * Torque;

		bool isBraking = Math.Sign( verticalInput * _currentTorque ) == -1;
		bool isDecelerating = verticalInput == 0;

		float lerpRate = AccelerationRate;
		if ( isBraking )
			lerpRate = BrakingRate;
		else if ( isDecelerating )
			lerpRate = DecelerationRate;

		_currentTorque = _currentTorque.LerpTo( targetTorque, lerpRate * Time.Delta );

		foreach ( Wheel wheel in _wheels )
		{
			wheel.ApplyMotorTorque( _currentTorque );
		}

		var groundVel = Rigidbody.Velocity.WithZ( 0f );
		if ( verticalInput == 0f && groundVel.Length < 32f )
		{
			var z = Rigidbody.Velocity.z;
			Rigidbody.Velocity = Vector3.Zero.WithZ( z );
		}
	}
}
