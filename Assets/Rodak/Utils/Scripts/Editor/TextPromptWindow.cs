using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rodak.Utils.Editor
{
    public class TextPromptWindow : EditorWindow
    {
        public static void ShowPrompt(string prompt, Action<string> onSubmit, Action onCancel, string defaultValue = "")
        {
            TextPromptWindow window = CreateInstance<TextPromptWindow>();
            window.Prompt = prompt;
            window.DefaultValue = defaultValue;
            window.OnSubmit = onSubmit;
            window.OnCancel = onCancel;
            window.titleContent = new GUIContent(prompt);

            window.maxSize = new(300, 100);
            window.ShowUtility();
        }

        public string Prompt = "Prompt";
        public string DefaultValue = "";

        public Action<string> OnSubmit;
        public Action OnCancel;

        private bool isSubmitted;

        private TextField input;

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            Label label = new(Prompt);
            root.Add(label);

            input = new("Value")
            {
                value = DefaultValue,
            };
            input.RegisterCallback<KeyDownEvent>((evt) =>
            {
                if (evt.keyCode == KeyCode.Return)
                {
                    Submit();
                }
            });
            root.Add(input);

            VisualElement row = new();
            row.style.flexDirection = FlexDirection.Row;
            root.Add(row);

            Button cancelButton = new()
            {
                name = "Cancel",
                text = "Cancel"
            };
            cancelButton.clicked += Cancel;
            cancelButton.style.flexGrow = 1;
            row.Add(cancelButton);

            Button submitButton = new()
            {
                name = "Submit",
                text = "Submit"
            };
            submitButton.clicked += Submit;
            submitButton.style.flexGrow = 1;
            row.Add(submitButton);

            input.Focus();
        }

        private void Cancel()
        {
            Close();
        }

        private void Submit()
        {
            OnSubmit?.Invoke(input.value);
            isSubmitted = true;
            Close();
        }

        private void OnDestroy()
        {
            if (isSubmitted) return;
            OnCancel?.Invoke();
        }
    }
}