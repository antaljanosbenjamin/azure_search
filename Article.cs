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
    public partial class Article
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable, IsSortable]
        public string ID { get; set; }

        [IsFilterable, IsSearchable, Analyzer("hu.microsoft")]
        public string Title { get; set; }

        [IsFilterable, IsSearchable, Analyzer("hu.microsoft")]
        public string Text { get; set; }

        [IsFilterable, IsSearchable, Analyzer("hu.microsoft")]
        public string Author { get; set; }

        [IsFilterable, IsSortable]
        public DateTime CreationDate { get; set; }

    }
}