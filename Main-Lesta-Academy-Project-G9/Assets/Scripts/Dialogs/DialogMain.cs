using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogMain : MonoBehaviour
{
    DialogAsset _currentDialog;
    int _currentDialogPosition;
    float _updateTime = 0.05f;
    float _dialogTime;
    Text _messageText;
    Text _answersText;
    AudioSource _source;
    Coroutine _currentUpdateTextCoroutine;
    private Color _noActiveColor;
    private New.InputManager _inputManager;
    public Sprite[] boxSprites;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _messageText = transform.Find("Parent/MessagePanel/MSG").GetComponent<Text>();
        _answersText = transform.Find("Parent/MessagePanel/MSG_Answers").GetComponent<Text>();
        _inputManager = FindObjectOfType<New.InputManager>();
        _noActiveColor = new Color32(125, 125, 125, 255);
    }

    public void Activate(DialogAsset asset)
    {
        if (asset.arrayGroups == null || asset.arrayGroups.Length == 0) return;
        if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
        _currentDialogPosition = -1;
        _currentDialog = asset;
        transform.Find("Parent").gameObject.SetActive(true);
        SelectNext();
        if (_inputManager != null)
        {
            _inputManager.enabled = false;
            _inputManager.GetComponent<New.PlayerMovement>().Move(0f);
        }
    }

    IEnumerator TextUpdate()
    {
        while (_messageText.text != _currentDialog.arrayGroups[_currentDialogPosition].dialog)
        {
            yield return new WaitForSeconds(_updateTime);
            _messageText.text = _currentDialog.arrayGroups[_currentDialogPosition].dialog.Substring(0, _messageText.text.Length + 1);
        }
    }

    void SelectNext()
    {
        Transform answersParent = transform.Find("Parent/MessagePanel/Answers");
        for (int i = 1; i < answersParent.childCount; i++)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }
        _dialogTime = 0f;
        _currentDialogPosition += 1;
        _source.Stop();
        if (_currentDialogPosition >= _currentDialog.arrayGroups.Length)
        {
            Stop();
            return;
        }
        switch (_currentDialog.arrayGroups[_currentDialogPosition].groupType)
        {
            case DialogAsset.ArrayGroup.GroupType.DIALOG:
                _messageText.text = "";
                _messageText.enabled = true;
                _answersText.enabled = false;
                _source.clip = _currentDialog.arrayGroups[_currentDialogPosition].clip;
                _source.Play();
                foreach (DialogAsset.ArrayCharacters ac in _currentDialog.arrayCharacters)
                {
                    if (ac.characterName == _currentDialog.arrayGroups[_currentDialogPosition].characterName)
                    {
                        Image leftImg = transform.Find("Parent/Left").GetComponent<Image>();
                        Image rightImg = transform.Find("Parent/Right").GetComponent<Image>();
                        switch (ac.position)
                        {
                            case DialogAsset.ArrayCharacters.CharacterPosition.LEFT:
                                leftImg.sprite = ac.sprite;
                                leftImg.color = Color.white;
                                rightImg.color = rightImg.sprite != null ? _noActiveColor : Color.clear;
                                transform.Find("Parent/MessagePanel").GetComponent<Image>().sprite = boxSprites[0];
                                break;
                            case DialogAsset.ArrayCharacters.CharacterPosition.RIGHT:
                                rightImg.sprite = ac.sprite;
                                rightImg.color = Color.white;
                                leftImg.color = leftImg.sprite != null ? _noActiveColor : Color.clear;
                                transform.Find("Parent/MessagePanel").GetComponent<Image>().sprite = boxSprites[1];
                                break;
                        }
                    }
                }
                transform.Find("Parent/MessagePanel/CharacterName").GetComponent<Text>().text = _currentDialog.arrayGroups[_currentDialogPosition].hideName ? "???" : _currentDialog.arrayGroups[_currentDialogPosition].characterName;
                _currentUpdateTextCoroutine = StartCoroutine(TextUpdate());
                if (!string.IsNullOrWhiteSpace(_currentDialog.arrayGroups[_currentDialogPosition].eventName))
                    RunEvent(_currentDialog.arrayGroups[_currentDialogPosition].eventName);
                break;
            case DialogAsset.ArrayGroup.GroupType.EVENT:
                if (!string.IsNullOrWhiteSpace(_currentDialog.arrayGroups[_currentDialogPosition].eventName))
                    RunEvent(_currentDialog.arrayGroups[_currentDialogPosition].eventName);
                break;
            case DialogAsset.ArrayGroup.GroupType.ANSWER:
                _messageText.enabled = false;
                _answersText.text = _messageText.text;
                _answersText.enabled = true;
                GameObject panel = transform.Find("Parent/MessagePanel/Answers/Panel").gameObject;
                for (int i = 0; i < _currentDialog.arrayGroups[_currentDialogPosition].answers.Length; i++)
                {
                    GameObject inst = Instantiate(panel, answersParent);
                    inst.GetComponentInChildren<Text>().text = _currentDialog.arrayGroups[_currentDialogPosition].answers[i].msg;
                    int x = i;
                    inst.GetComponent<Button>().onClick.AddListener(() => { SelectAnswer(inst); });
                    inst.SetActive(true);
                }
                break;
        }
    }

    void SelectAnswer(GameObject obj)
    {
        Transform answersParent = transform.Find("Parent/MessagePanel/Answers");
        int pos = 0;
        for (int i = 1; i < answersParent.childCount; i++)
        {
            if (obj == answersParent.GetChild(i).gameObject)
            {
                pos = i - 1;
                break;
            }
        }
        _currentDialogPosition = _currentDialog.arrayGroups[_currentDialogPosition].answers[pos].force - 1;
        SelectNext();
    }

    void RunEvent(string eventName)
    {
        foreach (DialogEvent de in FindObjectsOfType<DialogEvent>())
        {
            if (de.eventName == eventName)
            {
                de.Run();
            }
        }
    }

    void Stop()
    {
        Transform answersParent = transform.Find("Parent/MessagePanel/Answers");
        for (int i = 1; i < answersParent.childCount; i++)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }
        _dialogTime = 0f;
        _currentDialogPosition = -1;
        _messageText.text = "";
        if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
        transform.Find("Parent").gameObject.SetActive(false);
        _currentDialog = null;
        if (_inputManager != null)
            _inputManager.enabled = true;
    }

    private void Update()
    {
        if (!_currentDialog) return;
        _dialogTime += Time.deltaTime;
        if (_currentDialog.arrayGroups[_currentDialogPosition].groupType == DialogAsset.ArrayGroup.GroupType.DIALOG)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _messageText.text != _currentDialog.arrayGroups[_currentDialogPosition].dialog)
            {
                if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
                _messageText.text = _currentDialog.arrayGroups[_currentDialogPosition].dialog;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (_dialogTime > _currentDialog.arrayGroups[_currentDialogPosition].minDialogTime)
                {
                    if (_currentDialog.arrayGroups[_currentDialogPosition].force > 0)
                        _currentDialogPosition = _currentDialog.arrayGroups[_currentDialogPosition].force - 1;
                    SelectNext();
                }
            }
        }
        else if (_currentDialog.arrayGroups[_currentDialogPosition].groupType == DialogAsset.ArrayGroup.GroupType.EVENT)
        {
            if (_dialogTime > _currentDialog.arrayGroups[_currentDialogPosition].minDialogTime)
            {
                if (_currentDialog.arrayGroups[_currentDialogPosition].force > 0)
                    _currentDialogPosition = _currentDialog.arrayGroups[_currentDialogPosition].force - 1;
                SelectNext();
            }
        }
    }
}
