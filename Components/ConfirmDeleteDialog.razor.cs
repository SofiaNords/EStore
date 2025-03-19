using Microsoft.AspNetCore.Components;

namespace EStore.Components
{
    public partial class ConfirmDeleteDialog
    {
        [Parameter] public EventCallback<bool> OnConfirmed { get; set; }
        [Parameter] public bool IsVisible { get; set; }

        private void Close()
        {
            IsVisible = false;
            OnConfirmed.InvokeAsync(false);  
        }

        private void Cancel()
        {
            IsVisible = false;
            OnConfirmed.InvokeAsync(false); 
        }

        private void Confirm()
        {
            IsVisible = false;
            OnConfirmed.InvokeAsync(true);
        }
    }
}
