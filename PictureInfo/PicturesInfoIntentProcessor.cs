using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PictureInfo.Pictures;

namespace PictureInfo
{
    public class PicturesInfoIntentProcessor :AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "pictureName";
        PicturesInfo _chosenPicture;
        private Task<ScanResponse> result;
        public PicturesInfoIntentProcessor(Task<ScanResponse> result)
        {
            this.result = result;
        }
        public override LexResponse Process(LexEvent lexEvent, ILambdaContext context) 
        {
            IDictionary<string, string> slots = lexEvent.CurrentIntent.Slots;
            IDictionary<string, string> sessionAttributes = lexEvent.SessionAttributes ?? new Dictionary<string, string>();
            if (slots[TYPE_SLOT] != null)
            {
                var validatePictureName = ValidatePictureName(slots[TYPE_SLOT]);
                if (!validatePictureName.IsValid)
                {
                    slots[validatePictureName.ViolationSlot] = null;
                    return ElicitSlot(sessionAttributes, lexEvent.CurrentIntent.Name, slots, validatePictureName.ViolationSlot, validatePictureName.Message);
                }

            }
            List<LexResponse.LexGenericAttachments> attachments = new List<LexResponse.LexGenericAttachments>();
            attachments.Add(getImageAttachmentForChosenPicture(_chosenPicture));

            return PictureChosen(sessionAttributes, "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content = String.Format(getMessageForChosenPicture(_chosenPicture))
                        },
                        new LexResponse.LexResponseCard
                        {
                            GenericAttachments = attachments,

                        }
                    );
        }
        private String getMessageForChosenPicture(PicturesInfo picturesInfo)
        {
            List<Item> picturesItems = result.Result.Items.Select(Map).ToList();
            String resultDescription = "I am so sorry :( We don't know about this picture yet. Could you please try another one?";
            foreach (Item item in picturesItems)
            {
                if (item.PictureName.Equals(picturesInfo._values[0]))
                {
                    resultDescription = item.Description;
                    break;
                }
            }

            return resultDescription;
        }
        private LexResponse.LexGenericAttachments getImageAttachmentForChosenPicture(PicturesInfo picturesInfo)
        {
            List<Item> picturesItems = result.Result.Items.Select(Map).ToList();
            LexResponse.LexGenericAttachments imageUrl = new LexResponse.LexGenericAttachments();
            foreach (Item item in picturesItems)
            {
                if (item.PictureName.Equals(picturesInfo._values[0]))
                {
                    /*List<LexResponse.LexButton> buttons = new List<LexResponse.LexButton>();
                    LexResponse.LexButton lexButton = new LexResponse.LexButton();
                    lexButton.Text = "More about artist";
                    lexButton.Value = "";
                    buttons.Add(lexButton);*/

                    imageUrl = new LexResponse.LexGenericAttachments()
                    {
                        ImageUrl = item.ImageURL,
                        Title = "Picture image",
                        SubTitle = "See the image of selected picture"
                        // Buttons = buttons

                    };
                    break;
                }
            }
            return imageUrl;
        }
        public class Item
        {
            public string Description { get; set; }
            public string ImageURL { get; set; }
            public string PictureName { get; set; }
           // public string ArtistName { get; set; }


        }
        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                Description = result["Description"].S,
                ImageURL = result["ImageURL"].S,
                PictureName = result["PictureName"].S
               // ArtistName = result["ArtistName"].S
            };
        }
        private ValidationResult ValidatePictureName(string pictureNameString)
        {

            Pictures pictures = new Pictures();

            if (pictures.isDefined(pictureNameString))
            {
                _chosenPicture = pictures.getPictureByName(pictureNameString);
                return ValidationResult.VALID_RESULT;
            }
            else
            {
                return new ValidationResult(false, TYPE_SLOT, String.Format("We do not have information about {0}, would you like to try a different picture?", pictureNameString));
            }
        }
    }
}
