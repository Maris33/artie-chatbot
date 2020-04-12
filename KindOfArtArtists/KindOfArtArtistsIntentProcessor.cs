using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KindOfArtArtists.kindOfArt;

namespace KindOfArtArtists
{
    public class KindOfArtArtistsIntentProcessor : AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "artKind";
        KindsOfART _chosenArtType = KindsOfART.Null;
        private Task<ScanResponse> result;

        public KindOfArtArtistsIntentProcessor(Task<ScanResponse> result)
        {
            this.result = result;
        }
        public override LexResponse Process(LexEvent lexEvent, ILambdaContext context)
        {
            IDictionary<string, string> slots = lexEvent.CurrentIntent.Slots;
            IDictionary<string, string> sessionAttributes = lexEvent.SessionAttributes ?? new Dictionary<string, string>();
            //if the KindOfArt slot has a value, validate that it is contained within the enum list available.
            if (slots[TYPE_SLOT] != null)
            {
                var validateKindOfArtType = ValidateKindOfArtType(slots[TYPE_SLOT]);
                if (!validateKindOfArtType.IsValid)
                {
                    slots[validateKindOfArtType.ViolationSlot] = null;
                    return ElicitSlot(sessionAttributes, lexEvent.CurrentIntent.Name, slots, validateKindOfArtType.ViolationSlot, validateKindOfArtType.Message);
                }

            }
            List<String> artistsOfChosenArtType = showArtistsForChosenArtType(_chosenArtType);
            String finalArtistsOfChosenArtType = "";
            foreach (String artist in artistsOfChosenArtType)
            {
                finalArtistsOfChosenArtType += (artist + "\n");
            }

            return ArtTypeChosen(sessionAttributes, "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content = finalArtistsOfChosenArtType
                        }
                    );
        }
        private List<String> showArtistsForChosenArtType(KindsOfART kindOfART)
        {
            List<Item> kindOfArtItems = result.Result.Items.Select(Map).ToList();
            List<String> resultShowArtistsForChosenArtType = new List<string>();
            resultShowArtistsForChosenArtType.Add("Sorry, I have no avaliable list of artists for this art type. Could you please try another one?");
            foreach (Item item in kindOfArtItems)
            {
                if (item.KindOfArtName.Equals(kindOfART.ToString()))
                {
                    resultShowArtistsForChosenArtType = item.KindOfArtArtists;
                    break;
                }
            }

            return resultShowArtistsForChosenArtType;
        }
        public class Item
        {
            public string KindOfArtName { get; set; }
            public string Description { get; set; }
            public List<string> KindOfArtArtists { get; set; }
        }

        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                KindOfArtName = result["KindOfArtName"].S,
                Description = result["Description"].S,
                KindOfArtArtists = result.ContainsKey("KindOfArtArtists") ? result["KindOfArtArtists"].SS : new List<string>()
            };
        }
        private ValidationResult ValidateKindOfArtType(string kindOfArtTypeString)
        {
            bool kindOfArtTypeValid = Enum.IsDefined(typeof(KindsOfART), kindOfArtTypeString.ToUpper());

            if (Enum.TryParse(typeof(KindsOfART), kindOfArtTypeString, true, out object kindOfArtType))
            {
                _chosenArtType = (KindsOfART)kindOfArtType;
                return ValidationResult.VALID_RESULT;
            }
            else
            {
                return new ValidationResult(false, TYPE_SLOT, String.Format("We do not have information about {0}, would you like a different kind of ART? Our most popular request is about modernism", kindOfArtTypeString));
            }
        }
    }
}
