using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Members;
using Shared.VirtualMachines;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [SwaggerOperation("Returns a list of members.")]
    [HttpGet]
    public async Task<MemberResult.Index> GetIndex([FromQuery] MemberRequest.Index request)
    {
        return await _memberService.GetIndexAsync(request);
    }



    [SwaggerOperation("Returns a specific member.")]
    [HttpGet("{id}")]
    public async Task<MemberResult.Detail> GetDetail([FromRoute] MemberRequest.Detail request)
    {
        return await _memberService.GetDetailAsync(request);
    }


    [SwaggerOperation("Returns the VMs of a specific member.")]
    [HttpGet("{id}/vms")]
    public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromMember([FromRoute] MemberRequest.Detail request)
    {
        return await _memberService.GetVirtualMachinesFromMemberAsync(request);
    }


    [SwaggerOperation("Creates a new member.")]
    [HttpPost]
    public async Task<MemberResult.Create> Create([FromBody] MemberRequest.Create request)
    {
        return await _memberService.CreateAsync(request);
    }


    [SwaggerOperation("Edits an existing member.")]
    [HttpPut("{id}")]
    public async Task<MemberResult.Edit> Edit([FromBody] MemberRequest.Edit request)
    {
        return await _memberService.EditAsync(request);
    }


    [SwaggerOperation("Deletes an existing member.")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] MemberRequest.Delete request)
    {
        await _memberService.DeleteAsync(request);
        return NoContent();
    }
}
