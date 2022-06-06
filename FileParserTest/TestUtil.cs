namespace FileParserTest
{
    internal class TestUtil
    {
        /// <summary>
        /// Returns the full path to the given testFile
        /// </summary>
        /// <param name="testFile">The name of the file</param>
        /// <returns>The full path to the given testFile</returns>
        public static string GetTestFilePath(string testFile)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = path.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return projectPath + "\\testFiles\\" + testFile;
        }
    }
}
