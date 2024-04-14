using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CardGame;
    
    public Texture2D[] cardImages;
    // Start is called before the first frame update
    void Start()
    {
        //StartCardGame(10, cardImages);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Texture2D image in cardImages)
        {
            Debug.Log(image.width); Debug.Log(image.height);
        }
       
    }



   /* public void StartCardGame(int numberOfCardsToStartWith, Texture2D[] texturesToPutOnCards)
    {
        CardGameMaker cardGameMakerToStart;

        if (CardGame.GetComponent<CardGameMaker>() == null)
        {
            Debug.Log("this object isn't a cardgame");
            return;
        }
        else
        {
            cardGameMakerToStart = CardGame.GetComponent<CardGameMaker>();
        }
        cardGameMakerToStart.numberOfCards = numberOfCardsToStartWith;
        cardGameMakerToStart.imagesToPutOnCards = texturesToPutOnCards;
        CardGame.SetActive(true);
       
    }*/
}
