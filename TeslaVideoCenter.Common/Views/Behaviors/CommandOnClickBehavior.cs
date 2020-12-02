using Avalonia.Input;

namespace TeslaVideoCenter.Views.Behaviors
{

    public class CommandOnClickBehavior : CommandBasedBehavior<InputElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.AddHandler(InputElement.PointerPressedEvent, (sender, e) => e.Handled = ExecuteCommand());
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
		}
	}
}