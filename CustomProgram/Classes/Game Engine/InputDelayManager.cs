using Microsoft.Xna.Framework.Input;

namespace SpaceDefenders
{
    // Responsible for delaying keys before they can be pressed again
    public class InputDelayManager
    {
        private float _inputFrames;
        private bool _inputDelay;
        private float _delayFrames;

        public InputDelayManager()
        {
            _inputFrames = 0;
            _inputDelay = false;

        }
        // Add any function that requires updating
        public void Update()
        {
            IncrementFrame();
        }

        // Increment the delay counter
        public void IncrementFrame()
        {
            if (_inputDelay == true)
            {
                _inputFrames++;

                if (_inputFrames >= _delayFrames)
                {
                    _inputDelay = false;
                    _inputFrames = 0;
                }
            }
        }

        // Check if passed in Key is delayed
        public bool KeyDelayed(Keys key)
        {
            return _inputDelay;
        }

        // Delay the desired key by certain amount of frames
        public void DelayKey(Keys key, float delayFrames)
        {
            if (!_inputDelay)
            {
                _inputDelay = true;
                _inputFrames = 0;
                _delayFrames = delayFrames;
            }
        }

        //public bool KeyboardKeyPressed(KeyboardState state)
        //{
        //    foreach (Keys key in Enum.GetValues(typeof(Keys)))
        //    {
        //        if (state.IsKeyDown(key))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public Keys ReadInput(KeyboardState state)
        //{
        //    foreach (Keys key in state.GetPressedKeys())
        //    {
        //        return key;
        //    }
        //    return Keys.None;
        //}

    }
}
