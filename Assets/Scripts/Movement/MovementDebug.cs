using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementDebug : MonoBehaviour
{
    [System.Serializable]
    public class VectorDisplay
    {
        [SerializeField]
        private Text _valueText;

        [SerializeField]
        private RectTransform _dot;

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private float _scale;

        public void SetValue(float x, float y)
        {
            Vector2 vec = new Vector2(x, y);

            _valueText.text = vec.magnitude.ToString("n3");

            _dot.anchoredPosition = Vector2.ClampMagnitude(vec / _scale, 1) * _maxDistance;
        }
    }

    [SerializeField]
    private PersonMovement _movement;

    [SerializeField]
    private VectorDisplay _inputDisplay;

    [SerializeField]
    private VectorDisplay _desiredDisplay;

    [SerializeField]
    private VectorDisplay _actualDisplay;

    [SerializeField]
    private Text _isGroundedText;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVec = Vector2.ClampMagnitude(inputVec, 1);

        _inputDisplay.SetValue(inputVec.x, inputVec.y);

        _desiredDisplay.SetValue(_movement.DesiredVelocity.x, _movement.DesiredVelocity.z);
        _actualDisplay.SetValue(_movement.ActualVelocity.x, _movement.ActualVelocity.z);

        _isGroundedText.text = _movement.IsGrounded ? "Is Grounded" : "Is Airbourne";
    }
}
