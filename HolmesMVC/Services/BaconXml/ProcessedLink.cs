namespace HolmesMVC.Services.BaconXml
{
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable IDE1006 // Naming Styles
    using System.Collections.Generic;

    public class ProcessedLink
    {
        public ProcessedLink(link link)
        {
            ProcessedMovies = new List<ProcessedMovie>();
            for (int i = 1; i < link.Items.Length; i += 2)
            {
                if (string.IsNullOrWhiteSpace(link.Items[i - 1])
                    || string.IsNullOrWhiteSpace(link.Items[i])
                    || string.IsNullOrWhiteSpace(link.Items[i + 1]))
                {
                    continue;
                }

                var procM = new ProcessedMovie
                                    {
                                        Name = link.Items[i],
                                        Actor1 = link.Items[i - 1],
                                        Actor2 = link.Items[i + 1]
                                    };
                    ProcessedMovies.Add(procM);
            }
        }

        public List<ProcessedMovie> ProcessedMovies { get; set; }
    }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning restore IDE1006 // Naming Styles
}