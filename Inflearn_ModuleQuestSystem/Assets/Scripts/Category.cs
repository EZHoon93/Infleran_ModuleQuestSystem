using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Category" , fileName = "Catergory_")]
public class Category : ScriptableObject
{
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;

    public string CodeName => codeName;
    public string DisplayName => displayName;

    public bool Equals(Category other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(other, this))
            return true;
        if (GetType() != other.GetType())
            return false;

        return codeName == other.codeName;
    }

    public override int GetHashCode()
    {
        return (CodeName, displayName).GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public static bool operator ==(Category lhs , string rhs)
    {
        if (lhs is null)
            return ReferenceEquals(rhs, null);

        return lhs.codeName == rhs || lhs.displayName == rhs;
    }

    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);

    //category.codeName == "Kill" 이런식교가아니라 아래처럼 비교가능
    // category == "kill"
}
