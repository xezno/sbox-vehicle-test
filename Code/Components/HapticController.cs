using Sandbox;

public sealed class HapticController : Component, Component.ICollisionListener
{
	[RequireComponent] public Rigidbody Rigidbody { get; set; }

	private Vector3 _lastVelocity;
	private Vector3 _velocityDelta;

	public void OnCollisionStart( Collision other )
	{
		if ( _velocityDelta.Length < 20f )
			Input.TriggerHaptics( HapticEffect.SoftImpact );
		else
			Input.TriggerHaptics( HapticEffect.HardImpact );
	}

	public void OnCollisionStop( CollisionStop other )
	{
	}

	public void OnCollisionUpdate( Collision other )
	{
	}

	protected override void OnFixedUpdate()
	{
		_velocityDelta = Rigidbody.Velocity - _lastVelocity;
		_lastVelocity = Rigidbody.Velocity;
	}
}
