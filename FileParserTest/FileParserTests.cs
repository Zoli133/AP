namespace FileParserTest
{
    /// <summary>
    /// Unit tests for <c>FileParser</c> class
    /// </summary>
    [TestClass]
    public class FileParserTests
    {
        /// <summary>
        /// Tests that the constructor default values
        /// </summary>
        [TestMethod]
        public void TestFileParserConstructor()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            Assert.IsFalse(parser.IsInProgress);
            Assert.AreEqual(parser.Progress, 0);
        }

        /// <summary>
        /// Tests the parser with invalid file
        /// </summary>
        [TestMethod]
        public async Task TestInvalidFile()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Exception ex = await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => parser.Start(TestUtil.GetTestFilePath("invalid_file.txt"), cancellationTokenSource.Token));
            StringAssert.Contains(ex.Message, "Could not find file");
        }

        /// <summary>
        /// Tests the parser with empty file
        /// </summary>
        [TestMethod]
        public async Task TestEmptyTextFile()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("empty_sample.txt"), cancellationTokenSource.Token);
            Assert.AreEqual(result.Count, 0);
        }

        /// <summary>
        /// Tests the parser with various white space characters
        /// </summary>
        [TestMethod]
        public async Task TestWhiteSpaceHandling()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("white_spaces.txt"), cancellationTokenSource.Token);
            Assert.AreEqual(result.Count, 4);
        }

        /// <summary>
        /// Tests then parser with the provided basic sample
        /// </summary>
        [TestMethod]
        public async Task TestBasicSampleFile()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("short_sample.txt"), cancellationTokenSource.Token);
            Assert.AreEqual(parser.Progress, 100);
            Assert.AreEqual(result.Count, 7);
            Assert.AreEqual(result["Adam"], 2);
            Assert.AreEqual(result["Seth"], 2);
            Assert.AreEqual(result["1:1"], 1);
            Assert.AreEqual(result["Enos"], 1);
            Assert.AreEqual(result["1:2"], 1);
            Assert.AreEqual(result["Cainan"], 1);
            Assert.AreEqual(result["Iared"], 1);
        }

        /// <summary>
        /// Tests the oredering with KeyAscending option
        /// </summary>
        [TestMethod]
        public async Task TestOrderingKeyAscending()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("order_sample.txt"), cancellationTokenSource.Token, FileParser.OrderBy.KeyAscending);
            Assert.AreEqual(result.ElementAt(0).Key, "A");
            Assert.AreEqual(result.ElementAt(1).Key, "B");
            Assert.AreEqual(result.ElementAt(2).Key, "C");
        }

        /// <summary>
        /// Tests the oredering with KeyDescending option
        /// </summary>
        [TestMethod]
        public async Task TestOrderingKeyDescending()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("order_sample.txt"), cancellationTokenSource.Token, FileParser.OrderBy.KeyDescending);
            Assert.AreEqual(result.ElementAt(0).Key, "C");
            Assert.AreEqual(result.ElementAt(1).Key, "B");
            Assert.AreEqual(result.ElementAt(2).Key, "A");
        }

        /// <summary>
        /// Tests the oredering with ValueAscending option
        /// </summary>
        [TestMethod]
        public async Task TestOrderingvalueAscending()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("order_sample.txt"), cancellationTokenSource.Token, FileParser.OrderBy.ValueAscending);
            Assert.AreEqual(result.ElementAt(0).Value, 1);
            Assert.AreEqual(result.ElementAt(1).Value, 2);
            Assert.AreEqual(result.ElementAt(2).Value, 3);
        }

        /// <summary>
        /// Tests the oredering with ValueDescending option
        /// </summary>
        [TestMethod]
        public async Task TestOrderingValueDescending()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Dictionary<string, int> result = await parser.Start(TestUtil.GetTestFilePath("order_sample.txt"), cancellationTokenSource.Token, FileParser.OrderBy.ValueDescending);
            Assert.AreEqual(result.ElementAt(0).Value, 3);
            Assert.AreEqual(result.ElementAt(1).Value, 2);
            Assert.AreEqual(result.ElementAt(2).Value, 1);
        }

        /// <summary>
        /// Tests cancellation token handling
        /// </summary>
        [TestMethod]
        public async Task TestCancellation()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            Exception ex = await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => parser.Start(TestUtil.GetTestFilePath("order_sample.txt"), cancellationTokenSource.Token));
            StringAssert.Contains(ex.Message, "The operation was canceled");
        }

        /// <summary>
        /// Tests binary file handling in Start function
        /// </summary>
        [TestMethod]
        public async Task TestNonTextBasedFile()
        {
            FileParser.FileParser parser = new FileParser.FileParser();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Exception ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => parser.Start(TestUtil.GetTestFilePath("test.pdf"), cancellationTokenSource.Token));
            StringAssert.Contains(ex.Message, "File is not a text file");
        }
    }
}