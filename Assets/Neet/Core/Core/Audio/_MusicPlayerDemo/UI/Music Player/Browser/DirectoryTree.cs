using Neet.Collections;
using System.IO;
using Neet.File;

namespace Neet.Scroller
{
    public class DirectoryTree
    {
        private DirectoryItem root;

        public DirectoryTree(DirectoryItem root)
        {
            this.root = root;
            Populate(this.root);
        }
        public DirectoryTree(string root)
        {
            this.root = new DirectoryItem(root, 0, true);
            Populate(this.root);
        }

        /// <summary>
        /// Recursively populates the given parent's children
        /// </summary>
        /// <param name="parent"></param>
        private void Populate(DirectoryItem parent)
        {
            string[] childPaths = GetContents(parent.path);

            if (childPaths != null && childPaths.Length > 0)
            {
                for (int i = 0; i < childPaths.Length; i++)
                {
                    DirectoryItem child = new DirectoryItem(childPaths[i], parent.level + 1, false);
                    parent.children.AddLast(child);
                    Populate(child);
                }
            }
        }

        /// <summary>
        /// Returns an array of all sub-directories and valid audio files contained in this directory
        /// Returns null if path cannot be recognized as a valid directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] GetContents(string path)
        {
            // check if parameter is a valid directory
            // if NO, return an empty array or null
            // if YES, return an array containing all sub-directories and valid music files

            if (FileManager.IsDirectory(path))
            {
                ArrayList<string> aList = new ArrayList<string>(Directory.GetDirectories(path));
                aList.AddRange(FileManager.SanitizeAudio(Directory.GetFiles(path)));
                return aList.ToArray();
            }
            else
                return null;
        }

        public DirectoryItem[] GetVisibleItems()
        {
            IList<DirectoryItem> levelOneChildren = root.children;
            IList<DirectoryItem> visible = new List<DirectoryItem>();

            foreach (DirectoryItem item in levelOneChildren)
                // all of level one is visible...
                RecursivelyAddVisibleItems(ref visible, item);

            return visible.ToArray();
        }
        private void RecursivelyAddVisibleItems(ref IList<DirectoryItem> items, DirectoryItem item)
        {
            items.AddLast(item);
            if (item.isOpen && item.children.Count() > 0)
            {
                foreach (DirectoryItem d in item.children)
                    RecursivelyAddVisibleItems(ref items, d);
            }
        }
    }
}
