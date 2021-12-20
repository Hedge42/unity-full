using System;
using System.Collections.Generic;

namespace Neat.Audio.Music
{
    public enum GameStateType
    {
        Play,
        Edit,
        Menu,
    }
    public interface GameState
    {
        ChartPlayer controller { get; }
        void Transition(GameState state);
    }

    public class PlayState : GameState
    {
        public ChartPlayer controller { get; private set; }

        public PlayState(GameState previous)
        {
            controller = previous.controller;
        }

        public void Transition(GameState state)
        {
        }
    }
    public class EditState : GameState
    {
        public ChartPlayer controller { get; private set; }

        public void Transition(GameState state)
        {
            throw new NotImplementedException();
        }
    }
    public class MenuState : GameState
    {
        public ChartPlayer controller { get; private set; }

        public void Transition(GameState state)
        {
        }
    }
    public class GameStateTransition
    {
        public GameStateTransition(MenuState menu)
        {
        }
        public GameStateTransition(EditState edit)
        {
        }
        public GameStateTransition(PlayState play)
        {
        }
    }

}
