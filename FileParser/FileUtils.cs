namespace FileParser
{
    public class FileUtils
    {
        /// <summary>
        /// Checks if the given file is binary
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <returns>A bool which is true in case of binary file</returns>
        public static bool IsBinary(string filePath)
        {
            const int charsToCheck = 8000;
            const char nulChar = '\0';

            using (var streamReader = new StreamReader(filePath))
            {
                for (var i = 0; i < charsToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                    {
                        return false;
                    }

                    if ((char)streamReader.Read() == nulChar)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}