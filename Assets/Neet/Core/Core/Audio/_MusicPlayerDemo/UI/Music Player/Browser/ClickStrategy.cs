using System;
using System.IO;
using UnityEngine;

namespace Neat.Scroller
{
    public interface ClickStrategy
    {
        void Click();
    }

    public abstract class AbstractClickStrategy
    {
        protected DirectoryItem item;

        public AbstractClickStrategy(DirectoryItem d)
        {
            item = d;
        }
    }

    public class FolderClickStrategy : AbstractClickStrategy, ClickStrategy
    {
        private BrowserManager bm;

        public FolderClickStrategy(DirectoryItem d, BrowserManager b) : base(d)
        {
            bm = b;
        }

        void ClickStrategy.Click()
        {
            bm.ExpandOrCollapseFolder(item);
        }
    }

    public class FileClickStrategy : AbstractClickStrategy, ClickStrategy
    {
        public FileClickStrategy(DirectoryItem d) : base(d) { }

        void ClickStrategy.Click()
        {
            Debug.Log("Trying to play " + item.path);

            try
            {
                Neat.Audio.MusicPlayer.MusicPlayer.instance.LoadSong(item.path, true);
            }
            catch
            {
                Debug.LogWarning("Nothing to do");
            }
        }
    }
}
