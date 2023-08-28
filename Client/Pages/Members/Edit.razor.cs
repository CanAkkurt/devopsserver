using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Members;

namespace Client.Pages.Members
{
    public partial class Edit
    {
        [Parameter] public int Id { get; set; }

        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] public IMemberService MemberService { get; set; }

        private MemberDto.Detail member;
        private MemberDto.Update memberToUpdate;

        MudForm? form;
        bool success;
        bool _isRequestingCancel = false;
        string[] errors = Array.Empty<string>();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (MemberService != null)
            {
                MemberRequest.Detail request = new() { Id = Id };
                MemberResult.Detail result = await MemberService.GetDetailAsync(request);
                member = result.Member;  
            }
            SetMemberToUpdate();
        }

        private async Task UpdateMemberAsync()
        {
            //validate form
            await form!.Validate();
            if (!form.IsValid)
            {
                Snackbar.Add("Niet alel velden zijn geldig!", Severity.Error);
                return;
            }

            if (MemberService != null)
            {
                MemberRequest.Edit request = new() { Id = Id, Member = memberToUpdate };
                var result = await MemberService.EditAsync(request);
                if(NavigationManager != null)
                {
                    NavigationManager.NavigateTo("members/" + memberToUpdate.Id);
                }
                else { Snackbar.Add("Failed to navigate to member detail.", Severity.Error); }
            }
            else { Snackbar.Add("Failed to update member.", Severity.Error); }
        }

        private void ChangeCancel()
        {
            _isRequestingCancel = !_isRequestingCancel;
        }

        private async void Reset()
        {
            _isRequestingCancel = !_isRequestingCancel;
            SetMemberToUpdate();
            //form!.Reset();
        }

        private void SetMemberToUpdate()
        {
            memberToUpdate = new()
            {
                Id = member.Id,
                Name = member.Name,
                Active = member.Active,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Role = member.Role,
                Department = member.Department
            };
        }
    }
}