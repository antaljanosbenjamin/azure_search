namespace azure_search
{
    using System;
    using System.Text;

    public partial class Article
    {
        // This implementation of ToString() is only for the purposes of the sample console application.
        // You can override ToString() in your own model class if you want, but you don't need to in order
        // to use the Azure Search .NET SDK.
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Article {0}: {1}", ID, Title).AppendLine();
            builder.AppendFormat("Author: {0}", Author).AppendLine();
            builder.AppendFormat("CreationDate: {0}", CreationDate.ToString()).AppendLine();
            return builder.ToString();
        }
    }
}