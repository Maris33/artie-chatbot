using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtistInfo.Artists;

namespace ArtistInfo
{
    public class ArtistsInfoIntentProcessor :AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "artistName";
        ArtistsInfo _chosenArtist;
        private Task<ScanResponse> result;

        public ArtistsInfoIntentProcessor(Task<ScanResponse> result)
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
            return ArtistChosen(sessionAttributes, "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content = String.Format(getMessageForChosenArtist(_chosenArtist))
                        }
                    );
        }

        private String getMessageForChosenArtist(ArtistsInfo artistsInfo)
        {
            List<Item> artistsItems = result.Result.Items.Select(Map).ToList();
            String resultDescription = "I am so sorry :( We don't know about this artist yet. Could you please try another one?";
            foreach (Item item in artistsItems)
            {
                if (item.ArtistName.Equals(artistsInfo._values[0]))
                {
                    resultDescription = item.Description;
                    break;
                }
            }

            return resultDescription;
        }
        public class Item
        {
            public string ArtistName { get; set; }
            public string Description { get; set; }
        }
        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                ArtistName = result["ArtistName"].S,
                Description = result["Description"].S
            };
        }
        private ValidationResult ValidateArtistName(string artistNameString)
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
