using System;
using System.Collections.Generic;

namespace Neat.Audio.Music
{
    [Serializable]
    public class KeyMap
    {
        public List<KeySignature> signatures = new List<KeySignature>();

        public void Sort()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;
                for (int i = 0; i < signatures.Count - 1; i += 2)
                {
                    if (signatures[i + 1].time < signatures[i].time)
                    {
                        var temp = signatures[i];
                        signatures[i] = signatures[i + 1];
                        signatures[i + 1] = temp;
                        flag = true;
                    }
                }
            }
        }
        public void UpdateNextPrev()
        {
            // requires sorted list
            // sets data in key signatures for next and prev
            for (int i = 0; i < signatures.Count - 1; i++)
            {
                signatures[i].next = signatures[i + 1];
                signatures[i + 1].prev = signatures[i];
            }
        }

        public void Add(KeySignature s)
        {
            signatures.Add(s);
            Sort();
            UpdateNextPrev();
        }

        public KeySignature GetSignatureAt(float time)
        {
            // requires sorted list

            if (signatures.Count == 0)
                return null;

            var _return = signatures[0];
            for (int i = 1; i < signatures.Count; i++)
                if (time < signatures[i].time)
                    return _return;

            return _return;
        }
    }
}
