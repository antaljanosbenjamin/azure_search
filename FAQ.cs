namespace azure_search
{
    using System;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Spatial;
    using Newtonsoft.Json;

    // The SerializePropertyNamesAsCamelCase attribute is defined in the Azure Search .NET SDK.
    // It ensures that Pascal-case property names in the model class are mapped to camel-case
    // field names in the index.
    [SerializePropertyNamesAsCamelCase]
    public partial class FAQ
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable, IsSortable]
        public string ID { get; set; }

        [IsFilterable, IsSortable, IsFacetable, IsSearchable, Analyzer("en.microsoft")]
        public string Category { get; set; }

        [IsFilterable]
        public string Url { get; set; }

        [IsFilterable, IsSearchable, Analyzer("en.microsoft")]
        public string Question { get; set; }

        [IsFilterable, IsSearchable, Analyzer("en.microsoft")]
        public string Answer { get; set; }

        [IsFilterable, IsSearchable, Analyzer("en.microsoft")]
        public string Keywords { get; set; }

    }
}