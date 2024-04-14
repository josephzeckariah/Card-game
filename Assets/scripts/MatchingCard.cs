using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MatchingCard : MonoBehaviour         //this script uses AlwaysUtilities static script.
{
     public CardGameMaker theGameThisCardBelongsTo ;
   
                                 //define and know parts of the objects to work with.
    public SpriteRenderer baseFrontImage;
    public SpriteRenderer backImage;
    public SpriteRenderer changableFrontImge;

	[HideInInspector]
	public string nameOfImageAttacked;
	[HideInInspector]
	public Vector2 worldUnitsizeOfCard;
	// Start is called before the first frame update
	void Start()
    {
        /* baseFrontImage = this.transform.Find("front").GetComponent<SpriteRenderer>();
		 backImage = this.transform.Find("back").GetComponent<SpriteRenderer>();
		 changableFrontImge = this.transform.Find("front").transform.Find("changable image").GetComponent<SpriteRenderer>();*/



   
		//all the behaviours this machine performes.
		ScaleImageToBeSameSizeAsAnotherImageHollowedArea(changableFrontImge, baseFrontImage.sprite.border, baseFrontImage);
        MoveImageToTheCentreOfHollowedArea(changableFrontImge, baseFrontImage.sprite.border, baseFrontImage);

        StartCoroutine(CorrectChangableimageLayer(changableFrontImge, theGameThisCardBelongsTo.cameraToPrintCardInFront));


        worldUnitsizeOfCard = baseFrontImage.sprite.bounds.size;
    }




     
    // Update is called once per frame
    void Update()
    {
        /* Debug.Log(Camera.main.ScreenToWorldPoint(vector2));
		 Debug.DrawLine(Camera.main.ScreenToWorldPoint(vector), Camera.main.ScreenToWorldPoint(vector2));*/

        /* ScaleImageToBeSameSizeAsAnotherImageHollowedArea(backImage, Vector4.zero, baseFrontImage);
         MoveImageToTheCentreOfHollowedArea(backImage, Vector4.zero, baseFrontImage);*/



        Debug.Log(baseFrontImage.bounds.size);


	}
           
                                                                                               //A repeating main behaviour              
    IEnumerator CorrectChangableimageLayer(SpriteRenderer changableImage, Camera cameraToCorrectSrtingBasedOnIfItsLooking)
    {
        while (true)
        {
            float angleBetweenCardAndCamera = Vector3.Angle(cameraToCorrectSrtingBasedOnIfItsLooking.transform.forward, changableImage.transform.forward);
            bool cardIsFlippedAwayFromCamera = false;

            if (angleBetweenCardAndCamera > 90)
            {
                cardIsFlippedAwayFromCamera = true;
            }
            else if (angleBetweenCardAndCamera <= 90)
            {
                cardIsFlippedAwayFromCamera = false;
            }



            if (cardIsFlippedAwayFromCamera)
            {
                changableImage.sortingOrder = 0;
            }
            else
            {
                changableImage.sortingOrder = 2;
            }

            yield return new WaitForSecondsRealtime(0.04f);
        }

  
    }

  
   

                                                                                            //Main single time behavoiurs.
    void MoveImageToTheCentreOfHollowedArea(SpriteRenderer imageToMove, Vector4 borderOfHollowedAreaInPixels, SpriteRenderer backgroundWithTheHollowedArea)
    {
        Vector3 pixelCentreOfHollowedAreaOfBackgroundImage = FindCentreOfAHollowedAreaInPixelsBasedOnItsDimenstions(backgroundWithTheHollowedArea, borderOfHollowedAreaInPixels);

        Vector3 offsetFromPivotToHollowedAreCentreInPixels =
            pixelCentreOfHollowedAreaOfBackgroundImage - new Vector3(backgroundWithTheHollowedArea.sprite.pivot.x, backgroundWithTheHollowedArea.sprite.pivot.y, 0.1f);
        Vector3 offsetFromPivotToHollowedAreCentreInWorldUnits = offsetFromPivotToHollowedAreCentreInPixels / baseFrontImage.sprite.pixelsPerUnit;
        Vector3 positionOfHollowedreCentreInWorldSpace = backgroundWithTheHollowedArea.transform.localPosition + offsetFromPivotToHollowedAreCentreInWorldUnits;

        imageToMove.transform.localPosition = positionOfHollowedreCentreInWorldSpace;                    //if the chngable have a differnet pivot than its centre add n offset to calculation.
        changableFrontImge.transform.localPosition =AlwaysUtilities.Vector3ChangeNotAllProperties(changableFrontImge.transform.localPosition, new Vector3(0, 0, -0.001f));


    }


    void ScaleImageToBeSameSizeAsAnotherImageHollowedArea(SpriteRenderer imageToScale, Vector4 borderOfHollowedAreaInPixels, SpriteRenderer backgroundWithTheHollowedArea)
    {
        Vector3 totalPixelSizeOfBackgroundImageWithHollowedArea =
            new Vector3(backgroundWithTheHollowedArea.sprite.texture.width, backgroundWithTheHollowedArea.sprite.texture.height, 0.1f);


        Vector3 baseImageHollowedAreaSizeInPixels=
             FindTotalPixelSizeOfAHollowedAreaBasedOnItsBorderDimenstions(backgroundWithTheHollowedArea, borderOfHollowedAreaInPixels);


        Vector3 baseImageHollowedAreaPercentOfWhole =AlwaysUtilities.Vector3Divide(baseImageHollowedAreaSizeInPixels, totalPixelSizeOfBackgroundImageWithHollowedArea);
        Vector3 baseImageHollowedAreaInWorldUnit = Vector3.Scale(baseImageHollowedAreaPercentOfWhole, backgroundWithTheHollowedArea.sprite.bounds.size);

        ScaleImageToSpecificWorldSize(imageToScale, baseImageHollowedAreaInWorldUnit);
    }






                                                                                                 //secondry function used by the main functions.

    Vector3 FindCentreOfAHollowedAreaInPixelsBasedOnItsDimenstions(SpriteRenderer imageOfTheOperation, Vector4 dimensionsOfHollowedAreaInPixels)
    {
        Vector3 baseImageHollowedAreaSizeInPixels =
            FindTotalPixelSizeOfAHollowedAreaBasedOnItsBorderDimenstions(imageOfTheOperation, dimensionsOfHollowedAreaInPixels);



        Vector3 mediumPointOfHollowedAreaFromHollowedArea = AlwaysUtilities.Vector3Divide(baseImageHollowedAreaSizeInPixels, new Vector3(2, 2, 2));
        Vector3 pixelsFromImageStartToHollowedAreaStart = new Vector3(dimensionsOfHollowedAreaInPixels.x, dimensionsOfHollowedAreaInPixels.y, 0.1f);
        Vector3 mediumPointOfHollowedAreaFromWholeImage = mediumPointOfHollowedAreaFromHollowedArea + pixelsFromImageStartToHollowedAreaStart;

        return mediumPointOfHollowedAreaFromWholeImage;
    }

    Vector3 FindTotalPixelSizeOfAHollowedAreaBasedOnItsBorderDimenstions(SpriteRenderer imageOfTheOperation, Vector4 borderOfHollowedAreaInPixels)
    {
        Vector3 totalPixelSizeOfBackgroundImageWithHollowedArea =
        new Vector3(imageOfTheOperation.sprite.texture.width, imageOfTheOperation.sprite.texture.height, 0.1f);

        Vector3 baseImageHollowedAreaSizeInPixels = new Vector3
        (totalPixelSizeOfBackgroundImageWithHollowedArea.x - (borderOfHollowedAreaInPixels.x + borderOfHollowedAreaInPixels.z),
        totalPixelSizeOfBackgroundImageWithHollowedArea.y - (borderOfHollowedAreaInPixels.y + borderOfHollowedAreaInPixels.w),
        0.1f);

        return baseImageHollowedAreaSizeInPixels;
    }

    void ScaleImageToSpecificWorldSize(SpriteRenderer imageToScale, Vector3 worldUnitsToScaleTo)   //takes in a world unit and scales an image to be that size.
    {
        Vector3 currentWorldUnitOfChangable = imageToScale.sprite.bounds.size;
        Vector3 targetWorldUnitOfChangable = worldUnitsToScaleTo;
        Vector3 percentToScaleChangableImage =AlwaysUtilities.Vector3Divide(targetWorldUnitOfChangable, currentWorldUnitOfChangable);

        changableFrontImge.transform.localScale = Vector3.Scale(Vector3.one, percentToScaleChangableImage);

    }







 




}
