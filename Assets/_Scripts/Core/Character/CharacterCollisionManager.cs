using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-40)]
public class CharacterCollisionManager : SingletonMonoBehaviour<CharacterCollisionManager>
{
    private List<CharacterController> characters = new List<CharacterController>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void CheckCollision(CharacterController firstCharacter, GameObject secondCharacterGO)
    {
        CharacterController secondCharacter = null;

        foreach (var character in characters)
        {
            if (secondCharacterGO == character.gameObject)
            {
                secondCharacter = character;
            }
        }

        if (firstCharacter.GetStackCount() > secondCharacter.GetStackCount())
        {
            secondCharacter.DropAllCube();
        }
        else
        {
            firstCharacter.DropAllCube();
        }
    }

    public void AddToList(CharacterController character)
    {
        characters.Add(character);
    }

    public void RemoveFromList(CharacterController character)
    {
        characters.Remove(character);
    }
}
