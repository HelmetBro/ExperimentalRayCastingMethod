using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace The_E_Project.Animation
{
    public class Animation
    {
        private readonly List<AnimationFrame> _frames = new List<AnimationFrame>();
        private TimeSpan _timeIntoAnimation;
        public string Name;

        private TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in _frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;

                // See if we can find the frame
                var accumulatedTime = new TimeSpan();
                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.Duration >= _timeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    accumulatedTime += frame.Duration;
                }

                // If no frame was found, then try the last frame, 
                // just in case timeIntoAnimation somehow exceeds Duration
                if (currentFrame == null)
                {
                    currentFrame = _frames.LastOrDefault();
                }

                // If we found a frame, return its rectangle, otherwise
                // return an empty rectangle (one with no width or height)
                if (currentFrame != null)
                {
                    return currentFrame.SourceRectangle;
                }
                return Rectangle.Empty;
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            var newFrame = new AnimationFrame
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            _frames.Add(newFrame);
        }

        public void Update(GameTime gameTime)
        {
            var secondsIntoAnimation =
                _timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            var remainder = secondsIntoAnimation%Duration.TotalSeconds;

            _timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }
    }
}