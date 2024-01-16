using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(-50)]
public class InvestManager : SingletonMonoBehaviour<InvestManager>
{
    [Header("DEPENDENCIES")]
    [SerializeField] PigyBank pigyBank;
    [SerializeField] int gameWinCount = 25;
    [SerializeField] Collider collide;
    [SerializeField] List<ReceiverCharacter> receiverCharacters;

    private List<ReceiverCharacter> sortedList = new List<ReceiverCharacter>();
    private List<CharacterController> investers = new List<CharacterController>();

    protected override void Awake()
    {
        base.Awake();

        CheckPlacements();
    }

    public void InvestCube(ColorType colorType, Cube cube)
    {
        foreach (var receiverCharacter in receiverCharacters)
        {
            if (receiverCharacter.ColorType == colorType)
            {
                var localInvestPos = receiverCharacter.ACubeInvested(0f, cube);

                cube.Invest(receiverCharacter.transform, localInvestPos);

                CheckReceiverReachTop(receiverCharacter);
            }
        }

        CheckPlacements();
    }

    private void CheckPlacements()
    {
        sortedList = receiverCharacters.OrderByDescending(o => o.GetInvestedCubeCount()).ToList();

        foreach (var receiverCharacter in receiverCharacters)
        {
            receiverCharacter.SetPlacement(sortedList.IndexOf(receiverCharacter) + 1);
        }
    }

    private void CheckReceiverReachTop(ReceiverCharacter receiverCharacter)
    {
        if (receiverCharacter.GetInvestedCubeCount() == gameWinCount)
        {
            Transform winnerCharacterTransform = null;

            foreach (var character in investers)
            {
                if (character.GetColorType() == receiverCharacter.ColorType)
                {
                    character.WinTheGame();
                    winnerCharacterTransform = character.transform;
                }
                else
                {
                    character.LoseTheGame();
                }
            }

            foreach (var receiver in receiverCharacters)
            {
                if (receiverCharacter != receiver)
                {
                    receiver.LoseTheGame();
                }
            }

            receiverCharacter.WinTheGame();

            pigyBank.GameEnd(winnerCharacterTransform);

            collide.isTrigger = true;
        }
    }

    public void AddToInvesterList(CharacterController character)
    {
        investers.Add(character);
    }

    public void RemoveFromInvesterList(CharacterController character)
    {
        investers.Remove(character);
    }
}
