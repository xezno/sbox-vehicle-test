using Sandbox;
using System;

namespace RacingGame;

/// <summary>
/// Basic component for vehicle camera
/// </summary>
[Category( "Racing Game" )]
[Title( "Orbit Camera" )]
[Icon( "camera" )]
public sealed class OrbitCamera : Component
{
	[Property] public GameObject Target { get; set; }
	[Property] public Car Car { get; set; }
	[Group( "Reset" ), Property] public float ResetDebounce { get; set; } = 3.0f;
	[Group( "Reset" ), Property] public float ResetLerpTime { get; set; } = 2.5f;
	[Group( "Following" ), Property] public float Distance { get; set; } = 256f;
	[Group( "Following" ), Property] public float Height { get; set; } = 32f;

	[Group( "Sensitivity" ), Property] public float LookSensitivity { get; set; } = 5.0f;
	[Group( "Sensitivity" ), Property] public float PositionSensitivity { get; set; } = 5.0f;
	[Group( "Sensitivity" ), Property] public float RotationSensitivity { get; set; } = 5.0f;

	[Group( "Tuning" ), Property] public float PullbackRate { get; set; } = 0.05f;
	[Group( "Tuning" ), Property] public RangedFloat PullbackRange { get; set; } = new RangedFloat( 0, 800f );
	[Group( "Tuning" ), Property] public float PivotRate { get; set; } = 90f;

	private Angles _lookDir;

	protected override void OnFixedUpdate()
	{
		if ( Scene.IsEditor )
			return;

		//
		// Analog look
		//
		{
			var analogLook = Input.AnalogLook;

			if ( Input.UsingController )
			{
				var theta = MathF.Atan2( -analogLook.yaw, analogLook.pitch );
				var angleDeg = theta.RadianToDegree();
				var targetDir = new Angles( 0, angleDeg, 0 );
				_lookDir = _lookDir.LerpTo( targetDir, LookSensitivity * Time.Delta );
			}
			else
			{
				_lookDir += analogLook;
			}
		}

		//
		// Follow camera
		//
		{
			var offset = _lookDir.ToRotation().Backward * Distance * Target.Transform.World.Rotation;
			offset += Vector3.Up * Height;

			var targetPosition = Target.Transform.Position + offset;
			var targetRotation = Rotation.LookAt( Target.Transform.Position - targetPosition ).Angles();

			Transform.Position = Vector3.Lerp( Transform.Position, targetPosition, PositionSensitivity * Time.Delta );
			Transform.Rotation = Rotation.Lerp( Transform.Rotation, targetRotation, RotationSensitivity * Time.Delta );
		}
	}
}
