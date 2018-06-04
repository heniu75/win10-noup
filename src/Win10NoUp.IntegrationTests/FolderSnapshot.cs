using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Win10NoUp.IntegrationTests
{
    public class FolderSnapshot
    {
        private readonly string _folder;
        public List<string> Files = new List<string>();
        private DateTime _created;

        public FolderSnapshot(string folder)
        {
            _folder = folder;
            TakeSnapshot();
            _created = DateTime.Now;
        }

        public void TakeSnapshot()
        {
            Files = new List<string>();
            Files.AddRange(Directory.GetFiles(_folder, "*.*"));
        }

        public List<string> NewFiles()
        {
            var newFiles = new List<string>();
            newFiles.AddRange(Directory.GetFiles(_folder, "*.*"));
            var retVal = newFiles.Except(Files).ToList();
            return retVal;
        }
        public bool HasNewFiles()
        {
            return NewFiles().Any();
        }

        public int NewFileCount()
        {
            return NewFiles().Count();
        }

        public bool IsNewlyListedFile(string file)
        {
            var isNewFile = NewFiles().Contains(file);
            return isNewFile;
        }

        public bool IsNewFile(string file)
        {
            var isNewFile = NewFiles().Contains(file);
            var isRecentlyModified = IsRecentlyModified(file);
            return isNewFile || isRecentlyModified;
        }

        public bool NoListDifferences()
        {
            var currentFiles = new List<string>();
            currentFiles.AddRange(Directory.GetFiles(_folder, "*.*"));
            var retVal = currentFiles.Count() == Files.Count()
                   && !currentFiles.Except(Files).Any();
            return retVal;
        }

        public bool ListDifferences()
        {
            var retVal = !NoListDifferences();
            return retVal;
        }

        public bool IsRecentlyModified(string targetFile)
        {
            var lastWriteTime = File.GetLastWriteTime(targetFile);
            var isRecentlyModified = lastWriteTime >= _created;
            return isRecentlyModified;
        }
    }
}
