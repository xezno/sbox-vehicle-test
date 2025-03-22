using Sandbox;

public sealed class ColorPicker : Component
{
	[RequireComponent] public ModelRenderer ModelRenderer { get; set; }
	[Property] public Color[] Colors { get; set; }

	[Sync( SyncFlags.FromHost | SyncFlags.Query )] public Color CurrentColor { get; set; }

	protected override void OnEnabled()
	{
		if ( !IsProxy )
		{
			CurrentColor = Game.Random.FromArray( Colors );
		}

		ModelRenderer.Tint = CurrentColor;
	}
}
