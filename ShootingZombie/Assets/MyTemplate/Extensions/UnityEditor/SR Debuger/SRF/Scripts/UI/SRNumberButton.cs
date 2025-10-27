namespace SRF.UI
{
    using Internal;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [AddComponentMenu(ComponentMenuPaths.NumberButton)]
    public class SRNumberButton : UnityEngine.UI.Button, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public double Amount = 1;
        public SRNumberSpinner TargetField;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (!interactable)
            {
                return;
            }

            Apply();
        }

        private void Apply()
        {
            var currentValue = double.Parse(TargetField.text);
            currentValue += Amount;

            if (currentValue > TargetField.MaxValue)
            {
                currentValue = TargetField.MaxValue;
            }
            if (currentValue < TargetField.MinValue)
            {
                currentValue = TargetField.MinValue;
            }

            TargetField.text = currentValue.ToString();
            TargetField.onEndEdit.Invoke(TargetField.text);
        }
    }
}
