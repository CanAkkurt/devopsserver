using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.VirtualMachines;



namespace Client.Pages.VirtualMachines
{
    public partial class Analytics  
    {
        [Inject]
        public IVirtualMachineService VirtualMachineService { get; set; }

        private IEnumerable<VirtualMachineDto> _vms = new List<VirtualMachineDto>();
        protected string[] XAxisLabels;
        public List<ChartSeries> RamGraph = new List<ChartSeries>();
        public List<ChartSeries> vCpuGraph = new List<ChartSeries>();
        public List<ChartSeries> StorageGraph = new List<ChartSeries>();



        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (VirtualMachineService != null)
            {
                VirtualMachineResult.Usage usages = await VirtualMachineService.GetUsageAsync();

                if (usages.VirtualMachines == null)
                {
                    return;
                }

                RamGraph.Add(makeGraph(usages, "RamGraph"));
                vCpuGraph.Add(makeGraph(usages, "vCpuGraph"));
                StorageGraph.Add(makeGraph(usages, "StorageGraph"));
            }
            else
            {
                RamGraph = new List<ChartSeries>();
                vCpuGraph = new List<ChartSeries>();
                StorageGraph = new List<ChartSeries>();
            }
        }
        public class DataPerDay
        {
            public DateTime Date { get; set; }
            public double Data { get; set; }



        }

        public List<DataPerDay> getRamPerDay(DateTime startDate, DateTime endDate, double usage)
        {
            List<DateTime> dates = new List<DateTime>();
            while (startDate <= endDate)
            {
                dates.Add(startDate.Date);
                startDate = startDate.AddDays(1);

            }
            return dates.Select(i => new DataPerDay() { Data = usage, Date = i }).ToList();
        }

        public ChartSeries makeGraph(VirtualMachineResult.Usage usages, String graphType)
        {
            DateTime startMonth = DateTime.UtcNow.Date.AddMonths(-1);
            DateTime endMonth = DateTime.UtcNow.Date.AddMonths(1);

            var allData = graphType.ToLower() switch
            {
                "ramgraph" => usages.VirtualMachines.Select(usage => getRamPerDay(usage.StartDate, usage.EndDate, usage.MemoryAmount)).SelectMany(usage => usage),
                "vcpugraph" => usages.VirtualMachines.Select(usage => getRamPerDay(usage.StartDate, usage.EndDate, usage.VCPUamount)).SelectMany(usage => usage),
                "storagegraph" => usages.VirtualMachines.Select(usage => getRamPerDay(usage.StartDate, usage.EndDate, usage.StorageAmount)).SelectMany(usage => usage),
                _ => null,
            };
            var usagesInTimeFrame = allData.Where(usage => usage.Date >= startMonth && usage.Date <= endMonth);
            var totalUsagePerDate = usagesInTimeFrame.GroupBy(usage => usage.Date).Select(usagesPerDay => new DataPerDay() { Date = usagesPerDay.First().Date, Data = usagesPerDay.Sum(usage => usage.Data) })
                .OrderBy(usage => usage.Date);
            XAxisLabels = totalUsagePerDate.Select(usage => usage.Date.ToString("dd/MM/yy")).ToArray();

            int labelCounter = 7;
            List<String> labels = new List<String>();

            foreach (var usage in totalUsagePerDate)
            {
                if (labelCounter % 7 == 0)
                {
                    labels.Add(usage.Date.ToString("dd/MM/yy"));
                }
                else
                {
                    labels.Add("");
                }
                labelCounter++;
            }
            XAxisLabels = labels.ToArray();

            return new ChartSeries() { Name = graphType, Data = totalUsagePerDate.Select(usage => usage.Data).ToArray() };
        }
    }
}



