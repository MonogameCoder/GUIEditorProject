
using Microsoft.Xna.Framework.Input;
using _GUIProject.UI;
using System;
using _GUIProject.Events;

namespace _GUIProject
{
    public class InputManager
    {
        public KeyboardState State
        {
            get { return _currentKeyState; }
        }      
       
        public Keys CurrentKey { get; set; }
        public bool IsReceivingInput { get; private set; }

        private KeyboardState _currentKeyState, _prevKeyState;


        public void Update()
        {
            _prevKeyState = _currentKeyState;
            
            _currentKeyState = Keyboard.GetState();
           
            if (_currentKeyState.GetPressedKeyCount() > 0)
            {
                CurrentKey = _currentKeyState.GetPressedKeys()[_currentKeyState.GetPressedKeyCount() - 1];            
            }
         
            if (_prevKeyState.IsKeyUp(CurrentKey))
            {
                CurrentKey = Keys.None;
                IsReceivingInput = false;
            }
        }
        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (_currentKeyState.IsKeyDown(key) && _prevKeyState.IsKeyUp(key))
                {                    
                    return true;
                }
                   
            }            
            return false;
        }
        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if(_currentKeyState.IsKeyUp(key) && _prevKeyState.IsKeyDown(key))
                {                    
                    return true;
                }
                
            }         
            return false;
        }
        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if(_currentKeyState.IsKeyDown(key))
                {                    
                   return true;
                }
            }           
            return false;
        }
    }
}
