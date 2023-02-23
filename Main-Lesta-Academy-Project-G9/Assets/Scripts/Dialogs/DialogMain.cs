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
    Coroutine _currentUpdateTextCoroutine;

    private void Start()
    {
        _messageText = transform.Find("MessagePanel/MSG").GetComponent<Text>();
    }

    public void Activate(DialogAsset asset)
    {
        if (asset.arrayGroups == null || asset.arrayGroups.Length == 0) return;
        if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
        _currentDialogPosition = -1;
        _currentDialog = asset;
        transform.Find("MessagePanel").gameObject.SetActive(true);
        SelectNext();
    }

    IEnumerator TextUpdate()
    {
        while(_messageText.text != _currentDialog.arrayGroups[_currentDialogPosition].dialog)
        {
            yield return new WaitForSeconds(_updateTime);
            _messageText.text = _currentDialog.arrayGroups[_currentDialogPosition].dialog.Substring(0, _messageText.text.Length + 1);
        }
    }

    void SelectNext()
    {
        Transform answersParent = transform.Find("Answers");
        for(int i = 1; i < answersParent.childCount; i++)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }
        _dialogTime = 0f;
        _currentDialogPosition += 1;
        if (_currentDialogPosition >= _currentDialog.arrayGroups.Length)
        {
            Stop();
            return;
        }
        switch(_currentDialog.arrayGroups[_currentDialogPosition].groupType)
        {
            case DialogAsset.ArrayGroup.GroupType.DIALOG:
                _messageText.text = "";
                transform.Find("MessagePanel/CharacterName").GetComponent<Text>().text = _currentDialog.arrayGroups[_currentDialogPosition].characterName;
                _currentUpdateTextCoroutine = StartCoroutine(TextUpdate());
                if (!string.IsNullOrWhiteSpace(_currentDialog.arrayGroups[_currentDialogPosition].eventName))
                    RunEvent(_currentDialog.arrayGroups[_currentDialogPosition].eventName);
                break;
            case DialogAsset.ArrayGroup.GroupType.EVENT:
                if (!string.IsNullOrWhiteSpace(_currentDialog.arrayGroups[_currentDialogPosition].eventName))
                    RunEvent(_currentDialog.arrayGroups[_currentDialogPosition].eventName);
                break;
            case DialogAsset.ArrayGroup.GroupType.ANSWER:
                GameObject panel = transform.Find("Answers/Panel").gameObject;
                for(int i = 0; i < _currentDialog.arrayGroups[_currentDialogPosition].answers.Length; i++)
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
        Transform answersParent = transform.Find("Answers");
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
        foreach(DialogEvent de in FindObjectsOfType<DialogEvent>())
        {
            if(de.eventName == eventName)
            {
                de.Run();
            }
        }
    }

    void Stop()
    {
        Transform answersParent = transform.Find("Answers");
        for (int i = 1; i < answersParent.childCount; i++)
        {
            Destroy(answersParent.GetChild(i).gameObject);
        }
        _dialogTime = 0f;
        _currentDialogPosition = -1;
        _messageText.text = "";
        if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
        transform.Find("MessagePanel").gameObject.SetActive(false);
        _currentDialog = null;
    }

    private void Update()
    {
        if (!_currentDialog) return;
        _dialogTime += Time.deltaTime;
        if(_currentDialog.arrayGroups[_currentDialogPosition].groupType == DialogAsset.ArrayGroup.GroupType.DIALOG)
        {
            if (Input.GetMouseButtonDown(0) && _messageText.text != _currentDialog.arrayGroups[_currentDialogPosition].dialog)
            {
                if (_currentUpdateTextCoroutine != null) StopCoroutine(_currentUpdateTextCoroutine);
                _messageText.text = _currentDialog.arrayGroups[_currentDialogPosition].dialog;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (_dialogTime > _currentDialog.arrayGroups[_currentDialogPosition].minDialogTime)
                {
                    if (_currentDialog.arrayGroups[_currentDialogPosition].force > 0)
                        _currentDialogPosition = _currentDialog.arrayGroups[_currentDialogPosition].force-1;
                    SelectNext();
                }
            }
        }
        else if(_currentDialog.arrayGroups[_currentDialogPosition].groupType == DialogAsset.ArrayGroup.GroupType.EVENT)
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
