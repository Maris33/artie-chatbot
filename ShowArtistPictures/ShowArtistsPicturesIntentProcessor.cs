using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShowArtistPictures.Artists;

namespace ShowArtistPictures
{
    public class ShowArtistsPicturesIntentProcessor : AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "artistName";
        ArtistsInfo _chosenArtist;
        private Task<ScanResponse> result;
        public ShowArtistsPicturesIntentProcessor(Task<ScanResponse> result)
        {
            this.result = result;
        }
        public override LexResponse Process(LexEvent lexEvent, ILambdaContext context)
        {
            IDictionary<string, string> slots = lexEvent.CurrentIntent.Slots;
            IDictionary<string, string> sessionAttributes = lexEvent.SessionAttributes ?? new Dictionary<string, string>();
            if (slots[TYPE_SLOT] != null)
            {
                var validateArtistName = ValidateArtistName(slots[TYPE_SLOT]);
                if (!validateArtistName.IsValid)
                {
                    slots[validateArtistName.ViolationSlot] = null;
                    return ElicitSlot(sessionAttributes, lexEvent.CurrentIntent.Name, slots, validateArtistName.ViolationSlot, validateArtistName.Message);
                }

            }

            List<String> picturesOfChosenArtist = showPicturesOfChosenArtist(_chosenArtist);
            String finalPicturesOfChosenArtistString = "";
            //picturesOfChosenArtist
            foreach (String picture in picturesOfChosenArtist)
            {
                finalPicturesOfChosenArtistString += ( picture + "\n ");
            }


            return ArtistChosen(sessionAttributes, "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content = finalPicturesOfChosenArtistString
                            //String.Format(showPicturesOfChosenArtist(_chosenArtist))
                        }
                    );
        }

        private List<String> showPicturesOfChosenArtist(ArtistsInfo artistsInfo)
        {
            List<Item> artistsItems = result.Result.Items.Select(Map).ToList();
            List<String> resultShowArtistPaintings = new List<string>();
            resultShowArtistPaintings.Add("Sorry, I have no avaliable list of pictures for this artist. Could you please try another one?");
            foreach (Item item in artistsItems)
            {
                if (item.ArtistName.Equals(artistsInfo._values[0]))
                {
                    resultShowArtistPaintings = item.Pictures;
                    break;
                }
            }

            return resultShowArtistPaintings;
        }
        public class Item
        {
            public string ArtistName { get; set; }
            public string Description { get; set; }
            public string ImageURL { get; set; }
            public List<string> Pictures { get; set; }
        }
        private Item Map(Dictionary<String, AttributeValue> result)
        { 
            return new Item
            {
                ArtistName = result["ArtistName"].S,
                Pictures = result.ContainsKey("Pictures") ? result["Pictures"].SS : new List<string>(),
                ImageURL = result["ImageURL"].S,
                Description = result["Description"].S
            };
        }
        private ValidationResult ValidateArtistName(String artistNameString)
        {

            Artists artists = new Artists();

            if (artists.isDefined(artistNameString))
            {
                _chosenArtist = artists.getArtistByName(artistNameString);
                return ValidationResult.VALID_RESULT;
            }
            else
            {
                return new ValidationResult(false, TYPE_SLOT, String.Format("We do not have information about {0}, would you like to try a different artist?", artistNameString));
            }
        }
    }
}
