namespace FileParser
{
    public enum OrderBy
    {
        KeyAscending,
        KeyDescending,
        ValueAscending,
        ValueDescending
    }

    public class FileParser
    {
        /// <summary>
        /// The progress of the file parsing, in percentage between 0 and 100
        /// </summary>
        private int _progress = 0;
        public int Progress => _progress;

        /// <summary>
        /// True if there is file parsing in progress
        /// </summary>
        private bool _isInprogress = false;
        public bool IsInProgress => _isInprogress;

        /// <summary>
        /// Parses a text file and returns the occurence for each word in it
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <param name="cToken">Cancellation token</param>
        /// <returns>A Dictionary with the Word - Occurence pairs</returns>
        /// <exception cref="InvalidOperationException">Thrown when the file is not a text file</exception>
        public async Task<Dictionary<string, int>> Start(string filePath, CancellationToken cToken)
        {
            if (IsInProgress)
            {
                throw new InvalidOperationException("Another parsing is already in progress");
            }
            _progress = 0;
            if (FileUtils.IsBinary(filePath))
            {
                throw new InvalidOperationException("File is not a text file");
            }
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            using (StreamReader streamReader = new StreamReader(filePath))
            {

                Stream baseStream = streamReader.BaseStream;
                long fileLength = baseStream.Length;

                string line = streamReader.ReadLine();
                while (line != null && !cToken.IsCancellationRequested)
                {
                    foreach (string key in line.Split(null as string[], StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!wordCount.ContainsKey(key))
                        {
                            wordCount.Add(key, 1);
                        }
                        else
                        {
                            wordCount[key] += 1;
                        }
                    }
                    _progress = (int)(baseStream.Position / (double)fileLength * 100);
                    line = streamReader.ReadLine();
                }
            }
            if (cToken.IsCancellationRequested)
            {
                cToken.ThrowIfCancellationRequested();
            }
            _progress = 100;
            return wordCount;
        }

        /// <summary>
        /// Parses a text file and returns the occurence for each word in it in the requested order
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <param name="cToken">Cancellation token</param>
        /// <param name="order">The requested order represented by an OrdedBy enum</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when the file is not a text file</exception>
        public async Task<Dictionary<string, int>> Start(string filePath, CancellationToken cToken, OrderBy order)
        {
            Dictionary<string, int> wordCount = await Start(filePath, cToken);
            switch (order)
            {
                case OrderBy.KeyAscending:
                    wordCount = wordCount.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    break;
                case OrderBy.KeyDescending:
                    wordCount = wordCount.OrderByDescending(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
                    break;
                case OrderBy.ValueAscending:
                    wordCount = wordCount.OrderBy(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
                    break;
                case OrderBy.ValueDescending:
                    wordCount = wordCount.OrderByDescending(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return wordCount;
        }
    }
}