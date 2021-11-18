using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Neat.Music
{
    public class NoteEdit
    {
        private static Stack<NoteEdit> _commands;
        private static Stack<NoteEdit> commands
        {
            get
            {
                if (_commands == null)
                    _commands = new Stack<NoteEdit>();
                return _commands;
            }
        }
        public virtual void Execute()
        {
            commands.Push(this);
        }
        public virtual void Undo()
        {

        }

        public static void UndoEdit()
        {
            var cmd = commands.Pop();
            cmd.Undo();
        }
    }
}
