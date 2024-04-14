using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CardGameMaker : MonoBehaviour
{
    [Header("Card Making Information")]
    public int numberOfCards;                                                                              //Information needed for card fabrication  machine( (
    public  List<Sprite> imagesToPutOnCards;
    [SerializeField] MatchingCard baseCardPrefab;        //<--------must hve a matchingCard Class                                           ) )//

	[Range(0, 1)]                                                                                    //Information needed for card displaying machine( (
	public float spaceBetweenCards;
	public Camera cameraToPrintCardInFront;                                                                      //                ) )
    public float distanceInfrontOfCamera = 10f;
	
    public Vector4 areaToDrawCards;

    Vector2 formationChosenForSpawn;
	Dictionary<Vector2, MatchingCard> dictionaryOfCardsAndIndexes = new Dictionary<Vector2, MatchingCard>();

	 

    void OnEnable()
	{   
        numberOfCards =CheckNumberAndMakeItEven(numberOfCards);
        //Initalizion thought.
		formationChosenForSpawn = CalculateARandomCardFormationBasedOnNumberOfCards(numberOfCards);

        dictionaryOfCardsAndIndexes = MakeCardsAndGiveThemImageAndIndex(baseCardPrefab, imagesToPutOnCards, formationChosenForSpawn);




		FigureOutWorldPositionAndSizeToFitThisCamera(
          dictionaryOfCardsAndIndexes, formationChosenForSpawn, spaceBetweenCards, cameraToPrintCardInFront, distanceInfrontOfCamera, areaToDrawCards);




	}
	private void Update()
	{
	


	}

	Vector2 CalculateARandomCardFormationBasedOnNumberOfCards(int numberOfCards)                         //used by (card maker) and (positioner and sizer)
    {
		int evenNumberOfCards = CheckNumberAndMakeItEven(numberOfCards);
		List<Vector2> possibleCardFormation = FigureOutMultiplicandAndMultiplierFromProduct(evenNumberOfCards);
        Vector2 formationChosenForSpawn = possibleCardFormation[Random.Range(0, possibleCardFormation.Count)];

      
		return formationChosenForSpawn;
	}





	Dictionary<Vector2, MatchingCard> MakeCardsAndGiveThemImageAndIndex    (MatchingCard cardPrefabToInstantiate, List<Sprite> imagesToPutOnNewCards ,Vector2 cardFormation)
	{
        Dictionary<Vector2, MatchingCard> theCardsInformationIndex = new Dictionary<Vector2, MatchingCard>();              //[                                           

		int numberOfCardsInFormation = (int)cardFormation.x * (int)cardFormation.y;
		List<Sprite> ListOfimagesThatsTheSameSizeAsCards = MakeTheRightImageListForNumberOfCard(numberOfCardsInFormation, imagesToPutOnNewCards);

		GameObject spawnedCardGroup = new GameObject("spawned Card Group");                                                                      //      Preparation phase
		                                                                                                              //                          ]

		for (int currentXAxisCount = 1; currentXAxisCount <= cardFormation.x; currentXAxisCount++)
		{
			for (int currentYAxisCount = 1; currentYAxisCount <= cardFormation.y; currentYAxisCount++)
            {
				MatchingCard theNewCard = Instantiate(cardPrefabToInstantiate.gameObject).GetComponent<MatchingCard>();
				theNewCard.name = "Card " + currentXAxisCount.ToString() +","+ currentYAxisCount.ToString();
                theNewCard.transform.parent = spawnedCardGroup.transform;

                int indexOfChosenImage = Random.Range(0, ListOfimagesThatsTheSameSizeAsCards.Count);
				Sprite imageChosenToPlace = ListOfimagesThatsTheSameSizeAsCards[indexOfChosenImage];
				theNewCard.changableFrontImge.sprite = imageChosenToPlace;
                theNewCard.nameOfImageAttacked = imageChosenToPlace.name;               
				ListOfimagesThatsTheSameSizeAsCards.RemoveAt(indexOfChosenImage);

				theNewCard.transform.position = new Vector3(currentXAxisCount, currentYAxisCount, 0);

				theCardsInformationIndex.Add(new Vector2(currentXAxisCount, currentYAxisCount), theNewCard);
			}
		}
        return theCardsInformationIndex;
        
	}
    List<Sprite> MakeTheRightImageListForNumberOfCard(int cardNumber, List<Sprite> imagesToPutOnCards)
    {
        List<Sprite> workingImageList = new List<Sprite>();

        workingImageList.AddRange(imagesToPutOnCards);		
		
		List<Sprite> finalImageList = new List<Sprite>();

		for (int currentnumberInFinaList = 0; currentnumberInFinaList < cardNumber; currentnumberInFinaList += 2)
        {
            if(workingImageList.Count == 0)
            {
				workingImageList.AddRange(imagesToPutOnCards);
				
			}
            int indexOfCardToTake = Random.Range(0, workingImageList.Count);
            finalImageList.Add(workingImageList[indexOfCardToTake]);
			finalImageList.Add(workingImageList[indexOfCardToTake]);
            workingImageList.RemoveAt(indexOfCardToTake);

			//insertRandomimageIntoFinalTwiceANdREmoveFOrmWOrking.
		      
        }
		return finalImageList;
    }




    /// ////////////////////


	void DrawCardsOnScreen(GameObject CameraToGetScreenSize,Vector2 cardFormation,float spaceBetweencards)
    {

    }

    void FigureOutWorldPositionAndSizeToFitThisCamera(Dictionary<Vector2, MatchingCard> theCardsToPLaceWithTheirIndex,Vector2 howManycardsInXaxisAndYAxis, float spaceBetweenCards, Camera cameraToDrawCardInFront, float distanceInFrontOfCameraToDrawCardsIn,Vector4 areaOnScreenToDrawCardsInLftRigtDonUp)
    {
		List<Vector3> verticesOfCardDrawAreaInWOrldUnit = 
			FigureOutVerticesOfAreaInsideOfCamera_BTLFT_BTRGT_UPLFT_UpRGT(cameraToDrawCardInFront, distanceInFrontOfCameraToDrawCardsIn, areaOnScreenToDrawCardsInLftRigtDonUp);

		Vector2 sizeOfCanvas = new Vector2(Vector3.Distance(verticesOfCardDrawAreaInWOrldUnit[0], verticesOfCardDrawAreaInWOrldUnit[1]), Vector3.Distance(verticesOfCardDrawAreaInWOrldUnit[0], verticesOfCardDrawAreaInWOrldUnit[2]));  //FigureOutScreenWorldUnitSizeInACertainDisntanceInFront(cameraToDrawCardInFront, distanceInFrontOfCameraToDrawCardsIn,areaOnScreenToDrawCardsInLftRigtDonUp);
		

		float theSizeOfCardsToFitInYAxis = FigureOutScaleOfCardsInAxisOfCertainSize(sizeOfCanvas.y, theCardsToPLaceWithTheirIndex[new Vector2(1, 1)].baseFrontImage.bounds.size.y,(int)howManycardsInXaxisAndYAxis.y ,spaceBetweenCards);
		float theSizeOfCArdsToFitInXAxis = FigureOutScaleOfCardsInAxisOfCertainSize(sizeOfCanvas.x, theCardsToPLaceWithTheirIndex[new Vector2(1, 1)].baseFrontImage.bounds.size.x, (int)howManycardsInXaxisAndYAxis.x, spaceBetweenCards);
        float theSizeToBeUsedForScalling = Mathf.Min(theSizeOfCardsToFitInYAxis, theSizeOfCArdsToFitInXAxis);

		Vector3 basicCardSize = theCardsToPLaceWithTheirIndex[new Vector2(1, 1)].baseFrontImage.bounds.size;
		Vector3 WorldUnitOfCardAfterScalling = basicCardSize* theSizeToBeUsedForScalling;
		Debug.Log(WorldUnitOfCardAfterScalling);

		foreach (Vector2 index in theCardsToPLaceWithTheirIndex.Keys)
		{


			theCardsToPLaceWithTheirIndex[index].transform.localScale = new Vector3(theSizeOfCArdsToFitInXAxis, theSizeOfCardsToFitInYAxis, 1f);
			//Debug.Log(theCardsToPLaceWithTheirIndex[index].baseFrontImage.bounds.size);
		}
		




		//Debug.Log(theSizeOfCArdsToFitInXAxis  );

		/*  Debug.Log(theSizeOfCardsToFitInYAxis);

		  foreach (MatchingCard vv in theCardsToPLaceWithTheirIndex.Values)
		  {
			  vv.transform.localScale = new Vector3(theSizeOfCArdsToFitInXAxis, theSizeOfCardsToFitInYAxis, 1f);
		  }*/



		/* Vector2 wordUnitSizeOfScaledCard =Vector2.Scale( cardsToPLace.worldUnitsizeOfCard,scaleOfCard);

		 Vector2[] cardPosition;
		 void FigurePositionsOfCardsOfSpecificIndex(Vector2 wordUnitSizeOfScaledCard, Vector3 scaleOfCard, Vector2 cardIndex, Vector2 totalWorldUnitsOfCanvas)
		 {
			 float xAxisPositionCounter = wordUnitSizeOfScaledCard.x / 2;
			 for(int currentCard = 1; currentCard < cardIndex.x; currentCard++)
			 {
				// xAxisPositionCounter +=
			 }
		 }*/
	}

	float FigureOutScaleOfCardsInAxisOfCertainSize(float totalWorldUnitsOfAxis, float worldUnitSizeOfCardInThatAxis, int numberOfCardsToFitIn, float spaceBetweenCardsRatio)
	{
		float sizeOfAxisTakenBySpceBetween = totalWorldUnitsOfAxis * spaceBetweenCardsRatio;
		float sizeOfAxisLeftForCards = totalWorldUnitsOfAxis - sizeOfAxisTakenBySpceBetween;
		float sizeOfEachCardInWorldUnits = sizeOfAxisLeftForCards / numberOfCardsToFitIn;

        float scaleOfEachCard = sizeOfEachCardInWorldUnits / worldUnitSizeOfCardInThatAxis;
		
        return scaleOfEachCard;

		// Debug.Log(baseCardPrefab.GetComponent<MatchingCard>().baseFrontImage.sprite.texture.name);
	}
	

	//Use Criteria cards pivot is at centre of image
	
	List<Vector3> FigureOutVerticesOfAreaInsideOfCamera_BTLFT_BTRGT_UPLFT_UpRGT(Camera cameraToFindItsSize, float distanceInFrontOfCamera, Vector4 screenAreaToUseFromTotalInLftRigtDonUp)
	{
		Vector3 buttonLeftPoint = cameraToFindItsSize.ScreenToWorldPoint(new Vector3(0 + cameraToFindItsSize.scaledPixelWidth * screenAreaToUseFromTotalInLftRigtDonUp.x, 0 + cameraToFindItsSize.scaledPixelHeight * screenAreaToUseFromTotalInLftRigtDonUp.z, distanceInFrontOfCamera));
		Vector3 buttonRightPoint = cameraToFindItsSize.ScreenToWorldPoint(new Vector3(cameraToFindItsSize.scaledPixelWidth - (cameraToFindItsSize.scaledPixelWidth * screenAreaToUseFromTotalInLftRigtDonUp.y), 0 + cameraToFindItsSize.scaledPixelHeight * screenAreaToUseFromTotalInLftRigtDonUp.z, distanceInFrontOfCamera));
		Vector3 topLeftPoint = cameraToFindItsSize.ScreenToWorldPoint(new Vector3(0 + cameraToFindItsSize.scaledPixelWidth * screenAreaToUseFromTotalInLftRigtDonUp.x, 0 + cameraToFindItsSize.scaledPixelHeight - (cameraToFindItsSize.scaledPixelHeight * screenAreaToUseFromTotalInLftRigtDonUp.w), distanceInFrontOfCamera));
		Vector3 topRightPoint = cameraToFindItsSize.ScreenToWorldPoint(new Vector3(cameraToFindItsSize.scaledPixelWidth - (cameraToFindItsSize.scaledPixelWidth * screenAreaToUseFromTotalInLftRigtDonUp.y), 0 + cameraToFindItsSize.scaledPixelHeight - (cameraToFindItsSize.scaledPixelHeight * screenAreaToUseFromTotalInLftRigtDonUp.w), distanceInFrontOfCamera));

		List<Vector3> pointsOfAreaInsideAnother = new List<Vector3>
		{
			buttonLeftPoint,
			buttonRightPoint,
			topLeftPoint,
			topRightPoint
		};


		return pointsOfAreaInsideAnother;
	}




	/*List<Vector2> FigureOutScreenWorldUnitSizeInACertainDisntanceInFront(List<Vector3> screenAreaToUseFromTotalInLftRigtDonUp)        //in orthographic camera the float distance doesn't matter.
    {
        

		float camerWidthUnitArea = Vector3.Distance(cameraToFindItsSize.ScreenToWorldPoint(new Vector3(cameraToFindItsSize.scaledPixelWidth, 0, distanceInFrontOfCamera)), Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceInFrontOfCamera)));
        float camerHeightUnitArea = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, cameraToFindItsSize.scaledPixelHeight, distanceInFrontOfCamera)), Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceInFrontOfCamera)));

		return pointsOfAreaInsideAnother;                  // new Vector2(camerWidthUnitArea, camerHeightUnitArea);
    }*/


	private void OnDrawGizmos()
	{
		foreach (Vector3 v in FigureOutVerticesOfAreaInsideOfCamera_BTLFT_BTRGT_UPLFT_UpRGT(cameraToPrintCardInFront,distanceInfrontOfCamera,areaToDrawCards))
		{
			Gizmos.DrawSphere(v,0.2f);
		}
	}




	/// /////////////////////////////////////////


	int CheckNumberAndMakeItEven(int numberToCheck)
    {
        if (numberToCheck % 2 == 0)
        {
            return numberToCheck;
        }
        else
        {
            return CheckNumberAndMakeItEven(numberToCheck + 1);
        }
    }



    List<Vector2> FigureOutMultiplicandAndMultiplierFromProduct (int productToUse)
    {
        var listOfPossibleMultiplication = new List<Vector2>();

        if (productToUse < 0)
        {
            Debug.Log("function FigureOutMultiplicandAndMultiplierFromProduct only works with positive.");
            return listOfPossibleMultiplication;
        }


        for (int currentMultiplicandToTry = productToUse ;     currentMultiplicandToTry>0 ;      currentMultiplicandToTry-=1)
        {
            if ((productToUse % currentMultiplicandToTry) == 0)
            {
                int multiplierOfOpertion = productToUse / currentMultiplicandToTry;
                listOfPossibleMultiplication.Add(new Vector2(currentMultiplicandToTry, multiplierOfOpertion));                                     //the division is even go along.
            }
        }


   
        return listOfPossibleMultiplication;
    }


   


}
