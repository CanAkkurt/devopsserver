using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.VirtualMachines
{
    [Parallelizable(ParallelScope.Self)]
    public class VirtualMachineListTest : PageTest
    {
        private readonly string VM_URI = TestHelper.BaseUri + "/virtualmachines";
        private readonly string VM_OVERVIEW = "id=vm-overview";
        private readonly string VM_ITEM = "id=vm-item";
        private readonly string VM_SEARCH = "id=vm-search";

        [Test]
        public async Task Show_List_With_10_Items()
        {
            //Navigate & Wait
            await Page.GotoAsync(VM_URI);
            await Page.WaitForSelectorAsync(VM_OVERVIEW);

            //Locate & Assert
            var amountOfVms = await Page.Locator(VM_ITEM).CountAsync();
            amountOfVms.ShouldBe(10);
        }
    }
}
