using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaLibrary
{

    public enum BUTTON_STATE
    {
        RELEASED,
        PRESSED,
        OFF,
    }
    public enum BUTTON_SELECT
    {
        SELECTED,
        NONE,
    }

    public class Button : Entity
    {
        protected LOCK _lock;
        protected BUTTON_STATE _state;
        protected BUTTON_SELECT _selection;
        protected Sprite _buttonSprite;
        protected MouseInput _mouseInput;

        public Button(Sprite sprite, MouseInput mouseInput)
        {
            _buttonSprite = sprite;
            _buttonSprite.BindParent(this);
            _mouseInput = mouseInput;
            _state = BUTTON_STATE.RELEASED;
            _selection = BUTTON_SELECT.NONE;
            _lock = LOCK.LOCKED;
        }
        public virtual void Clear()
        {
            //SpriteManager.DeleteSprite(ref _buttonSprite);
        }
        public virtual void Lock()
        {
            _lock = LOCK.LOCKED;
            OnLock();
        }
        public virtual void Unlock()
        {
            _lock = LOCK.UNLOCKED;
            OnUnlock();
        }

        public virtual void Update()
        {
            if (_lock == LOCK.UNLOCKED)
            {
                switch (_state)
                {
                    case BUTTON_STATE.RELEASED:
                        UpdateReleased();
                        break;
                    case BUTTON_STATE.PRESSED:
                        UpdatePressed();
                        break;
                }

                if (_lock == LOCK.UNLOCKED)
                    switch (_selection)
                    {
                        case BUTTON_SELECT.SELECTED:
                            UpdateSelected();
                            break;
                        case BUTTON_SELECT.NONE:
                            UpdateNotSelected();
                            break;
                    }
            }
            else
                UpdateLocked();
        }

        protected virtual void OnSelect() { _selection = BUTTON_SELECT.SELECTED; }
        protected virtual void OnDeselect() { _selection = BUTTON_SELECT.NONE; }
        protected virtual void OnPress() { _state = BUTTON_STATE.PRESSED; }
        protected virtual void OnLock() { _lock = LOCK.LOCKED; OnDeselect(); }
        protected virtual void OnUnlock() { _lock = LOCK.UNLOCKED; }
        protected virtual void OnRelease() { _state = BUTTON_STATE.RELEASED; }

        protected virtual void UpdateNotSelected()
        {
            if (_mouseInput.MouseOverArea(_buttonSprite.BoundingRectangle))
                OnSelect();
        }
        protected virtual void UpdateSelected()
        {
            if (!_mouseInput.MouseOverArea(_buttonSprite.BoundingRectangle))
                OnDeselect();
        }
        protected virtual void UpdatePressed()
        {
            if (!_mouseInput.ClickInArea(CLICK_TYPE.LEFT, _buttonSprite.BoundingRectangle))
                OnRelease();
        }
        protected virtual void UpdateReleased()
        {
            if (_mouseInput.ClickInArea(CLICK_TYPE.LEFT, _buttonSprite.BoundingRectangle))
                OnPress();
        }
        protected virtual void UpdateLocked()
        {

        }

    }
}
