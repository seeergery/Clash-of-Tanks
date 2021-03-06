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
        private static Panel BattlefieldPanel { get; set; }

        public static void SetupControls(Panel battlefieldPanel)
        {
            BattlefieldPanel = battlefieldPanel;
            GameplayProcessor.SetupGameplay();
        }

        public static void UpdateControls(IEnumerable<GameplayElement> gameplayElements)
        {
            BattlefieldPanel.Children.Clear();

            foreach (GameplayElement gameplayElement in gameplayElements)
            {
                Control control;

                if (gameplayElement.Control == null)
                {
                    control = new Control();

                    switch (gameplayElement.Type)
                    {
                        case GameplayElement.Types.Tank:
                            {
                                control.Template = BattlefieldPanel.FindResource("TankControlTemplate") as ControlTemplate;
                                control.Background = Brushes.Red;
                                Panel.SetZIndex(control, 1);
                                break;
                            }
                        case GameplayElement.Types.Projectile:
                            {
                                control.Template = BattlefieldPanel.FindResource("ProjectileControlTemplate") as ControlTemplate;
                                control.Background = Brushes.Silver;
                                break;
                            }
                        case GameplayElement.Types.Explosion:
                            {
                                control.Template = BattlefieldPanel.FindResource("ExplosionControlTemplate") as ControlTemplate;
                                Panel.SetZIndex(control, 2);
                                break;
                            }
                    }

                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new ScaleTransform());
                    transformGroup.Children.Add(new RotateTransform());

                    control.RenderTransform = transformGroup;
                    control.RenderTransformOrigin = new Point(0.5, 0.5);

                    gameplayElement.Control = control;
                }
                else
                {
                    control = gameplayElement.Control as Control;
                }

                Canvas.SetLeft(control, gameplayElement.X - GameWindow.СontrolRadius);
                Canvas.SetTop(control, GameplayElement.Battlefield.Height - gameplayElement.Y - GameWindow.СontrolRadius); // "Минус" gameplayElement.Y из-за инверсии оси Y.

                ScaleTransform scaleTransform = (control.RenderTransform as TransformGroup).Children[0] as ScaleTransform;
                scaleTransform.ScaleX = gameplayElement.Radius;
                scaleTransform.ScaleY = gameplayElement.Radius;

                RotateTransform rotateTransform = (control.RenderTransform as TransformGroup).Children[1] as RotateTransform;
                rotateTransform.Angle = -gameplayElement.Angle; // "Минус" gameplayElement.Angle из-за инверсии угла.

                BattlefieldPanel.Children.Add(control);
            }
        }
    }

    public static class FrameProcessor
    {
        private static bool isFirstFrameUpdated = false;

        private static bool IsFirstFrameUpdated
        {
            get => isFirstFrameUpdated;
            set => isFirstFrameUpdated = value;
        }

        public static void UpdateFrame(double frameTimeInterval)
        {
            IEnumerable<GameplayElement> gameplayElements;

            if (!IsFirstFrameUpdated)
            {
                gameplayElements = GameplayProcessor.UpdateGameplay(0);
                IsFirstFrameUpdated = true;
            }
            else
            {
                gameplayElements = GameplayProcessor.UpdateGameplay(frameTimeInterval);
            }

            ControlProcessor.UpdateControls(gameplayElements);
        }
    }

    public static class InputProcessor
    {
        public static void UpdateKeyInput(KeyEventArgs e)
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
                case Key.Space:
                    {
                        UserInput.KeySpacePressed = e.IsDown;
                        break;
                    }
            }
        }
    }
}
