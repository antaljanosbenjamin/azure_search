namespace azure_search
{
    using System;
    using System.Text;

    public partial class FAQ
    {
        // This implementation of ToString() is only for the purposes of the sample console application.
        // You can override ToString() in your own model class if you want, but you don't need to in order
        // to use the Azure Search .NET SDK.
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Question {0}: {1}", ID, Question).AppendLine();
            builder.AppendFormat("Answer: {0}", Answer).AppendLine();
            builder.AppendFormat("Keywords: {0}", Keywords).AppendLine();
            builder.AppendFormat("Url: {0}", Url).AppendLine();
            return builder.ToString();
        }
    }
}