using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Honeymustard
{
    public class Partition {
        public int Index { get; set; }
        public int Length { get; set; }
    }

    public class Parser
    {
        public string Blob { get; private set; }

        public Parser(string blob)
        {
            Blob = blob;
        }

        public List<int> FindIndices(string key)
        {
            var indices = new List<int>();
            var offset = 0;

            while (true)
            {
                var index = Blob.IndexOf(key, offset);

                if (index == -1)
                {
                    break;
                }

                indices.Add(index);
                offset = index + key.Length;
            }

            return indices;
        }

        public List<Partition> Partition(List<int> indices)
        {
            var partitions = new List<Partition>();

            for (var i = 0; i < indices.Count; i++)
            {
                var end = i + 1 < indices.Count ? indices[i+1] : Blob.Length;

                partitions.Add(new Partition {
                    Index = indices[i],
                    Length = end - indices[i],
                });
            }

            return partitions;
        }

        public List<string> Chunk(List<Partition> partitions)
        {
            var chunks = new List<string>();

            partitions.ForEach(partition => {
                var chunk = Blob.Substring(partition.Index, partition.Length);
                var regex = new Regex(@"(\r|\n|  )");
                chunks.Add(regex.Replace(chunk, ""));
            });

            return chunks;
        }
    }
}