using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaLibrary
{
    public class SwitchButton : Button
    {
        public SwitchButton(Sprite sprite, MouseInput mouseInput) : base(sprite, mouseInput)
        {        
        }

        protected override void UpdateReleased()
        {
            if (_mouseInput.SingleClickInArea(CLICK_TYPE.LEFT, _buttonSprite.BoundingRectangle))
                OnPress();
        }
        protected override void UpdatePressed()
        {
            if (_mouseInput.SingleClickInArea(CLICK_TYPE.LEFT, _buttonSprite.BoundingRectangle))
                OnRelease();
        }
    }
}
