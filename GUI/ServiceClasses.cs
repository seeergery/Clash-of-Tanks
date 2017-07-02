﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using ClashOfTanks.Core.Gameplay;
using ClashOfTanks.Core.User;

namespace ClashOfTanks.GUI.Service
{
    public static class ControlProcessor
    {
        public static void GenerateInitialControls(Panel battlefieldPanel)
        {
            IEnumerable<GameplayElement> gameplayElements = GameplayProcessor.GenerateInitialGameplayElements();

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control = new Control()
                {
                    Template = battlefieldPanel.FindResource("TankControlTemplate") as ControlTemplate,
                    Background = Brushes.Red,
                    RenderTransform = new RotateTransform(),
                    RenderTransformOrigin = new Point(0.5, 0.5)
                };

                gameplayElement.Control = control;
                battlefieldPanel.Children.Add(gameplayElement.Control as Control);
            }

            FrameProcessor.ProcessFrame(battlefieldPanel);
        }
    }

    public static class FrameProcessor
    {
        public static void ProcessFrame(Panel battlefieldPanel)
        {
            IEnumerable<GameplayElement> gameplayElements = GameplayProcessor.ProcessGameplay();

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control = gameplayElement.Control as Control;

                Canvas.SetLeft(control, gameplayElement.X - gameplayElement.Radius);
                Canvas.SetTop(control, GameplayElement.Battlefield.Height - gameplayElement.Y - gameplayElement.Radius); // Минус "gameplayElement.Y" из-за инверсии оси Y.
                (control.RenderTransform as RotateTransform).Angle = -gameplayElement.Angle; // Минус "gameplayElement.Angle" из-за инверсии угла.
            }
        }
    }

    public static class InputProcessor
    {
        public static void ProcessKeyInput(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    {
                        UserInput.KeyWPressed = e.IsDown;
                        break;
                    }
                case Key.S:
                    {
                        UserInput.KeySPressed = e.IsDown;
                        break;
                    }
                case Key.A:
                    {
                        UserInput.KeyAPressed = e.IsDown;
                        break;
                    }
                case Key.D:
                    {
                        UserInput.KeyDPressed = e.IsDown;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
