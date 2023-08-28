using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;

namespace IntegrationTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task HomepageWorks()
    {
        await Page.GotoAsync("https://localhost:5001/home");
        await Expect(Page).ToHaveTitleAsync(new Regex("VirtualITCompany"));
    }
}
