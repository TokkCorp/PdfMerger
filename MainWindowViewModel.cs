using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PdfMerger
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IList<string> documents;
        public IList<string> Documents
        {
            get
            {
                if(documents == null)
                {
                    documents = new ObservableCollection<string>();
                }

                return documents;
            }
        }

        private string selectedDocument;
        public string SelectedDocument
        {
            get
            {
                return selectedDocument;
            }
            set
            {
                selectedDocument = value;
                OnPropertyChanged();
                NotifyButtonChanged();
            }
        }


        public bool CanMoveUp
        {
            get
            {
                if (SelectedDocument == null || !Documents.Contains(SelectedDocument) || Documents.Count == 1 || Documents.IndexOf(SelectedDocument) == 0)
                {
                    return false;
                }
                return true;
            }
        }
        public bool CanMoveDown
        {
            get
            {
                if (SelectedDocument == null || !Documents.Contains(SelectedDocument) || Documents.Count == 1 || Documents.IndexOf(SelectedDocument) == Documents.Count -1)
                {
                    return false;
                }
                return true;
            }
        }
        public bool CanRemove
        {
            get
            {
                if(SelectedDocument == null || !Documents.Contains(SelectedDocument))
                {
                    return false;
                }
                return true;
            }
        }
        public bool CanCreate
        {
            get
            {
                return Documents.Count > 0;
            }
        }

        internal void AddPdf()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF-Dateien (*.pdf)|*.pdf";
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;

            if(dialog.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            string newFile = string.Empty;

            foreach(var fileName in dialog.FileNames)
            {
                newFile = fileName;

                if (!Documents.Contains(newFile))
                {
                    Documents.Add(newFile);
                }
            }

            SelectedDocument = newFile;
            NotifyButtonChanged();
        }

        internal void RemovePdf()
        {
            if (!CanRemove)
            {
                return;
            }

            var curIndex = Documents.IndexOf(SelectedDocument);
            Documents.Remove(SelectedDocument);
            if (Documents.Count > 0)
            {
                SelectedDocument = Documents.ElementAt(Math.Min(Math.Max(curIndex - 1, 0), Documents.Count -1));
            }
            else
            {
                SelectedDocument = null;
            }
            NotifyButtonChanged();
        }

        internal void MoveUp()
        {
            if (!CanMoveUp)
            {
                return;
            }

            var curIndex = Documents.IndexOf(SelectedDocument);
            (Documents as ObservableCollection<string>).Move(curIndex, curIndex - 1);
            NotifyButtonChanged();
        }

        internal void MoveDown()
        {
            if (!CanMoveDown)
            {
                return;
            }

            var curIndex = Documents.IndexOf(SelectedDocument);
            (Documents as ObservableCollection<string>).Move(curIndex, curIndex + 1);
            NotifyButtonChanged();
        }

        
        internal void CreatePdf()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF-Dateien (*.pdf)|*.pdf";
            dialog.RestoreDirectory = true;

            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var fileName = dialog.FileName;
            if(File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var pdfFiles = Documents.Select(file => PdfReader.Open(file, PdfDocumentOpenMode.Import)).ToList();
            if (!pdfFiles.Any())
            {
                return;
            }

            var outputFile = new PdfDocument();
            foreach (var pdfDocument in pdfFiles)
            {
                foreach (PdfPage page in pdfDocument.Pages)
                {
                    outputFile.AddPage(page);
                }
            }
            outputFile.Save(fileName);
            Process.Start(fileName);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyButtonChanged()
        {
            OnPropertyChanged("CanMoveUp");
            OnPropertyChanged("CanMoveDown");
            OnPropertyChanged("CanRemove");
            OnPropertyChanged("CanCreate");
        }
    }
}
