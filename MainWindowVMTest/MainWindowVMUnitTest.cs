using Moq;

namespace MainWindowVMTest
{
    [TestClass]
    public class MainWindowVMUnitTest
    {
        Mock<FileParser.FileParser> getMockParser()
        {
            Mock<FileParser.FileParser> parser = new Mock<FileParser.FileParser>();
            return parser;
        }

        [TestMethod]
        public void TestConstructor()
        {
            AP_test.MainWindowViewModel viewModel = new AP_test.MainWindowViewModel(getMockParser().Object);
            Assert.AreEqual(viewModel.StartCancelText, "Start");
            Assert.IsFalse(viewModel.IsFileSelected);
        }

        [TestMethod]
        public void TestFileSelection()
        {
            AP_test.MainWindowViewModel viewModel = new AP_test.MainWindowViewModel(getMockParser().Object);
            const string path = "FilePath";
            viewModel.FilePath = path;
            Assert.AreEqual(viewModel.FilePath, path);
            Assert.IsTrue(viewModel.IsFileSelected);

            viewModel.FilePath = "";
            Assert.AreEqual(viewModel.FilePath, "");
            Assert.IsFalse(viewModel.IsFileSelected);
        }

        [TestMethod]
        public void TestInProgressSetter()
        {
            AP_test.MainWindowViewModel viewModel = new AP_test.MainWindowViewModel(getMockParser().Object);
            viewModel.IsInProgress = true;
            Assert.AreEqual(viewModel.StartCancelText, "Cancel");

            viewModel.IsInProgress = false;
            Assert.AreEqual(viewModel.StartCancelText, "Start");
        }
    }
}