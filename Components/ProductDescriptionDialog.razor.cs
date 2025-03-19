using Microsoft.AspNetCore.Components;

namespace EStore.Components
{
    public partial class ProductDescriptionDialog
    {
        [Parameter] public string Description { get; set; }
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        private void CloseModal()
        {
            OnClose.InvokeAsync();
        }
    }
}
