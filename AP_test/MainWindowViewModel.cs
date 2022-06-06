using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AP_test
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _filePath = "";
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                IsCurrentParsed = false;
                if (!String.IsNullOrEmpty(_filePath))
                {
                    IsFileSelected = true;
                }
                else
                {
                    IsFileSelected = false;
                }
                OnPropertyChanged();
            }
        }

        private string _startCancelText;
        public string StartCancelText
        {
            get { return _startCancelText; }
            set
            {
                _startCancelText = value;
                OnPropertyChanged();
            }
        }

        private bool _isInProgress = false;
        public bool IsInProgress
        {
            get { return _isInProgress; }
            set
            {
                _isInProgress = value;
                StartCancelText = IsInProgress ? "Cancel" : "Start";
                OnPropertyChanged();
            }
        }

        private bool _isFileSelected = false;
        public bool IsFileSelected
        {
            get { return _isFileSelected; }
            set { _isFileSelected = value; OnPropertyChanged(); }
        }

        private bool _isCurrentParsed = false;
        public bool IsCurrentParsed
        {
            get { return _isCurrentParsed; }
            set { _isCurrentParsed = value; OnPropertyChanged(); }
        }

        DelegateCommand? openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                if (openFileCommand == null)
                {
                    openFileCommand = new DelegateCommand(_ => OpenFile());
                }
                return openFileCommand;
            }
        }
        void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        public FileParser.FileParser fileParser;

        public DelegateCommand StartStopCommand { get; set; }

        private Dictionary<string, int> wordCount;
        public Dictionary<string, int> WordCount
        {
            get { return wordCount; }
            set { wordCount = value; OnPropertyChanged(); }
        }

        private CancellationTokenSource _tokenSource;

        async void StartStopParsing(object _)
        {
            if (IsInProgress)
            {
                if (_tokenSource != null)
                {
                    _tokenSource.Cancel();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                IsInProgress = true;
                WordCount = null;
                _tokenSource = new CancellationTokenSource();
                CancellationToken ct = _tokenSource.Token;
                try
                {
                    await Task.Run(async () =>
                    {
                        WordCount = await fileParser.Start(FilePath, ct, FileParser.OrderBy.ValueDescending);
                        IsCurrentParsed = true;
                    }, ct);
                }
                catch (OperationCanceledException ex)
                {
                    IsInProgress = false;
                }
                catch (InvalidOperationException ex)
                {
                    _showErrorPopup("Given file is not a text file. Please select a text file.");
                }
                catch
                {
                    _showErrorPopup("Unexpected error!");
                }
            }
            IsInProgress = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void _showErrorPopup(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MainWindowViewModel()
        {
            this.fileParser = new FileParser.FileParser();
            IsInProgress = false;
            StartStopCommand = new DelegateCommand(StartStopParsing);
        }

        public MainWindowViewModel(FileParser.FileParser fileParser)
        {
            this.fileParser = fileParser;
            IsInProgress = false;
            StartStopCommand = new DelegateCommand(StartStopParsing);
        }
    }
}
