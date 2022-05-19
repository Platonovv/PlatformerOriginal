using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.HUD.Dialogs;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;


        private DialogBoxController _dialogBox;
        public void Show()
        {
            _dialogBox = FiindDialogController();
            _dialogBox.ShowDialog(Data);
        }

        private DialogBoxController FiindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject controllerGo;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                default:
                    throw new AggregateException("Undefined dialog type");
            }

            return _dialogBox = controllerGo.GetComponent<DialogBoxController>();
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }
        
        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public enum Mode
        {
            Bound,
            External
        }
    }
}