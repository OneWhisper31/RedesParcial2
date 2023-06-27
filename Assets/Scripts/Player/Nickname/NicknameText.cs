using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    const float HEAD_OFFSET = 2.5f;
    
    Transform _owner;

    Text _myText;

    public NicknameText SetOwner(NetworkPlayer owner)
    {
        _owner = owner.transform;

        _myText = GetComponent<Text>();

        return this;
    }

    public void UpdateText(string str)
    {
        _myText.text = str;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * HEAD_OFFSET;
    }
}
