using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Members;

namespace Client.Pages.Members;

public partial class Add
{
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] public IMemberService MemberService { get; set; }

    private MemberDto.Create newMember = new();

    MudForm? form;
    bool success;
    bool _isRequestingCancel = false;
    string[] errors = Array.Empty<string>();

    private async Task CreateMemberAsync()
    {
        //validate form
        await form!.Validate();
        if (!form.IsValid)
        {
            Snackbar.Add("Niet alle velden zijn geldig!", Severity.Error);
            return;
        }

        if(MemberService != null)
        {
            MemberRequest.Create request = new() { Member = newMember };
            var result = await MemberService.CreateAsync(request);

            if(NavigationManager != null)
            {
                NavigationManager.NavigateTo("members/" + result.Id);
            }
            else { Snackbar.Add("Failed to navigate to member detail", Severity.Error); }
        }
        else { Snackbar.Add("Failed to create member", Severity.Error); }
    }

    private void ChangeCancel()
    {
        _isRequestingCancel = !_isRequestingCancel;
    }

    private void Reset()
    {
        _isRequestingCancel = !_isRequestingCancel;
        form!.Reset();
    }
}