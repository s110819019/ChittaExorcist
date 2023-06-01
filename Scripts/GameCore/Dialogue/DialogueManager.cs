using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using ChittaExorcist.PlayerSettings.InputHandler;
using ChittaExorcist.Common.Module;
using ChittaExorcist.UISettings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace ChittaExorcist.GameCore.DialogueSettings
{
    public class DialogueManager : MonoSingleton<DialogueManager>
    {
        #region w/ Const

        private const string SPEAKER_TAG = "speaker";
        private const float FADE_ANIMATION_TIME = 0.5f;

        #endregion

        #region w/ Varialbles

        private Story _currentStory = null;
        // private PlayerInputHandler _inputHandler;
        private UIInputHandler _uiInputHandler;

        private Coroutine _displayLineCoroutine;
        private bool _canContinueToNextLine = false;
        public bool IsPlayingDialogue { get; private set; }

        #endregion

        #region w/ Components

        [Header("Params")] [SerializeField] private float typingSpeed = 0.04f;

        [Header("Dialogue UI")] [SerializeField] private Canvas dialoguePanel;
        [SerializeField] private GameObject dialogueFrame;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [Header("Choices UI")] [SerializeField] private GameObject[] choices;
        private TextMeshProUGUI[] _choicesText;

        [Header("Display Name")] [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private GameObject dialogueSpeakerFrame;
        
        private CanvasGroup _dialoguePanelGroup;
        private CanvasGroup[] _choicesGroup;

        private Animator _dialogueFrameAnimator;
        private Animator _dialogueSpeakerFrameAnimator;

        private void SetDialoguePanelGroupState(bool value)
        {
            _dialoguePanelGroup.DOKill();
            if (value) // 啟用淡入
            {
                dialoguePanel.enabled = true;
                _dialoguePanelGroup.alpha = 0;
                _dialoguePanelGroup.DOFade(1, FADE_ANIMATION_TIME)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true);
            }
            else // 關閉淡出
            {
                _dialoguePanelGroup.DOFade(0, FADE_ANIMATION_TIME)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .OnComplete(() => dialoguePanel.enabled = false);
            }
        }

        #endregion

        #region w/ Singleton

        // private static DialogueManager _instance;
        // public static DialogueManager GetInstance() => _instance;
        //
        // private void SetInstance()
        // {
        //     if (_instance != null)
        //     {
        //         Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        //     }
        //
        //     _instance = this;
        // }

        #endregion

        #region w/ Unity Callback Functions

        // private void Awake()
        // {
        //     SetInstance();
        // }

        private void Start()
        {
            IsPlayingDialogue = false;
            dialoguePanel.enabled = false;

            _choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (GameObject choice in choices)
            {
                _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }

            TryGetComponent(out _uiInputHandler);

            _dialoguePanelGroup = dialoguePanel.GetComponent<CanvasGroup>();

            dialogueFrame.TryGetComponent(out _dialogueFrameAnimator);
            dialogueSpeakerFrame.TryGetComponent(out _dialogueSpeakerFrameAnimator);
        }

        private void Update()
        {
            if (!IsPlayingDialogue) return;

            if (_canContinueToNextLine
                && _currentStory.currentChoices.Count == 0
                && _uiInputHandler.SubmitInput)
            {
                _uiInputHandler.UseSubmitInput();
                ContinueStory();
            }

            // TODO: 待修正文字顏色
            for (int i = 0; i < choices.Length; i++)
            {
                if (EventSystem.current.currentSelectedGameObject == choices[i])
                {
                    // Debug.Log(this.choices[i].name + " was selected");
                    // _choicesText[i].color = new Color(252, 250, 242);
                    _choicesText[i].color = Color.white;
                }
                else
                {
                    _choicesText[i].color = Color.black;
                }
            }
        }

        #endregion

        #region w/ Dialogue

        public void EnterDialogueMode(TextAsset inkJSON)
        {
            InputManager.Instance.SwitchToUIplayMap();
            _currentStory = new Story(inkJSON.text);
            IsPlayingDialogue = true;

            // dialoguePanel.SetActive(true);
            SetDialoguePanelGroupState(true);

           _dialogueFrameAnimator.Play("Dialogue_Frame_Start");
           _dialogueSpeakerFrameAnimator.Play("Dialogue_SpeakerFrame_Start");

            ContinueStory();
        }

        public IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);

            IsPlayingDialogue = false;

            // dialoguePanel.SetActive(false);
            SetDialoguePanelGroupState(false);

            dialogueText.text = "";
            InputManager.Instance.SwitchToGameplayMap();
            UIInput.Instance.DisableAllUIInputs();
        }

        #endregion

        #region w/ Line

        private IEnumerator DisplayLine(string line)
        {
            // empty the dialogue text
            // dialogueText.text = "";


            // set the text to the full line, but set the visible characters to 0
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;


            // hide items while text is typing
            HideChoices();
            _canContinueToNextLine = false;

            // display each letter once at a time
            foreach (char letter in line.ToCharArray())
            {
                if (_uiInputHandler.SubmitInput)
                {
                    // TODO: 沒有這行的話有 bug
                    _uiInputHandler.UseSubmitInput();

                    // dialogueText.text = line;

                    dialogueText.maxVisibleCharacters = line.Length;

                    break;
                }

                // dialogueText.text += letter;
                dialogueText.maxVisibleCharacters++;

                yield return new WaitForSeconds(typingSpeed);
            }

            // display choices, if any, for this dialogue line
            DisplayChoices();

            _canContinueToNextLine = true;
        }

        #endregion

        #region w/ Story

        private void ContinueStory()
        {
            if (_currentStory.canContinue)
            {
                // set text for the current dialogue line
                // dialogueText.text = _currentStory.Continue();

                if (_displayLineCoroutine != null)
                {
                    StopCoroutine(_displayLineCoroutine);
                }

                _displayLineCoroutine = StartCoroutine(DisplayLine(_currentStory.Continue()));

                // // display choices, if any, for this dialogue line
                // DisplayChoices();

                // handle tags
                HandleTags(_currentStory.currentTags);
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        #region w/ Tags

        private void HandleTags(List<string> currentTags)
        {
            foreach (string tag in currentTags)
            {
                string[] splitTag = tag.Split(":");
                if (splitTag.Length != 2)
                {
                    Debug.Log("Tag could not be appropriately parsed : " + tag);
                }

                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                // handle the tag
                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        // Debug.Log("speaker = " + tagValue);
                        displayNameText.text = tagValue;
                        break;
                    default:
                        Debug.LogWarning("Tag came in but is not currently being handled : " + tag);
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region w/ Choices

        // 設定預選選項
        private IEnumerator SelectFirstChoice()
        {
            // Event System requires we clear it first, then wait
            // for at least one frame before we set the current selected object.
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        }

        // 根據索引來選擇選項
        public void MakeChoice(int choiceIndex)
        {
            if (_canContinueToNextLine)
            {
                _currentStory.ChooseChoiceIndex(choiceIndex);
                _uiInputHandler.UseSubmitInput();
                ContinueStory();
            }
        }

        // 顯示選項 UI
        private void DisplayChoices()
        {
            List<Choice> currentChoices = _currentStory.currentChoices;

            // 確認是否超過 UI 支援的數量
            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("More choices were given than the UI can support. Number of choices given: " +
                               currentChoices.Count);
            }

            int index = 0;
            // 對應選項與 UI
            foreach (Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                _choicesText[index].text = choice.text;
                index++;
            }

            // 確認隱藏剩餘的選項
            for (int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }

            StartCoroutine(SelectFirstChoice());
        }

        private void HideChoices()
        {
            foreach (GameObject choiceButton in choices)
            {
                choiceButton.SetActive(false);
            }
        }

        #endregion
    }
}