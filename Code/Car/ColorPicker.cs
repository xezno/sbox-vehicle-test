using Sandbox;

public sealed class ColorPicker : Component
{
	[RequireComponent] public ModelRenderer ModelRenderer { get; set; }
	[Property] public Color[] Colors { get; set; }

	[HostSync( Query = true )] public Color CurrentColor { get; set; }

	protected override void OnEnabled()
	{
		if ( !IsProxy )
		{
			CurrentColor = Game.Random.FromArray( Colors );
		}

		ModelRenderer.Tint = CurrentColor;
	}
}
