using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameHandler : MonoBehaviour
{
    /*public static NicknameHandler Instance { get; private set; }

    [SerializeField] NicknameText _nicknamePrefab;

    List<NicknameText> _allNicknames;

    public NicknameText AddNickname(NetworkPlayer owner)
    {
        var newNickname = Instantiate(_nicknamePrefab, transform)
                          .SetOwner(owner);

        _allNicknames.Add(newNickname);

        owner.OnLeft += () =>
        {
            _allNicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };

        return newNickname;
    }

    void LateUpdate()
    {
        foreach (var nickname in _allNicknames)
        {
            nickname.UpdatePosition();
        }
    }*/
}
