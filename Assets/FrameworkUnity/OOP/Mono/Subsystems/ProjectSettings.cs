using UnityEngine;

namespace FrameworkUnity.OOP.Mono.Subsystemsspace
{
    public class ProjectSettings : MonoBehaviour
    {
        [Header("Cursor")]
        [Tooltip("Скрыть курсор в приложении")]
        [SerializeField] private bool _cursorIsHide = false;

        [Header("FPS")]
        [Tooltip("При выставлении значения равным -1 снимается ограничение на FPS")]
        [SerializeField] private int _targetFrameRate = 144;


        void Start()
        {
            if (_cursorIsHide)
            {
                Cursor.visible = false;
            }
            SetLimitFPS();
        }

        public void SetStateCursor(bool value)
        {
            Cursor.visible = value;
        }

        private void OnValidate()
        {
            SetLimitFPS();
        }

        private void SetLimitFPS()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
