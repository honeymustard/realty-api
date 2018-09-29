using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Honeymustard
{
    public class TextParser
    {
        public string Text { get; private set; }
        public static Regex WhiteSpace = new Regex(@"(\r|\n|  )");
        public static Regex ScriptTags = new Regex(@"<script.*?</script>", RegexOptions.Singleline);

        public TextParser(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Strips all text that matches a given regular expression.
        /// </summary>
        /// <param name="regex">The regular expression</param>
        /// <returns>Returns a new TextParser object.</returns>
        public TextParser Strip(Regex regex)
        {
            foreach (Match match in regex.Matches(Text))
            {
                Text = Text.Replace(match.Value, "");
            }

            return new TextParser(Text);
        }

        /// <summary>
        /// Finds all the start indices of a given string.
        /// </summary>
        /// <param name="needle">The string to look for</param>
        public List<int> FindIndices(string needle)
        {
            var indices = new List<int>();
            var offset = 0;

            while (true)
            {
                var index = Text.IndexOf(needle, offset);

                if (index == -1)
                {
                    break;
                }

                indices.Add(index);
                offset = index + needle.Length;
            }

            return indices;
        }

        /// <summary>
        /// Creates a list of (start, length) partitions from a list of indices.
        /// </summary>
        /// <param name="indices">A list of valid indices</param>
        public List<Partition> Partition(List<int> indices)
        {
            var partitions = new List<Partition>();

            for (var i = 0; i < indices.Count; i++)
            {
                var end = i + 1 < indices.Count ? indices[i+1] : Text.Length;

                partitions.Add(new Partition {
                    Index = indices[i],
                    Length = end - indices[i],
                });
            }

            return partitions;
        }

        /// <summary>
        /// Creates a list of text chunks from a list of partitions.
        /// </summary>
        /// <param name="partitions">A list of valid partitions</param>
        public List<string> Chunk(List<Partition> partitions)
        {
            var chunks = new List<string>();

            partitions.ForEach(partition => {
                chunks.Add(Text.Substring(partition.Index, partition.Length));
            });

            return chunks;
        }
    }
}