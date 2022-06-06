namespace FileParserTest
{
    /// <summary>
    /// Unit tests for <c>FileUtils</c> class
    /// </summary>
    [TestClass]
    public class FileUtilsTest
    {
        /// <summary>
        /// Tests IsBinary function with text file
        /// </summary>
        [TestMethod]
        public void TestIsBinaryWithText()
        {
            Assert.IsFalse(FileParser.FileUtils.IsBinary(TestUtil.GetTestFilePath("order_sample.txt")));
        }

        /// <summary>
        /// Tests IsBinary function with pdf file
        /// </summary>
        [TestMethod]
        public void TestIsBinaryWithPDF()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            Assert.IsTrue(FileParser.FileUtils.IsBinary(TestUtil.GetTestFilePath("test.pdf")));
        }
    }
}
